using System;
using System.Collections.Generic;
using System.Security.Claims;
using Api.Core.Security.Roles;
using Api.Core.Security.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using static Api.Accounts.Web.AccountController;

namespace Api.Accounts.Posts.Web
{
  [Route(RouteUrl), ApiController]
  public class PostController : ControllerBase
  {
    private const string RouteUrl = "posts";
    
    private readonly PostService postService;
    private readonly IMapper mapper;

    public PostController(PostService postService, IMapper mapper)
    {
      this.postService = postService;
      this.mapper = mapper;
    }
    
    [HttpGet(RouteUrlId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<PostResponse> GetById(int id)
    {     
      var post = postService.GetById(id);

      if (post == null)
      {
        return NotFound();
      }

      return Ok(mapper.Map<PostResponse>(post));
    }

    [HttpGet]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<IList<SelfPostResponse>> GetByAccountId(int accountId)
    {
      if (accountId == 0)
      {
        return BadRequest();
      }
      
      var posts = postService.GetByAccountId(accountId);
      return Ok(mapper.Map<IList<SelfPostResponse>>(posts));
    }

    [HttpGet(RouteUrlSelf)]
    public ActionResult<IList<SelfPostResponse>> GetSelf()
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return GetByAccountId(accountId);
    }

    [HttpPost]
    public IActionResult Add(int accountId, CreatePostRequest postRequest)
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

      var post = mapper.Map<Post>(postRequest);
      var isCreated = postService.Add(accountId, post);

      if (!isCreated)
      {
        return Conflict();
      }

      return NoContent();
    }

    [HttpPut(RouteUrlId)]
    public IActionResult Update(int id, int accountId, UpdatePostRequest postRequest)
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
      
      var post = mapper.Map<Post>(postRequest);
      var isUpdated = postService.Update(id, accountId, post);

      if (!isUpdated)
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
      
      var isDeleted = postService.Delete(id, accountId);

      if (!isDeleted)
      {
        return Conflict();
      }

      return NoContent();
    }
  }
}