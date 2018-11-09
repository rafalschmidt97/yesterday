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
    private const string RouteUrl = "posts/{postId}/reactions";
    
    private readonly ReactionService reactionService;
    private readonly IMapper mapper;

    public ReactionController(ReactionService reactionService, IMapper mapper)
    {
      this.reactionService = reactionService;
      this.mapper = mapper;
    }
    
    [HttpGet(RouteUrlId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<ReactionRequest> GetById(int id)
    {
      var profile = reactionService.GetById(id);

      if (profile == null)
      {
        return NotFound();
      }

      return mapper.Map<ReactionRequest>(profile);
    }
    
    [HttpGet]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<IList<ReactionRequest>> GetAll(int postId)
    {
      var reactions = reactionService.GetByPostId(postId);
      return Ok(mapper.Map<IList<ReactionRequest>>(reactions));
    }
    
    [HttpPost]
    public IActionResult Add(int accountId, int postId, ReactionRequest reactionRequest)
    {
      var isAdmin = User.IsInRole(RoleConstants.Admin);

      if (isAdmin)
      {
        if (accountId == 0)
        {
          return BadRequest();
        }
      }
      else
      {
        if (accountId != 0)
        {
          return Forbid();
        }
        
        accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      }

      var reaction = mapper.Map<Reaction>(reactionRequest);
      var isCreated = reactionService.Add(accountId, postId, reaction);

      if (!isCreated)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpDelete(RouteUrlId)]
    public IActionResult Delete(int id, int accountId)
    {
      var isAdmin = User.IsInRole(RoleConstants.Admin);

      if (isAdmin)
      {
        if (accountId == 0)
        {
          return BadRequest();
        }
      }
      else
      {
        if (accountId != 0)
        {
          return Forbid();
        }
        
        accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      }
      
      var isDeleted = reactionService.Delete(id, accountId);

      if (!isDeleted)
      {
        return Conflict();
      }

      return NoContent();
    }
  }
}