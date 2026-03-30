using FYPManager.WinForms.DAL;
using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.BL;

public sealed class AdvisorBL
{
    private readonly AdvisorDAL _advisorDal;

    public AdvisorBL(AdvisorDAL advisorDal)
    {
        _advisorDal = advisorDal;
    }

    public async Task<OperationResult<IReadOnlyList<AdvisorListItem>>> SearchAsync(string? searchTerm)
    {
        try
        {
            return OperationResult<IReadOnlyList<AdvisorListItem>>.Success(await _advisorDal.SearchAsync(searchTerm));
        }
        catch (Exception ex)
        {
            return OperationResult<IReadOnlyList<AdvisorListItem>>.Failure("Unable to load advisors.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult<AdvisorUpsertModel>> GetByIdAsync(int advisorId)
    {
        try
        {
            AdvisorUpsertModel? advisor = await _advisorDal.GetByIdAsync(advisorId);
            return advisor is null
                ? OperationResult<AdvisorUpsertModel>.Failure("The selected advisor could not be found.")
                : OperationResult<AdvisorUpsertModel>.Success(advisor);
        }
        catch (Exception ex)
        {
            return OperationResult<AdvisorUpsertModel>.Failure("Unable to load the advisor.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> CreateAsync(AdvisorUpsertModel model)
    {
        ValidationResult validation = Validate(model, false);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _advisorDal.CreateAsync(model);
            return OperationResult.Success("Advisor created successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to create the advisor.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> UpdateAsync(AdvisorUpsertModel model)
    {
        ValidationResult validation = Validate(model, true);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _advisorDal.UpdateAsync(model);
            return OperationResult.Success("Advisor updated successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to update the advisor.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> DeleteAsync(int advisorId)
    {
        try
        {
            await _advisorDal.DeleteAsync(advisorId);
            return OperationResult.Success("Advisor deleted successfully.");
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.RowIsReferenced2 || ex.ErrorCode == MySqlErrorCode.RowIsReferenced)
        {
            return OperationResult.Failure("This advisor cannot be deleted because it is already referenced by project assignment records.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to delete the advisor.", new[] { ex.Message });
        }
    }

    private static ValidationResult Validate(AdvisorUpsertModel model, bool isUpdate)
    {
        ValidationResult result = new();

        if (!ValidationHelper.HasValue(model.FirstName))
        {
            result.AddError("First name is required.");
        }

        if (!ValidationHelper.HasValue(model.Email))
        {
            result.AddError("Email is required.");
        }
        else if (!ValidationHelper.IsValidEmail(model.Email))
        {
            result.AddError("Email format is invalid.");
        }

        if (!ValidationHelper.IsValidContact(model.Contact))
        {
            result.AddError("Contact number format is invalid.");
        }

        if (model.DesignationId <= 0)
        {
            result.AddError("Designation is required.");
        }

        if (model.Salary.HasValue && model.Salary.Value < 0)
        {
            result.AddError("Salary cannot be negative.");
        }

        if (isUpdate && model.Id <= 0)
        {
            result.AddError("A valid advisor must be selected before update.");
        }

        return result;
    }
}
