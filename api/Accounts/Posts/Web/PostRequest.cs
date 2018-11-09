using System.ComponentModel.DataAnnotations;

namespace Api.Accounts.Posts.Web
{
  public class PostRequest
  {
    [Required(AllowEmptyStrings = false)]
    public string Content { get; set; }
    
    public string Photo { get; set; }
  }
}