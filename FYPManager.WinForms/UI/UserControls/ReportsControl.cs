using System.Diagnostics;
using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;

namespace FYPManager.WinForms.UI.UserControls;

public partial class ReportsControl : UserControl
{
    public ReportsControl(AppServices services)
    {
        Services = services;
        InitializeComponent();
    }

    private AppServices Services { get; }

    private void ToggleBusyState(bool isBusy)
    {
        btnProjectReport.Enabled = !isBusy;
        btnMarksReport.Enabled = !isBusy;
    }

    private void ShowBanner(string message, bool isSuccess, IEnumerable<string>? details = null)
    {
        lblStatus.Text = details is null || !details.Any()
            ? message
            : $"{message}{Environment.NewLine}{UiMessageHelper.JoinLines(details)}";
        lblStatus.ForeColor = isSuccess ? AppTheme.SuccessColor : AppTheme.DangerColor;
    }

    private async void btnProjectReport_Click(object sender, EventArgs e)
    {
        using SaveFileDialog dialog = new()
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            FileName = $"project-list-{DateTime.Now:yyyyMMdd-HHmm}.pdf"
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.ReportBL.GenerateProjectListReportAsync(dialog.FileName);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            TryOpenFile(dialog.FileName);
        }
    }

    private async void btnMarksReport_Click(object sender, EventArgs e)
    {
        using SaveFileDialog dialog = new()
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            FileName = $"marks-sheet-{DateTime.Now:yyyyMMdd-HHmm}.pdf"
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.ReportBL.GenerateMarksSheetReportAsync(dialog.FileName);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            TryOpenFile(dialog.FileName);
        }
    }

    private void TryOpenFile(string filePath)
    {
        try
        {
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            ShowBanner("Report was generated, but the file could not be opened automatically.", false, new[] { ex.Message });
        }
    }
}
