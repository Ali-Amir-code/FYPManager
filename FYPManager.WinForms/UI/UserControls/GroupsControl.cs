using FYPManager.WinForms.Models;
using FYPManager.WinForms.UI.Dialogs;
using FYPManager.WinForms.Utilities;

namespace FYPManager.WinForms.UI.UserControls;

public partial class GroupsControl : UserControl
{
    private BindingSource _groupsBindingSource = new();
    private BindingSource _membersBindingSource = new();

    public GroupsControl(AppServices services)
    {
        Services = services;
        InitializeComponent();
        ConfigureGrids();
    }

    private AppServices Services { get; }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadMemberFormLookupsAsync();
        await LoadProjectOptionsAsync();
        await LoadGroupsAsync();
    }

    private void ConfigureGrids()
    {
        dgvGroups.AutoGenerateColumns = false;
        dgvGroups.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(GroupListItem.Id), HeaderText = "Group ID" });
        dgvGroups.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(GroupListItem.CreatedOn), HeaderText = "Created On" });
        dgvGroups.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(GroupListItem.MemberCount), HeaderText = "Members" });

        dgvMembers.AutoGenerateColumns = false;
        dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(GroupMemberItem.RegistrationNo), HeaderText = "Registration No." });
        dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(GroupMemberItem.StudentName), HeaderText = "Student" });
        dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(GroupMemberItem.StatusValue), HeaderText = "Status" });
    }

    private async Task LoadMemberFormLookupsAsync()
    {
        OperationResult<IReadOnlyList<StudentListItem>> studentResult = await Services.StudentBL.GetUnassignedStudentsAsync();
        IReadOnlyList<StudentListItem> students = studentResult.Data ?? Array.Empty<StudentListItem>();

        List<StudentOptionItem> studentOptions = students
            .Select(x => new StudentOptionItem
            {
                Id = x.Id,
                DisplayText = $"{x.RegistrationNo} - {x.FullName}"
            })
            .ToList();

        cboStudents.DisplayMember = nameof(StudentOptionItem.DisplayText);
        cboStudents.ValueMember = nameof(StudentOptionItem.Id);
        cboStudents.DataSource = studentOptions;

        if (!studentResult.Succeeded)
        {
            ShowBanner(studentResult.Message, false, studentResult.Errors);
        }

        IReadOnlyList<Lookup> statuses = await Services.LookupBL.GetStatusesAsync();
        cboStatus.DisplayMember = nameof(Lookup.Value);
        cboStatus.ValueMember = nameof(Lookup.Id);
        cboStatus.DataSource = statuses.ToList();
    }

    private async Task LoadGroupsAsync()
    {
        ToggleBusyState(true);
        OperationResult<IReadOnlyList<GroupListItem>> result = await Services.GroupBL.SearchGroupsAsync(txtGroupSearch.Text);
        ToggleBusyState(false);

        if (!result.Succeeded || result.Data is null)
        {
            ShowBanner(result.Message, false, result.Errors);
            return;
        }

        _groupsBindingSource = new BindingSource { DataSource = result.Data.ToList() };
        dgvGroups.DataSource = _groupsBindingSource;
        lblGroupCount.Text = $"{_groupsBindingSource.Count} groups";
        ShowBanner(_groupsBindingSource.Count == 0 ? "No groups found for the current search." : "Groups loaded successfully.", true);

        if (_groupsBindingSource.Count > 0)
        {
            dgvGroups.Rows[0].Selected = true;
            await LoadMembersAsync();
            await LoadProjectAssignmentAsync();
        }
        else
        {
            _membersBindingSource = new BindingSource { DataSource = Array.Empty<GroupMemberItem>() };
            dgvMembers.DataSource = _membersBindingSource;
            lblSelectedGroup.Text = "No group selected";
            lblProjectAssignment.Text = "No project assigned";
        }
    }

    private async Task LoadMembersAsync()
    {
        GroupListItem? selectedGroup = GetSelectedGroup();
        if (selectedGroup is null)
        {
            _membersBindingSource = new BindingSource { DataSource = Array.Empty<GroupMemberItem>() };
            dgvMembers.DataSource = _membersBindingSource;
            lblSelectedGroup.Text = "No group selected";
            return;
        }

        OperationResult<IReadOnlyList<GroupMemberItem>> result = await Services.GroupBL.GetMembersAsync(selectedGroup.Id);
        if (!result.Succeeded || result.Data is null)
        {
            ShowBanner(result.Message, false, result.Errors);
            return;
        }

        _membersBindingSource = new BindingSource { DataSource = result.Data.ToList() };
        dgvMembers.DataSource = _membersBindingSource;
        lblSelectedGroup.Text = $"Group #{selectedGroup.Id} members";
    }

    private async Task LoadProjectOptionsAsync()
    {
        OperationResult<IReadOnlyList<ProjectListItem>> projectResult = await Services.ProjectBL.SearchAsync(null);
        IReadOnlyList<ProjectListItem> projects = projectResult.Data ?? Array.Empty<ProjectListItem>();

        cboProjects.DisplayMember = nameof(ProjectListItem.Title);
        cboProjects.ValueMember = nameof(ProjectListItem.Id);
        cboProjects.DataSource = projects.ToList();

        if (!projectResult.Succeeded)
        {
            ShowBanner(projectResult.Message, false, projectResult.Errors);
        }
    }

    private async Task LoadProjectAssignmentAsync()
    {
        GroupListItem? selectedGroup = GetSelectedGroup();
        if (selectedGroup is null)
        {
            lblProjectAssignment.Text = "No project assigned";
            return;
        }

        OperationResult<GroupProjectAssignmentItem> result = await Services.GroupBL.GetProjectAssignmentAsync(selectedGroup.Id);
        if (!result.Succeeded || result.Data is null)
        {
            lblProjectAssignment.Text = "No project assigned";
            return;
        }

        lblProjectAssignment.Text = $"{result.Data.ProjectTitle} ({result.Data.AssignmentDate:yyyy-MM-dd})";
        cboProjects.SelectedValue = result.Data.ProjectId;
        dtpProjectAssignmentDate.Value = result.Data.AssignmentDate;
    }

    private void ToggleBusyState(bool isBusy)
    {
        btnSearchGroups.Enabled = !isBusy;
        btnCreateGroup.Enabled = !isBusy;
        btnEditGroup.Enabled = !isBusy;
        btnDeleteGroup.Enabled = !isBusy;
        btnAddMember.Enabled = !isBusy;
        btnRemoveMember.Enabled = !isBusy;
        progressBar.Visible = isBusy;
    }

    private GroupListItem? GetSelectedGroup() => dgvGroups.CurrentRow?.DataBoundItem as GroupListItem;
    private GroupMemberItem? GetSelectedMember() => dgvMembers.CurrentRow?.DataBoundItem as GroupMemberItem;

    private void ShowBanner(string message, bool isSuccess, IEnumerable<string>? details = null)
    {
        lblStatus.Text = details is null || !details.Any()
            ? message
            : $"{message}{Environment.NewLine}{UiMessageHelper.JoinLines(details)}";
        lblStatus.ForeColor = isSuccess ? AppTheme.SuccessColor : AppTheme.DangerColor;
    }

    private async void btnSearchGroups_Click(object sender, EventArgs e) => await LoadGroupsAsync();

    private async void dgvGroups_SelectionChanged(object sender, EventArgs e)
    {
        if (IsHandleCreated)
        {
            await LoadMembersAsync();
            await LoadProjectAssignmentAsync();
        }
    }

    private async void btnCreateGroup_Click(object sender, EventArgs e)
    {
        using GroupDialog dialog = new();
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.GroupBL.CreateGroupAsync(dialog.Model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadMemberFormLookupsAsync();
            await LoadGroupsAsync();
        }
    }

    private async void btnEditGroup_Click(object sender, EventArgs e)
    {
        GroupListItem? selectedGroup = GetSelectedGroup();
        if (selectedGroup is null)
        {
            ShowBanner("Please select a group first.", false);
            return;
        }

        OperationResult<GroupUpsertModel> loadResult = await Services.GroupBL.GetGroupByIdAsync(selectedGroup.Id);
        if (!loadResult.Succeeded || loadResult.Data is null)
        {
            ShowBanner(loadResult.Message, false, loadResult.Errors);
            return;
        }

        using GroupDialog dialog = new(loadResult.Data);
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.GroupBL.UpdateGroupAsync(dialog.Model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadGroupsAsync();
        }
    }

    private async void btnDeleteGroup_Click(object sender, EventArgs e)
    {
        GroupListItem? selectedGroup = GetSelectedGroup();
        if (selectedGroup is null)
        {
            ShowBanner("Please select a group first.", false);
            return;
        }

        DialogResult confirmation = MessageBox.Show(
            this,
            $"Delete group #{selectedGroup.Id}?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirmation != DialogResult.Yes)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.GroupBL.DeleteGroupAsync(selectedGroup.Id);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadGroupsAsync();
        }
    }

    private async void btnAddMember_Click(object sender, EventArgs e)
    {
        GroupListItem? selectedGroup = GetSelectedGroup();
        if (selectedGroup is null)
        {
            ShowBanner("Please select a group first.", false);
            return;
        }

        if (cboStudents.SelectedValue is not int studentId || cboStatus.SelectedValue is not int statusId)
        {
            ShowBanner("Please select both a student and a status.", false);
            return;
        }

        GroupMemberUpsertModel model = new()
        {
            GroupId = selectedGroup.Id,
            StudentId = studentId,
            StatusId = statusId,
            AssignmentDate = dtpAssignmentDate.Value
        };

        ToggleBusyState(true);
        OperationResult result = await Services.GroupBL.AddMemberAsync(model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadMemberFormLookupsAsync();
            await LoadGroupsAsync();
        }
    }

    private async void btnRemoveMember_Click(object sender, EventArgs e)
    {
        GroupListItem? selectedGroup = GetSelectedGroup();
        GroupMemberItem? selectedMember = GetSelectedMember();
        if (selectedGroup is null || selectedMember is null)
        {
            ShowBanner("Please select a group member first.", false);
            return;
        }

        DialogResult confirmation = MessageBox.Show(
            this,
            $"Remove {selectedMember.StudentName} from group #{selectedGroup.Id}?",
            "Confirm Remove",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirmation != DialogResult.Yes)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.GroupBL.RemoveMemberAsync(selectedGroup.Id, selectedMember.StudentId);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadMemberFormLookupsAsync();
            await LoadGroupsAsync();
        }
    }

    private async void btnAssignProject_Click(object sender, EventArgs e)
    {
        GroupListItem? selectedGroup = GetSelectedGroup();
        if (selectedGroup is null)
        {
            ShowBanner("Please select a group first.", false);
            return;
        }

        if (cboProjects.SelectedValue is not int projectId)
        {
            ShowBanner("Please select a project.", false);
            return;
        }

        GroupProjectAssignmentModel model = new()
        {
            GroupId = selectedGroup.Id,
            ProjectId = projectId,
            AssignmentDate = dtpProjectAssignmentDate.Value
        };

        ToggleBusyState(true);
        OperationResult result = await Services.GroupBL.AssignProjectAsync(model);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadGroupsAsync();
        }
    }

    private async void btnRemoveProject_Click(object sender, EventArgs e)
    {
        GroupListItem? selectedGroup = GetSelectedGroup();
        if (selectedGroup is null)
        {
            ShowBanner("Please select a group first.", false);
            return;
        }

        DialogResult confirmation = MessageBox.Show(
            this,
            $"Remove the assigned project from group #{selectedGroup.Id}?",
            "Confirm Remove",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirmation != DialogResult.Yes)
        {
            return;
        }

        ToggleBusyState(true);
        OperationResult result = await Services.GroupBL.RemoveProjectAssignmentAsync(selectedGroup.Id);
        ToggleBusyState(false);
        ShowBanner(result.Message, result.Succeeded, result.Errors);
        if (result.Succeeded)
        {
            await LoadGroupsAsync();
        }
    }
}
