using System.ComponentModel.DataAnnotations;

namespace Api.Accounts.Profiles.Web
{
  public class ProfileRequest
  {
    [Required(AllowEmptyStrings = false)]
    public string Firstname { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Lastname { get; set; }

    [Required]
    public Gender? Gender { get; set; }
    
    public string Photo { get; set; }
    
    public string Description { get; set; }
  }
}