using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.DAL;

public sealed class EvaluationDAL
{
    private readonly IDatabaseHelper _databaseHelper;

    public EvaluationDAL(IDatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    public async Task<IReadOnlyList<EvaluationListItem>> SearchAsync(string? searchTerm)
    {
        const string sql = """
            SELECT Id, Name, TotalMarks, TotalWeightage
            FROM evaluation
            WHERE @SearchTerm = ''
               OR Name LIKE CONCAT('%', @SearchTerm, '%')
            ORDER BY Name;
            """;

        List<EvaluationListItem> evaluations = new();
        string normalizedSearch = searchTerm?.Trim() ?? string.Empty;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@SearchTerm", normalizedSearch);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            evaluations.Add(new EvaluationListItem
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name"),
                TotalMarks = reader.GetInt32("TotalMarks"),
                TotalWeightage = reader.GetInt32("TotalWeightage")
            });
        }

        return evaluations;
    }

    public async Task<EvaluationUpsertModel?> GetByIdAsync(int evaluationId)
    {
        const string sql = """
            SELECT Id, Name, TotalMarks, TotalWeightage
            FROM evaluation
            WHERE Id = @Id;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", evaluationId);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new EvaluationUpsertModel
        {
            Id = reader.GetInt32("Id"),
            Name = reader.GetString("Name"),
            TotalMarks = reader.GetInt32("TotalMarks"),
            TotalWeightage = reader.GetInt32("TotalWeightage")
        };
    }

    public async Task<int> CreateAsync(EvaluationUpsertModel model)
    {
        const string sql = """
            INSERT INTO evaluation (Name, TotalMarks, TotalWeightage)
            VALUES (@Name, @TotalMarks, @TotalWeightage);
            SELECT LAST_INSERT_ID();
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        ApplyEvaluationParameters(command, model);
        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task UpdateAsync(EvaluationUpsertModel model)
    {
        const string sql = """
            UPDATE evaluation
            SET Name = @Name,
                TotalMarks = @TotalMarks,
                TotalWeightage = @TotalWeightage
            WHERE Id = @Id;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", model.Id);
        ApplyEvaluationParameters(command, model);
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int evaluationId)
    {
        const string sql = "DELETE FROM evaluation WHERE Id = @Id;";

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", evaluationId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<int> GetTotalMarksAsync(int evaluationId)
    {
        const string sql = """
            SELECT TotalMarks
            FROM evaluation
            WHERE Id = @Id;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", evaluationId);
        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<bool> GroupEvaluationExistsAsync(int groupId, int evaluationId)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM groupevaluation
            WHERE GroupId = @GroupId AND EvaluationId = @EvaluationId;
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@GroupId", groupId);
        command.Parameters.AddWithValue("@EvaluationId", evaluationId);
        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task AddGroupEvaluationAsync(GroupEvaluationUpsertModel model)
    {
        const string sql = """
            INSERT INTO groupevaluation (GroupId, EvaluationId, ObtainedMarks, EvaluationDate)
            VALUES (@GroupId, @EvaluationId, @ObtainedMarks, @EvaluationDate);
            """;

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@GroupId", model.GroupId);
        command.Parameters.AddWithValue("@EvaluationId", model.EvaluationId);
        command.Parameters.AddWithValue("@ObtainedMarks", model.ObtainedMarks);
        command.Parameters.AddWithValue("@EvaluationDate", model.EvaluationDate);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<GroupEvaluationEntryItem>> GetGroupEvaluationsAsync(int groupId)
    {
        const string sql = """
            SELECT ge.GroupId,
                   ge.EvaluationId,
                   e.Name AS EvaluationName,
                   e.TotalMarks,
                   ge.ObtainedMarks,
                   ge.EvaluationDate
            FROM groupevaluation ge
            INNER JOIN evaluation e ON e.Id = ge.EvaluationId
            WHERE ge.GroupId = @GroupId
            ORDER BY ge.EvaluationDate DESC, e.Name;
            """;

        List<GroupEvaluationEntryItem> entries = new();

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@GroupId", groupId);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            entries.Add(new GroupEvaluationEntryItem
            {
                GroupId = reader.GetInt32("GroupId"),
                EvaluationId = reader.GetInt32("EvaluationId"),
                EvaluationName = reader.GetString("EvaluationName"),
                TotalMarks = reader.GetInt32("TotalMarks"),
                ObtainedMarks = reader.GetInt32("ObtainedMarks"),
                EvaluationDate = reader.GetDateTime("EvaluationDate")
            });
        }

        return entries;
    }

    private static void ApplyEvaluationParameters(MySqlCommand command, EvaluationUpsertModel model)
    {
        command.Parameters.AddWithValue("@Name", model.Name.Trim());
        command.Parameters.AddWithValue("@TotalMarks", model.TotalMarks);
        command.Parameters.AddWithValue("@TotalWeightage", model.TotalWeightage);
    }
}
