namespace ScientificReportService.App.Domain.Models;

public class FileInfoResult
{
    public string FileId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public DateTime UploadedAt { get; set; }
}