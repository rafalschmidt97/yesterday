using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Core.Security
{
  public static class SecureMvcExtension
  {
    public static void AddSecureMvc(this IServiceCollection services)
    {
      services.AddMvc(config =>
      {
        config.Filters.Add(new AuthorizeFilter(
          new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build()
        ));
      }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }
  }
}