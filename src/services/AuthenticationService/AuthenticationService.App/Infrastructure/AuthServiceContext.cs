using AuthenticationService.App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.App.Infrastructure;

internal sealed class AuthServiceContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();
    
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    
    public AuthServiceContext(DbContextOptions<AuthServiceContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}