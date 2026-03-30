namespace FYPManager.WinForms.Models;

public sealed class ProjectUpsertModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}
