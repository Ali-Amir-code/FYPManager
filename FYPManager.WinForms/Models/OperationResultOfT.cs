namespace FYPManager.WinForms.Models;

public sealed class OperationResult<T> : OperationResult
{
    public T? Data { get; init; }

    public static OperationResult<T> Success(T data, string message = "") => new()
    {
        Succeeded = true,
        Data = data,
        Message = message
    };

    public static new OperationResult<T> Failure(string message, IEnumerable<string>? errors = null) => new()
    {
        Succeeded = false,
        Message = message,
        Errors = errors?.ToArray() ?? Array.Empty<string>()
    };
}
