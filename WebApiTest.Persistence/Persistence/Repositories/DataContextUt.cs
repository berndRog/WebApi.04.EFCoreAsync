using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApi.Core;
using WebApi.Di;
using WebApi.Persistence;
namespace WebApiTest.Persistence.Repositories;
[Collection(nameof(SystemTestCollectionDefinition))]
public  class DataContextUt {

   private readonly IConfiguration _configuration;
   private readonly IOwnersRepository _ownersRepository;
   private readonly IAccountsRepository _accountsRepository;
   private readonly IDataContext _dataContext;
   private readonly DataContext _dbContext;

   protected readonly ArrangeTest _arrangeTest;
   protected readonly Seed _seed;

   public DataContextUt() {
      IServiceCollection services = new ServiceCollection();
      services.AddCore();
//    services.AddPersistenceTest();

      // Configuration
      // Nuget:  Microsoft.Extensions.Configuration
      //       + Microsoft.Extensions.Configuration.Json
      _configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettingsTest.json", false)
         .Build();
      services.AddSingleton<IConfiguration>(_configuration);
      
      // Logging
      // Nuget:  Microsoft.Extensions.Logging
      //       + Microsoft.Extensions.Logging.Configuration
      //       + Microsoft.Extensions.Logging.Debug
      var logging = _configuration.GetSection("Logging");
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
      var (useDatabase, dataSource) = DataContext.EvalDatabaseConfiguration(_configuration);
      
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

      services.AddScoped<ArrangeTest>();
      services.AddScoped<Seed>();
      
      
      var serviceProvider = services.BuildServiceProvider()
         ?? throw new Exception("Failed to create an instance of ServiceProvider");

      //-- Service Locator    
      _dbContext = serviceProvider.GetRequiredService<DataContext>()
         ?? throw new Exception("Failed to create CDbContext");
      _dbContext.Database.EnsureDeleted();
      _dbContext.Database.EnsureCreated();

      _dataContext = serviceProvider.GetRequiredService<IDataContext>()
         ?? throw new Exception("Failed to create an instance of IDataContext");

      _ownersRepository = serviceProvider.GetRequiredService<IOwnersRepository>()
         ?? throw new Exception("Failed create an instance of IOwnersRepository");
      _accountsRepository = serviceProvider.GetRequiredService<IAccountsRepository>()
         ?? throw new Exception("Failed create an instance of IAccountsRepository");

      _arrangeTest = serviceProvider.GetRequiredService<ArrangeTest>()
         ?? throw new Exception("Failed create an instance of ArrangeTest");

      _seed = new Seed();
   }


   
   private void ShowRepository(string text){
#if DEBUG
      _dataContext.LogChangeTracker(text);
#endif
   }

   [Fact]
   public async Task DataContextInit() {
      
      // ClearChangeTracker
      _dataContext.ClearChangeTracker();
      _dataContext.LogChangeTracker("Init ClearChangeTracker");
      
      // DomainModel Logic
      _seed.InitAccounts();
           
      // Add to Repositories
      _ownersRepository.AddRange(_seed.Owners);  
      _accountsRepository.AddRange(_seed.Accounts);
      var countEntries = _dbContext.ChangeTracker.Entries().Count();
      Debug.WriteLine($"Entries in ChangeTracker {countEntries}");
      // _dbContext.ChangeTracker.Entries().ToList().ForEach(entry => {
      //    Debug.WriteLine($"{entry.Entity.ToString()} {entry.State}");
      // });
      Debug.WriteLine(_dbContext.ChangeTracker.DebugView.ShortView);
      _dataContext.LogChangeTracker("After AddRange");
      
      // Write to Database
      await _dataContext.SaveAllChangesAsync();
      var countOwners = _dbContext.Owners.Count();
      var countAcoounts = _dbContext.Accounts.Count();
      Debug.WriteLine($"Count Owners in database {countEntries}");
      
      // ClearChangeTracker
      _dataContext.ClearChangeTracker();
      _dataContext.LogChangeTracker("Init ClearChangeTracker");

   }
   
   [Fact]
   public void EvalDatabaseConfigurationUt() {
      // Act
      var (useDatabase, dataSource) = DataContext.EvalDatabaseConfiguration(_configuration);
      var file = new FileInfo(dataSource);
      var fileName = file.Name;
      
      // Assert   
      useDatabase.Should().Be("Sqlite");
      fileName.Should().Be("WebApi04Test.db");   
   }



}