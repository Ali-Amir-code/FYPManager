namespace FYPManager.WinForms.UI.Dialogs;

partial class ProjectDialog
{
    private System.ComponentModel.IContainer components = null;
    private Label lblTitle;
    private TextBox txtTitle;
    private Label lblDescription;
    private TextBox txtDescription;
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
        lblTitle = new Label();
        txtTitle = new TextBox();
        lblDescription = new Label();
        txtDescription = new TextBox();
        btnSave = new Button();
        btnCancel = new Button();
        lblValidation = new Label();
        SuspendLayout();
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(24, 22);
        lblTitle.Text = "Title *";
        txtTitle.Location = new Point(24, 40);
        txtTitle.Size = new Size(486, 23);
        lblDescription.AutoSize = true;
        lblDescription.Location = new Point(24, 78);
        lblDescription.Text = "Description";
        txtDescription.Location = new Point(24, 96);
        txtDescription.Multiline = true;
        txtDescription.ScrollBars = ScrollBars.Vertical;
        txtDescription.Size = new Size(486, 137);
        btnSave.BackColor = AppTheme.SidebarAccentColor;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(354, 270);
        btnSave.Size = new Size(75, 32);
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Location = new Point(435, 270);
        btnCancel.Size = new Size(75, 32);
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        lblValidation.ForeColor = AppTheme.DangerColor;
        lblValidation.Location = new Point(24, 246);
        lblValidation.Size = new Size(302, 56);
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(534, 321);
        Controls.Add(lblValidation);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(txtDescription);
        Controls.Add(lblDescription);
        Controls.Add(txtTitle);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Project";
        ResumeLayout(false);
        PerformLayout();
    }
}
