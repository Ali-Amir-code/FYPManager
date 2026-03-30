namespace FYPManager.WinForms.Models;

public sealed class Project
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string Title { get; set; } = string.Empty;
}
