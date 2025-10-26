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
        
        builder.Property(r => r.Id).ValueGeneratedOnAdd().HasColumnName("id");
        builder.Property(r => r.Name).HasMaxLength(128).HasColumnName("name");
        builder.Property(r => r.NormalizedName).HasMaxLength(128).HasColumnName("normalized_name");

        builder.HasData(
            new Role()
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new Role()
            {
                Id = 2,
                Name = "Scientist",
                NormalizedName = "SCIENTIST"
            },
            new Role()
            {
                Id = 3,
                Name = "Technical Specialist",
                NormalizedName = "TECHNICAL_SPECIALIST"
            });
    }
}