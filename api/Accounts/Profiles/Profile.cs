using System.ComponentModel.DataAnnotations;

namespace Api.Accounts.Profiles
{
  public class Profile
  {
    public int Id { get; set; }

    [Required]
    public string Firstname { get; set; }

    [Required]
    public string Lastname { get; set; }

    [Required]
    public Gender Gender { get; set; }
    
    public string Photo { get; set; }
    
    public string Description { get; set; }
    
    public int AccountId { get; set; }
    public Account Account { get; set; }
  }
}