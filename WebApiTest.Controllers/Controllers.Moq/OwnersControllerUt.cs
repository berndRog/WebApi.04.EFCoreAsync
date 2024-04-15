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
public class OwnersControllerUt : BaseControllerUt {
   [Fact]
   public async Task GetOwners() {
      // Arrange
      var repoResult = _seed.Owners;
      // mock the result of the repository
      _mockOwnersRepository.Setup(r => r.SelectAsync(false))
         .ReturnsAsync(repoResult);
      var expected = _mapper.Map<IEnumerable<OwnerDto>>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwners();

      // Assert
      THelper.IsEnumerableOk(actionResult, expected);
   }

   [Fact]
   public async Task GetOwnerById_Ok() {
      // Arrange
      var id = _seed.Owner1.Id;
      var repoResult = _seed.Owner1;
      // mock the result of the repository
      _mockOwnersRepository.Setup(r => r.FindByIdAsync(id))
         .ReturnsAsync(repoResult);
      var expected = _mapper.Map<OwnerDto>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnerById(id);

      // Assert
      THelper.IsOk(actionResult, expected);
   }

   [Fact]
   public async Task GetOwnerById_NotFound() {
      // Arrange
      var id = Guid.NewGuid();
      var repoResult = (Owner?)null;
      _mockOwnersRepository.Setup(r => r.FindByIdAsync(id))
         .ReturnsAsync(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnerById(id);

      // Assert
      THelper.IsNotFound(actionResult);
   }

   [Fact]
   public async Task GetOwnerByName_Ok() {
      // Arrange
      var name = _seed.Owner1.Name;
      var repoResult = _seed.Owner1;
      _mockOwnersRepository.Setup(r => r.FindByAsync(o => o.Name == name))
         //                            r.FindByAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
         .ReturnsAsync(repoResult);
      var expected = _mapper.Map<OwnerDto?>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnerByName(name);

      // Assert
      THelper.IsOk(actionResult, expected);
   }

   [Fact]
   public async Task GetOwnerByName_NotFound() {
      // Arrange
      var name = "Micky Mouse";
      var repoResult = _seed.Owner1;
      _mockOwnersRepository.Setup(r => r.FindByAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
         .ReturnsAsync(repoResult);
      var expected = _mapper.Map<OwnerDto>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnerByName(name);

      // Assert
      THelper.IsOk(actionResult, expected);
   }

   [Fact]
   public async Task GetOwnerByEmail_Ok() {
      // Arrange
      var email = _seed.Owner1.Email;
      var repoResult = _seed.Owner1;
      // mock the result of the repository
      _mockOwnersRepository.Setup(r => r.FindByAsync(owner => owner.Email == email))
         //                            r.FindByAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
         .ReturnsAsync(repoResult);
      var expected = _mapper.Map<OwnerDto>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnerByEmail(email);

      // Assert
      THelper.IsOk(actionResult, expected);
   }

   [Fact]
   public async Task GetOwnerByEmail_NotFound() {
      // Arrange
      var email = "a.b@c.de";
      Owner? repoResult = null;
      _mockOwnersRepository.Setup(r => r.FindByAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
         .ReturnsAsync(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnerByEmail(email);

      // Assert
      THelper.IsNotFound(actionResult);
   }

   [Fact]
   public async Task GetOwnersByBirthDate_Ok() {
      // Arrange
      var repoResult = new List<Owner> { _seed.Owner3, _seed.Owner4 };
      _mockOwnersRepository.Setup(r => r.FilterByAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
         .ReturnsAsync(repoResult);
      var expected = _mapper.Map<IEnumerable<OwnerDto>>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnersByBirthdate(
         "1960-01-01", "1969-12-31");

      // Assert
      THelper.IsEnumerableOk(actionResult, expected);
   }

   [Fact]
   public async Task GetOwnersByBirthDate_EmptyList() {
      // Arrange
      var repoResult = new List<Owner>();
      _mockOwnersRepository.Setup(r => r.FilterByAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
         .ReturnsAsync(repoResult);
      var expected = _mapper.Map<IEnumerable<OwnerDto>>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnersByBirthdate(
         "1950-01-01", "1959-12-31"
      );

      // Assert
      THelper.IsEnumerableOk(actionResult, expected);
   }

   [Fact]
   public async Task CreateOwner_Created() {
      // Arrange
      var owner1Dto = _mapper.Map<OwnerDto>(_seed.Owner1);
      Owner? addedOwner = null;
      // mock the repository's Add method
      _mockOwnersRepository.Setup(r => r.Add(It.IsAny<Owner>()))
         .Callback<Owner>(owner => addedOwner = owner);
      // mock the data context's SaveAllChangesAsync method
      _mockDataContext.Setup(c => c.SaveAllChangesAsync())
         .ReturnsAsync(true);

      // Act
      var actionResult = await _ownersController.CreateOwner(owner1Dto);

      // Assert
      THelper.IsCreated(actionResult!, owner1Dto);
      // Verify that the repository's Add method was called once
      _mockOwnersRepository.Verify(r => r.Add(It.IsAny<Owner>()), Times.Once);
      // Verify that the data context's SaveAllChangesAsync method was called once
      _mockDataContext.Verify(c => c.SaveAllChangesAsync(), Times.Once);
   }

   [Fact]
   public async Task CreateOwner_Conflict() {
      // Arrange
      var owner1Dto = _mapper.Map<OwnerDto>(_seed.Owner1);
      // mock the repository's FindByIdAsync method to return an existing owner
      _mockOwnersRepository.Setup(r => r.FindByIdAsync(owner1Dto.Id))
         .ReturnsAsync(_seed.Owner1);

      // Act
      var actionResult = await _ownersController.CreateOwner(owner1Dto);

      // Assert
      THelper.IsConflict(actionResult!);
      // Verify that the repository's Add method was not called
      _mockOwnersRepository.Verify(r => r.Add(It.IsAny<Owner>()), Times.Never);
      // Verify that the data context's SaveAllChangesAsync method was not called
      _mockDataContext.Verify(c => c.SaveAllChangesAsync(), Times.Never);
   }

   [Fact]
   public async Task UpdateOwner_Created() {
      // Arrange
      var owner1Dto = _mapper.Map<OwnerDto>(_seed.Owner1);
      Owner? updatedOwner = null;
      // mock the repository's FindByIdAsync method to return an existing owner
      _mockOwnersRepository.Setup(r => r.FindByIdAsync(_seed.Owner1.Id))
         .ReturnsAsync(_seed.Owner1);
      // mock the repository's Update method
      _mockOwnersRepository.Setup(r => r.UpdateAsync(It.IsAny<Owner>()))
         .Callback<Owner>(owner => updatedOwner = owner);
      // mock the data context's SaveAllChangesAsync method
      _mockDataContext.Setup(c => c.SaveAllChangesAsync())
         .ReturnsAsync(true);

      // Act
      var actionResult = await _ownersController.UpdateOwner(owner1Dto.Id, owner1Dto);

      // Assert
      THelper.IsOk(actionResult!, owner1Dto);
      // Verify that the repository's Update method was called once
      _mockOwnersRepository.Verify(r => r.UpdateAsync(It.IsAny<Owner>()), Times.Once);
      // Verify that the data context's SaveAllChangesAsync method was called once
      _mockDataContext.Verify(c => c.SaveAllChangesAsync(), Times.Once);
   }

   [Fact]
   public async Task DeleteOwner_NoContent() {
      var owner = _seed.Owner1;
      var id = owner.Id;

      _mockOwnersRepository.Setup(r => r.FindByIdAsync(id))
         .ReturnsAsync(owner);
      _mockOwnersRepository.Setup(r => r.Remove(owner))
         .Callback<Owner>(ownerToRemove => { ownerToRemove = owner; });
      _mockDataContext.Setup(c => c.SaveAllChangesAsync())
         .ReturnsAsync(true);

      // Act
      var result = await _ownersController.DeleteOwner(id);

      // Assert
      _mockOwnersRepository.Verify(r => r.Remove(owner), Times.Once);
      _mockDataContext.Verify(c => c.SaveAllChangesAsync(), Times.Once);
   }
}