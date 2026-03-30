using FYPManager.WinForms.BL;
using FYPManager.WinForms.DAL;
using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;

namespace FYPManager.WinForms.UI;

public sealed class AppServices
{
    public AppServices()
    {
        DatabaseOptions databaseOptions = AppSettings.LoadDatabaseOptions();
        IDatabaseHelper databaseHelper = new DatabaseHelper(databaseOptions);
        LookupDAL lookupDal = new(databaseHelper);
        StudentDAL studentDal = new(databaseHelper);
        AdvisorDAL advisorDal = new(databaseHelper);
        ProjectDAL projectDal = new(databaseHelper);
        ProjectAdvisorDAL projectAdvisorDal = new(databaseHelper);
        GroupDAL groupDal = new(databaseHelper);
        EvaluationDAL evaluationDal = new(databaseHelper);
        ReportDAL reportDal = new(databaseHelper);

        PdfHelper pdfHelper = new();

        LookupBL = new LookupBL(lookupDal);
        StudentBL = new StudentBL(studentDal);
        AdvisorBL = new AdvisorBL(advisorDal);
        ProjectBL = new ProjectBL(projectDal);
        ProjectAdvisorBL = new ProjectAdvisorBL(projectAdvisorDal);
        GroupBL = new GroupBL(groupDal, LookupBL);
        EvaluationBL = new EvaluationBL(evaluationDal);
        ReportBL = new ReportBL(reportDal, pdfHelper);
        PdfHelper = pdfHelper;
    }

    public LookupBL LookupBL { get; }
    public StudentBL StudentBL { get; }
    public AdvisorBL AdvisorBL { get; }
    public ProjectBL ProjectBL { get; }
    public ProjectAdvisorBL ProjectAdvisorBL { get; }
    public GroupBL GroupBL { get; }
    public EvaluationBL EvaluationBL { get; }
    public ReportBL ReportBL { get; }
    public PdfHelper PdfHelper { get; }
}
