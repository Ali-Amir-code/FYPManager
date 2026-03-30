namespace FYPManager.WinForms.Models;

public sealed class ProjectReportRow
{
    public string ProjectTitle { get; set; } = string.Empty;
    public int? GroupId { get; set; }
    public string? AdvisorName { get; set; }
    public string? AdvisorRoleValue { get; set; }
    public string? StudentName { get; set; }
    public string? RegistrationNo { get; set; }
}
