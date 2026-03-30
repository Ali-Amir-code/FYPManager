using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.DAL;

public sealed class AdvisorDAL
{
    private readonly IDatabaseHelper _databaseHelper;

    public AdvisorDAL(IDatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    public async Task<int> CreateAsync(AdvisorUpsertModel model)
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
            ApplyPersonParameters(insertPersonCommand, model);
            int personId = Convert.ToInt32(await insertPersonCommand.ExecuteScalarAsync());

            const string insertAdvisorSql = """
                INSERT INTO advisor (Id, Designation, Salary)
                VALUES (@Id, @Designation, @Salary);
                """;

            await using MySqlCommand insertAdvisorCommand = new(insertAdvisorSql, connection, transaction);
            insertAdvisorCommand.Parameters.AddWithValue("@Id", personId);
            insertAdvisorCommand.Parameters.AddWithValue("@Designation", model.DesignationId);
            insertAdvisorCommand.Parameters.AddWithValue("@Salary", model.Salary.HasValue ? model.Salary.Value : DBNull.Value);
            await insertAdvisorCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            return personId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateAsync(AdvisorUpsertModel model)
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
            ApplyPersonParameters(updatePersonCommand, model);
            await updatePersonCommand.ExecuteNonQueryAsync();

            const string updateAdvisorSql = """
                UPDATE advisor
                SET Designation = @Designation,
                    Salary = @Salary
                WHERE Id = @Id;
                """;

            await using MySqlCommand updateAdvisorCommand = new(updateAdvisorSql, connection, transaction);
            updateAdvisorCommand.Parameters.AddWithValue("@Id", model.Id);
            updateAdvisorCommand.Parameters.AddWithValue("@Designation", model.DesignationId);
            updateAdvisorCommand.Parameters.AddWithValue("@Salary", model.Salary.HasValue ? model.Salary.Value : DBNull.Value);
            await updateAdvisorCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteAsync(int advisorId)
    {
        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlTransaction transaction = await connection.BeginTransactionAsync();

        try
        {
            const string deleteAdvisorSql = "DELETE FROM advisor WHERE Id = @Id;";
            await using MySqlCommand deleteAdvisorCommand = new(deleteAdvisorSql, connection, transaction);
            deleteAdvisorCommand.Parameters.AddWithValue("@Id", advisorId);
            await deleteAdvisorCommand.ExecuteNonQueryAsync();

            const string deletePersonSql = "DELETE FROM person WHERE Id = @Id;";
            await using MySqlCommand deletePersonCommand = new(deletePersonSql, connection, transaction);
            deletePersonCommand.Parameters.AddWithValue("@Id", advisorId);
            await deletePersonCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<AdvisorUpsertModel?> GetByIdAsync(int advisorId)
    {
        const string sql = """
            SELECT a.Id,
                   a.Designation,
                   a.Salary,
                   p.FirstName,
                   p.LastName,
                   p.Contact,
                   p.Email,
                   p.DateOfBirth,
                   p.Gender
            FROM advisor a
            INNER JOIN person p ON p.Id = a.Id
            WHERE a.Id = @Id;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", advisorId);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        int lastNameOrdinal = reader.GetOrdinal("LastName");
        int contactOrdinal = reader.GetOrdinal("Contact");
        int dateOfBirthOrdinal = reader.GetOrdinal("DateOfBirth");
        int genderOrdinal = reader.GetOrdinal("Gender");
        int salaryOrdinal = reader.GetOrdinal("Salary");

        return new AdvisorUpsertModel
        {
            Id = reader.GetInt32("Id"),
            FirstName = reader.GetString("FirstName"),
            LastName = reader.IsDBNull(lastNameOrdinal) ? null : reader.GetString(lastNameOrdinal),
            Contact = reader.IsDBNull(contactOrdinal) ? null : reader.GetString(contactOrdinal),
            Email = reader.GetString("Email"),
            DateOfBirth = reader.IsDBNull(dateOfBirthOrdinal) ? null : reader.GetDateTime(dateOfBirthOrdinal),
            GenderId = reader.IsDBNull(genderOrdinal) ? null : reader.GetInt32(genderOrdinal),
            DesignationId = reader.GetInt32("Designation"),
            Salary = reader.IsDBNull(salaryOrdinal) ? null : reader.GetDecimal(salaryOrdinal)
        };
    }

    public async Task<IReadOnlyList<AdvisorListItem>> SearchAsync(string? searchTerm)
    {
        const string sql = """
            SELECT a.Id,
                   CONCAT(p.FirstName, IFNULL(CONCAT(' ', p.LastName), '')) AS FullName,
                   p.Email,
                   l.Value AS DesignationValue,
                   a.Salary
            FROM advisor a
            INNER JOIN person p ON p.Id = a.Id
            INNER JOIN lookup l ON l.Id = a.Designation
            WHERE @SearchTerm = ''
               OR p.FirstName LIKE CONCAT('%', @SearchTerm, '%')
               OR IFNULL(p.LastName, '') LIKE CONCAT('%', @SearchTerm, '%')
               OR p.Email LIKE CONCAT('%', @SearchTerm, '%')
               OR l.Value LIKE CONCAT('%', @SearchTerm, '%')
            ORDER BY p.FirstName, p.LastName;
            """;

        List<AdvisorListItem> advisors = new();
        string normalizedSearch = searchTerm?.Trim() ?? string.Empty;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@SearchTerm", normalizedSearch);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            int salaryOrdinal = reader.GetOrdinal("Salary");

            advisors.Add(new AdvisorListItem
            {
                Id = reader.GetInt32("Id"),
                FullName = reader.GetString("FullName").Trim(),
                Email = reader.GetString("Email"),
                DesignationValue = reader.GetString("DesignationValue"),
                Salary = reader.IsDBNull(salaryOrdinal) ? null : reader.GetDecimal(salaryOrdinal)
            });
        }

        return advisors;
    }

    private static void ApplyPersonParameters(MySqlCommand command, AdvisorUpsertModel model)
    {
        command.Parameters.AddWithValue("@FirstName", model.FirstName.Trim());
        command.Parameters.AddWithValue("@LastName", string.IsNullOrWhiteSpace(model.LastName) ? DBNull.Value : model.LastName.Trim());
        command.Parameters.AddWithValue("@Contact", string.IsNullOrWhiteSpace(model.Contact) ? DBNull.Value : model.Contact.Trim());
        command.Parameters.AddWithValue("@Email", model.Email.Trim());
        command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth.HasValue ? model.DateOfBirth.Value : DBNull.Value);
        command.Parameters.AddWithValue("@Gender", model.GenderId.HasValue ? model.GenderId.Value : DBNull.Value);
    }
}
