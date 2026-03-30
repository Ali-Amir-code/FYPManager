using FYPManager.WinForms.DAL;
using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;

namespace FYPManager.WinForms.BL;

public sealed class ReportBL
{
    private readonly ReportDAL _reportDal;
    private readonly PdfHelper _pdfHelper;

    public ReportBL(ReportDAL reportDal, PdfHelper pdfHelper)
    {
        _reportDal = reportDal;
        _pdfHelper = pdfHelper;
    }

    public async Task<OperationResult> GenerateProjectListReportAsync(string filePath)
    {
        try
        {
            IReadOnlyList<ProjectReportRow> rows = await _reportDal.GetProjectReportRowsAsync();
            _pdfHelper.GenerateProjectListReport(filePath, rows);
            return OperationResult.Success("Project report generated successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to generate the project report.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> GenerateMarksSheetReportAsync(string filePath)
    {
        try
        {
            IReadOnlyList<MarksReportRow> rows = await _reportDal.GetMarksReportRowsAsync();
            _pdfHelper.GenerateMarksSheetReport(filePath, rows);
            return OperationResult.Success("Marks sheet report generated successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to generate the marks sheet report.", new[] { ex.Message });
        }
    }
}
