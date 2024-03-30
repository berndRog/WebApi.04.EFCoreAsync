using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Controllers;
using WebApi.Core;
using WebApi.Core.Dto;
using WebApi.Persistence;
using WebApiTest.Di;
using WebApiTest.Persistence;

namespace WebApiTest.Controllers;
[Collection(nameof(SystemTestCollectionDefinition))]
public class AccountsControllerTest {

   private readonly AccountsController _accountsController;
   private readonly IOwnersRepository _ownersRepository;
   private readonly IDataContext _dataContext;
   private readonly ArrangeTest _arrangeTest;
   private readonly IMapper _mapper;
   private readonly Seed _seed;

   public AccountsControllerTest() {

      IServiceCollection serviceCollection = new ServiceCollection();
      serviceCollection.AddPersistenceTest();
      serviceCollection.AddControllersTest();
      ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider()
         ?? throw new Exception("Failed to build Serviceprovider");

      DataContext dbContext = serviceProvider.GetRequiredService<DataContext>()
         ?? throw new Exception("Failed to create an instance of AppDbContext");
      dbContext.Database.EnsureDeleted();
      dbContext.Database.EnsureCreated();
      
      _dataContext = serviceProvider.GetRequiredService<IDataContext>() 
         ?? throw new Exception("Failed to create an instance of IDataContext");
      
      _ownersRepository = serviceProvider.GetRequiredService<IOwnersRepository>()
         ?? throw new Exception("Failed to create an instance of IOwnersRepository");
      _accountsController = serviceProvider.GetRequiredService<AccountsController>()
         ?? throw new Exception("Failed to create an instance of AccountsController");

      _arrangeTest = serviceProvider.GetRequiredService<ArrangeTest>()
         ?? throw new Exception("Failed create an instance of CArrangeTest");
      _mapper = serviceProvider.GetRequiredService<IMapper>();
      _seed = new Seed();
   }

   [Fact]
   public async Task GetTest() {
      // Arrange
      await _arrangeTest.Owner1WithAccountsAsync(_seed);
      var expected = new List<AccountDto> {
         _mapper.Map<AccountDto>(_seed.Account1),
         _mapper.Map<AccountDto>(_seed.Account2),
      };
     
      // Act
      ActionResult<IEnumerable<AccountDto>> response =
         await _accountsController.GetAccountsByOwnerId(_seed.Owner1.Id);
      
      // Assert
      var (success, result, value) =
         Helper.ResultFromResponse<OkObjectResult, IEnumerable<AccountDto>>(response);
      result.StatusCode.Should().Be(200);
      value.Should().NotBeNull().And
         .HaveCount(2);
      IList<AccountDto> actual = value.ToList(); 
      actual[0].Should().BeEquivalentTo(expected[0]);
      actual[1].Should().BeEquivalentTo(expected[1]);
   }
   [Fact]
   public async Task GetByIdTest() {
      // Arrange
      await _arrangeTest.Owner1WithAccountsAsync(_seed);
      AccountDto expected = _mapper.Map<AccountDto>(_seed.Account1);
      
      // Act
      ActionResult<AccountDto?> response = 
         await _accountsController.GetAccountById(_seed.Account1.Id);
      
      // Assert
      var (success, result, value) =
         Helper.ResultFromResponse<OkObjectResult, AccountDto>(response!);
      result.StatusCode.Should().Be(200);
      value.Should().NotBeNull().And
         .BeEquivalentTo(expected);
   }

   [Fact]
   public async Task GetByIbanTest() {
      // Arrange
      await _arrangeTest.OwnersWithAccountsAsync(_seed);
      var account6Dto = _mapper.Map<AccountDto>(_seed.Account6);
      // Act
      ActionResult<AccountDto?> response = 
         await _accountsController.GetAccountByIban("DE50 10000000 0000000000");
      // Assert
      response.Should().NotBeNull();
      var (success, result, value) =
         Helper.ResultFromResponse<OkObjectResult, AccountDto>(response!);
      result.StatusCode.Should().Be(200);
      value.Should().NotBeNull().And
         .BeEquivalentTo(account6Dto);
   }

   [Fact]
   public async Task GetByAccountByIdNotFoundTest() {
      // Arrange
      await _arrangeTest.Owner1WithAccountsAsync(_seed);
      Guid idError = new Guid("12345678-0000-0000-0000-000000000000");
      // Act
      ActionResult<AccountDto?> response =
         await _accountsController.GetAccountById(idError);
      // Assert
      response.Should().NotBeNull();
      var (success, result, value) =
         Helper.ResultFromResponse<NotFoundObjectResult, AccountDto>(response!);
      result.StatusCode.Should().Be(404);
   }
   
   [Fact]
   public async Task CreateAccountTest() {
      // Arrange
      _ownersRepository.Add(_seed.Owner1);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();   
      AccountDto account1Dto = _mapper.Map<AccountDto>(_seed.Account1);
      
      // Act
      ActionResult<AccountDto> response =
         await _accountsController.CreateAccount(_seed.Owner1.Id, account1Dto);
      
      // Assert
      var (success, result, value) =
         Helper.ResultFromResponse<CreatedResult, AccountDto>(response);
      result.StatusCode.Should().Be(201);
      value.Should().NotBeNull().And
         .BeEquivalentTo(account1Dto);
   }
   [Fact]
   public async Task CreateAccountConflictTest() {
      // Arrange
      _ownersRepository.Add(_seed.Owner1);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();   
      _seed.Owner1.Add(_seed.Account1);
      AccountDto account1Dto = _mapper.Map<AccountDto>(_seed.Account1);
      await _accountsController.CreateAccount(_seed.Owner1.Id, account1Dto);
      // Act
      ActionResult<AccountDto> response
         = await _accountsController.CreateAccount(_seed.Owner1.Id, account1Dto);
      // Assert
      var (success, result, value) = 
         Helper.ResultFromResponse<ConflictObjectResult, AccountDto>(response);
      result.StatusCode.Should().Be(409);
   }
}