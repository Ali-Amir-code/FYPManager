namespace FYPManager.WinForms.Models;

public sealed class GroupEvaluationEntryItem
{
    public int GroupId { get; set; }
    public int EvaluationId { get; set; }
    public string EvaluationName { get; set; } = string.Empty;
    public int TotalMarks { get; set; }
    public int ObtainedMarks { get; set; }
    public DateTime EvaluationDate { get; set; }
}
