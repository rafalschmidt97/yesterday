using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Accounts.Posts.Web
{
  public class CreatePostRequest
  {
    [Required]
    public DateTime? Created { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string Content { get; set; }
    
    public string Photo { get; set; }
  }
}