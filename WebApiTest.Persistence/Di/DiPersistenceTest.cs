using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApi.Core;
using WebApi.Di;
using WebApi.Persistence;
using WebApiTest.Persistence;
namespace WebApiTest.Di;

public static class DiTestPersistence {

   public static void AddPersistenceTest(
      this IServiceCollection services
   ) {

      // Configuration
      // Nuget:  Microsoft.Extensions.Configuration
      //       + Microsoft.Extensions.Configuration.Json
      var configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettingsTest.json", false)
         .Build();
      services.AddSingleton<IConfiguration>(configuration);
      
      // Logging
      // Nuget:  Microsoft.Extensions.Logging
      //       + Microsoft.Extensions.Logging.Configuration
      //       + Microsoft.Extensions.Logging.Debug
      var logging = configuration.GetSection("Logging");
      services.AddLogging(builder => {
         builder.ClearProviders();
         builder.AddConfiguration(logging);
         builder.AddDebug();
      });
      
      // UseCases, Mapper ...
      services.AddCore();
      
      // Repository, Database ...
      services.AddSingleton<IOwnersRepository, OwnersRepository>();
      services.AddSingleton<IAccountsRepository, AccountsRepository>();      
       
      services.AddSingleton<IDataContext, DataContext>();
      
      // Add DbContext (Database) to DI-Container
      var (useDatabase, dataSource) = DataContext.EvalDatabaseConfiguration(configuration);
      
      switch (useDatabase) {
         case "LocalDb":
         case "SqlServer":
            services.AddDbContext<IDataContext, DataContext>(options => 
               options.UseSqlServer(dataSource)
            );
            break;
         case "Sqlite":
            services.AddDbContext<IDataContext, DataContext>(options => 
               options.UseSqlite(dataSource)
            );
            break;
         default:
            throw new Exception("appsettings.json UseDatabase not available");
      }

//    services.AddPersistence(configuration);
      services.AddScoped<ArrangeTest>();
      services.AddScoped<Seed>();

   }
}