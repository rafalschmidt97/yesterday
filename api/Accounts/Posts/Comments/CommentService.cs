using System;
using System.Collections.Generic;
using System.Linq;
using Api.Core.AutomaticDI;
using Api.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Api.Accounts.Posts.Comments
{
  public class CommentService : ISingleton
  {
    private readonly DatabaseContext db;
    private readonly AccountService accountService;
    private readonly PostService postService;

    public CommentService(DatabaseContext db, AccountService accountService, PostService postService)
    {
      this.db = db;
      this.accountService = accountService;
      this.postService = postService;
    }
    
    public Comment GetById(int id)
    {
      return db.Comments
        .Include(r => r.Account)
          .ThenInclude(a => a.Profile)
        .FirstOrDefault(r => r.Id.Equals(id));
    }

    public IList<Comment> GetByPostId(int id)
    {
      return db.Comments.Where(r => r.PostId.Equals(id))
        .Include(r => r.Account)
        .ThenInclude(a => a.Profile)
        .ToList();
    }

    public bool Add(int accountId, int postId, Comment comment)
    {
      var post = postService.GetById(postId);

      if (accountService.GetById(accountId) == null ||
          post == null ||
          IsPostTooOldToComment(post))
      {
        return false;
      }

      comment.AccountId = accountId;
      comment.PostId = postId;
      comment.Created = DateTime.Now;

      db.Comments.Add(comment);
      db.SaveChanges();

      return true;
    }

    public bool Delete(int id, int accountId)
    {
      var comment = GetByIdWithPost(id);

      if (comment == null ||
          !comment.AccountId.Equals(accountId) ||
          IsPostTooOldToComment(comment.Post))
      {
        return false;
      }
      
      db.Comments.Remove(comment);
      db.SaveChanges();

      return true;
    }

    private Comment GetByIdWithPost(int id)
    {
      return db.Comments
        .Include(r => r.Post)
        .FirstOrDefault(r => r.Id.Equals(id));
    }

    private static bool IsPostTooOldToComment(Post post)
    {
      var dayAgo = DateTime.Now.AddDays(-1);
      return post.Created < dayAgo;
    }
  }
}