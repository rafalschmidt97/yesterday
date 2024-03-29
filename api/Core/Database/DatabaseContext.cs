using Api.Accounts;
using Api.Accounts.Posts;
using Api.Accounts.Posts.Comments;
using Api.Accounts.Posts.Reactions;
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
      #region ManyToMany

      modelBuilder.Entity<AccountRole>().HasKey(e => new {e.AccountId, e.RoleId});
      modelBuilder.Entity<AccountRole>().HasOne(e => e.Account).WithMany(e => e.AccountRoles)
        .HasForeignKey(e => e.AccountId);
      modelBuilder.Entity<AccountRole>().HasOne(e => e.Role).WithMany(e => e.AccountRoles)
        .HasForeignKey(e => e.RoleId);

      modelBuilder.Entity<AccountFollow>().HasKey(e => new {e.AccountId, e.FollowingId});
      modelBuilder.Entity<AccountFollow>().HasOne(e => e.Account).WithMany(e => e.AccountFollows)
        .HasForeignKey(e => e.AccountId).OnDelete(DeleteBehavior.Restrict);

      #endregion
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<AccountFollow> AccountFollows { get; set; }
  }
}