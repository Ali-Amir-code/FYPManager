namespace FYPManager.WinForms.UI.UserControls;

partial class AdvisorsControl
{
    private System.ComponentModel.IContainer components = null;
    private Panel topPanel;
    private Label lblSectionInfo;
    private TextBox txtSearch;
    private Button btnSearch;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
    private DataGridView dgvAdvisors;
    private Label lblStatus;
    private ProgressBar progressBar;
    private Label lblRecordCount;

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
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        topPanel = new Panel();
        lblRecordCount = new Label();
        progressBar = new ProgressBar();
        btnDelete = new Button();
        btnEdit = new Button();
        btnAdd = new Button();
        btnSearch = new Button();
        txtSearch = new TextBox();
        lblSectionInfo = new Label();
        dgvAdvisors = new DataGridView();
        lblStatus = new Label();
        topPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAdvisors).BeginInit();
        SuspendLayout();
        topPanel.BackColor = AppTheme.CardBackColor;
        topPanel.BorderStyle = BorderStyle.FixedSingle;
        topPanel.Controls.Add(lblRecordCount);
        topPanel.Controls.Add(progressBar);
        topPanel.Controls.Add(btnDelete);
        topPanel.Controls.Add(btnEdit);
        topPanel.Controls.Add(btnAdd);
        topPanel.Controls.Add(btnSearch);
        topPanel.Controls.Add(txtSearch);
        topPanel.Controls.Add(lblSectionInfo);
        topPanel.Dock = DockStyle.Top;
        topPanel.Padding = new Padding(18);
        topPanel.Size = new Size(960, 140);
        lblRecordCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblRecordCount.ForeColor = AppTheme.TextMuted;
        lblRecordCount.Location = new Point(817, 21);
        lblRecordCount.Size = new Size(120, 23);
        lblRecordCount.Text = "0 advisors";
        lblRecordCount.TextAlign = ContentAlignment.MiddleRight;
        progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        progressBar.Location = new Point(826, 104);
        progressBar.MarqueeAnimationSpeed = 30;
        progressBar.Size = new Size(111, 10);
        progressBar.Style = ProgressBarStyle.Marquee;
        progressBar.Visible = false;
        btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.ForeColor = AppTheme.DangerColor;
        btnDelete.Location = new Point(846, 64);
        btnDelete.Size = new Size(91, 30);
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += btnDelete_Click;
        btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Location = new Point(749, 64);
        btnEdit.Size = new Size(91, 30);
        btnEdit.Text = "Edit";
        btnEdit.UseVisualStyleBackColor = true;
        btnEdit.Click += btnEdit_Click;
        btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAdd.BackColor = AppTheme.SidebarAccentColor;
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.ForeColor = Color.White;
        btnAdd.Location = new Point(652, 64);
        btnAdd.Size = new Size(91, 30);
        btnAdd.Text = "Add Advisor";
        btnAdd.UseVisualStyleBackColor = false;
        btnAdd.Click += btnAdd_Click;
        btnSearch.FlatStyle = FlatStyle.Flat;
        btnSearch.Location = new Point(301, 69);
        btnSearch.Size = new Size(80, 26);
        btnSearch.Text = "Search";
        btnSearch.UseVisualStyleBackColor = true;
        btnSearch.Click += btnSearch_Click;
        txtSearch.Location = new Point(21, 70);
        txtSearch.PlaceholderText = "Search by name, email, or designation";
        txtSearch.Size = new Size(274, 23);
        lblSectionInfo.Dock = DockStyle.Top;
        lblSectionInfo.Font = new Font("Segoe UI", 10F);
        lblSectionInfo.ForeColor = AppTheme.TextMuted;
        lblSectionInfo.Location = new Point(18, 18);
        lblSectionInfo.Size = new Size(922, 40);
        lblSectionInfo.Text = "Manage advisors with designation lookup values and salary validation using the shared person profile flow.";
        dgvAdvisors.AllowUserToAddRows = false;
        dgvAdvisors.AllowUserToDeleteRows = false;
        dgvAdvisors.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvAdvisors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvAdvisors.BackgroundColor = AppTheme.CardBackColor;
        dgvAdvisors.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(226, 232, 240);
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle1.ForeColor = AppTheme.TextPrimary;
        dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(226, 232, 240);
        dataGridViewCellStyle1.SelectionForeColor = AppTheme.TextPrimary;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dgvAdvisors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dgvAdvisors.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvAdvisors.EnableHeadersVisualStyles = false;
        dgvAdvisors.Location = new Point(0, 159);
        dgvAdvisors.MultiSelect = false;
        dgvAdvisors.ReadOnly = true;
        dgvAdvisors.RowHeadersVisible = false;
        dgvAdvisors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAdvisors.Size = new Size(960, 364);
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
        Controls.Add(dgvAdvisors);
        Controls.Add(topPanel);
        Name = "AdvisorsControl";
        Size = new Size(960, 560);
        topPanel.ResumeLayout(false);
        topPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAdvisors).EndInit();
        ResumeLayout(false);
    }
}
