using System.ComponentModel.DataAnnotations;

namespace Api.Accounts.Web
{
  public class ChangePasswordRequest
  {
    [Required(AllowEmptyStrings = false)]
    public string NewPassword { get; set; }

    [Required(AllowEmptyStrings = false)]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords don't match.")]
    public string ConfirmPassword { get; set; }
  }
}