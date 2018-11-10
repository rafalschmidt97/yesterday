using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Core.Database
{
  public static class DatabaseExtension
  {
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<DatabaseContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("Database")));
    }
  }
}