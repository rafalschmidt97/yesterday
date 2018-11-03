using System;
using Microsoft.AspNetCore.Authorization;

namespace Api.Core.Security.Web
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
  public class AuthorizeRoleAttribute : AuthorizeAttribute
  {
    public AuthorizeRoleAttribute(params string[] roles)
    {
      Roles = string.Join(",", roles);
    }
  }
}