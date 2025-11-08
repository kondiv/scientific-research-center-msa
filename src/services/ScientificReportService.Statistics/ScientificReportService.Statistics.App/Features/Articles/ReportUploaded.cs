using MassTransit;
using ScientificReportService.Statistics.App.Domain.Entities;
using ScientificReportService.Statistics.App.Domain.Enums;
using ScientificReportService.Statistics.App.Infrastructure;
using Shared.Contracts.ReportEvents;

namespace ScientificReportService.Statistics.App.Features.Articles;

internal sealed class ReportUploaded : IConsumer<ReportCreatedEvent>
{
    private readonly ReportsStatisticsContext _dbContext;

    private readonly ILogger<ReportUploaded> _logger;

    public ReportUploaded(ReportsStatisticsContext dbContext, ILogger<ReportUploaded> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ReportCreatedEvent> context)
    {
        _logger.LogInformation("From MQ recieved event report created. Creating record");
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var report = new Report(
                id: context.Message.Id,
                title: context.Message.Title,
                createdOnUtc: context.Message.CreatedAtUtc);

            await _dbContext.Reports.AddAsync(report);

            var reportEvent = new ReportEvent(
                reportId: report.Id,
                date: DateTime.UtcNow,
                ActionType.Create);

            await _dbContext.ReportEvents.AddAsync(reportEvent);

            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Something went wrong, rolling back changes");
            await transaction.RollbackAsync();
            throw;
        }
    }
}
