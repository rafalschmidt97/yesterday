using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.Accounts;

namespace Api.Core.Security.Roles
{
  public class Role
  {
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public ICollection<AccountRole> AccountRoles { get; set; }
  }
}