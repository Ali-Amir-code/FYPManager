namespace FYPManager.WinForms.UI.UserControls;

partial class ReportsControl
{
    private System.ComponentModel.IContainer components = null;
    private Panel cardProjectReport;
    private Panel cardMarksReport;
    private Label lblProjectReportTitle;
    private Label lblProjectReportDesc;
    private Button btnProjectReport;
    private Label lblMarksReportTitle;
    private Label lblMarksReportDesc;
    private Button btnMarksReport;
    private Label lblStatus;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        cardProjectReport = new Panel();
        btnProjectReport = new Button();
        lblProjectReportDesc = new Label();
        lblProjectReportTitle = new Label();
        cardMarksReport = new Panel();
        btnMarksReport = new Button();
        lblMarksReportDesc = new Label();
        lblMarksReportTitle = new Label();
        lblStatus = new Label();
        cardProjectReport.SuspendLayout();
        cardMarksReport.SuspendLayout();
        SuspendLayout();
        cardProjectReport.BackColor = AppTheme.CardBackColor;
        cardProjectReport.BorderStyle = BorderStyle.FixedSingle;
        cardProjectReport.Controls.Add(btnProjectReport);
        cardProjectReport.Controls.Add(lblProjectReportDesc);
        cardProjectReport.Controls.Add(lblProjectReportTitle);
        cardProjectReport.Location = new Point(32, 32);
        cardProjectReport.Padding = new Padding(20);
        cardProjectReport.Size = new Size(420, 170);
        btnProjectReport.BackColor = AppTheme.SidebarAccentColor;
        btnProjectReport.FlatAppearance.BorderSize = 0;
        btnProjectReport.FlatStyle = FlatStyle.Flat;
        btnProjectReport.ForeColor = Color.White;
        btnProjectReport.Location = new Point(23, 117);
        btnProjectReport.Size = new Size(122, 32);
        btnProjectReport.Text = "Export PDF";
        btnProjectReport.UseVisualStyleBackColor = false;
        btnProjectReport.Click += btnProjectReport_Click;
        lblProjectReportDesc.ForeColor = AppTheme.TextMuted;
        lblProjectReportDesc.Location = new Point(23, 61);
        lblProjectReportDesc.Size = new Size(372, 44);
        lblProjectReportDesc.Text = "Generate a project-wise report with assigned groups, advisory board members, and student list.";
        lblProjectReportTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
        lblProjectReportTitle.ForeColor = AppTheme.TextPrimary;
        lblProjectReportTitle.Location = new Point(23, 19);
        lblProjectReportTitle.Size = new Size(299, 28);
        lblProjectReportTitle.Text = "Project List Report";
        cardMarksReport.BackColor = AppTheme.CardBackColor;
        cardMarksReport.BorderStyle = BorderStyle.FixedSingle;
        cardMarksReport.Controls.Add(btnMarksReport);
        cardMarksReport.Controls.Add(lblMarksReportDesc);
        cardMarksReport.Controls.Add(lblMarksReportTitle);
        cardMarksReport.Location = new Point(482, 32);
        cardMarksReport.Padding = new Padding(20);
        cardMarksReport.Size = new Size(420, 170);
        btnMarksReport.BackColor = AppTheme.SidebarAccentColor;
        btnMarksReport.FlatAppearance.BorderSize = 0;
        btnMarksReport.FlatStyle = FlatStyle.Flat;
        btnMarksReport.ForeColor = Color.White;
        btnMarksReport.Location = new Point(23, 117);
        btnMarksReport.Size = new Size(122, 32);
        btnMarksReport.Text = "Export PDF";
        btnMarksReport.UseVisualStyleBackColor = false;
        btnMarksReport.Click += btnMarksReport_Click;
        lblMarksReportDesc.ForeColor = AppTheme.TextMuted;
        lblMarksReportDesc.Location = new Point(23, 61);
        lblMarksReportDesc.Size = new Size(372, 44);
        lblMarksReportDesc.Text = "Generate the marks sheet showing project, group, student, evaluation, marks, and weightage.";
        lblMarksReportTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
        lblMarksReportTitle.ForeColor = AppTheme.TextPrimary;
        lblMarksReportTitle.Location = new Point(23, 19);
        lblMarksReportTitle.Size = new Size(299, 28);
        lblMarksReportTitle.Text = "Marks Sheet Report";
        lblStatus.Dock = DockStyle.Bottom;
        lblStatus.ForeColor = AppTheme.TextMuted;
        lblStatus.Location = new Point(0, 534);
        lblStatus.Padding = new Padding(4, 8, 4, 0);
        lblStatus.Size = new Size(960, 26);
        lblStatus.Text = "Ready";
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = AppTheme.ContentBackColor;
        Controls.Add(lblStatus);
        Controls.Add(cardMarksReport);
        Controls.Add(cardProjectReport);
        Name = "ReportsControl";
        Size = new Size(960, 560);
        cardProjectReport.ResumeLayout(false);
        cardMarksReport.ResumeLayout(false);
        ResumeLayout(false);
    }
}
