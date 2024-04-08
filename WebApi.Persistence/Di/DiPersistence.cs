using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Core;
using WebApi.Persistence;
namespace WebApi.Di;

public static class DiPersistence {
   public static void AddPersistence(
      this IServiceCollection services,
      IConfiguration configuration
   ){
      services.AddScoped<IOwnersRepository, OwnersRepository>();
      services.AddScoped<IAccountsRepository, AccountsRepository>();
      
      // Add DbContext (Database) to DI-Container
      var (useDatabase, dataSource) = DataContext.EvalDatabaseConfiguration(configuration);
      
      switch (useDatabase) {
         case "LocalDb":
         case "SqlServer":
            services.AddDbContext<IDataContext,DataContext>(options => 
               options.UseSqlServer(dataSource)
            );
            break;
         case "Sqlite":
            services.AddDbContext<IDataContext,DataContext>(options => 
               options.UseSqlite(dataSource)
            );
            break;
         default:
            throw new Exception("appsettings.json UseDatabase not available");
      }
   }
}