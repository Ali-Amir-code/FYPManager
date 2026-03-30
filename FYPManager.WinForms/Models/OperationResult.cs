namespace FYPManager.WinForms.Models;

public class OperationResult
{
    public bool Succeeded { get; init; }
    public string Message { get; init; } = string.Empty;
    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();

    public static OperationResult Success(string message) => new()
    {
        Succeeded = true,
        Message = message
    };

    public static OperationResult Failure(string message, IEnumerable<string>? errors = null) => new()
    {
        Succeeded = false,
        Message = message,
        Errors = errors?.ToArray() ?? Array.Empty<string>()
    };
}
