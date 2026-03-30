namespace FYPManager.WinForms.Models;

public sealed class GroupListItem
{
    public int Id { get; set; }
    public DateOnly CreatedOn { get; set; }
    public int MemberCount { get; set; }
}
