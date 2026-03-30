using FYPManager.WinForms.Models;
using FYPManager.WinForms.UI.Dialogs;
using FYPManager.WinForms.Utilities;

namespace FYPManager.WinForms.UI.UserControls;

public partial class ProjectsControl : UserControl
{
    private BindingSource _projectsBindingSource = new();
    private BindingSource _boardBindingSource = new();
    private List<AdvisorListItem> _allAdvisors = new();
    private List<Lookup> _allAdvisorRoles = new();

    public ProjectsControl(AppServices services)
    {
        Services = services;
        InitializeComponent();
        ConfigureGrid();
    }

    private AppServices Services { get; }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadBoardLookupsAsync();
        await LoadProjectsAsync();
    }

    private void ConfigureGrid()
    {
        dgvProjects.AutoGenerateColumns = false;
        dgvProjects.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ProjectListItem.Title), HeaderText = "Title" });
        dgvProjects.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ProjectListItem.Description), HeaderText = "Description" });

        dgvBoard.AutoGenerateColumns = false;
        dgvBoard.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ProjectBoardMemberItem.AdvisorName), HeaderText = "Advisor" });
        dgvBoard.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ProjectBoardMemberItem.AdvisorRoleValue), HeaderText = "Role" });
        dgvBoard.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(ProjectBoardMemberItem.AssignmentDate),
            HeaderText = "Assigned On",
            DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" }
        });
    }

    private async Task LoadBoardLookupsAsync()
    {
        OperationResult<IReadOnlyList<AdvisorListItem>> advisorResult = await Services.AdvisorBL.SearchAsync(null);
        _allAdvisors = advisorResult.Data?.ToList() ?? new List<AdvisorListItem>();

        IReadOnlyList<Lookup> roles = await Services.LookupBL.GetAdvisorRolesAsync();
        _allAdvisorRoles = roles.ToList();

        BindAdvisorOptions(_allAdvisors);
        BindRoleOptions(_allAdvisorRoles);

        if (!advisorResult.Succeeded)
        {
            ShowBanner(advisorResult.Message, false, advisorResult.Errors);
        }
    }

    private async Task LoadProjectsAsync()
    {
        ToggleBusyState(true);
        OperationResult<IReadOnlyList<ProjectListItem>> result = await Services.ProjectBL.SearchAsync(txtSearch.Text);
        ToggleBusyState(false);

        if (!result.Succeeded || result.Data is null)
        {
            ShowBanner(result.Message, false, result.Errors);
            return;
        }

        _projectsBindingSource = new BindingSource { DataSource = result.Data.ToList() };
        dgvProjects.DataSource = _projectsBindingSource;
        lblRecordCount.Text = $"{_projectsBindingSource.Count} projects";
        ShowBanner(_projectsBindingSource.Count == 0 ? "No projects found for the current search." : "Projects loaded successfully.", true);

        if (_projectsBindingSource.Count > 0)
        {
            dgvProjects.Rows[0].Selected = true;
            await LoadBoardMembersAsync();
        }
        else
        {
            _boardBindingSource = new BindingSource { DataSource = Array.Empty<ProjectBoardMemberItem>() };
            dgvBoard.DataSource = _boardBindingSource;
            lblSelectedProject.Text = "No project selected";
            RefreshAssignmentOptions();
        }
    }

    private async Task LoadBoardMembersAsync()
    {
        ProjectListItem? selected = GetSelectedProject();
        if (selected is null)
        {
            _boardBindingSource = new BindingSource { DataSource = Array.Empty<ProjectBoardMemberItem>() };
            dgvBoard.DataSource = _boardBindingSource;
            lblSelectedProject.Text = "No project selected";
            RefreshAssignmentOptions();
            return;
        }

        OperationResult<IReadOnlyList<ProjectBoardMemberItem>> result = await Services.ProjectAdvisorBL.GetBoardMembersAsync(selected.Id);
        _boardBindingSource = new BindingSource { DataSource = result.Data?.ToList() ?? new List<ProjectBoardMemberItem>() };
        dgvBoard.DataSource = _boardBindingSource;
        lblSelectedProject.Text = $"Advisory board for \"{selected.Title}\"";
        RefreshAssignmentOptions();
    }

    private void ToggleBusyState(bool isBusy)
    {
        btnSearch.Enabled = !isBusy;
        btnAdd.Enabled = !isBusy;
        btnEdit.Enabled = !isBusy;
        btnDelete.Enabled = !isBusy;
        btnAssignAdvisor.Enabled = !isBusy;
        btnRemoveAdvisor.Enabled = !isBusy;
        dgvProjects.Enabled = !isBusy;
        dgvBoard.Enabled = !isBusy;
        progressBar.Visible = isBusy;
    }

    private ProjectListItem? GetSelectedProject() => dgvProjects.CurrentRow?.DataBoundItem as ProjectListItem;
    private ProjectBoardMemberItem? GetSelectedBoardMember() => dgvBoard.CurrentRow?.DataBoundItem as ProjectBoardMemberItem;

    private void RefreshAssignmentOptions()
    {
        if (GetSelectedProject() is null)
        {
            BindAdvisorOptions(Array.Empty<AdvisorListItem>());
            BindRoleOptions(Array.Empty<Lookup>());
            return;
        }

        HashSet<int> assignedAdvisorIds = _boardBindingSource.List
            .Cast<ProjectBoardMemberItem>()
            .Select(x => x.AdvisorId)
            .ToHashSet();

        HashSet<int> assignedRoleIds = _boardBindingSource.List
            .Cast<ProjectBoardMemberItem>()
            .Select(x => x.AdvisorRoleId)
            .ToHashSet();

        List<AdvisorListItem> availableAdvisors = _allAdvisors
            .Where(x => !assignedAdvisorIds.Contains(x.Id))
            .ToList();

        List<Lookup> availableRoles = _allAdvisorRoles
            .Where(x => !assignedRoleIds.Contains(x.Id))
            .ToList();

        BindAdvisorOptions(availableAdvisors);
        BindRoleOptions(availableRoles);
    }

    private void BindAdvisorOptions(IEnumerable<AdvisorListItem> advisors)
    {
        cboAdvisors.DisplayMember = nameof(AdvisorListItem.FullName);
        cboAdvisors.ValueMember = nameof(AdvisorListItem.Id);
        cboAdvisors.DataSource = advisors.ToList();
    }

    private void BindRoleOptions(IEnumerable<Lookup> roles)
    {
        cboAdvisorRoles.DisplayMember = nameof(Lookup.Value);
        cboAdvisorRoles.ValueMember = nameof(Lookup.Id);
        cboAdvisorRoles.DataSource = roles.ToList();
    }

    private void ShowBanner(string message, bool isSuccess, IEnumerable<string>? details = null)
    {
        lblStatus.Text = details is null || !details.Any()
            ? message
            : $"{message}{Environment.NewLine}{UiMessageHelper.JoinLines(details)}";
        lblStatus.ForeColor = isSuccess ? AppTheme.SuccessColor : AppTheme.DangerColor;
    }

    private async void btnSearch_Click(object sender, EventArgs e) => await LoadProjectsAsync();

    private async void dgvProjects_SelectionChanged(object sender, EventArgs e)
    {
        if (IsHandleCreated)
        {
            await LoadBoardMembersAsync();
        }
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        using ProjectDialog dialog = new();
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.ProjectBL.CreateAsync(dialog.Model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadProjectsAsync();
        }
    }

    private async void btnEdit_Click(object sender, EventArgs e)
    {
        ProjectListItem? selected = GetSelectedProject();
        if (selected is null)
        {
            ShowBanner("Please select a project first.", false);
            return;
        }

        OperationResult<ProjectUpsertModel> loadResult = await Services.ProjectBL.GetByIdAsync(selected.Id);
        if (!loadResult.Succeeded || loadResult.Data is null)
        {
            ShowBanner(loadResult.Message, false, loadResult.Errors);
            return;
        }

        using ProjectDialog dialog = new(loadResult.Data);
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.ProjectBL.UpdateAsync(dialog.Model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadProjectsAsync();
        }
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        ProjectListItem? selected = GetSelectedProject();
        if (selected is null)
        {
            ShowBanner("Please select a project first.", false);
            return;
        }

        DialogResult confirmation = MessageBox.Show(
            this,
            $"Delete project \"{selected.Title}\"?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirmation != DialogResult.Yes)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.ProjectBL.DeleteAsync(selected.Id);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadProjectsAsync();
        }
    }

    private async void btnAssignAdvisor_Click(object sender, EventArgs e)
    {
        ProjectListItem? selectedProject = GetSelectedProject();
        if (selectedProject is null)
        {
            ShowBanner("Please select a project first.", false);
            return;
        }

        if (cboAdvisors.SelectedValue is not int advisorId || cboAdvisorRoles.SelectedValue is not int advisorRoleId)
        {
            ShowBanner("Please select both an advisor and a role.", false);
            return;
        }

        ProjectAdvisorAssignmentModel model = new()
        {
            ProjectId = selectedProject.Id,
            AdvisorId = advisorId,
            AdvisorRoleId = advisorRoleId,
            AssignmentDate = dtpAdvisorAssignmentDate.Value
        };

        ToggleBusyState(true);
        OperationResult result = await Services.ProjectAdvisorBL.AssignAdvisorAsync(model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadBoardMembersAsync();
        }
    }

    private async void btnRemoveAdvisor_Click(object sender, EventArgs e)
    {
        ProjectListItem? selectedProject = GetSelectedProject();
        ProjectBoardMemberItem? selectedBoardMember = GetSelectedBoardMember();
        if (selectedProject is null || selectedBoardMember is null)
        {
            ShowBanner("Please select an advisory board member first.", false);
            return;
        }

        DialogResult confirmation = MessageBox.Show(
            this,
            $"Remove {selectedBoardMember.AdvisorName} from \"{selectedProject.Title}\"?",
            "Confirm Remove",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirmation != DialogResult.Yes)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.ProjectAdvisorBL.RemoveAdvisorAsync(selectedProject.Id, selectedBoardMember.AdvisorId);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadBoardMembersAsync();
        }
    }
}
