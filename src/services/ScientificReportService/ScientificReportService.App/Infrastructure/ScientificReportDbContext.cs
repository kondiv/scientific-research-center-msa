using Microsoft.EntityFrameworkCore;
using ScientificReportService.App.Domain.Entities;

namespace ScientificReportService.App.Infrastructure;

public sealed class ScientificReportDbContext : DbContext
{
    public DbSet<ScientificReport> ScientificReports => Set<ScientificReport>();
    
    public ScientificReportDbContext(DbContextOptions<ScientificReportDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}