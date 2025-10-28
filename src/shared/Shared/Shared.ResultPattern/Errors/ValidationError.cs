namespace Shared.ResultPattern.Errors;

public class ValidationError : Error
{
    public ValidationError(string message)
        : base(message)
    {
        ErrorCode = ErrorCode.Validation;
    }
}