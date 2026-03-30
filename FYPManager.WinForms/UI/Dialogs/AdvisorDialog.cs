using FYPManager.WinForms.BL;
using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;

namespace FYPManager.WinForms.UI.Dialogs;

public partial class AdvisorDialog : Form
{
    private readonly LookupBL _lookupBl;

    public AdvisorDialog(LookupBL lookupBl, AdvisorUpsertModel? model = null)
    {
        _lookupBl = lookupBl;
        Model = model is null
            ? new AdvisorUpsertModel()
            : new AdvisorUpsertModel
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Contact = model.Contact,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                GenderId = model.GenderId,
                DesignationId = model.DesignationId,
                Salary = model.Salary
            };
        InitializeComponent();
    }

    public AdvisorUpsertModel Model { get; }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        IReadOnlyList<Lookup> genders = await _lookupBl.GetGendersAsync();
        List<Lookup> genderOptions = new() { new Lookup { Id = 0, Value = "Select gender", Category = "GENDER" } };
        genderOptions.AddRange(genders);
        cboGender.DisplayMember = nameof(Lookup.Value);
        cboGender.ValueMember = nameof(Lookup.Id);
        cboGender.DataSource = genderOptions;

        IReadOnlyList<Lookup> designations = await _lookupBl.GetDesignationsAsync();
        List<Lookup> designationOptions = new() { new Lookup { Id = 0, Value = "Select designation", Category = "DESIGNATION" } };
        designationOptions.AddRange(designations);
        cboDesignation.DisplayMember = nameof(Lookup.Value);
        cboDesignation.ValueMember = nameof(Lookup.Id);
        cboDesignation.DataSource = designationOptions;

        Text = Model.Id > 0 ? "Edit Advisor" : "Add Advisor";
        txtFirstName.Text = Model.FirstName;
        txtLastName.Text = Model.LastName;
        txtContact.Text = Model.Contact;
        txtEmail.Text = Model.Email;
        txtSalary.Text = Model.Salary?.ToString("0.##") ?? string.Empty;
        chkDateOfBirth.Checked = Model.DateOfBirth.HasValue;
        if (Model.DateOfBirth.HasValue)
        {
            dtpDateOfBirth.Value = Model.DateOfBirth.Value;
        }

        cboGender.SelectedValue = Model.GenderId ?? 0;
        cboDesignation.SelectedValue = Model.DesignationId;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        Model.FirstName = txtFirstName.Text.Trim();
        Model.LastName = string.IsNullOrWhiteSpace(txtLastName.Text) ? null : txtLastName.Text.Trim();
        Model.Contact = string.IsNullOrWhiteSpace(txtContact.Text) ? null : txtContact.Text.Trim();
        Model.Email = txtEmail.Text.Trim();
        Model.DateOfBirth = chkDateOfBirth.Checked ? dtpDateOfBirth.Value.Date : null;
        Model.GenderId = cboGender.SelectedValue is int genderId && genderId > 0 ? genderId : null;
        Model.DesignationId = cboDesignation.SelectedValue is int designationId ? designationId : 0;
        Model.Salary = decimal.TryParse(txtSalary.Text.Trim(), out decimal salary) ? salary : null;

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

        if (!ValidationHelper.IsValidContact(Model.Contact))
        {
            errors.Add("Contact number format is invalid.");
        }

        if (Model.DesignationId <= 0)
        {
            errors.Add("Designation is required.");
        }

        if (ValidationHelper.HasValue(txtSalary.Text) && !decimal.TryParse(txtSalary.Text.Trim(), out _))
        {
            errors.Add("Salary must be a valid number.");
        }

        if (Model.Salary.HasValue && Model.Salary.Value < 0)
        {
            errors.Add("Salary cannot be negative.");
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
