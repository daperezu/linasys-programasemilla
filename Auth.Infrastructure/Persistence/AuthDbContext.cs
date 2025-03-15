using LinaSys.Auth.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinaSys.Auth.Infrastructure.Persistence;

public partial class AuthDbContext(DbContextOptions<AuthDbContext> options, IMediator mediator)
    : IdentityDbContext<User>(options)
{
    public virtual DbSet<ProtectedResource> ProtectedResources { get; set; }

    public virtual DbSet<RoleProtectedResourcePermission> RoleProtectedResourcePermissions { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UserProtectedResourcePermission> UserProtectedResourcePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProtectedResource>(entity =>
        {
            entity.HasIndex(e => e.ExternalId, "IX_ProtectedResources_ExternalId").IsUnique();

            entity.HasIndex(e => e.ResourceType, "IX_ProtectedResources_ResourceType");

            entity.Property(e => e.ExternalId).HasDefaultValueSql("NEWID()");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        });

        builder.Entity<RoleProtectedResourcePermission>(entity =>
        {
            entity.HasIndex(e => new { e.RoleId, e.ProtectedResourceId }, "IX_RoleProtectedResourcePermissions_ProtectedResourceId");

            entity.Property(e => e.RoleId).IsRequired().HasMaxLength(450);

            entity.HasOne(d => d.ProtectedResource).WithMany(p => p.RoleProtectedResourcePermissions).HasForeignKey(d => d.ProtectedResourceId);
        });

        builder.Entity<UserProfile>(entity =>
        {
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(450);
        });

        builder.Entity<UserProtectedResourcePermission>(entity =>
        {
            entity.HasIndex(e => new { e.ProtectedResourceId, e.UserId }, "IX_UserProtectedResourcePermissions_ProtectedResourceIdUser");

            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.ProtectedResource).WithMany(p => p.UserProtectedResourcePermissions).HasForeignKey(d => d.ProtectedResourceId);
        });
    }
}
