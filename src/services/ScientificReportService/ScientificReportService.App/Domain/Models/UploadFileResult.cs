namespace ScientificReportService.App.Domain.Models;

public class UploadFileResult
{
    public string FileId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string Url { get; set; }
    public DateTime UploadedAt { get; set; }
}