using System;
using Api.Accounts.Profiles;

namespace Api.Accounts.Posts.Reactions.Web
{
  public class ReactionResponse
  {
    public int Id { get; set; }
    public Action Action { get; set; }
    public DateTime Reacted { get; set; }
    public Account Account { get; set; }
  }
  
  public class Account
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public Profile Profile { get; set; }
  }
  
  public class Profile
  {
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public Gender Gender { get; set; }
    public string Photo { get; set; }
    public string Description { get; set; }
  }
}