namespace ScientificReportService.App.Domain.Configurations;

public class YandexCloudSettings
{
    public string Url { get; set; } = null!;
    public string BucketName { get; set; } = null!;
    public string AccessKey { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public string Region { get; set; } = null!;
}