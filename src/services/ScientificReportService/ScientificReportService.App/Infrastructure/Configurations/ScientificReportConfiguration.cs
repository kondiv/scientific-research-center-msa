using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScientificReportService.App.Domain.Entities;

namespace ScientificReportService.App.Infrastructure.Configurations;

public class ScientificReportConfiguration : IEntityTypeConfiguration<ScientificReport>
{
    public void Configure(EntityTypeBuilder<ScientificReport> builder)
    {
        builder.ToTable("scientific_report");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.StorageFileId).IsUnique();
        
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");
        
        builder.Property(x => x.Title)
            .HasMaxLength(128)
            .HasColumnName("title");
        
        builder.Property(x => x.Description)
            .HasMaxLength(1024)
            .HasColumnName("description");
        
        builder.Property(x => x.Author)
            .HasMaxLength(512)
            .HasColumnName("author");
        
        builder.Property(x => x.Tags)
            .HasMaxLength(1024)
            .HasColumnName("tags");
        
        builder.Property(x => x.PublishedAt)
            .HasColumnName("published_at");
        
        builder.Property(x => x.LastUpdatedAt)
            .HasColumnName("last_updated_at");
        
        builder.Property(x => x.StorageFileId)
            .HasMaxLength(256)
            .HasColumnName("object_key");
    }
}