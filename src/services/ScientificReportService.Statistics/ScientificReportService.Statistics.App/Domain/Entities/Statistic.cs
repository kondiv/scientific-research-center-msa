using ScientificReportService.Statistics.App.Domain.Enums;

namespace ScientificReportService.Statistics.App.Domain.Entities;

public sealed class Statistic
{
    public Guid Id { get; private init; }

    public DateTime Date { get; private init; }

    public ActionType Action { get; private init; }

    public Statistic(DateTime date, ActionType action)
    {
        Id = Guid.NewGuid();
        Date = date;
        Action = action;
    }
}
