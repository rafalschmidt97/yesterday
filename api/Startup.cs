using Api.Core.AutomaticDI;
using Api.Core.Cors;
using Api.Core.Mapper;
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
      services.AddAutomaticDI();
      services.AddMapper();
      services.AddCustomSwagger();
      services.AddCustomCors();
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseCustomSwagger();
      }

      app.UseCustomCors();
      app.UseMvc();
    }
  }
}