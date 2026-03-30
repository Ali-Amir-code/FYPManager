using FYPManager.WinForms.DAL;
using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.BL;

public sealed class ProjectAdvisorBL
{
    private readonly ProjectAdvisorDAL _projectAdvisorDal;

    public ProjectAdvisorBL(ProjectAdvisorDAL projectAdvisorDal)
    {
        _projectAdvisorDal = projectAdvisorDal;
    }

    public async Task<OperationResult<IReadOnlyList<ProjectBoardMemberItem>>> GetBoardMembersAsync(int projectId)
    {
        try
        {
            return OperationResult<IReadOnlyList<ProjectBoardMemberItem>>.Success(await _projectAdvisorDal.GetBoardMembersAsync(projectId));
        }
        catch (Exception ex)
        {
            return OperationResult<IReadOnlyList<ProjectBoardMemberItem>>.Failure("Unable to load advisory board members.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> AssignAdvisorAsync(ProjectAdvisorAssignmentModel model)
    {
        ValidationResult validation = await ValidateAssignmentAsync(model);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _projectAdvisorDal.AssignAdvisorAsync(model);
            return OperationResult.Success("Advisor assigned successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to assign the advisor.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> RemoveAdvisorAsync(int projectId, int advisorId)
    {
        try
        {
            await _projectAdvisorDal.RemoveAdvisorAsync(projectId, advisorId);
            return OperationResult.Success("Advisor removed successfully.");
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.RowIsReferenced2 || ex.ErrorCode == MySqlErrorCode.RowIsReferenced)
        {
            return OperationResult.Failure("This advisor assignment cannot be removed because it is referenced elsewhere.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to remove the advisor.", new[] { ex.Message });
        }
    }

    private async Task<ValidationResult> ValidateAssignmentAsync(ProjectAdvisorAssignmentModel model)
    {
        ValidationResult result = new();

        if (model.ProjectId <= 0)
        {
            result.AddError("Please select a project first.");
        }

        if (model.AdvisorId <= 0)
        {
            result.AddError("Please select an advisor.");
        }

        if (model.AdvisorRoleId <= 0)
        {
            result.AddError("Please select an advisor role.");
        }

        if (result.IsValid && await _projectAdvisorDal.AssignmentExistsAsync(model.ProjectId, model.AdvisorId))
        {
            result.AddError("This advisor is already assigned to the selected project.");
        }

        if (result.IsValid && await _projectAdvisorDal.RoleExistsAsync(model.ProjectId, model.AdvisorRoleId))
        {
            result.AddError("This advisor role is already assigned on the selected project.");
        }

        return result;
    }
}
