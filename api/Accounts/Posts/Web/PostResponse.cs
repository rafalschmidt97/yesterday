using System;
using System.Collections.Generic;
using Api.Accounts.Profiles;
using Action = Api.Accounts.Posts.Reactions.Action;

namespace Api.Accounts.Posts.Web
{
  public class PostResponse
  {
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string Content { get; set; }
    public string Photo { get; set; }
    public Account Account { get; set; }
    public ICollection<Reaction> Reactions { get; set; }
    public ICollection<Comment> Comments { get; set; }
  }
  
  public class Reaction
  {
    public int Id { get; set; }
    public Action Action { get; set; }
    public DateTime Reacted { get; set; }
    public Account Account { get; set; }
  }
  
  public class Comment
  {
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }
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