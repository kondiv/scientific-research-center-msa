using MediatR;
using ScientificReportService.App.Domain.Models;
using Shared.ResultPattern;

namespace ScientificReportService.App.Features.CreateReport;

internal sealed record CreateReportCommand(IFormFile File, string Title, string Description, string Author,
    string AuthorId, string Tags) : IRequest<Result<UploadFileResult>>;