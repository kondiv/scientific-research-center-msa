using MediatR;
using Microsoft.EntityFrameworkCore;
using ScientificReportService.App.Domain.Entities;
using ScientificReportService.App.Infrastructure;

namespace ScientificReportService.App.Features.ListReports;

internal sealed class ListReportsRequestHandler : IRequestHandler<ListReportsRequest, List<ScientificReport>>
{
    private readonly ScientificReportDbContext _dbContext;
    private readonly ILogger<ListReportsRequestHandler> _logger;

    public ListReportsRequestHandler(ScientificReportDbContext dbContext, ILogger<ListReportsRequestHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<ScientificReport>> Handle(ListReportsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("List reports. Page: {page}. Max page size: {size}", request.Page, request.MaxPageSize);

        var reports = await _dbContext
            .ScientificReports
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.MaxPageSize)
            .Take(request.MaxPageSize)
            .ToListAsync(cancellationToken);

        return reports;
    }
}