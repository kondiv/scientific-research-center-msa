using Shared.ResultPattern.Errors;

namespace ScientificReportService.App.Domain.Errors;

internal sealed class CloudStorageError : Error
{
    public CloudStorageError(string message)
        : base(message)
    {
        ErrorCode = ErrorCode.CloudStorage;
    }
}