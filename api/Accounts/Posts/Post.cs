using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.Accounts.Posts.Comments;
using Api.Accounts.Posts.Reactions;

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
    
    public ICollection<Reaction> Reactions { get; set; }
    public ICollection<Comment> Comments { get; set; }
    
    public int AccountId { get; set; }
    public Account Account { get; set; }
  }
}