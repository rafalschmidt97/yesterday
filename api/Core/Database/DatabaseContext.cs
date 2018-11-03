using Api.Accounts;
using Api.Core.Security.Roles;
using Microsoft.EntityFrameworkCore;

namespace Api.Core.Database
{
  public class DatabaseContext : DbContext
  {
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    
    // many-to-many relation requires fluent api
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<AccountRole>().HasKey(entity => new {entity.AccountId, entity.RoleId});
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
  }
}