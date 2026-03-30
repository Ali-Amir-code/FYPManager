using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.DAL;

public sealed class GroupDAL
{
    private readonly IDatabaseHelper _databaseHelper;

    public GroupDAL(IDatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    public async Task<IReadOnlyList<GroupListItem>> SearchGroupsAsync(string? searchTerm)
    {
        const string sql = """
            SELECT g.Id,
                   g.Created_On,
                   COUNT(gs.StudentId) AS MemberCount
            FROM `group` g
            LEFT JOIN groupstudent gs ON gs.GroupId = g.Id
            WHERE @SearchTerm = ''
               OR CAST(g.Id AS CHAR) LIKE CONCAT('%', @SearchTerm, '%')
               OR DATE_FORMAT(g.Created_On, '%Y-%m-%d') LIKE CONCAT('%', @SearchTerm, '%')
            GROUP BY g.Id, g.Created_On
            ORDER BY g.Id DESC;
            """;

        List<GroupListItem> groups = new();
        string normalizedSearch = searchTerm?.Trim() ?? string.Empty;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@SearchTerm", normalizedSearch);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            groups.Add(new GroupListItem
            {
                Id = reader.GetInt32("Id"),
                CreatedOn = DateOnly.FromDateTime(reader.GetDateTime("Created_On")),
                MemberCount = reader.GetInt32("MemberCount")
            });
        }

        return groups;
    }

    public async Task<GroupUpsertModel?> GetGroupByIdAsync(int groupId)
    {
        const string sql = """
            SELECT Id, Created_On
            FROM `group`
            WHERE Id = @Id;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", groupId);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new GroupUpsertModel
        {
            Id = reader.GetInt32("Id"),
            CreatedOn = DateOnly.FromDateTime(reader.GetDateTime("Created_On"))
        };
    }

    public async Task<int> CreateGroupAsync(GroupUpsertModel model)
    {
        const string sql = """
            INSERT INTO `group` (Created_On)
            VALUES (@CreatedOn);
            SELECT LAST_INSERT_ID();
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@CreatedOn", model.CreatedOn.ToDateTime(TimeOnly.MinValue));
        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task UpdateGroupAsync(GroupUpsertModel model)
    {
        const string sql = """
            UPDATE `group`
            SET Created_On = @CreatedOn
            WHERE Id = @Id;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", model.Id);
        command.Parameters.AddWithValue("@CreatedOn", model.CreatedOn.ToDateTime(TimeOnly.MinValue));
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteGroupAsync(int groupId)
    {
        const string sql = "DELETE FROM `group` WHERE Id = @Id;";

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", groupId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<GroupMemberItem>> GetMembersAsync(int groupId)
    {
        const string sql = """
            SELECT gs.GroupId,
                   gs.StudentId,
                   s.RegistrationNo,
                   CONCAT(p.FirstName, IFNULL(CONCAT(' ', p.LastName), '')) AS StudentName,
                   l.Value AS StatusValue
            FROM groupstudent gs
            INNER JOIN student s ON s.Id = gs.StudentId
            INNER JOIN person p ON p.Id = s.Id
            INNER JOIN lookup l ON l.Id = gs.Status
            WHERE gs.GroupId = @GroupId
            ORDER BY p.FirstName, p.LastName, s.RegistrationNo;
            """;

        List<GroupMemberItem> members = new();

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@GroupId", groupId);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            members.Add(new GroupMemberItem
            {
                GroupId = reader.GetInt32("GroupId"),
                StudentId = reader.GetInt32("StudentId"),
                RegistrationNo = reader.GetString("RegistrationNo"),
                StudentName = reader.GetString("StudentName").Trim(),
                StatusValue = reader.GetString("StatusValue")
            });
        }

        return members;
    }

    public async Task<bool> GroupMemberExistsAsync(int groupId, int studentId)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM groupstudent
            WHERE GroupId = @GroupId AND StudentId = @StudentId;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@GroupId", groupId);
        command.Parameters.AddWithValue("@StudentId", studentId);
        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task<bool> StudentHasActiveGroupAsync(int studentId, int activeStatusId, int excludingGroupId = 0)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM groupstudent
            WHERE StudentId = @StudentId
              AND Status = @ActiveStatusId
              AND (@ExcludingGroupId = 0 OR GroupId <> @ExcludingGroupId);
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@StudentId", studentId);
        command.Parameters.AddWithValue("@ActiveStatusId", activeStatusId);
        command.Parameters.AddWithValue("@ExcludingGroupId", excludingGroupId);
        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task AddMemberAsync(GroupMemberUpsertModel model)
    {
        const string sql = """
            INSERT INTO groupstudent (GroupId, StudentId, Status, AssignmentDate)
            VALUES (@GroupId, @StudentId, @StatusId, @AssignmentDate);
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@GroupId", model.GroupId);
        command.Parameters.AddWithValue("@StudentId", model.StudentId);
        command.Parameters.AddWithValue("@StatusId", model.StatusId);
        command.Parameters.AddWithValue("@AssignmentDate", model.AssignmentDate);
        await command.ExecuteNonQueryAsync();
    }

    public async Task RemoveMemberAsync(int groupId, int studentId)
    {
        const string sql = """
            DELETE FROM groupstudent
            WHERE GroupId = @GroupId AND StudentId = @StudentId;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@GroupId", groupId);
        command.Parameters.AddWithValue("@StudentId", studentId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<GroupProjectAssignmentItem?> GetProjectAssignmentAsync(int groupId)
    {
        const string sql = """
            SELECT gp.GroupId,
                   gp.ProjectId,
                   p.Title AS ProjectTitle,
                   gp.AssignmentDate
            FROM groupproject gp
            INNER JOIN project p ON p.Id = gp.ProjectId
            WHERE gp.GroupId = @GroupId
            ORDER BY gp.AssignmentDate DESC
            LIMIT 1;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@GroupId", groupId);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new GroupProjectAssignmentItem
        {
            GroupId = reader.GetInt32("GroupId"),
            ProjectId = reader.GetInt32("ProjectId"),
            ProjectTitle = reader.GetString("ProjectTitle"),
            AssignmentDate = reader.GetDateTime("AssignmentDate")
        };
    }

    public async Task<bool> GroupProjectAssignmentExistsAsync(int groupId, int projectId)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM groupproject
            WHERE GroupId = @GroupId AND ProjectId = @ProjectId;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@GroupId", groupId);
        command.Parameters.AddWithValue("@ProjectId", projectId);
        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task AssignProjectAsync(GroupProjectAssignmentModel model)
    {
        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlTransaction transaction = await connection.BeginTransactionAsync();

        try
        {
            const string deleteExistingSql = """
                DELETE FROM groupproject
                WHERE GroupId = @GroupId;
                """;

            await using MySqlCommand deleteExistingCommand = new(deleteExistingSql, connection, transaction);
            deleteExistingCommand.Parameters.AddWithValue("@GroupId", model.GroupId);
            await deleteExistingCommand.ExecuteNonQueryAsync();

            const string insertAssignmentSql = """
                INSERT INTO groupproject (ProjectId, GroupId, AssignmentDate)
                VALUES (@ProjectId, @GroupId, @AssignmentDate);
                """;

            await using MySqlCommand insertAssignmentCommand = new(insertAssignmentSql, connection, transaction);
            insertAssignmentCommand.Parameters.AddWithValue("@ProjectId", model.ProjectId);
            insertAssignmentCommand.Parameters.AddWithValue("@GroupId", model.GroupId);
            insertAssignmentCommand.Parameters.AddWithValue("@AssignmentDate", model.AssignmentDate);
            await insertAssignmentCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task RemoveProjectAssignmentAsync(int groupId)
    {
        const string sql = """
            DELETE FROM groupproject
            WHERE GroupId = @GroupId;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@GroupId", groupId);
        await command.ExecuteNonQueryAsync();
    }
}
