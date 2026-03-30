namespace FYPManager.WinForms.Models;

public sealed class ProjectBoardMemberItem
{
    public int ProjectId { get; set; }
    public int AdvisorId { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public string AdvisorName { get; set; } = string.Empty;
    public int AdvisorRoleId { get; set; }
    public string AdvisorRoleValue { get; set; } = string.Empty;
    public DateTime AssignmentDate { get; set; }
}
