using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<MemberEntity>(options)
{
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<ClientEntity> Clients => Set<ClientEntity>();
    public DbSet<AddressEntity> Addresses => Set<AddressEntity>();
    public DbSet<MemberEntity> Members => Set<MemberEntity>();
}

