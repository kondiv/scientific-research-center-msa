namespace Shared.ResultPattern.Errors;

public class ConflictError : Error
{
    public ConflictError(string message)
        : base(message)
    {
        ErrorCode = ErrorCode.Conflict;
    }
}