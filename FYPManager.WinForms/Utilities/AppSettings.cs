using System.Text.Json;
using FYPManager.WinForms.Models;

namespace FYPManager.WinForms.Utilities;

public static class AppSettings
{
    public static DatabaseOptions LoadDatabaseOptions()
    {
        string filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        if (!File.Exists(filePath))
        {
            return new DatabaseOptions();
        }

        string json = File.ReadAllText(filePath);
        using JsonDocument document = JsonDocument.Parse(json);

        if (!document.RootElement.TryGetProperty("Database", out JsonElement databaseElement))
        {
            return new DatabaseOptions();
        }

        DatabaseOptions options = new();

        if (databaseElement.TryGetProperty("Server", out JsonElement server))
        {
            options.Server = server.GetString() ?? options.Server;
        }

        if (databaseElement.TryGetProperty("Port", out JsonElement port) && port.TryGetInt32(out int parsedPort))
        {
            options.Port = parsedPort;
        }

        if (databaseElement.TryGetProperty("Database", out JsonElement database))
        {
            options.Database = database.GetString() ?? options.Database;
        }

        if (databaseElement.TryGetProperty("UserId", out JsonElement userId))
        {
            options.UserId = userId.GetString() ?? options.UserId;
        }

        if (databaseElement.TryGetProperty("Password", out JsonElement password))
        {
            options.Password = password.GetString() ?? options.Password;
        }

        if (databaseElement.TryGetProperty("SslMode", out JsonElement sslMode))
        {
            options.SslMode = sslMode.GetString() ?? options.SslMode;
        }

        return options;
    }
}
