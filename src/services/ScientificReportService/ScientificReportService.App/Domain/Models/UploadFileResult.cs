namespace ScientificReportService.App.Domain.Models;

public class UploadFileResult
{
    public string FileId { get; init; } = null!;
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public string Url { get; set; } = null!;
    public DateTime UploadedAt { get; set; }
}