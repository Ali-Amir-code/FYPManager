namespace FYPManager.WinForms.Models;

public sealed class GroupMemberUpsertModel
{
    public int GroupId { get; set; }
    public int StudentId { get; set; }
    public int StatusId { get; set; }
    public DateTime AssignmentDate { get; set; }
}
