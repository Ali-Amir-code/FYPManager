using FYPManager.WinForms.DAL;
using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;
using MySqlConnector;

namespace FYPManager.WinForms.BL;

public sealed class EvaluationBL
{
    private readonly EvaluationDAL _evaluationDal;

    public EvaluationBL(EvaluationDAL evaluationDal)
    {
        _evaluationDal = evaluationDal;
    }

    public async Task<OperationResult<IReadOnlyList<EvaluationListItem>>> SearchAsync(string? searchTerm)
    {
        try
        {
            return OperationResult<IReadOnlyList<EvaluationListItem>>.Success(await _evaluationDal.SearchAsync(searchTerm));
        }
        catch (Exception ex)
        {
            return OperationResult<IReadOnlyList<EvaluationListItem>>.Failure("Unable to load evaluations.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult<EvaluationUpsertModel>> GetByIdAsync(int evaluationId)
    {
        try
        {
            EvaluationUpsertModel? evaluation = await _evaluationDal.GetByIdAsync(evaluationId);
            return evaluation is null
                ? OperationResult<EvaluationUpsertModel>.Failure("The selected evaluation could not be found.")
                : OperationResult<EvaluationUpsertModel>.Success(evaluation);
        }
        catch (Exception ex)
        {
            return OperationResult<EvaluationUpsertModel>.Failure("Unable to load the evaluation.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> CreateAsync(EvaluationUpsertModel model)
    {
        ValidationResult validation = ValidateEvaluation(model, false);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _evaluationDal.CreateAsync(model);
            return OperationResult.Success("Evaluation created successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to create the evaluation.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> UpdateAsync(EvaluationUpsertModel model)
    {
        ValidationResult validation = ValidateEvaluation(model, true);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _evaluationDal.UpdateAsync(model);
            return OperationResult.Success("Evaluation updated successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to update the evaluation.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> DeleteAsync(int evaluationId)
    {
        try
        {
            await _evaluationDal.DeleteAsync(evaluationId);
            return OperationResult.Success("Evaluation deleted successfully.");
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.RowIsReferenced2 || ex.ErrorCode == MySqlErrorCode.RowIsReferenced)
        {
            return OperationResult.Failure("This evaluation cannot be deleted because marks are already recorded against it.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to delete the evaluation.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult<IReadOnlyList<GroupEvaluationEntryItem>>> GetGroupEvaluationsAsync(int groupId)
    {
        try
        {
            return OperationResult<IReadOnlyList<GroupEvaluationEntryItem>>.Success(await _evaluationDal.GetGroupEvaluationsAsync(groupId));
        }
        catch (Exception ex)
        {
            return OperationResult<IReadOnlyList<GroupEvaluationEntryItem>>.Failure("Unable to load group evaluations.", new[] { ex.Message });
        }
    }

    public async Task<OperationResult> AddGroupEvaluationAsync(GroupEvaluationUpsertModel model)
    {
        ValidationResult validation = await ValidateGroupEvaluationAsync(model);
        if (!validation.IsValid)
        {
            return OperationResult.Failure("Please fix the validation errors.", validation.Errors);
        }

        try
        {
            await _evaluationDal.AddGroupEvaluationAsync(model);
            return OperationResult.Success("Marks recorded successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure("Unable to record marks.", new[] { ex.Message });
        }
    }

    private static ValidationResult ValidateEvaluation(EvaluationUpsertModel model, bool isUpdate)
    {
        ValidationResult result = new();

        if (!ValidationHelper.HasValue(model.Name))
        {
            result.AddError("Evaluation name is required.");
        }

        if (model.TotalMarks <= 0)
        {
            result.AddError("Total marks must be greater than zero.");
        }

        if (model.TotalWeightage <= 0)
        {
            result.AddError("Total weightage must be greater than zero.");
        }

        if (isUpdate && model.Id <= 0)
        {
            result.AddError("A valid evaluation must be selected before update.");
        }

        return result;
    }

    private async Task<ValidationResult> ValidateGroupEvaluationAsync(GroupEvaluationUpsertModel model)
    {
        ValidationResult result = new();

        if (model.GroupId <= 0)
        {
            result.AddError("Please select a group.");
        }

        if (model.EvaluationId <= 0)
        {
            result.AddError("Please select an evaluation.");
        }

        if (model.ObtainedMarks < 0)
        {
            result.AddError("Obtained marks cannot be negative.");
        }

        if (result.IsValid)
        {
            int totalMarks = await _evaluationDal.GetTotalMarksAsync(model.EvaluationId);
            if (model.ObtainedMarks > totalMarks)
            {
                result.AddError("Obtained marks cannot exceed total marks.");
            }

            if (await _evaluationDal.GroupEvaluationExistsAsync(model.GroupId, model.EvaluationId))
            {
                result.AddError("Marks for this evaluation are already recorded against the selected group.");
            }
        }

        return result;
    }
}
