using System;
using Api.Accounts.Profiles;

namespace Api.Accounts.Posts.Reactions.Web
{
  public class ReactionResponse
  {
    public int Id { get; set; }
    public Action Action { get; set; }
    public DateTime Reacted { get; set; }
    public ReactionAccount Account { get; set; }
  }
  
  public class ReactionAccount
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public ReactionProfile Profile { get; set; }
  }
  
  public class ReactionProfile
  {
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public Gender Gender { get; set; }
    public string Photo { get; set; }
    public string Description { get; set; }
  }
}