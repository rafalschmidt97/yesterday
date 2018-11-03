using System.ComponentModel.DataAnnotations;

namespace Api.Core.Security.Web
{
  public class AuthenticationRequest
  {
    [Required(AllowEmptyStrings = false)]
    public string Username { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
  }
}