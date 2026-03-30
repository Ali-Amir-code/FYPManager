using FYPManager.WinForms.DAL;
using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.BL;

public sealed class ProjectBL
{
    private readonly ProjectDAL _projectDal;

    public ProjectBL(ProjectDAL projectDal)
    {
        _projectDal = projectDal;
    }

    public async Task<OperationResult<IReadOnlyList<ProjectListItem>>> SearchAsync(string? searchTerm)
    {
        try
        {
            return OperationResult<IReadOnlyList<ProjectListItem>>.Success(await _projectDal.SearchAsync(searchTerm));
        }
        catch (Exception ex)
        {
            return OperationResult<IReadOnlyList<ProjectListItem>>.Failure("Unable to load projects.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult<ProjectUpsertModel>> GetByIdAsync(int projectId)
    {
        try
        {
            ProjectUpsertModel? project = await _projectDal.GetByIdAsync(projectId);
            return project is null
                ? OperationResult<ProjectUpsertModel>.Failure("The selected project could not be found.")
                : OperationResult<ProjectUpsertModel>.Success(project);
        }
        catch (Exception ex)
        {
            return OperationResult<ProjectUpsertModel>.Failure("Unable to load the project.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> CreateAsync(ProjectUpsertModel model)
    {
        ValidationResult validation = await ValidateAsync(model, false);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _projectDal.CreateAsync(model);
            return OperationResult.Success("Project created successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to create the project.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> UpdateAsync(ProjectUpsertModel model)
    {
        ValidationResult validation = await ValidateAsync(model, true);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _projectDal.UpdateAsync(model);
            return OperationResult.Success("Project updated successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to update the project.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> DeleteAsync(int projectId)
    {
        try
        {
            await _projectDal.DeleteAsync(projectId);
            return OperationResult.Success("Project deleted successfully.");
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.RowIsReferenced2 || ex.ErrorCode == MySqlErrorCode.RowIsReferenced)
        {
            return OperationResult.Failure("This project cannot be deleted because it is already referenced by group or advisor assignment records.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to delete the project.", new[] { ex.Message });
        }
    }

    private async Task<ValidationResult> ValidateAsync(ProjectUpsertModel model, bool isUpdate)
    {
        ValidationResult result = new();

        if (!ValidationHelper.HasValue(model.Title))
        {
            result.AddError("Title is required.");
        }
        else if (model.Title.Trim().Length > 50)
        {
            result.AddError("Title cannot be longer than 50 characters.");
        }

        if (isUpdate && model.Id <= 0)
        {
            result.AddError("A valid project must be selected before update.");
        }

        if (result.IsValid && await _projectDal.TitleExistsAsync(model.Title, isUpdate ? model.Id : 0))
        {
            result.AddError("Project title already exists.");
        }

        return result;
    }
}
