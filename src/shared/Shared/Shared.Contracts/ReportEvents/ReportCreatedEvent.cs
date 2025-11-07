namespace Shared.Contracts.ReportEvents;

public sealed class ReportCreatedEvent
{
    public Guid Id { get; private init; }

    public DateTime CreatedAtUtc { get; private init; }

    public ReportCreatedEvent(Guid id, DateTime createdAtUtc)
    {
        Id = id;
        CreatedAtUtc = createdAtUtc;
    }
}
