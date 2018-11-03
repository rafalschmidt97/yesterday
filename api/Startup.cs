using Api.Core.AutomaticDI;
using Api.Core.Cors;
using Api.Core.Database;
using Api.Core.Mapper;
using Api.Core.Security;
using Api.Core.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
  public class Startup
  {
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
      this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDatabase(configuration);
      services.AddAutomaticDI();
      services.AddMapper();
      services.AddCustomCors();
      services.AddCustomSwagger();
      services.AddSecurity(configuration);
      services.AddSecureMvc();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseCustomSwagger();
      }

      app.UseCustomCors();
      app.UseSecurity();
      app.UseMvc();
    }
  }
}