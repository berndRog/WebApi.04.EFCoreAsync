using System.Collections.Generic;
using FluentAssertions;
using WebApi.Core.DomainModel.Entities;
namespace WebApiTest.Core.DomainModel.Entities;
public class OwnerUt {

   private readonly Seed _seed;

   public OwnerUt() {
      _seed = new Seed();
   }

   #region without Account
   [Fact]
   public void Ctor() {
      // Arrange
      // Act
      Owner actual = new ();
      // Assert
      actual.Should().NotBeNull();
      actual.Should().BeOfType<Owner>();
   }

   [Fact]
   public void ObjectInitializerUt() {
      // Arrange
      // Act
      Owner actual = new () {
         Id = _seed.Owner1.Id,
         Name = _seed.Owner1.Name,
         Birthdate = _seed.Owner1.Birthdate,
         Email = _seed.Owner1.Email
      };
      // Assert
      actual.Should().NotBeNull();
      actual.Should().BeOfType<Owner>();
      actual.Id.Should().Be(_seed.Owner1.Id);
      actual.Name.Should().Be(_seed.Owner1.Name);
      actual.Birthdate.Should().Be(_seed.Owner1.Birthdate);
      actual.Email.Should().Be(_seed.Owner1.Email);
   }
   [Fact]
   public void GetterUt() {
      // Arrange
      Owner actual = new () {
         Id = _seed.Owner1.Id,
         Name = _seed.Owner1.Name,
         Birthdate = _seed.Owner1.Birthdate,
         Email = _seed.Owner1.Email
      };
      // Act
      var actualId = actual.Id;
      var actualName = actual.Name;
      var actualBirthdate = actual.Birthdate;
      var actualEmail = actual.Email;
      // Assert
      actualId.Should().Be(_seed.Owner1.Id);
      actualName.Should().Be(_seed.Owner1.Name);
      actualBirthdate.Should().Be(_seed.Owner1.Birthdate);
      actualEmail.Should().Be(_seed.Owner1.Email);
   }

   [Fact]
   public void SetterUt() {
      // Arrange
      Owner actual = new ()  {
         Id = _seed.Owner1.Id,
         Birthdate = _seed.Owner1.Birthdate,
         // Act
         Name = _seed.Owner1.Name,
         Email = _seed.Owner1.Email
      };
      // Assert
      actual.Id.Should().Be(_seed.Owner1.Id);
      actual.Name.Should().Be(_seed.Owner1.Name);
      actual.Birthdate.Should().Be(_seed.Owner1.Birthdate);
      actual.Email.Should().Be(_seed.Owner1.Email);
   }
   #endregion

   #region with Accounts   
   [Fact]
   public void OwnerAddAccountUt() {
      // Arrange
      // Act
      _seed.Owner1.Add(_seed.Account1);
      var expected = new List<Account> { _seed.Account1 };
      // Assert
      _seed.Owner1.Accounts.Should()
         .HaveCount(1).And
         .BeEquivalentTo(expected);
   }
   #endregion
}