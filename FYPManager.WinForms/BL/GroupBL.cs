using FYPManager.WinForms.DAL;
using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.BL;

public sealed class GroupBL
{
    private readonly GroupDAL _groupDal;
    private readonly LookupBL _lookupBl;

    public GroupBL(GroupDAL groupDal, LookupBL lookupBl)
    {
        _groupDal = groupDal;
        _lookupBl = lookupBl;
    }

    public async Task<OperationResult<IReadOnlyList<GroupListItem>>> SearchGroupsAsync(string? searchTerm)
    {
        try
        {
            return OperationResult<IReadOnlyList<GroupListItem>>.Success(await _groupDal.SearchGroupsAsync(searchTerm));
        }
        catch (Exception ex)
        {
            return OperationResult<IReadOnlyList<GroupListItem>>.Failure("Unable to load groups.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult<GroupUpsertModel>> GetGroupByIdAsync(int groupId)
    {
        try
        {
            GroupUpsertModel? group = await _groupDal.GetGroupByIdAsync(groupId);
            return group is null
                ? OperationResult<GroupUpsertModel>.Failure("The selected group could not be found.")
                : OperationResult<GroupUpsertModel>.Success(group);
        }
        catch (Exception ex)
        {
            return OperationResult<GroupUpsertModel>.Failure("Unable to load the group.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> CreateGroupAsync(GroupUpsertModel model)
    {
        ValidationResult validation = ValidateGroup(model, false);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _groupDal.CreateGroupAsync(model);
            return OperationResult.Success("Group created successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to create the group.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> UpdateGroupAsync(GroupUpsertModel model)
    {
        ValidationResult validation = ValidateGroup(model, true);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _groupDal.UpdateGroupAsync(model);
            return OperationResult.Success("Group updated successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to update the group.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> DeleteGroupAsync(int groupId)
    {
        try
        {
            await _groupDal.DeleteGroupAsync(groupId);
            return OperationResult.Success("Group deleted successfully.");
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.RowIsReferenced2 || ex.ErrorCode == MySqlErrorCode.RowIsReferenced)
        {
            return OperationResult.Failure("This group cannot be deleted because it is already referenced by membership, project, or evaluation records.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to delete the group.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult<IReadOnlyList<GroupMemberItem>>> GetMembersAsync(int groupId)
    {
        try
        {
            return OperationResult<IReadOnlyList<GroupMemberItem>>.Success(await _groupDal.GetMembersAsync(groupId));
        }
        catch (Exception ex)
        {
            return OperationResult<IReadOnlyList<GroupMemberItem>>.Failure("Unable to load group members.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> AddMemberAsync(GroupMemberUpsertModel model)
    {
        ValidationResult validation = await ValidateGroupMemberAsync(model);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _groupDal.AddMemberAsync(model);
            return OperationResult.Success("Student added to group successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to add the student to the group.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> RemoveMemberAsync(int groupId, int studentId)
    {
        try
        {
            await _groupDal.RemoveMemberAsync(groupId, studentId);
            return OperationResult.Success("Student removed from group successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to remove the student from the group.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult<GroupProjectAssignmentItem>> GetProjectAssignmentAsync(int groupId)
    {
        try
        {
            GroupProjectAssignmentItem? assignment = await _groupDal.GetProjectAssignmentAsync(groupId);
            return assignment is null
                ? OperationResult<GroupProjectAssignmentItem>.Failure("No project is currently assigned to this group.")
                : OperationResult<GroupProjectAssignmentItem>.Success(assignment);
        }
        catch (Exception ex)
        {
            return OperationResult<GroupProjectAssignmentItem>.Failure("Unable to load the project assignment.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> AssignProjectAsync(GroupProjectAssignmentModel model)
    {
        ValidationResult validation = await ValidateProjectAssignmentAsync(model);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _groupDal.AssignProjectAsync(model);
            return OperationResult.Success("Project assigned successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to assign the project.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> RemoveProjectAssignmentAsync(int groupId)
    {
        try
        {
            await _groupDal.RemoveProjectAssignmentAsync(groupId);
            return OperationResult.Success("Project assignment removed successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to remove the project assignment.", new[] { ex.Message });
        }
    }

    private static ValidationResult ValidateGroup(GroupUpsertModel model, bool isUpdate)
    {
        ValidationResult result = new();

        if (model.CreatedOn == default)
        {
            result.AddError("Created date is required.");
        }

        if (isUpdate && model.Id <= 0)
        {
            result.AddError("A valid group must be selected before update.");
        }

        return result;
    }

    private async Task<ValidationResult> ValidateGroupMemberAsync(GroupMemberUpsertModel model)
    {
        ValidationResult result = new();

        if (model.GroupId <= 0)
        {
            result.AddError("Please select a group first.");
        }

        if (model.StudentId <= 0)
        {
            result.AddError("Please select a student.");
        }

        if (model.StatusId <= 0)
        {
            result.AddError("Status is required.");
        }

        if (result.IsValid && await _groupDal.GroupMemberExistsAsync(model.GroupId, model.StudentId))
        {
            result.AddError("This student is already part of the selected group.");
        }

        if (result.IsValid)
        {
            IReadOnlyList<Lookup> statuses = await _lookupBl.GetStatusesAsync();
            Lookup? activeStatus = statuses.FirstOrDefault(x => x.Value.Equals("Active", StringComparison.OrdinalIgnoreCase));

            if (activeStatus is not null && model.StatusId == activeStatus.Id)
            {
                bool alreadyActive = await _groupDal.StudentHasActiveGroupAsync(model.StudentId, activeStatus.Id, model.GroupId);
                if (alreadyActive)
                {
                    result.AddError("This student already belongs to another active group.");
                }
            }
        }

        return result;
    }

    private async Task<ValidationResult> ValidateProjectAssignmentAsync(GroupProjectAssignmentModel model)
    {
        ValidationResult result = new();

        if (model.GroupId <= 0)
        {
            result.AddError("Please select a group first.");
        }

        if (model.ProjectId <= 0)
        {
            result.AddError("Please select a project.");
        }

        if (result.IsValid && await _groupDal.GroupProjectAssignmentExistsAsync(model.GroupId, model.ProjectId))
        {
            result.AddError("This project is already assigned to the selected group.");
        }

        return result;
    }
}
