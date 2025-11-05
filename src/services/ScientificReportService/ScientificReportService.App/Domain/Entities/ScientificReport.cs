namespace ScientificReportService.App.Domain.Entities;

public sealed class ScientificReport
{
    public Guid Id { get; private init; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime PublishedAt { get; private init; }
    public DateTime LastUpdatedAt { get; private set; }
    public string StorageFileId { get; private set; } = null!;
    public string Author { get; private init; }
    public string AuthorId { get; private init; }
    public string Tags { get; private set; }

    public ScientificReport(string title, string description, string author, string authorId, string tags)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        PublishedAt = DateTime.UtcNow;
        LastUpdatedAt = DateTime.UtcNow;
        Author = author;
        AuthorId = authorId;
        Tags = tags;
    }

    public void SetObjectKey(string storageFileId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(storageFileId));
        
        StorageFileId = storageFileId;
    }
}