namespace FYPManager.WinForms.Models;

public sealed class MarksReportRow
{
    public string ProjectTitle { get; set; } = string.Empty;
    public int? GroupId { get; set; }
    public string RegistrationNo { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string EvaluationName { get; set; } = string.Empty;
    public int TotalMarks { get; set; }
    public int ObtainedMarks { get; set; }
    public int TotalWeightage { get; set; }
}
