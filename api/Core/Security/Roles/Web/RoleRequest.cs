using System.ComponentModel.DataAnnotations;

namespace api.Core.Security.Roles.Web
{
  public class RoleRequest
  {
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }
  }
}