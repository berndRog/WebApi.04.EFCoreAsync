using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Controllers;
using WebApi.Core.DomainModel.Entities;
using WebApi.Core.Mapping;
namespace WebApiTest.Di;

public static class DiControllersTest {
   public static IServiceCollection AddControllersTest(
      this IServiceCollection services
   ) {
      // Controllers
      services.AddScoped<OwnersController>();
      services.AddScoped<AccountsController>();
      return services;
   }
}