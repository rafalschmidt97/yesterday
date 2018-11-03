using System.Collections.Generic;
using System.Linq;
using Api.Core.AutomaticDI;
using Api.Core.Database;

namespace Api.Core.Security.Roles
{
  public class RoleService : ISingleton
  {    
    private readonly DatabaseContext db;

    public RoleService(DatabaseContext db)
    {
      this.db = db;
    }
    
    public Role GetById(int id)
    {
      return db.Roles.FirstOrDefault(u => u.Id.Equals(id));
    }
    
    public Role GetByName(string name)
    {
      return db.Roles.FirstOrDefault(u => u.Name.Equals(name));
    }

    public IList<Role> GetAll()
    {
      return db.Roles.ToList();
    }

    public bool Add(Role role)
    { 
      if (GetByName(role.Name) != null)
      {
        return false;
      }

      db.Add(role);
      db.SaveChanges();

      return true;
    }

    public bool Update(int id, Role role)
    {
      var oldRole = GetById(id);

      if (oldRole == null)
      {
        return false;
      }

      oldRole.Name = role.Name;

      db.Roles.Update(oldRole);
      db.SaveChanges();

      return true;
    }

    public bool Delete(int id)
    {
      var role = GetById(id);

      if (role == null)
      {
        return false;
      }

      db.Roles.Remove(role);
      db.SaveChanges();

      return true;
    }
  }
}