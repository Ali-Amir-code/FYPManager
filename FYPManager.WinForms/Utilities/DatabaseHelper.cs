using FYPManager.WinForms.Models;
using MySqlConnector;

namespace FYPManager.WinForms.Utilities;

public sealed class DatabaseHelper : IDatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(DatabaseOptions options)
    {
        MySqlConnectionStringBuilder builder = new()
        {
            Server = options.Server,
            Port = (uint)options.Port,
            Database = options.Database,
            UserID = options.UserId,
            Password = options.Password,
            SslMode = Enum.TryParse(options.SslMode, true, out MySqlSslMode mode)
                ? mode
                : MySqlSslMode.None,
            AllowUserVariables = true
        };

        _connectionString = builder.ConnectionString;
    }

    public MySqlConnection CreateConnection() => new(_connectionString);
}
