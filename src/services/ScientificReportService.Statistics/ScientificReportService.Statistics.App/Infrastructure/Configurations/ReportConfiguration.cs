using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScientificReportService.Statistics.App.Domain.Entities;

namespace ScientificReportService.Statistics.App.Infrastructure.Configurations;

internal sealed class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("report");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).ValueGeneratedNever().HasColumnName("id");
        builder.Property(r => r.Title).HasMaxLength(256).HasColumnName("title");
        builder.Property(r => r.CreatedOnUtc).ValueGeneratedNever().HasColumnName("created_on_utc");

        builder
            .HasMany(r => r.ReportEvents)
            .WithOne()
            .HasForeignKey(s => s.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
