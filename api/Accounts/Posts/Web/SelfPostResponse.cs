using System;
using System.Collections.Generic;

namespace Api.Accounts.Posts.Web
{
  public class SelfPostResponse
  {
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string Content { get; set; }
    public string Photo { get; set; }
    public ICollection<PostReaction> Reactions { get; set; }
    public ICollection<PostComment> Comments { get; set; }
  }
}