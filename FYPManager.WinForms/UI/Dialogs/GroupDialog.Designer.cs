namespace FYPManager.WinForms.UI.Dialogs;

partial class GroupDialog
{
    private System.ComponentModel.IContainer components = null;
    private Label lblCreatedOn;
    private DateTimePicker dtpCreatedOn;
    private Button btnSave;
    private Button btnCancel;

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
        lblCreatedOn = new Label();
        dtpCreatedOn = new DateTimePicker();
        btnSave = new Button();
        btnCancel = new Button();
        SuspendLayout();
        lblCreatedOn.AutoSize = true;
        lblCreatedOn.Location = new Point(24, 24);
        lblCreatedOn.Text = "Created On";
        dtpCreatedOn.Format = DateTimePickerFormat.Short;
        dtpCreatedOn.Location = new Point(24, 47);
        dtpCreatedOn.Size = new Size(220, 23);
        btnSave.BackColor = AppTheme.SidebarAccentColor;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(169, 95);
        btnSave.Size = new Size(75, 32);
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Location = new Point(250, 95);
        btnCancel.Size = new Size(75, 32);
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(349, 145);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(dtpCreatedOn);
        Controls.Add(lblCreatedOn);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Group";
        ResumeLayout(false);
        PerformLayout();
    }
}
