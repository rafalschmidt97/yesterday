using System;
using System.Collections.Generic;
using Api.Accounts.Profiles;
using Action = Api.Accounts.Posts.Reactions.Action;

namespace Api.Accounts.Web
{
  public class AccountResponse
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public AccountProfile Profile { get; set; }
    public ICollection<AccountPost> Posts { get; set; }
    public int Following { get; set; }
    public int Followers { get; set; }
  }

  public class AccountProfile
  {
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public Gender Gender { get; set; }
    public string Photo { get; set; }
    public string Description { get; set; }
  }

  public class AccountPost
  {
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string Content { get; set; }
    public string Photo { get; set; }
    public ICollection<AccountPostReaction> Reactions { get; set; }
    public ICollection<AccountPostComment> Comments { get; set; }
  }

  public class AccountPostReaction
  {
    public int Id { get; set; }
    public Action Action { get; set; }
    public DateTime Reacted { get; set; }
    public AccountPostAccount Account { get; set; }
  }

  public class AccountPostComment
  {
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }
    public AccountPostAccount Account { get; set; }
  }

  public class AccountPostAccount
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public AccountProfile Profile { get; set; }
  }
}