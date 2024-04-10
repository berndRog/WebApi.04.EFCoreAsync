using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Core.Dto;
using Xunit;
namespace WebApiTest.Controllers;
[Collection(nameof(SystemTestCollectionDefinition))]
public class AccountsControllerTest: BaseControllerTest {

   [Fact]
   public async Task GetAccountsByOwnerIdTest() {
      // Arrange
      await _arrangeTest.Owner1WithAccountsAsync(_seed);
      var expected = new List<AccountDto> {
         _mapper.Map<AccountDto>(_seed.Account1),
         _mapper.Map<AccountDto>(_seed.Account2)
      };
     
      // Act
      var actionResult = await _accountsController.GetAccountsByOwnerId(_seed.Owner1.Id);
      
      // Assert
      THelper.IsOk(actionResult!, expected);
   }
   
   [Fact]
   public async Task GetAccountByIdTest() {
      // Arrange
      await _arrangeTest.Owner1WithAccountsAsync(_seed);
      var expected = _mapper.Map<AccountDto>(_seed.Account1);
      
      // Act
      var actionResult = await _accountsController.GetAccountById(_seed.Account1.Id);
      
      // Assert
      THelper.IsOk(actionResult, expected);
   }

   [Fact]
   public async Task GetAccountByIbanTest() {
      // Arrange
      await _arrangeTest.OwnersWithAccountsAsync(_seed);
      var expected = _mapper.Map<AccountDto>(_seed.Account6);

      // Act
      var actionResult = await _accountsController.GetAccountByIban("DE50 10000000 0000000000");

      // Assert
      THelper.IsOk(actionResult, expected);
   }
   
   [Fact]
   public async Task CreateAccountTest() {
      // Arrange
      _ownersRepository.Add(_seed.Owner1);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();   
      var account1Dto = _mapper.Map<AccountDto>(_seed.Account1);
      var expected = account1Dto with { OwnerId = _seed.Owner1.Id };
      // Act
      var actionResult = 
         await _accountsController.CreateAccount(_seed.Owner1.Id, account1Dto);
      
      // Assert
      THelper.IsCreated(actionResult!, expected);
   }
   
   [Fact]
   public async Task DeleteAccountTest() {
      // Arrange
      await _arrangeTest.OwnersWithAccountsAsync(_seed);
      var owner = _seed.Owner1;
      var account = _seed.Account1;
      
      // Act
      var actionResult = await _accountsController.DeleteAccount(owner.Id, account.Id);      
      
      // Assert
      THelper.IsNoContent(actionResult);
   }
}