using FluentAssertions;
using WebApi.Core.Dto;
namespace WebApiTest.Core.Dto;

public class OwnerDtoUt {
   
   private readonly Seed _seed;

   public OwnerDtoUt(){ _seed = new Seed(); }
   
   #region OwnerDto 
   [Fact]
   public void CtorUt(){
      // Arrange
      // Act
      var actual = new OwnerDto(
         Id: _seed.Owner1.Id,
         Name: _seed.Owner1.Name,
         Birthdate: _seed.Owner1.Birthdate,
         Email: _seed.Owner1.Email
      );
      // Assert
      actual.Should().NotBeNull();
      actual.Should().BeOfType<OwnerDto>();
      actual.Id.Should().Be(_seed.Owner1.Id);
      actual.Name.Should().Be(_seed.Owner1.Name);
      actual.Birthdate.Should().Be(_seed.Owner1.Birthdate);
      actual.Email.Should().Be(_seed.Owner1.Email);
   }
   #endregion
}