namespace FYPManager.WinForms.UI.Dialogs;

partial class StudentDialog
{
    private System.ComponentModel.IContainer components = null;
    private Label lblRegistrationNo;
    private TextBox txtRegistrationNo;
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
        lblRegistrationNo = new Label();
        txtRegistrationNo = new TextBox();
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
        btnSave = new Button();
        btnCancel = new Button();
        lblValidation = new Label();
        SuspendLayout();
        lblRegistrationNo.AutoSize = true;
        lblRegistrationNo.Location = new Point(24, 23);
        lblRegistrationNo.Size = new Size(107, 15);
        lblRegistrationNo.Text = "Registration No. *";
        txtRegistrationNo.Location = new Point(24, 41);
        txtRegistrationNo.Size = new Size(228, 23);
        lblFirstName.AutoSize = true;
        lblFirstName.Location = new Point(24, 79);
        lblFirstName.Size = new Size(74, 15);
        lblFirstName.Text = "First Name *";
        txtFirstName.Location = new Point(24, 97);
        txtFirstName.Size = new Size(228, 23);
        lblLastName.AutoSize = true;
        lblLastName.Location = new Point(24, 135);
        lblLastName.Size = new Size(64, 15);
        lblLastName.Text = "Last Name";
        txtLastName.Location = new Point(24, 153);
        txtLastName.Size = new Size(228, 23);
        lblContact.AutoSize = true;
        lblContact.Location = new Point(279, 23);
        lblContact.Size = new Size(49, 15);
        lblContact.Text = "Contact";
        txtContact.Location = new Point(279, 41);
        txtContact.Size = new Size(228, 23);
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(279, 79);
        lblEmail.Size = new Size(48, 15);
        lblEmail.Text = "Email *";
        txtEmail.Location = new Point(279, 97);
        txtEmail.Size = new Size(228, 23);
        chkDateOfBirth.AutoSize = true;
        chkDateOfBirth.Location = new Point(279, 136);
        chkDateOfBirth.Size = new Size(93, 19);
        chkDateOfBirth.Text = "Date of Birth";
        chkDateOfBirth.UseVisualStyleBackColor = true;
        dtpDateOfBirth.Format = DateTimePickerFormat.Short;
        dtpDateOfBirth.Location = new Point(279, 154);
        dtpDateOfBirth.Size = new Size(228, 23);
        lblGender.AutoSize = true;
        lblGender.Location = new Point(24, 191);
        lblGender.Size = new Size(45, 15);
        lblGender.Text = "Gender";
        cboGender.DropDownStyle = ComboBoxStyle.DropDownList;
        cboGender.FormattingEnabled = true;
        cboGender.Location = new Point(24, 209);
        cboGender.Size = new Size(228, 23);
        btnSave.BackColor = AppTheme.SidebarAccentColor;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(351, 262);
        btnSave.Size = new Size(75, 32);
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Location = new Point(432, 262);
        btnCancel.Size = new Size(75, 32);
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        lblValidation.ForeColor = AppTheme.DangerColor;
        lblValidation.Location = new Point(24, 252);
        lblValidation.Size = new Size(306, 56);
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(534, 321);
        Controls.Add(lblValidation);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
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
        Controls.Add(txtRegistrationNo);
        Controls.Add(lblRegistrationNo);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Student";
        ResumeLayout(false);
        PerformLayout();
    }
}
