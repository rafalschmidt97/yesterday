using System;
using System.Collections.Generic;
using System.Security.Claims;
using Api.Core.Security.Roles;
using Api.Core.Security.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Accounts.Posts.Web
{
  [Route(RouteUrl), ApiController]
  public class PostController : ControllerBase
  {
    private const string RouteUrl = "accounts";
    private const string RouteUrlAccountId = "{accountId}/posts";
    private const string RouteUrlAccountIdPostId = "{accountId}/posts/{id}";
    private const string RouteUrlAccountIdFollowing = "{accountId}/posts/following";
    private const string RouteUrlSelf = "self/posts";
    private const string RouteUrlSelfPostId = "self/posts/{id}";
    private const string RouteUrlSelfFollowing = "self/posts/following";
    
    private readonly PostService postService;
    private readonly IMapper mapper;

    public PostController(PostService postService, IMapper mapper)
    {
      this.postService = postService;
      this.mapper = mapper;
    }
    
    [HttpGet(RouteUrlAccountIdPostId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<PostResponse> GetById(int id)
    {     
      var post = postService.GetByIdWithReactionsAndComments(id);

      if (post == null)
      {
        return NotFound();
      }

      return Ok(mapper.Map<PostResponse>(post));
    }

    [HttpGet(RouteUrlAccountId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<IList<SelfPostResponse>> GetByAccountId(int accountId)
    {
      var posts = postService.GetByAccountIdWithReactionsAndComments(accountId);
      return Ok(mapper.Map<IList<SelfPostResponse>>(posts));
    }

    [HttpGet(RouteUrlSelf)]
    public ActionResult<IList<SelfPostResponse>> GetSelf()
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return GetByAccountId(accountId);
    }
    
    [HttpGet(RouteUrlAccountIdFollowing)]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<IList<PostResponse>> GetFollowingByAccountId(int accountId)
    {
      var posts = postService.GetFollowingByAccountId(accountId);
      return Ok(mapper.Map<IList<PostResponse>>(posts));
    }

    [HttpGet(RouteUrlSelfFollowing)]
    public ActionResult<IList<PostResponse>> GetFollowingSelf()
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return GetFollowingByAccountId(accountId);
    }

    [HttpPost(RouteUrlAccountId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Add(int accountId, PostRequest postRequest)
    {
      var post = mapper.Map<Post>(postRequest);
      var isCreated = postService.Add(accountId, post);

      if (!isCreated)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpPost(RouteUrlSelf)]
    public IActionResult AddBySelf(PostRequest postRequest)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Add(accountId, postRequest);
    }

    [HttpPut(RouteUrlAccountIdPostId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Update(int id, int accountId, PostRequest postRequest)
    {
      var post = mapper.Map<Post>(postRequest);
      var isUpdated = postService.Update(id, accountId, post);

      if (!isUpdated)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpPut(RouteUrlSelfPostId)]
    public IActionResult UpdateBySelf(int id, PostRequest postRequest)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Update(id, accountId, postRequest);
    }
    
    [HttpDelete(RouteUrlAccountIdPostId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Delete(int id, int accountId)
    {
      var isDeleted = postService.Delete(id, accountId);

      if (!isDeleted)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpDelete(RouteUrlSelfPostId)]
    public IActionResult DeleteBySelf(int id)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Delete(id, accountId);
    }
  }
}