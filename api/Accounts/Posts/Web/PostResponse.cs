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
    public PostAccount Account { get; set; }
    public ICollection<PostReaction> Reactions { get; set; }
    public ICollection<PostComment> Comments { get; set; }
  }
  
  public class PostReaction
  {
    public int Id { get; set; }
    public Action Action { get; set; }
    public DateTime Reacted { get; set; }
    public PostAccount Account { get; set; }
  }
  
  public class PostComment
  {
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }
    public PostAccount Account { get; set; }
  }

  public class PostAccount
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public PostProfile Profile { get; set; }
  }
  
  public class PostProfile
  {
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public Gender Gender { get; set; }
    public string Photo { get; set; }
    public string Description { get; set; }
  }
}