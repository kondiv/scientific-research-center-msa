using MediatR;
using Shared.ResultPattern;

namespace ScientificReportService.App.Features.GetReportAuthor;

internal sealed record GetReportAuthorRequest(string ReportId) : IRequest<Result<Author>>;