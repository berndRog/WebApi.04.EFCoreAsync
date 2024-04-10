using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Controllers;
using WebApi.Core;
using WebApi.Di;
using WebApi.Persistence;
using WebApiTest.Di;
using WebApiTest.Persistence;
namespace WebApiTest.Controllers;

public class BaseControllerTest {

   protected readonly AccountsController _accountsController;
   protected readonly OwnersController _ownersController;
   protected readonly IOwnersRepository _ownersRepository;
   protected readonly IAccountsRepository _accountsRepository;
   protected readonly IDataContext _dataContext;
   protected readonly ArrangeTest _arrangeTest;
   protected readonly IMapper _mapper;
   protected readonly Seed _seed;

   protected BaseControllerTest() {
      
      // Prepare Test DI-Container
      IServiceCollection serviceCollection = new ServiceCollection();
      // Add Controllers   
      serviceCollection.AddControllersTest();
      // Add Core, UseCases, Mapper, ...
      serviceCollection.AddCore();
      // Add Persistence, Repositories, Database, ...
      serviceCollection.AddPersistenceTest();
      // Create Test DI-Container
      var serviceProvider = serviceCollection.BuildServiceProvider()
         ?? throw new Exception("Failed to build Serviceprovider");

      var dbContext = serviceProvider.GetRequiredService<DataContext>()
         ?? throw new Exception("Failed to create an instance of DataContext");
      dbContext.Database.EnsureDeleted();
      dbContext.Database.EnsureCreated();

      
      _ownersController = serviceProvider.GetRequiredService<OwnersController>()
         ?? throw new Exception("Failed to create an instance of OwnersController");
      _accountsController = serviceProvider.GetRequiredService<AccountsController>()
         ?? throw new Exception("Failed to create an instance of AccountsController");
      
      _ownersRepository = serviceProvider.GetRequiredService<IOwnersRepository>()
         ?? throw new Exception("Failed to create an instance of IOwnersRepository");
      _accountsRepository = serviceProvider.GetRequiredService<IAccountsRepository>()
         ?? throw new Exception("Failed to create an instance of IAccountsRepository");
      _dataContext = serviceProvider.GetRequiredService<IDataContext>() 
         ?? throw new Exception("Failed to create an instance of IDataContext");
      
      _arrangeTest = serviceProvider.GetRequiredService<ArrangeTest>()
         ?? throw new Exception("Failed create an instance of ArrangeTest");
      _mapper = serviceProvider.GetRequiredService<IMapper>();
      _seed = new Seed();
   }
}