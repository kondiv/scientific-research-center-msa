using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScientificReportService.Statistics.App.Domain.Entities;

namespace ScientificReportService.Statistics.App.Infrastructure.Configurations;

internal sealed class StatisticConfiguration : IEntityTypeConfiguration<Statistic>
{
    public void Configure(EntityTypeBuilder<Statistic> builder)
    {
        builder.ToTable("statistic");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).ValueGeneratedNever().HasColumnName("id");
        builder.Property(s => s.Action).HasConversion<string>().HasMaxLength(32).HasColumnName("action");
        builder.Property(s => s.Date).ValueGeneratedNever().HasColumnName("date");
    }
}
