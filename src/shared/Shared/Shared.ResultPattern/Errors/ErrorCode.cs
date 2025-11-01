namespace Shared.ResultPattern.Errors;

public enum ErrorCode
{
    SomeError,
    DbUpdate,
    NotFound,
    DbUpdateConcurrency,
    Conflict,
    AlreadyExists,
    AuthProblem,
    Validation,
    CloudStorage
}