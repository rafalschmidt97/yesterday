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
    private const string RouteUrl = "accounts";
    private const string RouteUrlAccountIdPostId = "{accountId}/posts/{postId}/comments";
    private const string RouteUrlAccountIdPostIdCommentId = "{accountId}/posts/{postId}/comments/{id}";
    private const string RouteUrlSelfPostId = "self/posts/{postId}/comments";
    private const string RouteUrlSelfPostIdCommentId = "self/posts/{postId}/comments/{id}";
    
    private readonly CommentService commentService;
    private readonly IMapper mapper;

    public CommentController(CommentService commentService, IMapper mapper)
    {
      this.commentService = commentService;
      this.mapper = mapper;
    }
    
    [HttpGet(RouteUrlAccountIdPostIdCommentId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<CommentResponse> GetById(int id)
    {
      var comment = commentService.GetByIdWithProfile(id);

      if (comment == null)
      {
        return NotFound();
      }

      return mapper.Map<CommentResponse>(comment);
    }
    
    [HttpGet(RouteUrlAccountIdPostId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<IList<CommentResponse>> GetAll(int postId)
    {
      var comments = commentService.GetByPostIdWithProfile(postId);
      return Ok(mapper.Map<IList<CommentResponse>>(comments));
    }
    
    [HttpPost(RouteUrlAccountIdPostId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Add(int accountId, int postId, CommentRequest commentRequest)
    {
      var comment = mapper.Map<Comment>(commentRequest);
      var isCreated = commentService.Add(accountId, postId, comment);

      if (!isCreated)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpPost(RouteUrlSelfPostId)]
    public IActionResult AddBySelf(int postId, CommentRequest commentRequest)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Add(accountId, postId, commentRequest);
    }
    
    [HttpDelete(RouteUrlAccountIdPostIdCommentId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Delete(int id, int accountId)
    {
      var isDeleted = commentService.Delete(id, accountId);

      if (!isDeleted)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpDelete(RouteUrlSelfPostIdCommentId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult DeleteBySelf(int id)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Delete(id, accountId);
    }
  }
}