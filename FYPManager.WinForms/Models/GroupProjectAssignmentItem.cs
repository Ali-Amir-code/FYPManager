namespace FYPManager.WinForms.Models;

public sealed class GroupProjectAssignmentItem
{
    public int GroupId { get; set; }
    public int ProjectId { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public DateTime AssignmentDate { get; set; }
}
