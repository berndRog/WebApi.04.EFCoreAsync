using Microsoft.Extensions.DependencyInjection;
using WebApi.Controllers;
namespace WebApiTest.Di;

public static class DiControllersTest {
   public static void AddControllersTest(
      this IServiceCollection services
   ) {
      // Controllers
      services.AddScoped<OwnersController>();
      services.AddScoped<AccountsController>();
   }
}