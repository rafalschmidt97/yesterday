using System;
using System.Collections.Generic;
using System.Security.Claims;
using Api.Accounts.Follows;
using Api.Core.Security.Roles;
using Api.Core.Security.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Accounts.Web
{
  [Route(RouteUrl), ApiController]
  public class AccountController : ControllerBase
  {
    public const string RouteUrlId = "{id}";
    public const string RouteUrlSelf = "self";
    private const string RouteUrl = "accounts";
    private const string RouteUrlIdPassword = "{id}/password";
    private const string RouteUrlSelfPassword = "self/password";

    private readonly AccountService accountService;
    private readonly IMapper mapper;
    private readonly FollowService followService;

    public AccountController(AccountService accountService, IMapper mapper, FollowService followService)
    {
      this.accountService = accountService;
      this.mapper = mapper;
      this.followService = followService;
    }

    [HttpGet(RouteUrlId)]
    public ActionResult<AccountResponse> GetById(int id)
    {
      var isAdmin = User.IsInRole(RoleConstants.Admin);
      var account = isAdmin ? accountService.GetByIdWithAllPosts(id) : accountService.GetByIdWithDayAgoPosts(id);

      if (account == null)
      {
        return NotFound();
      }

      var accountResponse = mapper.Map<AccountResponse>(account);
      accountResponse.Following = followService.GetFollowingCountByAccountId(id);
      accountResponse.Followers = followService.GetFollowersCountByAccountId(id);
      return accountResponse;
    }

    [HttpGet(RouteUrlSelf)]
    public ActionResult<AccountResponse> GetSelf()
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      var account = accountService.GetByIdWithAllPosts(accountId);

      if (account == null)
      {
        return NotFound();
      }

      var accountResponse = mapper.Map<AccountResponse>(account);
      accountResponse.Following = followService.GetFollowingCountByAccountId(accountId);
      accountResponse.Followers = followService.GetFollowersCountByAccountId(accountId);
      return accountResponse;
    }

    [HttpGet]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<IList<BasicAccountResponse>> GetAll()
    {
      var accounts = accountService.GetAll();
      return Ok(mapper.Map<IList<BasicAccountResponse>>(accounts));
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Add(AccountRequest accountRequest)
    {
      var account = mapper.Map<Account>(accountRequest);
      var isCreated = accountService.Add(account);

      if (!isCreated)
      {
        return Conflict();
      }

      return NoContent();
    }

    [HttpPut(RouteUrlId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Update(int id, AccountRequest accountRequest)
    {
      var account = mapper.Map<Account>(accountRequest);
      var isUpdated = accountService.Update(id, account);

      if (!isUpdated)
      {
        return Conflict();
      }

      return NoContent();
    }

    [HttpDelete(RouteUrlId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Delete(int id)
    {
      var isDeleted = accountService.Delete(id);

      if (!isDeleted)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpDelete(RouteUrlSelf)]
    public IActionResult DeleteSelf()
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Delete(accountId);
    }
    
    [HttpPatch(RouteUrlIdPassword)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult ChangePasswordById(int id, ChangePasswordRequest request)
    {
      var isChanged = accountService.ChangePassword(id, request.NewPassword);

      if (!isChanged)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpPatch(RouteUrlSelfPassword)]
    public IActionResult ChangePasswordSelf(ChangePasswordRequest request)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return ChangePasswordById(accountId, request);
    }
  }
}