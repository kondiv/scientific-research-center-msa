using MediatR;
using ScientificReportService.App.Common;
using ScientificReportService.App.Domain.Entities;
using ScientificReportService.App.Domain.Models;
using ScientificReportService.App.Infrastructure;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace ScientificReportService.App.Features.CreateReport;

internal sealed class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Result<UploadFileResult>>
{
    private readonly ScientificReportDbContext _context;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<CreateReportCommandHandler> _logger;

    public CreateReportCommandHandler(
        ScientificReportDbContext context,
        IFileStorage fileStorage,
        ILogger<CreateReportCommandHandler> logger)
    {
        _context = context;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<Result<UploadFileResult>> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating scientific report: {title} -- {fileName}", request.Title, request.File.FileName);

        var scientificReport = new ScientificReport(
            request.Title,
            request.Description,
            request.Author,
            request.Tags);

        var result = await _fileStorage.Upload(request.File, request.Author, cancellationToken);

        if (result.IsFailure)
        {
            return result;
        }
        
        scientificReport.SetObjectKey(result.Value.FileId);
        
        await _context.ScientificReports.AddAsync(scientificReport, cancellationToken);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            await _fileStorage.Delete(result.Value.FileId);
            return Result<UploadFileResult>.Failure(
                new DbUpdateError($"Cannot add report \nReason: {e.Message}"));
        }
    }
}