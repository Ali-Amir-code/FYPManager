using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.DAL;

public sealed class StudentDAL
{
    private readonly IDatabaseHelper _databaseHelper;

    public StudentDAL(IDatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    public async Task<bool> RegistrationNumberExistsAsync(string registrationNumber, int excludingStudentId = 0)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM student
            WHERE RegistrationNo = @RegistrationNo
              AND (@ExcludingStudentId = 0 OR Id <> @ExcludingStudentId);
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@RegistrationNo", registrationNumber.Trim());
        command.Parameters.AddWithValue("@ExcludingStudentId", excludingStudentId);

        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task<int> CreateAsync(StudentUpsertModel model)
    {
        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlTransaction transaction = await connection.BeginTransactionAsync();

        try
        {
            const string insertPersonSql = """
                INSERT INTO person (FirstName, LastName, Contact, Email, DateOfBirth, Gender)
                VALUES (@FirstName, @LastName, @Contact, @Email, @DateOfBirth, @Gender);
                SELECT LAST_INSERT_ID();
                """;

            await using MySqlCommand insertPersonCommand = new(insertPersonSql, connection, transaction);
            ApplyStudentParameters(insertPersonCommand, model);
            int personId = Convert.ToInt32(await insertPersonCommand.ExecuteScalarAsync());

            const string insertStudentSql = """
                INSERT INTO student (Id, RegistrationNo)
                VALUES (@Id, @RegistrationNo);
                """;

            await using MySqlCommand insertStudentCommand = new(insertStudentSql, connection, transaction);
            insertStudentCommand.Parameters.AddWithValue("@Id", personId);
            insertStudentCommand.Parameters.AddWithValue("@RegistrationNo", model.RegistrationNo.Trim());
            await insertStudentCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            return personId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateAsync(StudentUpsertModel model)
    {
        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlTransaction transaction = await connection.BeginTransactionAsync();

        try
        {
            const string updatePersonSql = """
                UPDATE person
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    Contact = @Contact,
                    Email = @Email,
                    DateOfBirth = @DateOfBirth,
                    Gender = @Gender
                WHERE Id = @Id;
                """;

            await using MySqlCommand updatePersonCommand = new(updatePersonSql, connection, transaction);
            updatePersonCommand.Parameters.AddWithValue("@Id", model.Id);
            ApplyStudentParameters(updatePersonCommand, model);
            await updatePersonCommand.ExecuteNonQueryAsync();

            const string updateStudentSql = """
                UPDATE student
                SET RegistrationNo = @RegistrationNo
                WHERE Id = @Id;
                """;

            await using MySqlCommand updateStudentCommand = new(updateStudentSql, connection, transaction);
            updateStudentCommand.Parameters.AddWithValue("@Id", model.Id);
            updateStudentCommand.Parameters.AddWithValue("@RegistrationNo", model.RegistrationNo.Trim());
            await updateStudentCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteAsync(int studentId)
    {
        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlTransaction transaction = await connection.BeginTransactionAsync();

        try
        {
            const string deleteStudentSql = "DELETE FROM student WHERE Id = @Id;";
            await using MySqlCommand deleteStudentCommand = new(deleteStudentSql, connection, transaction);
            deleteStudentCommand.Parameters.AddWithValue("@Id", studentId);
            await deleteStudentCommand.ExecuteNonQueryAsync();

            const string deletePersonSql = "DELETE FROM person WHERE Id = @Id;";
            await using MySqlCommand deletePersonCommand = new(deletePersonSql, connection, transaction);
            deletePersonCommand.Parameters.AddWithValue("@Id", studentId);
            await deletePersonCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<StudentUpsertModel?> GetByIdAsync(int studentId)
    {
        const string sql = """
            SELECT s.Id,
                   s.RegistrationNo,
                   p.FirstName,
                   p.LastName,
                   p.Contact,
                   p.Email,
                   p.DateOfBirth,
                   p.Gender
            FROM student s
            INNER JOIN person p ON p.Id = s.Id
            WHERE s.Id = @Id;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", studentId);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        int lastNameOrdinal = reader.GetOrdinal("LastName");
        int contactOrdinal = reader.GetOrdinal("Contact");
        int dateOfBirthOrdinal = reader.GetOrdinal("DateOfBirth");
        int genderOrdinal = reader.GetOrdinal("Gender");

        return new StudentUpsertModel
        {
            Id = reader.GetInt32("Id"),
            RegistrationNo = reader.GetString("RegistrationNo"),
            FirstName = reader.GetString("FirstName"),
            LastName = reader.IsDBNull(lastNameOrdinal) ? null : reader.GetString(lastNameOrdinal),
            Contact = reader.IsDBNull(contactOrdinal) ? null : reader.GetString(contactOrdinal),
            Email = reader.GetString("Email"),
            DateOfBirth = reader.IsDBNull(dateOfBirthOrdinal) ? null : reader.GetDateTime(dateOfBirthOrdinal),
            GenderId = reader.IsDBNull(genderOrdinal) ? null : reader.GetInt32(genderOrdinal)
        };
    }

    public async Task<IReadOnlyList<StudentListItem>> SearchAsync(string? searchTerm)
    {
        const string sql = """
            SELECT s.Id,
                   s.RegistrationNo,
                   CONCAT(p.FirstName, IFNULL(CONCAT(' ', p.LastName), '')) AS FullName,
                   p.Email,
                   p.Contact,
                   p.DateOfBirth,
                   p.Gender AS GenderId,
                   l.Value AS GenderValue
            FROM student s
            INNER JOIN person p ON p.Id = s.Id
            LEFT JOIN lookup l ON l.Id = p.Gender
            WHERE @SearchTerm = ''
               OR s.RegistrationNo LIKE CONCAT('%', @SearchTerm, '%')
               OR p.FirstName LIKE CONCAT('%', @SearchTerm, '%')
               OR IFNULL(p.LastName, '') LIKE CONCAT('%', @SearchTerm, '%')
               OR p.Email LIKE CONCAT('%', @SearchTerm, '%')
            ORDER BY p.FirstName, p.LastName, s.RegistrationNo;
            """;

        List<StudentListItem> students = new();
        string normalizedSearch = searchTerm?.Trim() ?? string.Empty;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@SearchTerm", normalizedSearch);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            int contactOrdinal = reader.GetOrdinal("Contact");
            int dateOfBirthOrdinal = reader.GetOrdinal("DateOfBirth");
            int genderIdOrdinal = reader.GetOrdinal("GenderId");
            int genderValueOrdinal = reader.GetOrdinal("GenderValue");

            students.Add(new StudentListItem
            {
                Id = reader.GetInt32("Id"),
                RegistrationNo = reader.GetString("RegistrationNo"),
                FullName = reader.GetString("FullName").Trim(),
                Email = reader.GetString("Email"),
                Contact = reader.IsDBNull(contactOrdinal) ? null : reader.GetString(contactOrdinal),
                DateOfBirth = reader.IsDBNull(dateOfBirthOrdinal) ? null : reader.GetDateTime(dateOfBirthOrdinal),
                GenderId = reader.IsDBNull(genderIdOrdinal) ? null : reader.GetInt32(genderIdOrdinal),
                GenderValue = reader.IsDBNull(genderValueOrdinal) ? null : reader.GetString(genderValueOrdinal)
            });
        }

        return students;
    }

    public async Task<IReadOnlyList<StudentListItem>> GetUnassignedStudentsAsync()
    {
        const string sql = """
            SELECT s.Id,
                   s.RegistrationNo,
                   CONCAT(p.FirstName, IFNULL(CONCAT(' ', p.LastName), '')) AS FullName,
                   p.Email,
                   p.Contact,
                   p.DateOfBirth,
                   p.Gender AS GenderId,
                   l.Value AS GenderValue
            FROM student s
            INNER JOIN person p ON p.Id = s.Id
            LEFT JOIN lookup l ON l.Id = p.Gender
            WHERE NOT EXISTS (
                SELECT 1
                FROM groupstudent gs
                WHERE gs.StudentId = s.Id
            )
            ORDER BY p.FirstName, p.LastName, s.RegistrationNo;
            """;

        List<StudentListItem> students = new();

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            int contactOrdinal = reader.GetOrdinal("Contact");
            int dateOfBirthOrdinal = reader.GetOrdinal("DateOfBirth");
            int genderIdOrdinal = reader.GetOrdinal("GenderId");
            int genderValueOrdinal = reader.GetOrdinal("GenderValue");

            students.Add(new StudentListItem
            {
                Id = reader.GetInt32("Id"),
                RegistrationNo = reader.GetString("RegistrationNo"),
                FullName = reader.GetString("FullName").Trim(),
                Email = reader.GetString("Email"),
                Contact = reader.IsDBNull(contactOrdinal) ? null : reader.GetString(contactOrdinal),
                DateOfBirth = reader.IsDBNull(dateOfBirthOrdinal) ? null : reader.GetDateTime(dateOfBirthOrdinal),
                GenderId = reader.IsDBNull(genderIdOrdinal) ? null : reader.GetInt32(genderIdOrdinal),
                GenderValue = reader.IsDBNull(genderValueOrdinal) ? null : reader.GetString(genderValueOrdinal)
            });
        }

        return students;
    }

    private static void ApplyStudentParameters(MySqlCommand command, StudentUpsertModel model)
    {
        command.Parameters.AddWithValue("@FirstName", model.FirstName.Trim());
        command.Parameters.AddWithValue("@LastName", string.IsNullOrWhiteSpace(model.LastName) ? DBNull.Value : model.LastName.Trim());
        command.Parameters.AddWithValue("@Contact", string.IsNullOrWhiteSpace(model.Contact) ? DBNull.Value : model.Contact.Trim());
        command.Parameters.AddWithValue("@Email", model.Email.Trim());
        command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth.HasValue ? model.DateOfBirth.Value : DBNull.Value);
        command.Parameters.AddWithValue("@Gender", model.GenderId.HasValue ? model.GenderId.Value : DBNull.Value);
    }
}
