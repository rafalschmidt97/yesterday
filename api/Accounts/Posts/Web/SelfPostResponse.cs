using System;

namespace Api.Accounts.Posts.Web
{
  public class SelfPostResponse
  {
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string Content { get; set; }
    public string Photo { get; set; }
  }
}