using AuthenticationService.App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationService.App.Infrastructure.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("role");
        
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.NormalizedName).IsUnique();
        
        builder.Property(r => r.Id).ValueGeneratedNever().HasColumnName("id");
        builder.Property(r => r.Name).HasMaxLength(128).HasColumnName("name");
        builder.Property(r => r.NormalizedName).HasMaxLength(128).HasColumnName("normalized_name");
    }
}