using MediatR;
using Shared.ResultPattern;

namespace ScientificReportService.App.Features.DeleteReport;

internal sealed record DeleteReportCommand(string ReportId) : IRequest<Result>;