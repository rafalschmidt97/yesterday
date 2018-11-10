using System;
using System.Collections.Generic;
using System.Linq;
using Api.Core.AutomaticDI;
using Api.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Api.Accounts.Posts
{
  public class PostService : ISingleton
  {
    private readonly DatabaseContext db;
    private readonly AccountService accountService;

    public PostService(DatabaseContext db, AccountService accountService)
    {
      this.db = db;
      this.accountService = accountService;
    }
    
    public Post GetById(int id)
    {
      return db.Posts.FirstOrDefault(p => p.Id.Equals(id));
    }
    
    public Post GetByIdWithReactionsAndComments(int id)
    {
      return db.Posts
        .Include(p => p.Account)
          .ThenInclude(a => a.Profile)
        .Include(p => p.Reactions)
          .ThenInclude(r => r.Account)
            .ThenInclude(a => a.Profile)
        .Include(p => p.Comments)
          .ThenInclude(r => r.Account)
            .ThenInclude(a => a.Profile)
        .FirstOrDefault(p => p.Id.Equals(id));
    }

    public IList<Post> GetByAccountIdWithReactionsAndComments(int id)
    {
      return db.Posts.Where(p => p.AccountId.Equals(id))
        .Include(p => p.Reactions)
          .ThenInclude(r => r.Account)
            .ThenInclude(a => a.Profile)
        .Include(p => p.Comments)
          .ThenInclude(r => r.Account)
            .ThenInclude(a => a.Profile)
        .ToList();
    }

    public IList<Post> GetFollowingByAccountId(int id)
    {
      var dayAgo = DateTime.Now.AddDays(-1);
      var following = db.AccountFollows.Where(f => f.AccountId.Equals(id)).Select(f => f.Following.Id);
      var dayAgoReactedPosts = db.Reactions.Where(r => r.AccountId.Equals(id) && r.Reacted > dayAgo).Select(r => r.PostId);
      var dayAgoCommentedPosts = db.Comments.Where(r => r.AccountId.Equals(id) && r.Created > dayAgo).Select(r => r.PostId);
      
      return db.Posts.Where(p => following.Contains(p.Account.Id)  && 
                                 p.Created > dayAgo &&
                                 !dayAgoReactedPosts.Contains(p.Id) &&
                                 !dayAgoCommentedPosts.Contains(p.Id))
        .Include(p => p.Account)
          .ThenInclude(a => a.Profile)
        .Include(p => p.Reactions)
          .ThenInclude(r => r.Account)
            .ThenInclude(a => a.Profile)
        .Include(p => p.Comments)
          .ThenInclude(r => r.Account)
            .ThenInclude(a => a.Profile)
        .ToList();
    }

    public bool Add(int accountId, Post post)
    {
      if (accountService.GetById(accountId) == null)
      {
        return false;
      }

      post.AccountId = accountId;
      post.Created = DateTime.Now;

      db.Posts.Add(post);
      db.SaveChanges();

      return true;
    }

    public bool Update(int id, int accountId, Post post)
    {
      var oldPost = GetByIdWithReactionsAndComments(id);

      if (oldPost == null || !oldPost.AccountId.Equals(accountId))
      {
        return false;
      }

      oldPost.Content = post.Content;
      oldPost.Photo = post.Photo;

      db.Posts.Update(oldPost);
      db.SaveChanges();

      return true;
    }

    public bool Delete(int id, int accountId)
    {
      var post = GetByIdWithReactionsAndComments(id);

      if (post == null || !post.AccountId.Equals(accountId))
      {
        return false;
      }

      db.Posts.Remove(post);
      db.SaveChanges();

      return true;
    }
  }
}