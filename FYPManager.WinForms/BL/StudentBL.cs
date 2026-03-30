using FYPManager.WinForms.DAL;
using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.BL;

public sealed class StudentBL
{
    private readonly StudentDAL _studentDal;

    public StudentBL(StudentDAL studentDal)
    {
        _studentDal = studentDal;
    }

    public async Task<OperationResult<IReadOnlyList<StudentListItem>>> SearchAsync(string? searchTerm)
    {
        try
        {
            return OperationResult<IReadOnlyList<StudentListItem>>.Success(await _studentDal.SearchAsync(searchTerm));
        }
        catch (Exception ex)
        {
            return OperationResult<IReadOnlyList<StudentListItem>>.Failure("Unable to load students.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult<IReadOnlyList<StudentListItem>>> GetUnassignedStudentsAsync()
    {
        try
        {
            return OperationResult<IReadOnlyList<StudentListItem>>.Success(await _studentDal.GetUnassignedStudentsAsync());
        }
        catch (Exception ex)
        {
            return OperationResult<IReadOnlyList<StudentListItem>>.Failure("Unable to load unassigned students.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult<StudentUpsertModel>> GetByIdAsync(int studentId)
    {
        try
        {
            StudentUpsertModel? student = await _studentDal.GetByIdAsync(studentId);
            return student is null
                ? OperationResult<StudentUpsertModel>.Failure("The selected student could not be found.")
                : OperationResult<StudentUpsertModel>.Success(student);
        }
        catch (Exception ex)
        {
            return OperationResult<StudentUpsertModel>.Failure("Unable to load the student.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> CreateAsync(StudentUpsertModel model)
    {
        ValidationResult validation = await ValidateAsync(model, false);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _studentDal.CreateAsync(model);
            return OperationResult.Success("Student created successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to create the student.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> UpdateAsync(StudentUpsertModel model)
    {
        ValidationResult validation = await ValidateAsync(model, true);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _studentDal.UpdateAsync(model);
            return OperationResult.Success("Student updated successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to update the student.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> DeleteAsync(int studentId)
    {
        try
        {
            await _studentDal.DeleteAsync(studentId);
            return OperationResult.Success("Student deleted successfully.");
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.RowIsReferenced2 || ex.ErrorCode == MySqlErrorCode.RowIsReferenced)
        {
            return OperationResult.Failure("This student cannot be deleted because it is already referenced by other records.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to delete the student.", new[] { ex.Message });
        }
    }

    private async Task<ValidationResult> ValidateAsync(StudentUpsertModel model, bool isUpdate)
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

        if (!ValidationHelper.HasValue(model.RegistrationNo))
        {
            result.AddError("Registration number is required.");
        }
        else if (!ValidationHelper.IsValidRegistrationNumber(model.RegistrationNo))
        {
            result.AddError("Registration number may only contain letters, numbers, and hyphens.");
        }

        if (!ValidationHelper.IsValidContact(model.Contact))
        {
            result.AddError("Contact number format is invalid.");
        }

        if (isUpdate && model.Id <= 0)
        {
            result.AddError("A valid student must be selected before update.");
        }

        if (result.IsValid && await _studentDal.RegistrationNumberExistsAsync(model.RegistrationNo, isUpdate ? model.Id : 0))
        {
            result.AddError("Registration number already exists.");
        }

        return result;
    }
}
