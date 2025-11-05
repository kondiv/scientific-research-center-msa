using MediatR;
using ScientificReportService.App.Domain.ValueTypes;
using Shared.ResultPattern;

namespace ScientificReportService.App.Features.DownloadReport;

internal sealed record DownloadReportRequest(string FileId) : IRequest<Result<FileData>>;