namespace FYPManager.WinForms.Models;

public sealed class GroupEvaluationUpsertModel
{
    public int GroupId { get; set; }
    public int EvaluationId { get; set; }
    public int ObtainedMarks { get; set; }
    public DateTime EvaluationDate { get; set; }
}
