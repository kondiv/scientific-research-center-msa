namespace ScientificReportService.App.Domain.Models;

public class FileInfoResult
{
    public string FileId { get; init; } = null!;
    public string FileName { get; init; } = null!;
    public string ContentType { get; init; } = null!;
    public long Size { get; set; }
    public DateTime UploadedAt { get; set; }
}