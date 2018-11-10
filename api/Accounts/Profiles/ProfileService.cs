using System.Linq;
using Api.Core.AutomaticDI;
using Api.Core.Database;

namespace Api.Accounts.Profiles
{
  public class ProfileService : ISingleton
  {
    private readonly DatabaseContext db;
    private readonly AccountService accountService;

    public ProfileService(DatabaseContext db, AccountService accountService)
    {
      this.db = db;
      this.accountService = accountService;
    }

    public Profile GetByAccountId(int id)
    {
      return db.Profiles.FirstOrDefault(u => u.AccountId.Equals(id));
    }

    public bool Add(int accountId, Profile profile)
    {
      if (accountService.GetById(accountId) == null ||
          GetByAccountId(accountId) != null)
      {
        return false;
      }

      profile.AccountId = accountId;

      db.Profiles.Add(profile);
      db.SaveChanges();

      return true;
    }

    public bool Update(int accountId, Profile profile)
    {
      var oldProfile = GetByAccountId(accountId);

      if (oldProfile == null)
      {
        return false;
      }

      oldProfile.Firstname = profile.Firstname;
      oldProfile.Lastname = profile.Lastname;
      oldProfile.Gender = profile.Gender;
      oldProfile.Photo = profile.Photo;
      oldProfile.Description = profile.Description;

      db.Profiles.Update(oldProfile);
      db.SaveChanges();

      return true;
    }
  }
}