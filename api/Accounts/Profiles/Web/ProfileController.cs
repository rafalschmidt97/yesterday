using System;
using System.Security.Claims;
using Api.Core.Security.Roles;
using Api.Core.Security.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Accounts.Profiles.Web
{
  [Route(RouteUrl), ApiController]
  public class ProfileController : ControllerBase
  {
    private const string RouteUrl = "accounts";
    private const string RouteUrlId = "{id}/profile";
    private const string RouteUrlSelf = "self/profile";
    
    private readonly ProfileService profileService;
    private readonly IMapper mapper;

    public ProfileController(ProfileService profileService, IMapper mapper)
    {
      this.profileService = profileService;
      this.mapper = mapper;
    }

    [HttpGet(RouteUrlId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public ActionResult<ProfileResponse> GetByAccountId(int id)
    {
      var profile = profileService.GetByAccountId(id);

      if (profile == null)
      {
        return NotFound();
      }

      return mapper.Map<ProfileResponse>(profile);
    }

    [HttpGet(RouteUrlSelf)]
    public ActionResult<ProfileResponse> GetSelf()
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return GetByAccountId(accountId);
    }

    [HttpPost(RouteUrlId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Add(int id, ProfileRequest profileRequest)
    {
      var profile = mapper.Map<Profile>(profileRequest);
      var isCreated = profileService.Add(id, profile);

      if (!isCreated)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpPost(RouteUrlSelf)]
    public IActionResult AddBySelf(ProfileRequest profileRequest)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Add(accountId, profileRequest);
    }

    [HttpPut(RouteUrlId)]
    [AuthorizeRole(RoleConstants.Admin)]
    public IActionResult Update(int id, ProfileRequest profileRequest)
    {
      var profile = mapper.Map<Profile>(profileRequest);
      var isUpdated = profileService.Update(id, profile);

      if (!isUpdated)
      {
        return Conflict();
      }

      return NoContent();
    }
    
    [HttpPut(RouteUrlSelf)]
    public IActionResult UpdateBySelf(ProfileRequest profileRequest)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Update(accountId, profileRequest);
    }
  }
}