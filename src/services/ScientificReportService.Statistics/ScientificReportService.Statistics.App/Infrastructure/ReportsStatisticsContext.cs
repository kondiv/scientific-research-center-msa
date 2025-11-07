using Microsoft.EntityFrameworkCore;
using ScientificReportService.Statistics.App.Domain.Entities;

namespace ScientificReportService.Statistics.App.Infrastructure;

internal sealed class ReportsStatisticsContext : DbContext
{
    public DbSet<Statistic> Statistics => Set<Statistic>();
    
    public ReportsStatisticsContext(DbContextOptions<ReportsStatisticsContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
