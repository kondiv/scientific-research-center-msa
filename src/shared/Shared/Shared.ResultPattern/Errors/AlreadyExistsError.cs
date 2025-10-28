namespace Shared.ResultPattern.Errors;

public class AlreadyExistsError : Error
{
    public AlreadyExistsError(string message)
        : base(message)
    {
        ErrorCode = ErrorCode.AlreadyExists;
    }
}