using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using WebApi.Core.DomainModel.Entities;
namespace WebApiTest.Persistence.Repositories;
[Collection(nameof(SystemTestCollectionDefinition))]
public  class OwnersRepositoryUt: BaseRepositoryUt {
   
   private void ShowRepository(string text){
#if DEBUG
      _dataContext.LogChangeTracker(text);
#endif
   }
   
   #region without account 
   [Fact]
   public async Task SelectAsyncUt() {
      // Arrange
      _ownersRepository.AddRange(_seed.Owners);
      ShowRepository("AddOwners");
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();
      
      // Act  with tracking
      var actual = await _ownersRepository.SelectAsync();   
      // Assert
      actual.Should()
         .NotBeNull().And
         .NotBeEmpty().And
         .HaveCount(6).And
         .BeEquivalentTo(_seed.Owners);
   }

   [Fact]
   public async Task FilterByAsyncNameUt() {
      // Arrange
      _ownersRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      var expected = new List<Owner> { _seed.Owner1, _seed.Owner2 };
      _dataContext.ClearChangeTracker();
      
      // Act
      var actual = 
         await _ownersRepository.FilterByAsync(o => o.Name.Contains("Mustermann"));   
      // Assert
      ShowRepository("FilterByName");
      actual.Should()
         .NotBeNull().And
         .HaveCount(2).And
         .BeEquivalentTo(expected);
   }

   [Fact]
   public async Task FilterByAsyncBirthdateUt() {
      // Arrange
      _ownersRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();
      var birthdateMin = new DateTime(1980, 01, 01).ToUniversalTime();
      var birthdateMax = new DateTime(1989, 12, 31).ToUniversalTime();
      var expected = new List<Owner> { _seed.Owner1, _seed.Owner2 };
      
      // Act
      var actual = 
         await _ownersRepository.FilterByAsync(o => o.Birthdate >= birthdateMin &&
                                                    o.Birthdate <= birthdateMax);   
      // Assert
      ShowRepository("FilterByBirthdate");
      actual.Should()
         .NotBeNull().And
         .HaveCount(2).And
         .BeEquivalentTo(expected);
   }

   [Fact]
   public async Task FilterByEmailUt() {
      // Arrange
      _ownersRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();
      var expected = new List<Owner> { _seed.Owner2, _seed.Owner4 };
      
      // Act
      var actual = 
         await _ownersRepository.FilterByAsync(o => o.Email.Contains("gmail"));   
      
      // Assert
      ShowRepository("FilterByEmail");
      actual.Should()
         .NotBeNull().And
         .HaveCount(2).And
         .BeEquivalentTo(expected);
   }
   
   [Fact]
   public async Task FilterByEmailAndBirthdateUt() {
      // Arrange
      _ownersRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();
      var birthdateMin = new DateTime(1960, 01, 01).ToUniversalTime();
      var birthdateMax = new DateTime(1969, 12, 31).ToUniversalTime();
      var expected = new List<Owner> { _seed.Owner4 };
      
      // Act
      var actual =
         await _ownersRepository.FilterByAsync(o => 
            o.Email.Contains("gmail") &&
            o.Birthdate >= birthdateMin &&
            o.Birthdate <= birthdateMax);   
      
      // Assert
      ShowRepository("FilterByEmailAndBirthdate");
      actual.Should()
         .NotBeNull().And
         .HaveCount(1).And
         .BeEquivalentTo(expected);
   }

   [Fact]
   public async Task FindByIdAsyncUt() {
      // Arrange
      _ownersRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();
      
      // Act
      var actual = await _ownersRepository.FindByIdAsync(_seed.Owner1.Id);
      
      // Assert
      ShowRepository("FindByIdAsync");
      actual.Should()
         .NotBeNull().And
         .BeEquivalentTo(_seed.Owner1);
   }
   
   [Fact]
   public async Task FindByAsyncNameUt() {
      // Arrange
      _ownersRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();
      var expected = _seed.Owner2;
      
      // Act
      var actual = await _ownersRepository.FindByAsync(o => 
         o.Name.Contains("Max"));   
      
      // Assert
      ShowRepository("FindByIdName");
      actual.Should()
         .NotBeNull().And
         .BeEquivalentTo(expected);
   }

   [Fact]
   public async Task FindByAsyncBirthdateUt() {
      // Arrange
      _ownersRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();
      var expected = _seed.Owner6;
      
      // Act
      var actual = await _ownersRepository.FindByAsync(o => 
         o.Birthdate == expected.Birthdate);   
      
      // Assert
      ShowRepository("FindByBirthdate");
      actual.Should()
         .NotBeNull().And
         .BeEquivalentTo(expected);
   }

   [Fact]
   public async Task FindByEmailUt() {
      // Arrange
      _ownersRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();
      var expected = _seed.Owner4;
      
      // Act
      var actual = 
         await _ownersRepository.FindByAsync(o => o.Email.Contains(expected.Email));   
      
      // Assert
      ShowRepository("FindByEmail");
      actual.Should().NotBeNull()
         .And.BeEquivalentTo(expected);
   }
   
   [Fact]
   public async Task FindByNameAndBirthdateUt() {
      // Arrange
      _ownersRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();
      var expected = _seed.Owner2;
      
      // Act
      var actual = 
         await _ownersRepository.FindByAsync(o => o.Name.Contains("Max") && 
                                     o.Birthdate == expected.Birthdate);   
      
      // Assert
      ShowRepository("FindByNameAndBirthdate");
      actual.Should().NotBeNull().And
         .BeEquivalentTo(expected);
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
      
      // Act
      _ownersRepository.Add(owner);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();
      
      // Assert
      var actual = await _ownersRepository.FindByIdAsync(owner.Id);
      ShowRepository("AddUt");
      actual.Should().BeEquivalentTo(owner);
   }
   
   [Fact]
   public async Task AddRangeUt() {
      // Arrange
      var expected = _seed.Owners;
      
      // Act
      _ownersRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();
      
      // Assert                         with tracking
      var actual = await _ownersRepository.SelectAsync(true);   
      ShowRepository("AddRangeUt");
      actual.Should().NotBeNull()
         .And.NotBeEmpty()
         .And.HaveCount(6)
         .And.BeEquivalentTo(expected);
   }
   
   [Fact]
   public async Task UpdateUt() {
      // Arrange
      await _arrangeTest.OwnersAsync(_seed);
      var expected = _seed.Owner1;
      
      // Act
      var updatedOwner = new Owner {
         Id = expected.Id,
         Name = "Erika Meier",
         Birthdate = expected.Birthdate,
         Email = "erika.meier@icloud.com"
      };
      await _ownersRepository.UpdateAsync(updatedOwner);
      await _dataContext.SaveAllChangesAsync();
      
      // Assert
      var actual = 
         await _ownersRepository.FindByIdAsync(updatedOwner.Id);
      ShowRepository("UpdateUt");
      actual.Should().BeEquivalentTo(updatedOwner);
   }
   
   [Fact]
   public async Task RemoveUt() {
      // Arrange
      await _arrangeTest.OwnersAsync(_seed);
      
      // Act
      _ownersRepository.Remove(_seed.Owner1);
      await _dataContext.SaveAllChangesAsync();
      
      // Assert
      var actual = 
         await _ownersRepository.FindByIdAsync(_seed.Owner1.Id);
      ShowRepository("RemoveUt");
      actual.Should().BeNull();
   }
   #endregion
   
   #region with accounts
   [Fact]
   public async Task FilterByJoinWithTracking_WithoutJoinAsyncUt() {
   
      // Arrange
      await _arrangeTest.OwnersWithAccountsAsync(_seed);
    
      // Act  with tracking
      var actual = 
         await _ownersRepository.FilterByJoinAsync(null, false, true);
     
      // Assert
      ShowRepository("FilterByJoinWithTracking_WithoutJoinAsyncUt");
      actual.Should()
         .NotBeNull().And
         .NotBeEmpty().And
         .HaveCount(6).And
         .BeOfType<List<Owner>>();
      var owners = (List<Owner>)actual;
      owners[0].Accounts.Should().HaveCount(0);
      owners[1].Accounts.Should().HaveCount(0);
      owners[2].Accounts.Should().HaveCount(0);
      owners[3].Accounts.Should().HaveCount(0);
      owners[4].Accounts.Should().HaveCount(0);
      owners[5].Accounts.Should().HaveCount(0);
   }

   [Fact]
   public async Task FilterByJoinAsync_WithTracking_WithJoinUt() {
      
      // Arrange
      await _arrangeTest.OwnersWithAccountsAsync(_seed);
      var expected = _seed.Owners;
      
      // Act  with tracking
      var actual = 
         await _ownersRepository.FilterByJoinAsync(null, true, true);

      // Assert
      ShowRepository("FilterByJoinAsync_WithTracking_WithJoinUt");
      actual.Should()
         .NotBeNull().And
         .HaveCount(6).And
         .BeOfType<List<Owner>>();
     actual.Should().BeEquivalentTo(expected, options => {
        options.For(owner => owner.Accounts).Exclude(accout => accout.Owner);
        options.IgnoringCyclicReferences(); 
        return options;
     });
   }
   private EquivalencyAssertionOptions<Owner> ExcludeReferences(
      EquivalencyAssertionOptions<Owner> options
   ) {
      options.For(owner => owner.Accounts).Exclude(accout => accout.Owner);
      options.IgnoringCyclicReferences();
      return options;
   }
   
   [Fact]
   public async Task FilterByOwner1JoinAsync_WithTracking_WithJoinUt() {
      // Arrange
      await _arrangeTest.OwnersWithAccountsAsync(_seed); 
      var expected = new List<Owner> { _seed.Owner1 };
      
      // Act  with tracking
      var actual = await _ownersRepository
         .FilterByJoinAsync(o => o.Id ==_seed.Owner1.Id, true, true);
      
      // Assert
      ShowRepository("FilterByOwner1JoinAsync_WithTracking_WithJoinUt");
      actual.Should()
         .NotBeNull().And
         .HaveCount(1).And
         .BeOfType<List<Owner>>();
     
      actual.Should().BeEquivalentTo(expected, options => {
         options.For(owner => owner.Accounts).Exclude(accout => accout.Owner);
         options.IgnoringCyclicReferences(); 
         return options;
      });
   }
   
   [Fact]
   public async Task RemoveWithAccountsUt() {
      // Arrange
      await _arrangeTest.OwnersWithAccountsAsync(_seed);
      
      // Act
      _ownersRepository.Remove(_seed.Owner1);
      await _dataContext.SaveAllChangesAsync();
      
      // Assert
      var actual = 
         await _ownersRepository.FindByIdAsync(_seed.Owner1.Id);
      ShowRepository("RemoveUt");
      actual.Should().BeNull();
   }
   #endregion
}