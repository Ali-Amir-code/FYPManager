using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.DAL;

public sealed class LookupDAL
{
    private readonly IDatabaseHelper _databaseHelper;

    public LookupDAL(IDatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    public async Task<IReadOnlyList<Lookup>> GetByCategoryAsync(string category)
    {
        const string sql = """
            SELECT Id, Value, Category
            FROM lookup
            WHERE Category = @Category
            ORDER BY Value;
            """;

        List<Lookup> lookups = new();

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Category", category);

        await using MySqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lookups.Add(new Lookup
            {
                Id = reader.GetInt32("Id"),
                Value = reader.GetString("Value"),
                Category = reader.GetString("Category")
            });
        }

        return lookups;
    }
}
