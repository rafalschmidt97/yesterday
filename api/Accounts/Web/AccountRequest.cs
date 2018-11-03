using System.ComponentModel.DataAnnotations;

namespace Api.Accounts.Web
{
  public class AccountRequest
  {
    [Required(AllowEmptyStrings = false)]
    public string Username { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
  }
}