namespace FYPManager.WinForms.UI.Dialogs;

partial class EvaluationDialog
{
    private System.ComponentModel.IContainer components = null;
    private Label lblName;
    private TextBox txtName;
    private Label lblTotalMarks;
    private NumericUpDown nudTotalMarks;
    private Label lblTotalWeightage;
    private NumericUpDown nudTotalWeightage;
    private Button btnSave;
    private Button btnCancel;
    private Label lblValidation;

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
        lblName = new Label();
        txtName = new TextBox();
        lblTotalMarks = new Label();
        nudTotalMarks = new NumericUpDown();
        lblTotalWeightage = new Label();
        nudTotalWeightage = new NumericUpDown();
        btnSave = new Button();
        btnCancel = new Button();
        lblValidation = new Label();
        ((System.ComponentModel.ISupportInitialize)nudTotalMarks).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudTotalWeightage).BeginInit();
        SuspendLayout();
        lblName.AutoSize = true;
        lblName.Location = new Point(24, 22);
        lblName.Text = "Name *";
        txtName.Location = new Point(24, 40);
        txtName.Size = new Size(320, 23);
        lblTotalMarks.AutoSize = true;
        lblTotalMarks.Location = new Point(24, 78);
        lblTotalMarks.Text = "Total Marks *";
        nudTotalMarks.Location = new Point(24, 96);
        nudTotalMarks.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        nudTotalMarks.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudTotalMarks.Size = new Size(150, 23);
        nudTotalMarks.Value = new decimal(new int[] { 1, 0, 0, 0 });
        lblTotalWeightage.AutoSize = true;
        lblTotalWeightage.Location = new Point(194, 78);
        lblTotalWeightage.Text = "Total Weightage *";
        nudTotalWeightage.Location = new Point(194, 96);
        nudTotalWeightage.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        nudTotalWeightage.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudTotalWeightage.Size = new Size(150, 23);
        nudTotalWeightage.Value = new decimal(new int[] { 1, 0, 0, 0 });
        btnSave.BackColor = AppTheme.SidebarAccentColor;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(188, 154);
        btnSave.Size = new Size(75, 32);
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Location = new Point(269, 154);
        btnCancel.Size = new Size(75, 32);
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        lblValidation.ForeColor = AppTheme.DangerColor;
        lblValidation.Location = new Point(24, 136);
        lblValidation.Size = new Size(150, 52);
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(370, 205);
        Controls.Add(lblValidation);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(nudTotalWeightage);
        Controls.Add(lblTotalWeightage);
        Controls.Add(nudTotalMarks);
        Controls.Add(lblTotalMarks);
        Controls.Add(txtName);
        Controls.Add(lblName);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Evaluation";
        ((System.ComponentModel.ISupportInitialize)nudTotalMarks).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudTotalWeightage).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
