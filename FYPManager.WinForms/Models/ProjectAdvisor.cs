namespace FYPManager.WinForms.Models;

public sealed class ProjectAdvisor
{
    public int AdvisorId { get; set; }
    public int ProjectId { get; set; }
    public int AdvisorRole { get; set; }
    public DateTime AssignmentDate { get; set; }
}
