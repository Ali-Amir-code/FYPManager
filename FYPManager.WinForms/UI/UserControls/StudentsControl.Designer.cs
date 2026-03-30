namespace FYPManager.WinForms.UI.UserControls;

partial class StudentsControl
{
    private System.ComponentModel.IContainer components = null;
    private Panel topPanel;
    private Label lblSectionInfo;
    private TextBox txtSearch;
    private Button btnSearch;
    private ComboBox cboGenderFilter;
    private Label lblFilter;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
    private DataGridView dgvStudents;
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
        lblFilter = new Label();
        cboGenderFilter = new ComboBox();
        btnSearch = new Button();
        txtSearch = new TextBox();
        lblSectionInfo = new Label();
        dgvStudents = new DataGridView();
        lblStatus = new Label();
        topPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
        SuspendLayout();
        topPanel.BackColor = AppTheme.CardBackColor;
        topPanel.BorderStyle = BorderStyle.FixedSingle;
        topPanel.Controls.Add(lblRecordCount);
        topPanel.Controls.Add(progressBar);
        topPanel.Controls.Add(btnDelete);
        topPanel.Controls.Add(btnEdit);
        topPanel.Controls.Add(btnAdd);
        topPanel.Controls.Add(lblFilter);
        topPanel.Controls.Add(cboGenderFilter);
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
        lblRecordCount.Text = "0 students";
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
        btnAdd.Text = "Add Student";
        btnAdd.UseVisualStyleBackColor = false;
        btnAdd.Click += btnAdd_Click;
        lblFilter.AutoSize = true;
        lblFilter.ForeColor = AppTheme.TextMuted;
        lblFilter.Location = new Point(301, 74);
        lblFilter.Text = "Gender";
        cboGenderFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cboGenderFilter.FormattingEnabled = true;
        cboGenderFilter.Location = new Point(352, 70);
        cboGenderFilter.Size = new Size(154, 23);
        cboGenderFilter.SelectedIndexChanged += cboGenderFilter_SelectedIndexChanged;
        btnSearch.FlatStyle = FlatStyle.Flat;
        btnSearch.Location = new Point(512, 69);
        btnSearch.Size = new Size(80, 26);
        btnSearch.Text = "Search";
        btnSearch.UseVisualStyleBackColor = true;
        btnSearch.Click += btnSearch_Click;
        txtSearch.Location = new Point(21, 70);
        txtSearch.PlaceholderText = "Search by registration number, name, or email";
        txtSearch.Size = new Size(274, 23);
        lblSectionInfo.Dock = DockStyle.Top;
        lblSectionInfo.Font = new Font("Segoe UI", 10F);
        lblSectionInfo.ForeColor = AppTheme.TextMuted;
        lblSectionInfo.Location = new Point(18, 18);
        lblSectionInfo.Size = new Size(922, 40);
        lblSectionInfo.Text = "Manage students and their linked person records with safe validation and searchable data access.";
        dgvStudents.AllowUserToAddRows = false;
        dgvStudents.AllowUserToDeleteRows = false;
        dgvStudents.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvStudents.BackgroundColor = AppTheme.CardBackColor;
        dgvStudents.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(226, 232, 240);
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle1.ForeColor = AppTheme.TextPrimary;
        dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(226, 232, 240);
        dataGridViewCellStyle1.SelectionForeColor = AppTheme.TextPrimary;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dgvStudents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvStudents.EnableHeadersVisualStyles = false;
        dgvStudents.Location = new Point(0, 159);
        dgvStudents.MultiSelect = false;
        dgvStudents.ReadOnly = true;
        dgvStudents.RowHeadersVisible = false;
        dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvStudents.Size = new Size(960, 364);
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
        Controls.Add(dgvStudents);
        Controls.Add(topPanel);
        Name = "StudentsControl";
        Size = new Size(960, 560);
        topPanel.ResumeLayout(false);
        topPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvStudents).EndInit();
        ResumeLayout(false);
    }
}
