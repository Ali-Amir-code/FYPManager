namespace FYPManager.WinForms.Models;

public sealed class Evaluation
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TotalMarks { get; set; }
    public int TotalWeightage { get; set; }
}
