namespace Shared.Contracts.ReportEvents;

public sealed class ReportDownloadedEvent
{
    public Guid Id { get; private init; }
    
    public DateTime DownloadedAtUtc { get; private init; }

    public ReportDownloadedEvent(Guid id, DateTime downloadedAtUtc)
    {
        Id = id;
        DownloadedAtUtc = downloadedAtUtc;
    }

}
