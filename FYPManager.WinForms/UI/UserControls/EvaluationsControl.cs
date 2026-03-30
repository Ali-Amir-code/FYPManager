using FYPManager.WinForms.Models;
using FYPManager.WinForms.UI.Dialogs;
using FYPManager.WinForms.Utilities;

namespace FYPManager.WinForms.UI.UserControls;

public partial class EvaluationsControl : UserControl
{
    private BindingSource _evaluationsBindingSource = new();
    private BindingSource _groupEntriesBindingSource = new();

    public EvaluationsControl(AppServices services)
    {
        Services = services;
        InitializeComponent();
        ConfigureGrids();
    }

    private AppServices Services { get; }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadMarkingLookupsAsync();
        await LoadEvaluationsAsync();
    }

    private void ConfigureGrids()
    {
        dgvEvaluations.AutoGenerateColumns = false;
        dgvEvaluations.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(EvaluationListItem.Name), HeaderText = "Evaluation" });
        dgvEvaluations.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(EvaluationListItem.TotalMarks), HeaderText = "Total Marks" });
        dgvEvaluations.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(EvaluationListItem.TotalWeightage), HeaderText = "Weightage" });

        dgvGroupEntries.AutoGenerateColumns = false;
        dgvGroupEntries.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(GroupEvaluationEntryItem.EvaluationName), HeaderText = "Evaluation" });
        dgvGroupEntries.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(GroupEvaluationEntryItem.TotalMarks), HeaderText = "Total Marks" });
        dgvGroupEntries.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(GroupEvaluationEntryItem.ObtainedMarks), HeaderText = "Obtained" });
        dgvGroupEntries.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(GroupEvaluationEntryItem.EvaluationDate), HeaderText = "Date", DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" } });
    }

    private async Task LoadMarkingLookupsAsync()
    {
        OperationResult<IReadOnlyList<GroupListItem>> groupsResult = await Services.GroupBL.SearchGroupsAsync(null);
        cboGroups.DisplayMember = nameof(GroupListItem.Id);
        cboGroups.ValueMember = nameof(GroupListItem.Id);
        cboGroups.DataSource = groupsResult.Data?.ToList() ?? new List<GroupListItem>();

        OperationResult<IReadOnlyList<EvaluationListItem>> evaluationsResult = await Services.EvaluationBL.SearchAsync(null);
        cboEvaluations.DisplayMember = nameof(EvaluationListItem.Name);
        cboEvaluations.ValueMember = nameof(EvaluationListItem.Id);
        cboEvaluations.DataSource = evaluationsResult.Data?.ToList() ?? new List<EvaluationListItem>();
    }

    private async Task LoadEvaluationsAsync()
    {
        ToggleBusyState(true);
        OperationResult<IReadOnlyList<EvaluationListItem>> result = await Services.EvaluationBL.SearchAsync(txtSearch.Text);
        ToggleBusyState(false);

        if (!result.Succeeded || result.Data is null)
        {
            ShowBanner(result.Message, false, result.Errors);
            return;
        }

        _evaluationsBindingSource = new BindingSource { DataSource = result.Data.ToList() };
        dgvEvaluations.DataSource = _evaluationsBindingSource;
        lblRecordCount.Text = $"{_evaluationsBindingSource.Count} evaluations";
        ShowBanner(_evaluationsBindingSource.Count == 0 ? "No evaluations found for the current search." : "Evaluations loaded successfully.", true);

        await LoadGroupEntriesAsync();
        await LoadMarkingLookupsAsync();
    }

    private async Task LoadGroupEntriesAsync()
    {
        if (cboGroups.SelectedValue is not int groupId)
        {
            _groupEntriesBindingSource = new BindingSource { DataSource = Array.Empty<GroupEvaluationEntryItem>() };
            dgvGroupEntries.DataSource = _groupEntriesBindingSource;
            return;
        }

        OperationResult<IReadOnlyList<GroupEvaluationEntryItem>> result = await Services.EvaluationBL.GetGroupEvaluationsAsync(groupId);
        _groupEntriesBindingSource = new BindingSource { DataSource = result.Data?.ToList() ?? new List<GroupEvaluationEntryItem>() };
        dgvGroupEntries.DataSource = _groupEntriesBindingSource;
    }

    private void ToggleBusyState(bool isBusy)
    {
        btnSearch.Enabled = !isBusy;
        btnAdd.Enabled = !isBusy;
        btnEdit.Enabled = !isBusy;
        btnDelete.Enabled = !isBusy;
        btnRecordMarks.Enabled = !isBusy;
        dgvEvaluations.Enabled = !isBusy;
        dgvGroupEntries.Enabled = !isBusy;
        progressBar.Visible = isBusy;
    }

    private EvaluationListItem? GetSelectedEvaluation() => dgvEvaluations.CurrentRow?.DataBoundItem as EvaluationListItem;

    private void ShowBanner(string message, bool isSuccess, IEnumerable<string>? details = null)
    {
        lblStatus.Text = details is null || !details.Any()
            ? message
            : $"{message}{Environment.NewLine}{UiMessageHelper.JoinLines(details)}";
        lblStatus.ForeColor = isSuccess ? AppTheme.SuccessColor : AppTheme.DangerColor;
    }

    private async void btnSearch_Click(object sender, EventArgs e) => await LoadEvaluationsAsync();

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        using EvaluationDialog dialog = new();
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.EvaluationBL.CreateAsync(dialog.Model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadEvaluationsAsync();
        }
    }

    private async void btnEdit_Click(object sender, EventArgs e)
    {
        EvaluationListItem? selected = GetSelectedEvaluation();
        if (selected is null)
        {
            ShowBanner("Please select an evaluation first.", false);
            return;
        }

        OperationResult<EvaluationUpsertModel> loadResult = await Services.EvaluationBL.GetByIdAsync(selected.Id);
        if (!loadResult.Succeeded || loadResult.Data is null)
        {
            ShowBanner(loadResult.Message, false, loadResult.Errors);
            return;
        }

        using EvaluationDialog dialog = new(loadResult.Data);
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.EvaluationBL.UpdateAsync(dialog.Model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadEvaluationsAsync();
        }
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        EvaluationListItem? selected = GetSelectedEvaluation();
        if (selected is null)
        {
            ShowBanner("Please select an evaluation first.", false);
            return;
        }

        DialogResult confirmation = MessageBox.Show(
            this,
            $"Delete evaluation \"{selected.Name}\"?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirmation != DialogResult.Yes)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.EvaluationBL.DeleteAsync(selected.Id);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadEvaluationsAsync();
        }
    }

    private async void cboGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IsHandleCreated)
        {
            await LoadGroupEntriesAsync();
        }
    }

    private async void btnRecordMarks_Click(object sender, EventArgs e)
    {
        if (cboGroups.SelectedValue is not int groupId || cboEvaluations.SelectedValue is not int evaluationId)
        {
            ShowBanner("Please select both a group and an evaluation.", false);
            return;
        }

        GroupEvaluationUpsertModel model = new()
        {
            GroupId = groupId,
            EvaluationId = evaluationId,
            ObtainedMarks = (int)nudObtainedMarks.Value,
            EvaluationDate = dtpEvaluationDate.Value
        };

        ToggleBusyState(true);
        OperationResult result = await Services.EvaluationBL.AddGroupEvaluationAsync(model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadGroupEntriesAsync();
        }
    }
}
