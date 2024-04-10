using System;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Core;
using WebApi.Di;
using WebApi.Persistence;
using WebApiTest.Di;
namespace WebApiTest.Persistence.Repositories;
[Collection(nameof(SystemTestCollectionDefinition))]
public abstract class BaseRepositoryUt {
   
   protected readonly IOwnersRepository _ownersRepository;
   protected readonly IAccountsRepository _accountsRepository;
   protected readonly IDataContext _dataContext;
   protected readonly ArrangeTest _arrangeTest;
   protected readonly Seed _seed;

   protected BaseRepositoryUt() {
      
      // Test DI-Container
      IServiceCollection services = new ServiceCollection();
      // Add Core, UseCases, Mapper, ...
      services.AddCore();
      // Add Repositories, Test Databases, ...
      services.AddPersistenceTest();
      // Build ServiceProvider,
      // and use Dependency Injection or Service Locator Pattern
      var serviceProvider = services.BuildServiceProvider()
         ?? throw new Exception("Failed to create an instance of ServiceProvider");

      //-- Service Locator    
      var dbContext = serviceProvider.GetRequiredService<DataContext>()
         ?? throw new Exception("Failed to create CDbContext");
      dbContext.Database.EnsureDeleted();
      dbContext.Database.EnsureCreated();

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
}