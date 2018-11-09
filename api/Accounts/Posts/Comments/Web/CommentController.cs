using System;
using System.Collections.Generic;
using System.Security.Claims;
using Api.Core.Security.Roles;
using Api.Core.Security.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using static Api.Accounts.Web.AccountController;

namespace Api.Accounts.Posts.Comments.Web
{
  [Route(RouteUrl), ApiController]
  public class CommentController : ControllerBase
  {
    private const string RouteUrl = "posts/{postId}/comments";
    
    private readonly CommentService commentService;
    private readonly IMapper mapper;

    public CommentController(CommentService commentService, IMapper mapper)
    {
      this.commentService = commentService;
      this.mapper = mapper;
    }
    
    [HttpGet(RouteUrlId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<CommentRequest> GetById(int id)
    {
      var comment = commentService.GetById(id);

      if (comment == null)
      {
        return NotFound();
      }

      return mapper.Map<CommentRequest>(comment);
    }
    
    [HttpGet]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<IList<CommentRequest>> GetAll(int postId)
    {
      var comments = commentService.GetByPostId(postId);
      return Ok(mapper.Map<IList<CommentRequest>>(comments));
    }
    
    [HttpPost]
    public IActionResult Add(int accountId, int postId, CommentRequest commentRequest)
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

      var comment = mapper.Map<Comment>(commentRequest);
      var isCreated = commentService.Add(accountId, postId, comment);

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
      
      var isDeleted = commentService.Delete(id, accountId);

      if (!isDeleted)
      {
        return Conflict();
      }

      return NoContent();
    }
  }
}