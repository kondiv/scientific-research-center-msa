using MediatR;
using Microsoft.EntityFrameworkCore;
using ScientificReportService.App.Common;
using ScientificReportService.App.Infrastructure;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace ScientificReportService.App.Features.DeleteReport;

internal sealed class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, Result>
{
    private readonly ScientificReportDbContext _context;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<DeleteReportCommandHandler> _logger;

    public DeleteReportCommandHandler(
        IFileStorage fileStorage,
        ILogger<DeleteReportCommandHandler> logger,
        ScientificReportDbContext context)
    {
        _fileStorage = fileStorage;
        _logger = logger;
        _context = context;
    }

    public async Task<Result> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting report. Cloud storage file id: {id}", request.ReportId);

        if (!Guid.TryParse(request.ReportId, out var reportGuidId))
        {
            _logger.LogError("Invalid format id provided");
            return Result.Failure(new ValidationError("InvalidReportId"));
        }

        var report = await _context
            .ScientificReports
            .FindAsync([reportGuidId], cancellationToken);

        if (report is null)
        {
            _logger.LogError("Cannot find report in database, report storage file id: {id}", request.ReportId);
            return Result.Failure(new NotFoundError("Cannot find report"));
        }
        
        var result = await _fileStorage.Delete(report.StorageFileId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError(result.InnerException, "Error while deleting report.");
            return Result.Failure(result.Error);
        }

        _context.Remove(report);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}