using Api.Accounts;
using Api.Accounts.Profiles;
using Api.Core.Security.Roles;
using Microsoft.EntityFrameworkCore;

namespace Api.Core.Database
{
  public class DatabaseContext : DbContext
  {
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    // many-to-many relation requires fluent api in ef core
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      #region AccountRole

      modelBuilder.Entity<AccountRole>().HasKey(e => new {e.AccountId, e.RoleId});

      modelBuilder.Entity<AccountRole>().HasOne(e => e.Account).WithMany(e => e.AccountRoles)
        .HasForeignKey(e => e.AccountId);

      modelBuilder.Entity<AccountRole>().HasOne(e => e.Role).WithMany(e => e.AccountRoles)
        .HasForeignKey(e => e.RoleId);

      #endregion
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Profile> Profiles { get; set; }
  }
}