using Edc.Core.AccountContext.Entities;
using Edc.Infra.AccountContext.Mappings;
using Microsoft.EntityFrameworkCore;

public class EdcDbContext : DbContext
{
    public EdcDbContext(DbContextOptions<EdcDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountMap());
        modelBuilder.ApplyConfiguration(new RoleMap());
    }
}