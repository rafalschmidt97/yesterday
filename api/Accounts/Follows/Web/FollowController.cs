using System;
using System.Collections.Generic;
using System.Security.Claims;
using Api.Core.Security.Roles;
using Api.Core.Security.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Accounts.Follows.Web
{
  [Route(RouteUrl), ApiController]
  public class FollowController : ControllerBase
  {
    private const string RouteUrl = "accounts";
    private const string RouteFollowingUrlId = "{id}/following";
    private const string RouteFollowingUrlSelf = "self/following";
    private const string RouteFollowersUrlId = "{id}/followers";
    private const string RouteFollowersUrlSelf = "self/followers";
    
    private readonly FollowService followService;
    private readonly IMapper mapper;

    public FollowController(FollowService followService, IMapper mapper)
    {
      this.followService = followService;
      this.mapper = mapper;
    }
    
    [HttpGet(RouteFollowingUrlId)]
    public ActionResult<IList<FollowResponse>> GetFollowingByAccountId(int id)
    {
      var follows = followService.GetFollowingByAccountId(id);
      return Ok(mapper.Map<IList<FollowResponse>>(follows));
    }
    
    [HttpGet(RouteFollowingUrlSelf)]
    public ActionResult<IList<FollowResponse>> GetFollowingSelf()
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return GetFollowingByAccountId(accountId);
    }
    
    [HttpGet(RouteFollowersUrlId)]
    public ActionResult<IList<FollowResponse>> GetFollowersByAccountId(int id)
    {
      var follows = followService.GetFollowersByAccountId(id);
      return Ok(mapper.Map<IList<FollowResponse>>(follows));
    }
    
    [HttpGet(RouteFollowersUrlSelf)]
    public ActionResult<IList<FollowResponse>> GetFollowersSelf()
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return GetFollowersByAccountId(accountId);
    }
    
    [HttpPost(RouteFollowingUrlId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Add(int id, FollowRequest followRequest)
    {
      var isCreated = followService.Add(id, followRequest.Id.GetValueOrDefault());

      if (!isCreated)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpPost(RouteFollowingUrlSelf)]
    public IActionResult AddBySelf(FollowRequest followRequest)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Add(accountId, followRequest);
    }
    
    [HttpDelete(RouteFollowingUrlId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Delete(int id, FollowRequest followRequest)
    {
      var isDeleted = followService.Delete(id, followRequest.Id.GetValueOrDefault());

      if (!isDeleted)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpDelete(RouteFollowingUrlSelf)]
    public IActionResult DeleteBySelf(FollowRequest followRequest)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Delete(accountId, followRequest);
    }
  }
}