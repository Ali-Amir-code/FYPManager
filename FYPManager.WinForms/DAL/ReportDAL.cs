using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.DAL;

public sealed class ReportDAL
{
    private readonly IDatabaseHelper _databaseHelper;

    public ReportDAL(IDatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    public async Task<IReadOnlyList<ProjectReportRow>> GetProjectReportRowsAsync()
    {
        const string sql = """
            SELECT p.Title AS ProjectTitle,
                   gp.GroupId,
                   CONCAT(pe.FirstName, IFNULL(CONCAT(' ', pe.LastName), '')) AS AdvisorName,
                   rl.Value AS AdvisorRoleValue,
                   CONCAT(ps.FirstName, IFNULL(CONCAT(' ', ps.LastName), '')) AS StudentName,
                   s.RegistrationNo
            FROM project p
            LEFT JOIN groupproject gp ON gp.ProjectId = p.Id
            LEFT JOIN projectadvisor pa ON pa.ProjectId = p.Id
            LEFT JOIN advisor a ON a.Id = pa.AdvisorId
            LEFT JOIN person pe ON pe.Id = a.Id
            LEFT JOIN lookup rl ON rl.Id = pa.AdvisorRole
            LEFT JOIN groupstudent gs ON gs.GroupId = gp.GroupId
            LEFT JOIN student s ON s.Id = gs.StudentId
            LEFT JOIN person ps ON ps.Id = s.Id
            ORDER BY p.Title, gp.GroupId, rl.Value, s.RegistrationNo;
            """;

        List<ProjectReportRow> rows = new();

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        await using MySqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            int groupIdOrdinal = reader.GetOrdinal("GroupId");
            int advisorNameOrdinal = reader.GetOrdinal("AdvisorName");
            int advisorRoleOrdinal = reader.GetOrdinal("AdvisorRoleValue");
            int studentNameOrdinal = reader.GetOrdinal("StudentName");
            int regNoOrdinal = reader.GetOrdinal("RegistrationNo");

            rows.Add(new ProjectReportRow
            {
                ProjectTitle = reader.GetString("ProjectTitle"),
                GroupId = reader.IsDBNull(groupIdOrdinal) ? null : reader.GetInt32(groupIdOrdinal),
                AdvisorName = reader.IsDBNull(advisorNameOrdinal) ? null : reader.GetString(advisorNameOrdinal).Trim(),
                AdvisorRoleValue = reader.IsDBNull(advisorRoleOrdinal) ? null : reader.GetString(advisorRoleOrdinal),
                StudentName = reader.IsDBNull(studentNameOrdinal) ? null : reader.GetString(studentNameOrdinal).Trim(),
                RegistrationNo = reader.IsDBNull(regNoOrdinal) ? null : reader.GetString(regNoOrdinal)
            });
        }

        return rows;
    }

    public async Task<IReadOnlyList<MarksReportRow>> GetMarksReportRowsAsync()
    {
        const string sql = """
            SELECT p.Title AS ProjectTitle,
                   gp.GroupId,
                   s.RegistrationNo,
                   CONCAT(ps.FirstName, IFNULL(CONCAT(' ', ps.LastName), '')) AS StudentName,
                   e.Name AS EvaluationName,
                   e.TotalMarks,
                   COALESCE(ge.ObtainedMarks, 0) AS ObtainedMarks,
                   e.TotalWeightage
            FROM evaluation e
            LEFT JOIN groupevaluation ge ON ge.EvaluationId = e.Id
            LEFT JOIN `group` g ON g.Id = ge.GroupId
            LEFT JOIN groupproject gp ON gp.GroupId = g.Id
            LEFT JOIN project p ON p.Id = gp.ProjectId
            LEFT JOIN groupstudent gs ON gs.GroupId = g.Id
            LEFT JOIN student s ON s.Id = gs.StudentId
            LEFT JOIN person ps ON ps.Id = s.Id
            ORDER BY p.Title, gp.GroupId, s.RegistrationNo, e.Name;
            """;

        List<MarksReportRow> rows = new();

        await using MySqlConnection connection = _databaseHelper.CreateConnection();
        await connection.OpenAsync();
        await using MySqlCommand command = new(sql, connection);
        await using MySqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            int projectTitleOrdinal = reader.GetOrdinal("ProjectTitle");
            int groupIdOrdinal = reader.GetOrdinal("GroupId");
            int regNoOrdinal = reader.GetOrdinal("RegistrationNo");
            int studentNameOrdinal = reader.GetOrdinal("StudentName");

            rows.Add(new MarksReportRow
            {
                ProjectTitle = reader.IsDBNull(projectTitleOrdinal) ? "Unassigned Project" : reader.GetString(projectTitleOrdinal),
                GroupId = reader.IsDBNull(groupIdOrdinal) ? null : reader.GetInt32(groupIdOrdinal),
                RegistrationNo = reader.IsDBNull(regNoOrdinal) ? string.Empty : reader.GetString(regNoOrdinal),
                StudentName = reader.IsDBNull(studentNameOrdinal) ? string.Empty : reader.GetString(studentNameOrdinal).Trim(),
                EvaluationName = reader.GetString("EvaluationName"),
                TotalMarks = reader.GetInt32("TotalMarks"),
                ObtainedMarks = reader.GetInt32("ObtainedMarks"),
                TotalWeightage = reader.GetInt32("TotalWeightage")
            });
        }

        return rows;
    }
}
