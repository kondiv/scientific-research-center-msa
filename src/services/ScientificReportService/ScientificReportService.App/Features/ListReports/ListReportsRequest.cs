using MediatR;
using ScientificReportService.App.Domain.Entities;

namespace ScientificReportService.App.Features.ListReports;

internal sealed record ListReportsRequest(int Page, int MaxPageSize) : IRequest<List<ScientificReport>>;