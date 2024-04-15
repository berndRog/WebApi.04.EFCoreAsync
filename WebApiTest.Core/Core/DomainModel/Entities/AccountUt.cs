using FluentAssertions;
using WebApi.Core.DomainModel.Entities;
using WebApi.Core.Misc;

namespace WebApiTest.Core.DomainModel.Entities;
public class AccountUt {
   private readonly Seed _seed;

   public AccountUt(){
      _seed = new Seed();
   }
   
   [Fact]
   public void Ctor(){
      // Arrange
      // Act
      var actual = new Account();
      // Assert
      actual.Should().NotBeNull();
      actual.Should().BeOfType<Account>();
   }
   [Fact]
   public void ObjectInitializerUt(){
      // Arrange
      var owner = _seed.Owner1;
      // Act
      var actual = new Account{
         Id = _seed.Account1.Id,
         Iban = _seed.Account1.Iban,
         Balance = _seed.Account1.Balance,
         Owner = owner,
         OwnerId = owner.Id
      };
      // Assert
      actual.Should().NotBeNull();
      actual.Should().BeOfType<Account>();
      actual.Id.Should().Be(_seed.Account1.Id);
      actual.Iban.Should().Be(_seed.Account1.Iban);
      actual.Balance.Should().Be(_seed.Account1.Balance);
      actual.Owner.Should().BeEquivalentTo(owner);
      actual.OwnerId.Should().Be(owner.Id);
   }
   
   [Fact]
   public void CreateIbanUt(){
      // Arrange
      var owner = _seed.Owner1;
      // Act
      var actual = new Account{
         Id = _seed.Account1.Id,
         Balance = _seed.Account1.Balance,
         Owner = owner,
         OwnerId = owner.Id
      };
      string ibanGrouped = actual.Iban.AsIban();
      // Assert
      actual.Should().NotBeNull();
      actual.Should().BeOfType<Account>();
      actual.Id.Should().Be(_seed.Account1.Id);
      actual.Balance.Should().Be(_seed.Account1.Balance);
      actual.Owner.Should().BeEquivalentTo(owner);
      actual.OwnerId.Should().Be(owner.Id);
   }
   
   [Fact]
   public void GetterUt(){
      // Arrange
      var owner = _seed.Owner1;
      var actual = new Account{
         Id = _seed.Account1.Id,
         Iban = _seed.Account1.Iban,
         Balance = _seed.Account1.Balance,
         Owner = owner,
         OwnerId = owner.Id
      };
      // Act
      var actualId = actual.Id;
      var actualIban = actual.Iban;
      var actualBalance = actual.Balance;
      var actualOwner = actual.Owner;
      var actualOwnerId = actual.OwnerId;
      // Assert
      actualId.Should().Be(_seed.Account1.Id);
      actualIban.Should().Be(_seed.Account1.Iban);
      actualBalance.Should().Be(_seed.Account1.Balance);
      actualOwner.Should().Be(owner);
      actualOwnerId.Should().Be(owner.Id);
   }
   [Fact]
   public void SetterUt(){
      // Arrange
      var owner = _seed.Owner1;
      var actual = new Account {
         Id = _seed.Account1.Id,
         Iban = _seed.Account1.Iban,
         Balance = _seed.Account1.Balance
      };
      // Act, Setter
      actual.Owner = owner;
      actual.OwnerId = owner.Id;
      // Assert
      actual.Id.Should().Be(_seed.Account1.Id);
      actual.Iban.Should().Be(_seed.Account1.Iban);
      actual.Balance.Should().Be(_seed.Account1.Balance);
      actual.Owner.Should().Be(owner);
      actual.OwnerId.Should().Be(owner.Id);
   }
   
}