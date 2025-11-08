using ScientificReportService.Statistics.App.Domain.Enums;

namespace ScientificReportService.Statistics.App.Domain.Entities;

public sealed class ReportEvent
{
    public Guid Id { get; private init; }

    public Guid ReportId { get; private init; }

    public DateTime Date { get; private init; }

    public ActionType Action { get; private init; }

    public ReportEvent(Guid reportId, DateTime date, ActionType action)
    {
        Id = Guid.NewGuid();
        ReportId = reportId;
        Date = date;
        Action = action;
    }
}
