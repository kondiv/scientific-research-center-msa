using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScientificReportService.Statistics.App.Domain.Entities;

namespace ScientificReportService.Statistics.App.Infrastructure.Configurations;

internal sealed class StatisticConfiguration : IEntityTypeConfiguration<ReportEvent>
{
    public void Configure(EntityTypeBuilder<ReportEvent> builder)
    {
        builder.ToTable("report_event");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).ValueGeneratedNever().HasColumnName("id");
        builder.Property(s => s.Action).HasConversion<string>().HasMaxLength(32).HasColumnName("action");
        builder.Property(s => s.Date).ValueGeneratedNever().HasColumnName("date");
        builder.Property(s => s.ReportId).HasColumnName("report_id");
    }
}