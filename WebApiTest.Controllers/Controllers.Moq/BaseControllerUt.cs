using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Controllers;
using WebApi.Core;
using WebApiTest.Di;
namespace WebApiTest.Controllers.Moq;


public class BaseControllerUt {
   protected readonly IMapper _mapper;
   protected readonly Seed _seed;
   protected readonly Mock<IOwnersRepository> _mockOwnersRepository;
   protected readonly Mock<IAccountsRepository> _mockAccountsRepository;
   protected readonly Mock<IDataContext> _mockDataContext;
   protected readonly OwnersController _ownersController;
   protected readonly AccountsController _accountsController;

   protected BaseControllerUt() {
      var serviceCollection = new ServiceCollection();
      serviceCollection.AddMoq();
      var serviceProvider = serviceCollection.BuildServiceProvider()
         ?? throw new Exception("Failed to build Serviceprovider");

      var loggerFactory = serviceProvider.GetService<ILoggerFactory>()
         ?? throw new Exception("Failed to build ILoggerFactory");
      
      _mapper = serviceProvider.GetRequiredService<IMapper>()
         ?? throw new Exception("Failed to build IMapper");
      _seed = new Seed();
      
      // Mocking the repository and the data context
      _mockOwnersRepository = new Mock<IOwnersRepository>();
      _mockAccountsRepository = new Mock<IAccountsRepository>();
      _mockDataContext = new Mock<IDataContext>();
      
      // Mocking the controller
      _ownersController = new OwnersController(
         _mockOwnersRepository.Object,
         _mockDataContext.Object,
         _mapper,
         loggerFactory.CreateLogger<OwnersController>()
      );
      _accountsController = new AccountsController(
         _mockOwnersRepository.Object,
         _mockAccountsRepository.Object,
         _mockDataContext.Object,
         _mapper,
         loggerFactory.CreateLogger<AccountsController>()
      );

   }
}