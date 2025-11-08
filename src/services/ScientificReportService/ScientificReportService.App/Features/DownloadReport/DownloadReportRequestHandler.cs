using MassTransit;
using MediatR;
using ScientificReportService.App.Common;
using ScientificReportService.App.Domain.Errors;
using ScientificReportService.App.Domain.ValueTypes;
using Shared.Contracts.ReportEvents;
using Shared.ResultPattern;

namespace ScientificReportService.App.Features.DownloadReport;

internal sealed class DownloadReportRequestHandler : IRequestHandler<DownloadReportRequest, Result<FileData>>
{
    private readonly IFileStorage _fileStorage;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<DownloadReportRequestHandler> _logger;

    public DownloadReportRequestHandler(
        IFileStorage fileStorage, 
        IPublishEndpoint publishEndpoint,
        ILogger<DownloadReportRequestHandler> logger)
    {
        _fileStorage = fileStorage;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<Result<FileData>> Handle(DownloadReportRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Downloading file {id}", request.FileId);

        var getFileStreamResult = await _fileStorage.Download(request.FileId, cancellationToken);

        if (getFileStreamResult.IsFailure)
        {
            _logger.LogError("Cannot download object. Reason {reason}", getFileStreamResult.Error.Message);
            return Result<FileData>.Failure(new CloudStorageError(getFileStreamResult.Error.Message));
        }
        
        var getFileInfoResult = await _fileStorage.GetFileInfo(request.FileId, cancellationToken);

        if (getFileInfoResult.IsFailure)
        {
            _logger.LogError("Cannot get file information from cloud storage. Reason: {reason}", getFileInfoResult.Error.Message);
            return Result<FileData>.Failure(new CloudStorageError(getFileInfoResult.Error.Message));
        }

        await _publishEndpoint.Publish(new ReportDownloadedEvent(Guid.Parse(request.FileId), DateTime.UtcNow));

        return Result<FileData>.Success(
            new FileData(
                getFileStreamResult.Value,
                getFileInfoResult.Value.ContentType,
                getFileInfoResult.Value.FileName));
    }
}