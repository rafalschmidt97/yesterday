using System.Collections.Generic;
using Api.Core.Security.Roles;
using Api.Core.Security.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Core.Security.Roles.Web
{
  [Route("roles"), ApiController]
  [AuthorizeRole(RoleConstants.Admin)]
  public class RoleController : ControllerBase
  {
    private readonly RoleService roleService;
    private readonly IMapper mapper;

    public RoleController(RoleService roleService, IMapper mapper)
    {
      this.roleService = roleService;
      this.mapper = mapper;
    }

    [HttpGet("{id}")]
    public ActionResult<RoleResponse> GetById(int id)
    {
      var role = roleService.GetById(id);

      if (role == null)
      {
        return NotFound();
      }

      return mapper.Map<RoleResponse>(role);
    }

    [HttpGet]
    public ActionResult<IList<RoleResponse>> GetAll()
    {
      var roles = roleService.GetAll();
      return Ok(mapper.Map<IList<RoleResponse>>(roles));
    }

    [HttpPost]
    public IActionResult Add(RoleRequest roleRequest)
    {
      var role = mapper.Map<Role>(roleRequest);
      var isCreated = roleService.Add(role);

      if (!isCreated)
      {
        return Conflict();
      }

      return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, RoleRequest roleRequest)
    {
      var role = mapper.Map<Role>(roleRequest);
      var isUpdated = roleService.Update(id, role);

      if (!isUpdated)
      {
        return Conflict();
      }

      return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var isDeleted = roleService.Delete(id);

      if (!isDeleted)
      {
        return Conflict();
      }

      return NoContent();
    }
  }
}