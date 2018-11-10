using System;
using Api.Accounts.Profiles;

namespace Api.Accounts.Posts.Comments.Web
{
  public class CommentResponse
  {
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }
    public CommentAccount Account { get; set; }
  }
  
  public class CommentAccount
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public CommentProfile Profile { get; set; }
  }
  
  public class CommentProfile
  {
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public Gender Gender { get; set; }
    public string Photo { get; set; }
    public string Description { get; set; }
  }
}