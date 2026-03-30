using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.DAL;

public sealed class ProjectDAL
{
    private readonly IDatabaseHelper _databaseHelper;

    public ProjectDAL(IDatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    public async Task<bool> TitleExistsAsync(string title, int excludingProjectId = 0)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM project
            WHERE Title = @Title
              AND (@ExcludingProjectId = 0 OR Id <> @ExcludingProjectId);
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Title", title.Trim());
        command.Parameters.AddWithValue("@ExcludingProjectId", excludingProjectId);
        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task<int> CreateAsync(ProjectUpsertModel model)
    {
        const string sql = """
            INSERT INTO project (Title, Description)
            VALUES (@Title, @Description);
            SELECT LAST_INSERT_ID();
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        ApplyParameters(command, model);
        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task UpdateAsync(ProjectUpsertModel model)
    {
        const string sql = """
            UPDATE project
            SET Title = @Title,
                Description = @Description
            WHERE Id = @Id;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", model.Id);
        ApplyParameters(command, model);
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int projectId)
    {
        const string sql = "DELETE FROM project WHERE Id = @Id;";

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", projectId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<ProjectUpsertModel?> GetByIdAsync(int projectId)
    {
        const string sql = """
            SELECT Id, Title, Description
            FROM project
            WHERE Id = @Id;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", projectId);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        int descriptionOrdinal = reader.GetOrdinal("Description");

        return new ProjectUpsertModel
        {
            Id = reader.GetInt32("Id"),
            Title = reader.GetString("Title"),
            Description = reader.IsDBNull(descriptionOrdinal) ? null : reader.GetString(descriptionOrdinal)
        };
    }

    public async Task<IReadOnlyList<ProjectListItem>> SearchAsync(string? searchTerm)
    {
        const string sql = """
            SELECT Id, Title, Description
            FROM project
            WHERE @SearchTerm = ''
               OR Title LIKE CONCAT('%', @SearchTerm, '%')
               OR IFNULL(Description, '') LIKE CONCAT('%', @SearchTerm, '%')
            ORDER BY Title;
            """;

        List<ProjectListItem> projects = new();
        string normalizedSearch = searchTerm?.Trim() ?? string.Empty;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@SearchTerm", normalizedSearch);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            int descriptionOrdinal = reader.GetOrdinal("Description");
            projects.Add(new ProjectListItem
            {
                Id = reader.GetInt32("Id"),
                Title = reader.GetString("Title"),
                Description = reader.IsDBNull(descriptionOrdinal) ? null : reader.GetString(descriptionOrdinal)
            });
        }

        return projects;
    }

    private static void ApplyParameters(MySqlCommand command, ProjectUpsertModel model)
    {
        command.Parameters.AddWithValue("@Title", model.Title.Trim());
        command.Parameters.AddWithValue("@Description", string.IsNullOrWhiteSpace(model.Description) ? DBNull.Value : model.Description.Trim());
    }
}
