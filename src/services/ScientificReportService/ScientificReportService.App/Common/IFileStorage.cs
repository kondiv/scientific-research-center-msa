using ScientificReportService.App.Domain.Models;
using Shared.ResultPattern;

namespace ScientificReportService.App.Common;

public interface IFileStorage
{
    Task<Result<UploadFileResult>> Upload(IFormFile file, string? uploadedBy = null, CancellationToken cancellationToken = default);
    Task<Result<Stream>> Download(string fileId, CancellationToken cancellationToken = default);
    Task<Result> Delete(string fileId, CancellationToken cancellationToken = default);
    Task<Result<FileInfoResult>> GetFileInfo(string fileId, CancellationToken cancellationToken = default);
}