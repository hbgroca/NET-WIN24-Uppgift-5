using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<MemberEntity>(options)
{
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<ClientEntity> Clients => Set<ClientEntity>();
    public DbSet<AddressEntity> Addresses => Set<AddressEntity>();
    public DbSet<MemberEntity> Members => Set<MemberEntity>();
    

    // Notifications
    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();
    public DbSet<NotificationDismissEntity> NotificationDismisses => Set<NotificationDismissEntity>();
    public DbSet<NotificationTypeEntity> NotificationTypes => Set<NotificationTypeEntity>();
    public DbSet<NotificationTargetGroupEntity> NotificationTargetGroups => Set<NotificationTargetGroupEntity>();


    // Status
    public DbSet<ClientStatusEntity> ClientStatus => Set<ClientStatusEntity>();
    public DbSet<MemberStatusEntity> MemberStatus => Set<MemberStatusEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the many-to-many relationship between MemberEntity and ProjectEntity
        modelBuilder.Entity<MemberEntity>()
        .HasMany(m => m.Projects)
        .WithMany(p => p.Members)
        .UsingEntity<Dictionary<string, object>>(
            join => join
                .HasOne<ProjectEntity>()
                .WithMany()
                .HasForeignKey("ProjectsId")
                .OnDelete(DeleteBehavior.Cascade), 
            join => join
                .HasOne<MemberEntity>()
                .WithMany()
                .HasForeignKey("MembersId")
                .OnDelete(DeleteBehavior.Restrict)
        );

        // Add base data for ClientStatusEntity, MemberStatusEntity, NotificationTargetGroupEntity, and NotificationTypeEntity
        modelBuilder.Entity<ClientStatusEntity>()
            .HasData(
                new ClientStatusEntity { Id = 1, Description = "Active" },
                new ClientStatusEntity { Id = 2, Description = "Inactive" },
                new ClientStatusEntity { Id = 3, Description = "Banned" }
            );
        modelBuilder.Entity<MemberStatusEntity>()
            .HasData(
                new MemberStatusEntity { Id = 1, Description = "Active" },
                new MemberStatusEntity { Id = 2, Description = "Busy" },
                new MemberStatusEntity { Id = 3, Description = "Vacation" },
                new MemberStatusEntity { Id = 4, Description = "Inactive" }
            );
        modelBuilder.Entity<NotificationTargetGroupEntity>()
            .HasData(
                new NotificationTargetGroupEntity { Id = 1, TargetGroup = "Member" },
                new NotificationTargetGroupEntity { Id = 2, TargetGroup = "Admin" }
            );
        modelBuilder.Entity<NotificationTypeEntity>()
            .HasData(
                new NotificationTypeEntity { Id = 1, NotificationType = "Client" },
                new NotificationTypeEntity { Id = 2, NotificationType = "Project" },
                new NotificationTypeEntity { Id = 3, NotificationType = "Member" },
                new NotificationTypeEntity { Id = 4, NotificationType = "Admin" }
            );
    }
}

