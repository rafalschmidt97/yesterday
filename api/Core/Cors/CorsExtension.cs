using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Core.Cors
{
  public static class CorsExtension
  {
    public static void AddCustomCors(this IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
          builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
      });
    }

    public static void UseCustomCors(this IApplicationBuilder app)
    {
      app.UseCors("CorsPolicy");
    }
  }
}