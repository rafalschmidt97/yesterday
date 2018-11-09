using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Accounts.Posts.Comments
{
  public class Comment
  {
    public int Id { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    [Required]
    public DateTime Created { get; set; }
    
    public int PostId { get; set; }
    public Post Post { get; set; }
    
    public int? AccountId { get; set; }
    public Account Account { get; set; }
  }
}