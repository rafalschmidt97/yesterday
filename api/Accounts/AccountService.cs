using System.Collections.Generic;
using System.Linq;
using Api.Core.AutomaticDI;
using Api.Core.Database;
using Api.Core.Security.Roles;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace Api.Accounts
{
  public class AccountService : ISingleton
  {
    private readonly DatabaseContext db;
    private readonly RoleService roleService;

    public AccountService(DatabaseContext db, RoleService roleService)
    {
      this.db = db;
      this.roleService = roleService;
    }

    public Account GetById(int id)
    {
      return db.Accounts.FirstOrDefault(u => u.Id.Equals(id));
    }

    public Account GetByUsername(string name)
    {
      return db.Accounts.FirstOrDefault(u => u.Username.Equals(name));
    }

    public Account GetByUsernameWithRoles(string name)
    {
      return db.Accounts
        .Include(u => u.AccountRoles)
          .ThenInclude(r => r.Role)
        .FirstOrDefault(u => u.Username.Equals(name));
    }

    public IList<Account> GetAll()
    {
      return db.Accounts.ToList();
    }

    public bool Add(Account account)
    { 
      if (GetByUsername(account.Username) != null)
      {
        return false;
      }

      var userRole = roleService.GetByName(RoleConstants.User);

      if (userRole == null)
      {
        return false;
      }
      
      account.Password = HashPassword(account.Password);

      db.Accounts.Add(account);
      db.AccountRoles.Add(new AccountRole
      {
        AccountId = account.Id,
        RoleId = userRole.Id
      });
      db.SaveChanges();

      return true;
    }

    public bool Update(int id, Account account)
    {
      var oldAccount = GetById(id);

      if (oldAccount == null)
      {
        return false;
      }

      oldAccount.Username = account.Username;

      db.Accounts.Update(oldAccount);
      db.SaveChanges();

      return true;
    }

    public bool Delete(int id)
    {
      var account = GetById(id);

      if (account == null)
      {
        return false;
      }

      var reactionsToDelete = db.Reactions.Where(r => r.AccountId.Equals(id));
      db.Reactions.RemoveRange(reactionsToDelete);
      
      db.Accounts.Remove(account);
      db.SaveChanges();

      return true;
    }

    public bool ChangePassword(int id, string password)
    {
      var account = GetById(id);

      if (account == null)
      {
        return false;
      }

      account.Password = HashPassword(password);

      db.Accounts.Update(account);
      db.SaveChanges();

      return true;
    }
  }
}