using System.ComponentModel.DataAnnotations;

namespace Api.Accounts.Posts.Reactions.Web
{
  public class ReactionRequest
  { 
    [Required]
    public Action? Action { get; set; }
  }
}