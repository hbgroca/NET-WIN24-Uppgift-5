using Data.Entities;
using Microsoft.EntityFrameworkCore;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<ClientEntity> Clients => Set<ClientEntity>();
    public DbSet<AddressEntity> Addresses => Set<AddressEntity>();
    public DbSet<MemberEntity> Members => Set<MemberEntity>();

   
}

