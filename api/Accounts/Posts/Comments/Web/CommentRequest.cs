using System.ComponentModel.DataAnnotations;

namespace Api.Accounts.Posts.Comments.Web
{
  public class CommentRequest
  {
    [Required(AllowEmptyStrings = false)]
    public string Content { get; set; } 
  }
}