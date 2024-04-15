using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Core.DomainModel.Entities;
using WebApi.Core.Dto;
using Xunit;
namespace WebApiTest.Controllers;
[Collection(nameof(SystemTestCollectionDefinition))]
public class OwnersControllerTest: BaseControllerTest {

   [Fact]
   public async Task GetOwners() {
      // Arrange
      await _arrangeTest.OwnersAsync(_seed);
      var expected = _mapper.Map<IEnumerable<OwnerDto>>(_seed.Owners); 
      
      // Act
      var actionResult = await _ownersController.GetOwners();
      
      // Assert
      THelper.IsEnumerableOk(actionResult, expected);
   }
   
   [Fact]
   public async Task GetOwnerByIdTest() {
      // Arrange
      await _arrangeTest.OwnersAsync(_seed);
      var expected = _mapper.Map<OwnerDto>(_seed.Owner1); 
      
      // Act
      var actionResult = await _ownersController.GetOwnerById(_seed.Owner1.Id);
      
      // Assert
      THelper.IsOk(actionResult, expected);
   }

   [Fact]
   public async Task GetOwnerByNameTest() {
      // Arrange
      var owner = _seed.Owner1;
      await _arrangeTest.OwnersAsync(_seed);
      var expected = _mapper.Map<OwnerDto>(owner);
      
      // Act
      var actionResult = await _ownersController.GetOwnerByName(owner.Name);
     
      // Assert
      THelper.IsOk(actionResult, expected);
   }
   
   [Fact]
   public async Task GetOwnerByEmailTest() {
      // Arrange
      await _arrangeTest.OwnersAsync(_seed);
      var email = _seed.Owner1.Email;
      var expected = _mapper.Map<OwnerDto>(_seed.Owner1);    
      
      // Act
      var actionResult = await _ownersController.GetOwnerByEmail(email);
      
      // Assert
      THelper.IsOk(actionResult, expected);
   }

   [Fact]
   public async Task GetOwnersByBirthDate() {
      // Arrange
      await _arrangeTest.OwnersAsync(_seed);
      var expected = 
         _mapper.Map<IEnumerable<OwnerDto>>(new List<Owner> { _seed.Owner3, _seed.Owner4 });
      
      // Act
      var actionResult = 
         await _ownersController.GetOwnersByBirthdate("1960-01-01", "1969-12-31");
      
      // Assert
      THelper.IsOk(actionResult!, expected!);
   }
   
   [Fact]
   public async Task CreateOwnerTest() {
      // Arrange
      var owner1Dto = _mapper.Map<OwnerDto>(_seed.Owner1); 
      
      // Act
      var actionResult = await _ownersController.CreateOwner(owner1Dto);
      
      // Assert
      THelper.IsCreated(actionResult!, owner1Dto);
   }

   [Fact]
   public async Task UpdateOwnerTest() {
      // Arrange
      await _arrangeTest.OwnersAsync(_seed);
      var owner1Dto = _mapper.Map<OwnerDto>(_seed.Owner1);
      var updatedOwner1Dto = owner1Dto with {
         Name = "Erika Meier", 
         Email = "erika.meier@icloud.com"
      };
      
      // Act
      var actionResult = 
         await _ownersController.UpdateOwner(owner1Dto.Id, updatedOwner1Dto);      
      
      // Assert
      THelper.IsOk(actionResult!, updatedOwner1Dto);
   }
   
   [Fact]
   public async Task DeleteOwnerTest() {
      // Arrange
      await _arrangeTest.OwnersAsync(_seed);
      var id = _seed.Owner1.Id;
      
      // Act
      var actionResult = await _ownersController.DeleteOwner(id);      
      
      // Assert
      THelper.IsNoContent(actionResult);
   }
}