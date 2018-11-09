using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Accounts.Posts.Reactions
{
  public class Reaction
  {
    public int Id { get; set; }
    
    [Required]
    public Action Action { get; set; }
    
    [Required]
    public DateTime Reacted { get; set; }
    
    public int PostId { get; set; }
    public Post Post { get; set; }
    
    public int? AccountId { get; set; }
    public Account Account { get; set; }
  }
}