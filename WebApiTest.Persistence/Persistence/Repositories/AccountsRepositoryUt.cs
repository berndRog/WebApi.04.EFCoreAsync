using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using WebApi.Core.DomainModel.Entities;
//using WebApiTest.Di;

namespace WebApiTest.Persistence.Repositories;
[Collection(nameof(SystemTestCollectionDefinition))]
public  class AccountsRepositoryUt: BaseRepositoryUt {
   
   #region Owners <-> Accounts   
   private EquivalencyAssertionOptions<Account> ExcludeReferences(
      EquivalencyAssertionOptions<Account> options
   ){
      options.Excluding(account => account.Owner);
      return options;
   }

   [Fact]
   public async Task AddUt() {
      // Arrange
      Owner owner = new(){
         Id = new Guid("10000000-0000-0000-0000-000000000000"),
         Name = "Erika Mustermann",
         Birthdate = new DateTime(1988, 2, 1).ToUniversalTime(),
         Email = "erika.mustermann@t-online.de"
      };  
      Account account = new(){
         Id = new Guid("01000000-0000-0000-0000-000000000000"),
         Iban = "DE10 10000000 0000000001",
         Balance = 2100.0
      };
      
      // Act 
      owner.Add(account);        // DomainModel
      _accountsRepository.Add(account);
      await _dataContext.SaveAllChangesAsync();
      
      // Assert
      _dataContext.ClearChangeTracker(); // clear repository cache
      var actual = await _accountsRepository.FindByIdAsync(account.Id);
      actual.Should()
         .NotBeNull().And
         .BeEquivalentTo(account, options => options.Excluding(a => a.Owner));
   }
   
   [Fact]
   public async Task FindByIdUt() {
      // Arrange Owner1 with Account 1
      await _arrangeTest.OwnerWith1AccountAsync(_seed);  // repository cache is cleared
      
      // Act 
      // no join operation -> i.e. no references loaded from database
      var actual = await _accountsRepository.FindByIdAsync(_seed.Account1.Id);
      
      // Assert
      _dataContext.LogChangeTracker("FindbyId");
      actual.Should().BeEquivalentTo(_seed.Account1, options => options.Excluding(a => a.Owner));
   }
   [Fact]
   public async Task FindByIbanUt() { 
      // Arrange
      await _arrangeTest.OwnersWithAccountsAsync(_seed); // repository cache is cleared
      // Act 
      var actual =  
         await _accountsRepository.FindByAsync(o => o.Iban.Contains("DE20 1000"));
      // Assert
      _dataContext.LogChangeTracker("FindbyIban");
      actual.Should().BeEquivalentTo(_seed.Account3, options => options.Excluding(a => a.Owner));
   }
   
   [Fact]
   public async Task SelectByOwner1IdUt() { 
      // Arrange
      await _arrangeTest.OwnersWithAccountsAsync(_seed); // repository cache is cleared
      var expected = new List<Account>{ _seed.Account1, _seed.Account2 };
      
      // Act 
      var actual =  
         await _accountsRepository.SelectByOwnerIdJoinAsync(_seed.Owner1.Id, true, true);
      
      // Assert
      _dataContext.LogChangeTracker("SelectByOwner1IdJoinAsync");
      actual.Should().BeEquivalentTo(expected, options => options.IgnoringCyclicReferences());
   }

   #endregion
}