namespace FYPManager.WinForms.Models;

public sealed class DatabaseOptions
{
    public string Server { get; set; } = "localhost";
    public int Port { get; set; } = 3306;
    public string Database { get; set; } = "projectadb26";
    public string UserId { get; set; } = "root";
    public string Password { get; set; } = "password";
    public string SslMode { get; set; } = "None";
}
