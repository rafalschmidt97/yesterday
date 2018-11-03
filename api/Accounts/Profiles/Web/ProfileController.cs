using System;
using System.Collections.Generic;
using System.Security.Claims;
using Api.Accounts.Web;
using Api.Core.Security.Roles;
using Api.Core.Security.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Accounts.Profiles.Web
{
  [Route("accounts"), ApiController]
  public class ProfileController : ControllerBase
  {
    private readonly ProfileService profileService;
    private readonly IMapper mapper;

    public ProfileController(ProfileService profileService, IMapper mapper)
    {
      this.profileService = profileService;
      this.mapper = mapper;
    }

    [HttpGet("{id}/profile")]
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

    [HttpGet("self/profile")]
    public ActionResult<ProfileResponse> GetSelf()
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return GetByAccountId(accountId);
    }

    [HttpPost("{id}/profile")]
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
    
    [HttpPost("self/profile")]
    public IActionResult AddBySelf(ProfileRequest profileRequest)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Add(accountId, profileRequest);
    }

    [HttpPut("{id}/profile")]
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
    
    [HttpPut("self/profile")]
    public IActionResult UpdateBySelf(ProfileRequest profileRequest)
    {
      var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
      return Update(accountId, profileRequest);
    }
  }
}