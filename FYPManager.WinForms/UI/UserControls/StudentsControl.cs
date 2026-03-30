using FYPManager.WinForms.Models;
using FYPManager.WinForms.UI.Dialogs;
using FYPManager.WinForms.Utilities;

namespace FYPManager.WinForms.UI.UserControls;

public partial class StudentsControl : UserControl
{
    private BindingSource _bindingSource = new();
    private bool _loaded;

    public StudentsControl(AppServices services)
    {
        Services = services;
        InitializeComponent();
        ConfigureGrid();
    }

    private AppServices Services { get; }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (_loaded)
        {
            return;
        }

        _loaded = true;
        await LoadGenderOptionsAsync();
        await LoadStudentsAsync();
    }

    private void ConfigureGrid()
    {
        dgvStudents.AutoGenerateColumns = false;
        dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(StudentListItem.RegistrationNo), HeaderText = "Registration No." });
        dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(StudentListItem.FullName), HeaderText = "Name" });
        dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(StudentListItem.Email), HeaderText = "Email" });
        dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(StudentListItem.Contact), HeaderText = "Contact" });
        dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(StudentListItem.GenderValue), HeaderText = "Gender" });
    }

    private async Task LoadGenderOptionsAsync()
    {
        try
        {
            IReadOnlyList<Lookup> genders = await Services.LookupBL.GetGendersAsync();
            List<Lookup> options = new() { new Lookup { Id = 0, Value = "All", Category = "GENDER" } };
            options.AddRange(genders);
            cboGenderFilter.DisplayMember = nameof(Lookup.Value);
            cboGenderFilter.ValueMember = nameof(Lookup.Id);
            cboGenderFilter.DataSource = options;
        }
        catch (Exception ex)
        {
            ShowBanner("Unable to load gender lookup values.", false, new[] { ex.Message });
        }
    }

    private async Task LoadStudentsAsync()
    {
        ToggleBusyState(true);
        OperationResult<IReadOnlyList<StudentListItem>> result = await Services.StudentBL.SearchAsync(txtSearch.Text);
        ToggleBusyState(false);

        if (!result.Succeeded || result.Data is null)
        {
            ShowBanner(result.Message, false, result.Errors);
            return;
        }

        IEnumerable<StudentListItem> students = result.Data;
        if (cboGenderFilter.SelectedItem is Lookup genderFilter && genderFilter.Id > 0)
        {
            students = students.Where(x => x.GenderId == genderFilter.Id);
        }

        _bindingSource = new BindingSource { DataSource = students.ToList() };
        dgvStudents.DataSource = _bindingSource;
        lblRecordCount.Text = $"{_bindingSource.Count} students";
        ShowBanner(_bindingSource.Count == 0 ? "No students found for the current filter." : "Students loaded successfully.", true);
    }

    private void ToggleBusyState(bool isBusy)
    {
        btnSearch.Enabled = !isBusy;
        btnAdd.Enabled = !isBusy;
        btnEdit.Enabled = !isBusy;
        btnDelete.Enabled = !isBusy;
        dgvStudents.Enabled = !isBusy;
        progressBar.Visible = isBusy;
    }

    private StudentListItem? GetSelectedStudent() => dgvStudents.CurrentRow?.DataBoundItem as StudentListItem;

    private void ShowBanner(string message, bool isSuccess, IEnumerable<string>? details = null)
    {
        lblStatus.Text = details is null || !details.Any()
            ? message
            : $"{message}{Environment.NewLine}{UiMessageHelper.JoinLines(details)}";
        lblStatus.ForeColor = isSuccess ? AppTheme.SuccessColor : AppTheme.DangerColor;
    }

    private async void btnSearch_Click(object sender, EventArgs e) => await LoadStudentsAsync();

    private async void cboGenderFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_loaded)
        {
            await LoadStudentsAsync();
        }
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        using StudentDialog dialog = new(Services.LookupBL);
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.StudentBL.CreateAsync(dialog.Model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadStudentsAsync();
        }
    }

    private async void btnEdit_Click(object sender, EventArgs e)
    {
        StudentListItem? selected = GetSelectedStudent();
        if (selected is null)
        {
            ShowBanner("Please select a student first.", false);
            return;
        }

        OperationResult<StudentUpsertModel> loadResult = await Services.StudentBL.GetByIdAsync(selected.Id);
        if (!loadResult.Succeeded || loadResult.Data is null)
        {
            ShowBanner(loadResult.Message, false, loadResult.Errors);
            return;
        }

        using StudentDialog dialog = new(Services.LookupBL, loadResult.Data);
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.StudentBL.UpdateAsync(dialog.Model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadStudentsAsync();
        }
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        StudentListItem? selected = GetSelectedStudent();
        if (selected is null)
        {
            ShowBanner("Please select a student first.", false);
            return;
        }

        DialogResult confirmation = MessageBox.Show(
            this,
            $"Delete student {selected.FullName} ({selected.RegistrationNo})?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirmation != DialogResult.Yes)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.StudentBL.DeleteAsync(selected.Id);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadStudentsAsync();
        }
    }
}
