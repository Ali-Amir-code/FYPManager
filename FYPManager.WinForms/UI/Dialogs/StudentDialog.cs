using FYPManager.WinForms.BL;
using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;

namespace FYPManager.WinForms.UI.Dialogs;

public partial class StudentDialog : Form
{
    private readonly LookupBL _lookupBl;

    public StudentDialog(LookupBL lookupBl, StudentUpsertModel? model = null)
    {
        _lookupBl = lookupBl;
        Model = model is null
            ? new StudentUpsertModel()
            : new StudentUpsertModel
            {
                Id = model.Id,
                RegistrationNo = model.RegistrationNo,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Contact = model.Contact,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                GenderId = model.GenderId
            };
        InitializeComponent();
    }

    public StudentUpsertModel Model { get; }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        IReadOnlyList<Lookup> genders = await _lookupBl.GetGendersAsync();
        List<Lookup> options = new() { new Lookup { Id = 0, Value = "Select gender", Category = "GENDER" } };
        options.AddRange(genders);
        cboGender.DisplayMember = nameof(Lookup.Value);
        cboGender.ValueMember = nameof(Lookup.Id);
        cboGender.DataSource = options;

        Text = Model.Id > 0 ? "Edit Student" : "Add Student";
        txtRegistrationNo.Text = Model.RegistrationNo;
        txtFirstName.Text = Model.FirstName;
        txtLastName.Text = Model.LastName;
        txtContact.Text = Model.Contact;
        txtEmail.Text = Model.Email;
        chkDateOfBirth.Checked = Model.DateOfBirth.HasValue;
        if (Model.DateOfBirth.HasValue)
        {
            dtpDateOfBirth.Value = Model.DateOfBirth.Value;
        }
        cboGender.SelectedValue = Model.GenderId ?? 0;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        Model.RegistrationNo = txtRegistrationNo.Text.Trim();
        Model.FirstName = txtFirstName.Text.Trim();
        Model.LastName = string.IsNullOrWhiteSpace(txtLastName.Text) ? null : txtLastName.Text.Trim();
        Model.Contact = string.IsNullOrWhiteSpace(txtContact.Text) ? null : txtContact.Text.Trim();
        Model.Email = txtEmail.Text.Trim();
        Model.DateOfBirth = chkDateOfBirth.Checked ? dtpDateOfBirth.Value.Date : null;
        Model.GenderId = cboGender.SelectedValue is int id && id > 0 ? id : null;

        List<string> errors = new();
        if (!ValidationHelper.HasValue(Model.FirstName))
        {
            errors.Add("First name is required.");
        }

        if (!ValidationHelper.HasValue(Model.Email))
        {
            errors.Add("Email is required.");
        }
        else if (!ValidationHelper.IsValidEmail(Model.Email))
        {
            errors.Add("Email format is invalid.");
        }

        if (!ValidationHelper.HasValue(Model.RegistrationNo))
        {
            errors.Add("Registration number is required.");
        }

        if (!ValidationHelper.IsValidContact(Model.Contact))
        {
            errors.Add("Contact number format is invalid.");
        }

        if (errors.Count > 0)
        {
            lblValidation.Text = UiMessageHelper.JoinLines(errors);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
