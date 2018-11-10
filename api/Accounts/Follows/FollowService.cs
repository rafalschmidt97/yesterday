using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Api.Core.AutomaticDI;
using Api.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Api.Accounts.Follows
{
  public class FollowService : ISingleton
  {
    private readonly DatabaseContext db;
    private readonly AccountService accountService;

    public FollowService(DatabaseContext db, AccountService accountService)
    {
      this.db = db;
      this.accountService = accountService;
    }

    public int GetFollowersCountByAccountId(int id)
    {
      return db.AccountFollows.Count(f => f.FollowingId.Equals(id));
    }
    
    public int GetFollowingCountByAccountId(int id)
    {
      return db.AccountFollows.Count(f => f.AccountId.Equals(id));
    }

    public IList<Account> GetFollowingByAccountId(int id)
    {
      return db.AccountFollows.Where(f => f.AccountId.Equals(id))
        .Include(f => f.Following)
          .ThenInclude(a => a.Profile)
        .ToList()
        .Select(f => f.Following) // select breaks joins so get everything and then select only following
        .ToList();
    }

    public IList<Account> GetFollowersByAccountId(int id)
    {
      return db.AccountFollows.Where(f => f.FollowingId.Equals(id))
        .Include(f => f.Account)
          .ThenInclude(a => a.Profile)
        .ToList()
        .Select(f => f.Account) // select breaks joins so get everything and then select only following
        .ToList();
    }

    public bool Add(int accountId, int followingId)
    {
      if (accountService.GetById(accountId) == null ||
          accountService.GetById(followingId) == null ||
          IsAccountFollowingAlready(accountId, followingId))
      {
        return false;
      }

      db.AccountFollows.Add(new AccountFollow
      {
        AccountId = accountId,
        FollowingId = followingId
      });
      db.SaveChanges();

      return true;
    }

    public bool Delete(int accountId, int followingId)
    {
      if (accountService.GetById(accountId) == null ||
          accountService.GetById(followingId) == null ||
          !IsAccountFollowingAlready(accountId, followingId))
      {
        return false;
      }

      db.AccountFollows.Remove(new AccountFollow
      {
        AccountId = accountId,
        FollowingId = followingId
      });
      db.SaveChanges();

      return true;
    }

    private bool IsAccountFollowingAlready(int accountId, int followingId)
    {
      return db.AccountFollows.Any(f => f.AccountId.Equals(accountId) && f.FollowingId.Equals(followingId));
    }
  }
}