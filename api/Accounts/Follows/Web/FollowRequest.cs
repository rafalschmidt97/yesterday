using System.ComponentModel.DataAnnotations;

namespace Api.Accounts.Follows.Web
{
  public class FollowRequest
  {
    [Required]
    public int? Id { get; set; }
  }
}