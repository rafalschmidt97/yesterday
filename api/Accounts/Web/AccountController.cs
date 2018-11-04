using System;
using System.Collections.Generic;
using System.Security.Claims;
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

    public AccountController(AccountService accountService, IMapper mapper)
    {
      this.accountService = accountService;
      this.mapper = mapper;
    }

    [HttpGet(RouteUrlId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<AccountResponse> GetById(int id)
    {
      var account = accountService.GetById(id);

      if (account == null)
      {
        return NotFound();
      }

      return mapper.Map<AccountResponse>(account);
    }

    [HttpGet(RouteUrlSelf)]
    public ActionResult<AccountResponse> GetSelf()
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return GetById(accountId);
    }

    [HttpGet]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<IList<AccountResponse>> GetAll()
    {
      var accounts = accountService.GetAll();
      return Ok(mapper.Map<IList<AccountResponse>>(accounts));
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