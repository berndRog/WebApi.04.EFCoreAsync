using System.IO;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApi.Core.DomainModel.Entities;
using WebApi.Core.Mapping;
namespace WebApiTest.Controllers.Moq;

public static class DiMoq {

   public static void AddMoq(
      this IServiceCollection serviceCollection
   ){
      // Configuration
      // Nuget:  Microsoft.Extensions.Configuration
      //       + Microsoft.Extensions.Configuration.Json
      var configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettingsTest.json", false)
         .Build();
      serviceCollection.AddSingleton<IConfiguration>(configuration);

      // Logging
      // Nuget:  Microsoft.Extensions.Logging
      //       + Microsoft.Extensions.Logging.Configuration
      //       + Microsoft.Extensions.Logging.Debug
      var logging = configuration.GetSection("Logging");
      serviceCollection.AddLogging(builder => {
         builder.ClearProviders();
         builder.AddConfiguration(logging);
         builder.AddDebug();
      });

      serviceCollection.AddAutoMapper(typeof(Owner), typeof(MappingProfile));
      // Auto Mapper Configurations
      var mapperConfig = new MapperConfiguration(mc => {
         mc.AddProfile(new MappingProfile());
      });
   }
}