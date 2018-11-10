using System;
using System.Collections.Generic;
using System.Linq;
using Api.Core.AutomaticDI;
using Api.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Api.Accounts.Posts.Reactions
{
  public class ReactionService : ISingleton
  {
    private readonly DatabaseContext db;
    private readonly AccountService accountService;
    private readonly PostService postService;

    public ReactionService(DatabaseContext db, AccountService accountService, PostService postService)
    {
      this.db = db;
      this.accountService = accountService;
      this.postService = postService;
    }
    
    public Reaction GetById(int id)
    {
      return db.Reactions.FirstOrDefault(r => r.Id.Equals(id));
    }

    public Reaction GetByIdWithProfile(int id)
    {
      return db.Reactions
        .Include(r => r.Account)
          .ThenInclude(a => a.Profile)
        .FirstOrDefault(r => r.Id.Equals(id));
    }

    public IList<Reaction> GetByPostIdWithProfile(int id)
    {
      return db.Reactions.Where(r => r.PostId.Equals(id))
        .Include(r => r.Account)
          .ThenInclude(a => a.Profile)
        .ToList();
    }

    public bool Add(int accountId, int postId, Reaction reaction)
    {
      var post = postService.GetById(postId);

      if (accountService.GetById(accountId) == null ||
          post == null ||
          IsPostReactedByAccount(post, accountId) ||
          IsPostCreatedByAccount(post, accountId) ||
          IsPostTooOldToReact(post))
      {
        return false;
      }

      reaction.AccountId = accountId;
      reaction.PostId = postId;
      reaction.Reacted = DateTime.Now;

      db.Reactions.Add(reaction);
      db.SaveChanges();

      return true;
    }

    public bool Delete(int id, int accountId)
    {
      var reaction = GetByIdWithPost(id);

      if (reaction == null ||
          !reaction.AccountId.Equals(accountId) ||
          IsPostTooOldToReact(reaction.Post))
      {
        return false;
      }

      db.Reactions.Remove(reaction);
      db.SaveChanges();

      return true;
    }

    private Reaction GetByIdWithPost(int id)
    {
      return db.Reactions
        .Include(r => r.Post)
        .FirstOrDefault(r => r.Id.Equals(id));
    }

    private static bool IsPostTooOldToReact(Post post)
    {
      var dayAgo = DateTime.Now.AddDays(-1);
      return post.Created < dayAgo;
    }

    private static bool IsPostCreatedByAccount(Post post, int accountId)
    {
      return post.AccountId.Equals(accountId);
    }

    private bool IsPostReactedByAccount(Post post, int accountId)
    {
      return db.Reactions.Any(r => r.PostId.Equals(post.Id) && r.AccountId.Equals(accountId));
    }
  }
}