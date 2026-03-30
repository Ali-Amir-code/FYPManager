namespace FYPManager.WinForms.Models;

public sealed class GroupMemberItem
{
    public int GroupId { get; set; }
    public int StudentId { get; set; }
    public string RegistrationNo { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string StatusValue { get; set; } = string.Empty;
}
