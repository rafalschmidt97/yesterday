using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Accounts
{
  public class Account
  {
    public int Id { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [ForeignKey("AccountId")]
    public ICollection<AccountRole> AccountRoles { get; set; }
  }
}