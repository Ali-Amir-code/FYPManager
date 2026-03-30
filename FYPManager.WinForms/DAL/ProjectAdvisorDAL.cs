using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.DAL;

public sealed class ProjectAdvisorDAL
{
    private readonly IDatabaseHelper _databaseHelper;

    public ProjectAdvisorDAL(IDatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    public async Task<IReadOnlyList<ProjectBoardMemberItem>> GetBoardMembersAsync(int projectId)
    {
        const string sql = """
            SELECT pa.ProjectId,
                   pa.AdvisorId,
                   p.Title AS ProjectTitle,
                   CONCAT(pe.FirstName, IFNULL(CONCAT(' ', pe.LastName), '')) AS AdvisorName,
                   pa.AdvisorRole AS AdvisorRoleId,
                   l.Value AS AdvisorRoleValue,
                   pa.AssignmentDate
            FROM projectadvisor pa
            INNER JOIN project p ON p.Id = pa.ProjectId
            INNER JOIN advisor a ON a.Id = pa.AdvisorId
            INNER JOIN person pe ON pe.Id = a.Id
            INNER JOIN lookup l ON l.Id = pa.AdvisorRole
            WHERE pa.ProjectId = @ProjectId
            ORDER BY pa.AssignmentDate, l.Value;
            """;

        List<ProjectBoardMemberItem> members = new();

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@ProjectId", projectId);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            members.Add(new ProjectBoardMemberItem
            {
                ProjectId = reader.GetInt32("ProjectId"),
                AdvisorId = reader.GetInt32("AdvisorId"),
                ProjectTitle = reader.GetString("ProjectTitle"),
                AdvisorName = reader.GetString("AdvisorName").Trim(),
                AdvisorRoleId = reader.GetInt32("AdvisorRoleId"),
                AdvisorRoleValue = reader.GetString("AdvisorRoleValue"),
                AssignmentDate = reader.GetDateTime("AssignmentDate")
            });
        }

        return members;
    }

    public async Task<bool> AssignmentExistsAsync(int projectId, int advisorId)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM projectadvisor
            WHERE ProjectId = @ProjectId AND AdvisorId = @AdvisorId;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@ProjectId", projectId);
        command.Parameters.AddWithValue("@AdvisorId", advisorId);
        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task<bool> RoleExistsAsync(int projectId, int advisorRoleId)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM projectadvisor
            WHERE ProjectId = @ProjectId AND AdvisorRole = @AdvisorRoleId;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@ProjectId", projectId);
        command.Parameters.AddWithValue("@AdvisorRoleId", advisorRoleId);
        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task AssignAdvisorAsync(ProjectAdvisorAssignmentModel model)
    {
        const string sql = """
            INSERT INTO projectadvisor (AdvisorId, ProjectId, AdvisorRole, AssignmentDate)
            VALUES (@AdvisorId, @ProjectId, @AdvisorRoleId, @AssignmentDate);
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@AdvisorId", model.AdvisorId);
        command.Parameters.AddWithValue("@ProjectId", model.ProjectId);
        command.Parameters.AddWithValue("@AdvisorRoleId", model.AdvisorRoleId);
        command.Parameters.AddWithValue("@AssignmentDate", model.AssignmentDate);
        await command.ExecuteNonQueryAsync();
    }

    public async Task RemoveAdvisorAsync(int projectId, int advisorId)
    {
        const string sql = """
            DELETE FROM projectadvisor
            WHERE ProjectId = @ProjectId AND AdvisorId = @AdvisorId;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@ProjectId", projectId);
        command.Parameters.AddWithValue("@AdvisorId", advisorId);
        await command.ExecuteNonQueryAsync();
    }
}
