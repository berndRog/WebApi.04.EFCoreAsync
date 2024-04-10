using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using WebApi.Core.DomainModel.Entities;
using WebApi.Core.Dto;
using Xunit;
namespace WebApiTest.Controllers.Moq;

[Collection(nameof(SystemTestCollectionDefinition))]
public class AccountsControllerUt : BaseControllerUt {

   [Fact]
   public async Task GetAccountById_Ok() {
      // Arrange
      _seed.InitAccountsForOwner1();
      var ownerId = _seed.Owner1.Id;
      var repoResult = new List<Account>() { _seed.Account1, _seed.Account2 };
      // mock the result of the repository
      _mockAccountsRepository.Setup(repository =>
            repository.FilterByAsync(a => a.OwnerId == ownerId))
                      .ReturnsAsync(repoResult);
      var expected = _mapper.Map<IEnumerable<AccountDto>>(repoResult);

      // Act
      var actionResult = await _accountsController.GetAccountsByOwnerId(ownerId);

      // Assert
      THelper.IsOk(actionResult!, expected);
   }

   [Fact]
   public async Task GetAccountById_EmptyList() {
      // Arrange
      _seed.InitAccountsForOwner1();
      var ownerId = _seed.Owner2.Id;
      var repoResult = new List<Account>();
      _mockAccountsRepository.Setup(repository => 
            repository.FilterByAsync(It.IsAny<Expression<Func<Account, bool>>>()))
         .ReturnsAsync(repoResult);
      var expectedDtos = _mapper.Map<IEnumerable<AccountDto>>(repoResult);

      // Act
      var actionResult = await _accountsController.GetAccountsByOwnerId(ownerId);

      // Assert
      THelper.IsOk(actionResult!, expectedDtos);
   }

 
   [Fact]
   public async Task GetAccountByIban_Ok() {
      // Arrange
      _seed.InitAccountsForOwner1();
      var repoResult = _seed.Account1;
      var iban = _seed.Account1.Iban;
      // mock the result of the repository
      _mockAccountsRepository.Setup(repository => 
            repository.FindByAsync(It.IsAny<Expression<Func<Account, bool>>>()))
                      .ReturnsAsync(repoResult);
      var expected = _mapper.Map<AccountDto>(repoResult);

      // Act
      var actionResult = await _accountsController.GetAccountByIban(iban);

      // Assert
      THelper.IsOk(actionResult, expected);
   }

   [Fact]
   public async Task GetAccountByIban_NotFound() {
      // Arrange
      _seed.InitAccountsForOwner1();
      Account? repoResult = null;
      var iban = _seed.Account8.Iban;
      // mock the result of the repository
      _mockAccountsRepository.Setup(repository => 
            repository.FindByAsync(It.IsAny<Expression<Func<Account, bool>>>()))
         .ReturnsAsync(repoResult);
      var expected = _mapper.Map<AccountDto>(repoResult);

      // Act
      var actionResult = await _accountsController.GetAccountByIban(iban);

      // Assert
      THelper.IsNotFound(actionResult);
   }
   
   [Fact]
   public async Task CreateAccount_Created() {
      // Arrange
      var owner = _seed.Owner1;
      var account = _seed.Account1;
      var accountDto = _mapper.Map<AccountDto>(_seed.Account1);
      var expected = accountDto with { OwnerId = owner.Id };
      
      // mock the repository's methods
      _mockOwnersRepository.Setup(repository => 
            repository.FindByIdAsync(owner.Id))
               .ReturnsAsync(owner);
      _mockAccountsRepository.Setup(repository => 
            repository.FindByIdAsync(account.Id))
               .ReturnsAsync((Account)null);
      _mockAccountsRepository.Setup(repository => 
            repository.Add(It.IsAny<Account>()))
               .Callback<Account>(a => account = a);
      _mockDataContext.Setup(context => context.SaveAllChangesAsync())
            .ReturnsAsync(true);
         
      // Act
      var actionResult = await _accountsController.CreateAccount(owner.Id, accountDto);

      // Assert
      THelper.IsCreated(actionResult!, expected);

      // Verify that the repository's Add method was called once
      _mockAccountsRepository.Verify(repository => 
         repository.Add(It.IsAny<Account>()), Times.Once);
      // Verify that the data context's SaveAllChangesAsync method was called once
      _mockDataContext.Verify(dataContext => 
         dataContext.SaveAllChangesAsync(), Times.Once);
   }

   [Fact]
   public async Task CreateAccount_Conflict() {
      // Arrange
      var owner = _seed.Owner1;
      var account = _seed.Account1;
      owner.Add(account);
      var accountDto = _mapper.Map<AccountDto>(_seed.Account1);
      // mock the repository's methods
      _mockOwnersRepository.Setup(repository => 
            repository.FindByIdAsync(owner.Id))
               .ReturnsAsync(owner);
      _mockAccountsRepository.Setup(repository => 
            repository.FindByIdAsync(account.Id))
               .ReturnsAsync(account);

      // Act
      var actionResult = await _accountsController.CreateAccount(owner.Id, accountDto);

      // Assert
      THelper.IsConflict(actionResult!);
      // Verify that the repository's Add method was not called
      _mockOwnersRepository
         .Verify(repository => repository.Add(It.IsAny<Owner>()), Times.Never);
      // Verify that the data context's SaveAllChangesAsync method was not called
      _mockDataContext
         .Verify(dataContext => dataContext.SaveAllChangesAsync(), Times.Never);

   }

}