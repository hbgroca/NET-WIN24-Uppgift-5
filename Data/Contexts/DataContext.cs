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
}

