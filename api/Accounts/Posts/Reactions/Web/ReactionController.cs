using System;
using System.Collections.Generic;
using System.Security.Claims;
using Api.Core.Security.Roles;
using Api.Core.Security.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using static Api.Accounts.Web.AccountController;

namespace Api.Accounts.Posts.Reactions.Web

{
  [Route(RouteUrl), ApiController]
  public class ReactionController : ControllerBase
  {    
    private const string RouteUrl = "accounts";
    private const string RouteUrlAccountIdPostId = "{accountId}/posts/{postId}/reactions";
    private const string RouteUrlAccountIdPostIdReactionId = "{accountId}/posts/{postId}/reactions/{id}";
    private const string RouteUrlSelfPostId = "self/posts/{postId}/reactions";
    private const string RouteUrlSelfPostIdReactionId = "self/posts/{postId}/reactions/{id}";
    
    private readonly ReactionService reactionService;
    private readonly IMapper mapper;

    public ReactionController(ReactionService reactionService, IMapper mapper)
    {
      this.reactionService = reactionService;
      this.mapper = mapper;
    }
    
    [HttpGet(RouteUrlAccountIdPostIdReactionId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<ReactionResponse> GetById(int id)
    {
      var reaction = reactionService.GetByIdWithProfile(id);

      if (reaction == null)
      {
        return NotFound();
      }

      return mapper.Map<ReactionResponse>(reaction);
    }
    
    [HttpGet(RouteUrlAccountIdPostId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<IList<ReactionResponse>> GetAll(int postId)
    {
      var reactions = reactionService.GetByPostIdWithProfile(postId);
      return Ok(mapper.Map<IList<ReactionResponse>>(reactions));
    }
    
    [HttpPost(RouteUrlAccountIdPostId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Add(int accountId, int postId, ReactionRequest reactionRequest)
    {
      var reaction = mapper.Map<Reaction>(reactionRequest);
      var isCreated = reactionService.Add(accountId, postId, reaction);

      if (!isCreated)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpPost(RouteUrlSelfPostId)]
    public IActionResult AddBySelf(int postId, ReactionRequest reactionRequest)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Add(accountId, postId, reactionRequest);
    }
    
    [HttpDelete(RouteUrlAccountIdPostIdReactionId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Delete(int id, int accountId)
    {
      var isDeleted = reactionService.Delete(id, accountId);

      if (!isDeleted)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpDelete(RouteUrlSelfPostIdReactionId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult DeleteBySelf(int id)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Delete(id, accountId);
    }
  }
}