using MediatR;
using Microsoft.EntityFrameworkCore;
using ScientificReportService.App.Infrastructure;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace ScientificReportService.App.Features.GetReportAuthor;

internal sealed class GetReportAuthorRequestHandler : IRequestHandler<GetReportAuthorRequest, Result<Author>>
{
    private readonly ScientificReportDbContext _context;
    private readonly ILogger<GetReportAuthorRequestHandler> _logger;

    public GetReportAuthorRequestHandler(ScientificReportDbContext context, ILogger<GetReportAuthorRequestHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Author>> Handle(GetReportAuthorRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get report author. Report id in database: {id}", request.ReportId);
        
        var author = await _context
            .ScientificReports
            .Where(r => r.Id.ToString() == request.ReportId)
            .Select(r => new Author(r.AuthorId, r.Author))
            .FirstOrDefaultAsync(cancellationToken);

        if (author is null)
        {
            _logger.LogError("Cannot get author for report: {id}", request.ReportId);
            return Result<Author>.Failure(new NotFoundError("Cannot get author for report"));
        }

        return Result<Author>.Success(author);
    }
}