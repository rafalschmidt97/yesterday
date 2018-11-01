using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Core.Mapper
{
  public static class MapperExtension
  {
    public static void AddMapper(this IServiceCollection services)
    {
      services.AddAutoMapper(configuration => { configuration.ValidateInlineMaps = false; });
    }
  }
}