namespace FYPManager.WinForms.UI.Dialogs;

partial class AdvisorDialog
{
    private System.ComponentModel.IContainer components = null;
    private Label lblFirstName;
    private TextBox txtFirstName;
    private Label lblLastName;
    private TextBox txtLastName;
    private Label lblContact;
    private TextBox txtContact;
    private Label lblEmail;
    private TextBox txtEmail;
    private CheckBox chkDateOfBirth;
    private DateTimePicker dtpDateOfBirth;
    private Label lblGender;
    private ComboBox cboGender;
    private Label lblDesignation;
    private ComboBox cboDesignation;
    private Label lblSalary;
    private TextBox txtSalary;
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
        lblFirstName = new Label();
        txtFirstName = new TextBox();
        lblLastName = new Label();
        txtLastName = new TextBox();
        lblContact = new Label();
        txtContact = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        chkDateOfBirth = new CheckBox();
        dtpDateOfBirth = new DateTimePicker();
        lblGender = new Label();
        cboGender = new ComboBox();
        lblDesignation = new Label();
        cboDesignation = new ComboBox();
        lblSalary = new Label();
        txtSalary = new TextBox();
        btnSave = new Button();
        btnCancel = new Button();
        lblValidation = new Label();
        SuspendLayout();
        lblFirstName.AutoSize = true;
        lblFirstName.Location = new Point(24, 22);
        lblFirstName.Text = "First Name *";
        txtFirstName.Location = new Point(24, 40);
        txtFirstName.Size = new Size(228, 23);
        lblLastName.AutoSize = true;
        lblLastName.Location = new Point(24, 78);
        lblLastName.Text = "Last Name";
        txtLastName.Location = new Point(24, 96);
        txtLastName.Size = new Size(228, 23);
        lblContact.AutoSize = true;
        lblContact.Location = new Point(279, 22);
        lblContact.Text = "Contact";
        txtContact.Location = new Point(279, 40);
        txtContact.Size = new Size(228, 23);
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(279, 78);
        lblEmail.Text = "Email *";
        txtEmail.Location = new Point(279, 96);
        txtEmail.Size = new Size(228, 23);
        chkDateOfBirth.AutoSize = true;
        chkDateOfBirth.Location = new Point(24, 136);
        chkDateOfBirth.Text = "Date of Birth";
        chkDateOfBirth.UseVisualStyleBackColor = true;
        dtpDateOfBirth.Format = DateTimePickerFormat.Short;
        dtpDateOfBirth.Location = new Point(24, 158);
        dtpDateOfBirth.Size = new Size(228, 23);
        lblGender.AutoSize = true;
        lblGender.Location = new Point(279, 136);
        lblGender.Text = "Gender";
        cboGender.DropDownStyle = ComboBoxStyle.DropDownList;
        cboGender.FormattingEnabled = true;
        cboGender.Location = new Point(279, 158);
        cboGender.Size = new Size(228, 23);
        lblDesignation.AutoSize = true;
        lblDesignation.Location = new Point(24, 197);
        lblDesignation.Text = "Designation *";
        cboDesignation.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDesignation.FormattingEnabled = true;
        cboDesignation.Location = new Point(24, 215);
        cboDesignation.Size = new Size(228, 23);
        lblSalary.AutoSize = true;
        lblSalary.Location = new Point(279, 197);
        lblSalary.Text = "Salary";
        txtSalary.Location = new Point(279, 215);
        txtSalary.Size = new Size(228, 23);
        btnSave.BackColor = AppTheme.SidebarAccentColor;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(351, 280);
        btnSave.Size = new Size(75, 32);
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Location = new Point(432, 280);
        btnCancel.Size = new Size(75, 32);
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        lblValidation.ForeColor = AppTheme.DangerColor;
        lblValidation.Location = new Point(24, 258);
        lblValidation.Size = new Size(306, 66);
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(534, 336);
        Controls.Add(lblValidation);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(txtSalary);
        Controls.Add(lblSalary);
        Controls.Add(cboDesignation);
        Controls.Add(lblDesignation);
        Controls.Add(cboGender);
        Controls.Add(lblGender);
        Controls.Add(dtpDateOfBirth);
        Controls.Add(chkDateOfBirth);
        Controls.Add(txtEmail);
        Controls.Add(lblEmail);
        Controls.Add(txtContact);
        Controls.Add(lblContact);
        Controls.Add(txtLastName);
        Controls.Add(lblLastName);
        Controls.Add(txtFirstName);
        Controls.Add(lblFirstName);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Advisor";
        ResumeLayout(false);
        PerformLayout();
    }
}
