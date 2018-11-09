using System;
using Api.Accounts.Profiles;

namespace Api.Accounts.Posts.Comments.Web
{
  public class CommentResponse
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