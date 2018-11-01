using Microsoft.Extensions.DependencyInjection;

namespace Api.Core.AutomaticDI
{
  public static class AutomaticDIExtension
  {
    public static void AddAutomaticDI(this IServiceCollection services)
    {
      services.Scan(service => service
        .FromAssemblyOf<ISingleton>()
        .AddClasses()
        .AsSelf()
        .WithSingletonLifetime());

      services.Scan(service => service
        .FromAssemblyOf<IScoped>()
        .AddClasses()
        .AsSelf()
        .WithScopedLifetime());

      services.Scan(service => service
        .FromAssemblyOf<ITransient>()
        .AddClasses()
        .AsSelf()
        .WithTransientLifetime());
    }
  }
}