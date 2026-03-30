using MySqlConnector;

namespace FYPManager.WinForms.Utilities;

public interface IDatabaseHelper
{
    MySqlConnection CreateConnection();
}
