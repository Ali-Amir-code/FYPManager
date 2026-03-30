namespace FYPManager.WinForms.Models;

public sealed class AdvisorListItem
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DesignationValue { get; set; } = string.Empty;
    public decimal? Salary { get; set; }
}
