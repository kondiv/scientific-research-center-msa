namespace Shared.Contracts.ReportEvents;

public sealed class ReportDeletedEvent
{
    public Guid Id { get; private init; }

    public DateTime DeletedAtUtc { get; private init; }

    public ReportDeletedEvent(Guid id, DateTime deletedAtUtc)
    {
        Id = id;
        DeletedAtUtc = deletedAtUtc;
    }
}
