using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Core.Security.Web
{
  [Route("auth"), ApiController]
  public class SecurityController : ControllerBase
  {
    private readonly SecurityService securityService;

    public SecurityController(SecurityService securityService)
    {
      this.securityService = securityService;
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Authenticate(AuthenticationRequest authentication)
    {
      if (!securityService.Authenticate(authentication.Username, authentication.Password)) return Unauthorized();

      var token = securityService.GenerateToken(authentication.Username);
      return Ok(new {token});
    }
  }
}