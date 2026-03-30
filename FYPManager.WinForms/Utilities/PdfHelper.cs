using FYPManager.WinForms.Models;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace FYPManager.WinForms.Utilities;

public sealed class PdfHelper
{
    public void GenerateProjectListReport(string filePath, IReadOnlyList<ProjectReportRow> rows)
    {
        using PdfWriter writer = new(filePath);
        using PdfDocument pdf = new(writer);
        using Document document = new(pdf);

        AddReportHeader(document, "Project List Report");

        if (rows.Count == 0)
        {
            document.Add(new Paragraph("No project data is available."));
            return;
        }

        Table table = CreateTable(new float[] { 3, 1, 2.5f, 2, 2.5f, 2 });
        AddHeaderRow(table, "Project", "Group", "Advisor", "Role", "Student", "Reg. No.");

        foreach (ProjectReportRow row in rows)
        {
            AddBodyCell(table, row.ProjectTitle);
            AddBodyCell(table, row.GroupId?.ToString() ?? "-");
            AddBodyCell(table, row.AdvisorName ?? "-");
            AddBodyCell(table, row.AdvisorRoleValue ?? "-");
            AddBodyCell(table, row.StudentName ?? "-");
            AddBodyCell(table, row.RegistrationNo ?? "-");
        }

        document.Add(table);
    }

    public void GenerateMarksSheetReport(string filePath, IReadOnlyList<MarksReportRow> rows)
    {
        using PdfWriter writer = new(filePath);
        using PdfDocument pdf = new(writer);
        using Document document = new(pdf);

        AddReportHeader(document, "Marks Sheet Report");

        if (rows.Count == 0)
        {
            document.Add(new Paragraph("No marks data is available."));
            return;
        }

        Table table = CreateTable(new float[] { 2.5f, 1, 2, 2.2f, 2.2f, 1.3f, 1.3f, 1.2f });
        AddHeaderRow(table, "Project", "Group", "Reg. No.", "Student", "Evaluation", "Total", "Obtained", "Weight");

        foreach (MarksReportRow row in rows)
        {
            AddBodyCell(table, row.ProjectTitle);
            AddBodyCell(table, row.GroupId?.ToString() ?? "-");
            AddBodyCell(table, string.IsNullOrWhiteSpace(row.RegistrationNo) ? "-" : row.RegistrationNo);
            AddBodyCell(table, string.IsNullOrWhiteSpace(row.StudentName) ? "-" : row.StudentName);
            AddBodyCell(table, row.EvaluationName);
            AddBodyCell(table, row.TotalMarks.ToString());
            AddBodyCell(table, row.ObtainedMarks.ToString());
            AddBodyCell(table, row.TotalWeightage.ToString());
        }

        document.Add(table);
    }

    private static void AddReportHeader(Document document, string title)
    {
        document.Add(new Paragraph(title)
            .SetFontSize(18)
            .SetBold()
            .SetMarginBottom(4));

        document.Add(new Paragraph($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm}")
            .SetFontSize(10)
            .SetFontColor(ColorConstants.DARK_GRAY)
            .SetMarginBottom(16));
    }

    private static Table CreateTable(float[] widths)
    {
        return new Table(UnitValue.CreatePercentArray(widths))
            .UseAllAvailableWidth();
    }

    private static void AddHeaderRow(Table table, params string[] headers)
    {
        foreach (string header in headers)
        {
            table.AddHeaderCell(new Cell()
                .Add(new Paragraph(header).SetBold())
                .SetBackgroundColor(new DeviceRgb(226, 232, 240))
                .SetBorder(new SolidBorder(new DeviceRgb(203, 213, 225), 1))
                .SetPadding(6));
        }
    }

    private static void AddBodyCell(Table table, string value)
    {
        table.AddCell(new Cell()
            .Add(new Paragraph(value))
            .SetBorder(new SolidBorder(new DeviceRgb(226, 232, 240), 1))
            .SetPadding(6));
    }
}
