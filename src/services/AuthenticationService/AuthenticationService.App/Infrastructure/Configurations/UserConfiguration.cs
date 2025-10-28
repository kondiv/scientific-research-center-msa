using AuthenticationService.App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationService.App.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(u => u.Id);
        
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Login).IsUnique();
        
        builder.Property(u => u.Id).ValueGeneratedNever().HasColumnName("id");
        builder.Property(u => u.Email).HasMaxLength(128).HasColumnName("email");
        builder.Property(u => u.Login).HasMaxLength(128).HasColumnName("login");
        builder.Property(u => u.HashPassword).HasMaxLength(1024).HasColumnName("hash_password");
        builder.Property(u => u.FullName).HasMaxLength(256).HasColumnName("full_name");
        builder.Property(u => u.RegisteredAt).HasColumnName("registered_at");
        builder.Property(u => u.RoleId).HasColumnName("role_id");
        
        builder
            .HasOne(u => u.Role)
            .WithMany(u => u.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}