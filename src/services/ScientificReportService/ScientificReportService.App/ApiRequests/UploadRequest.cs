namespace ScientificReportService.App.ApiRequests;

public sealed record UploadRequest(string Title, string Description, string Author, string Tags);