namespace FYPManager.WinForms.Models;

public sealed class ProjectAdvisorAssignmentModel
{
    public int ProjectId { get; set; }
    public int AdvisorId { get; set; }
    public int AdvisorRoleId { get; set; }
    public DateTime AssignmentDate { get; set; }
}
