using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.Accounts.Posts;
using Api.Accounts.Profiles;

namespace Api.Accounts
{
  public class Account
  {
    public int Id { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    public ICollection<AccountRole> AccountRoles { get; set; }
    public Profile Profile { get; set; }
    public ICollection<Post> Posts { get; set; }
    public ICollection<AccountFollow> AccountFollows { get; set; }
  }
}