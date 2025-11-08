namespace ScientificReportService.Statistics.App.Domain.Entities;

public sealed class Report
{
    public Guid Id { get; private init; }

    public string Title { get; private init; }

    public DateTime CreatedOnUtc { get; private init; }

    public ICollection<ReportEvent> ReportEvents { get; set; } = [];

    public Report(Guid id, string title, DateTime createdOnUtc)
    {
        Id = id;
        Title = title;
        CreatedOnUtc = createdOnUtc;
    }
}
