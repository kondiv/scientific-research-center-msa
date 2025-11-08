namespace Shared.Contracts.ReportEvents;

public sealed class ReportCreatedEvent
{
    public Guid Id { get; private init; }

    public string Title { get; private init; }

    public DateTime CreatedAtUtc { get; private init; }

    public ReportCreatedEvent(Guid id, string title, DateTime createdAtUtc)
    {
        Id = id;
        Title = title;
        CreatedAtUtc = createdAtUtc;
    }
}
