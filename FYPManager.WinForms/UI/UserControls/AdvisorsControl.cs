using FYPManager.WinForms.Models;
using FYPManager.WinForms.UI.Dialogs;
using FYPManager.WinForms.Utilities;

namespace FYPManager.WinForms.UI.UserControls;

public partial class AdvisorsControl : UserControl
{
    private BindingSource _bindingSource = new();

    public AdvisorsControl(AppServices services)
    {
        Services = services;
        InitializeComponent();
        ConfigureGrid();
    }

    private AppServices Services { get; }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadAdvisorsAsync();
    }

    private void ConfigureGrid()
    {
        dgvAdvisors.AutoGenerateColumns = false;
        dgvAdvisors.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AdvisorListItem.FullName), HeaderText = "Name" });
        dgvAdvisors.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AdvisorListItem.Email), HeaderText = "Email" });
        dgvAdvisors.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AdvisorListItem.DesignationValue), HeaderText = "Designation" });
        dgvAdvisors.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AdvisorListItem.Salary), HeaderText = "Salary", DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
    }

    private async Task LoadAdvisorsAsync()
    {
        ToggleBusyState(true);
        OperationResult<IReadOnlyList<AdvisorListItem>> result = await Services.AdvisorBL.SearchAsync(txtSearch.Text);
        ToggleBusyState(false);

        if (!result.Succeeded || result.Data is null)
        {
            ShowBanner(result.Message, false, result.Errors);
            return;
        }

        _bindingSource = new BindingSource { DataSource = result.Data.ToList() };
        dgvAdvisors.DataSource = _bindingSource;
        lblRecordCount.Text = $"{_bindingSource.Count} advisors";
        ShowBanner(_bindingSource.Count == 0 ? "No advisors found for the current search." : "Advisors loaded successfully.", true);
    }

    private void ToggleBusyState(bool isBusy)
    {
        btnSearch.Enabled = !isBusy;
        btnAdd.Enabled = !isBusy;
        btnEdit.Enabled = !isBusy;
        btnDelete.Enabled = !isBusy;
        dgvAdvisors.Enabled = !isBusy;
        progressBar.Visible = isBusy;
    }

    private AdvisorListItem? GetSelectedAdvisor() => dgvAdvisors.CurrentRow?.DataBoundItem as AdvisorListItem;

    private void ShowBanner(string message, bool isSuccess, IEnumerable<string>? details = null)
    {
        lblStatus.Text = details is null || !details.Any()
            ? message
            : $"{message}{Environment.NewLine}{UiMessageHelper.JoinLines(details)}";
        lblStatus.ForeColor = isSuccess ? AppTheme.SuccessColor : AppTheme.DangerColor;
    }

    private async void btnSearch_Click(object sender, EventArgs e) => await LoadAdvisorsAsync();

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        using AdvisorDialog dialog = new(Services.LookupBL);
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.AdvisorBL.CreateAsync(dialog.Model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadAdvisorsAsync();
        }
    }

    private async void btnEdit_Click(object sender, EventArgs e)
    {
        AdvisorListItem? selected = GetSelectedAdvisor();
        if (selected is null)
        {
            ShowBanner("Please select an advisor first.", false);
            return;
        }

        OperationResult<AdvisorUpsertModel> loadResult = await Services.AdvisorBL.GetByIdAsync(selected.Id);
        if (!loadResult.Succeeded || loadResult.Data is null)
        {
            ShowBanner(loadResult.Message, false, loadResult.Errors);
            return;
        }

        using AdvisorDialog dialog = new(Services.LookupBL, loadResult.Data);
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.AdvisorBL.UpdateAsync(dialog.Model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadAdvisorsAsync();
        }
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        AdvisorListItem? selected = GetSelectedAdvisor();
        if (selected is null)
        {
            ShowBanner("Please select an advisor first.", false);
            return;
        }

        DialogResult confirmation = MessageBox.Show(
            this,
            $"Delete advisor {selected.FullName}?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirmation != DialogResult.Yes)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.AdvisorBL.DeleteAsync(selected.Id);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadAdvisorsAsync();
        }
    }
}
