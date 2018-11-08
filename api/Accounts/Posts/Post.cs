using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Accounts.Posts
{
  public class Post
  {
    public int Id { get; set; }

    [Required]
    public DateTime Created { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    public string Photo { get; set; }
        
    public int AccountId { get; set; }
    public Account Account { get; set; }
  }
}