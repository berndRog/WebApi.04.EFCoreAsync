﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using WebApi.Core.DomainModel.Entities;
using WebApi.Core.Dto;
using Xunit;
namespace WebApiTest.Controllers.Moq;

[Collection(nameof(SystemTestCollectionDefinition))]
public class OwnersControllerUt: BaseControllerUt {

   [Fact]
   public async Task GetOwners() {
      // Arrange
      var repoResult = _seed.Owners;
      // mock the result of the repository
      _mockOwnersRepository.Setup(repository => 
            repository.SelectAsync(false))
                      .ReturnsAsync(repoResult);
      var expected = _mapper.Map<IEnumerable<OwnerDto>>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwners();

      // Assert
      THelper.IsOk(actionResult!, expected);
   }

   [Fact]
   public async Task GetOwnerById_Ok() {
      // Arrange
      var id = _seed.Owner1.Id;
      var repoResult = _seed.Owner1;
      // mock the result of the repository
      _mockOwnersRepository.Setup(repository => 
            repository.FindByIdAsync(id))
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
      _mockOwnersRepository.Setup(repository => 
            repository.FindByIdAsync(id))
                      .ReturnsAsync(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnerById(id);

      // Assert
      THelper.IsNotFound(actionResult);
   }

   [Fact]
   public async Task GetOwnersByName_Ok() {
      // Arrange
      var name = "Mustermann";
      var repoResult = new List<Owner> { _seed.Owner1, _seed.Owner2 };
      _mockOwnersRepository.Setup(repository => 
            repository.FilterByAsync(o => o.Name.Contains(name)))
//          repository.FilterByAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
                      .ReturnsAsync(repoResult);
      var expected = _mapper.Map<IEnumerable<OwnerDto>>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnersByName(name);

      // Assert
      THelper.IsOk(actionResult!, expected);
   }

   [Fact]
   public async Task GetOwnersByName_EmptyList() {
      // Arrange
      var name = "Mustermann";
      var repoResult = new List<Owner>();
      _mockOwnersRepository.Setup(repository => 
            repository.FilterByAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
                      .ReturnsAsync(repoResult);
      var expected = _mapper.Map<IEnumerable<OwnerDto>>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnersByName(name);

      // Assert
      THelper.IsOk(actionResult!, expected);
   }

   [Fact]
   public async Task GetOwnerByEmail_Ok() {
      // Arrange
      var email = _seed.Owner1.Email;
      var repoResult = _seed.Owner1;
      // mock the result of the repository
      _mockOwnersRepository.Setup(repository => 
            repository.FindByAsync(owner => owner.Email == email))
//          repository.FindByAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
                      .ReturnsAsync(repoResult);
      var expected = _mapper.Map<OwnerDto>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnerByEmail(email);

      // Assert
      THelper.IsOk<OwnerDto?>(actionResult, expected);
   }

   [Fact]
   public async Task GetOwnerByEmail_NotFound() {
      // Arrange
      var email = "a.b@c.de";
      Owner? repoResult = null;
      _mockOwnersRepository.Setup(repository => 
            repository.FindByAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
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
      _mockOwnersRepository.Setup(repository => 
            repository.FilterByAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
                      .ReturnsAsync(repoResult);
      var expected = _mapper.Map<IEnumerable<OwnerDto>>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnersByBirthdate(
         "1960-01-01", "1969-12-31");

      // Assert
      THelper.IsOk(actionResult!, expected);
   }

   [Fact]
   public async Task GetOwnersByBirthDate_EmptyList() {
      // Arrange
      var repoResult = new List<Owner>();
      _mockOwnersRepository.Setup(repository => 
            repository.FilterByAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
               .ReturnsAsync(repoResult);
      var expected = _mapper.Map<IEnumerable<OwnerDto>>(repoResult);

      // Act
      var actionResult = await _ownersController.GetOwnersByBirthdate(
         "1950-01-01", "1959-12-31"
      );

      // Assert
      THelper.IsOk(actionResult!, expected);
   }

   [Fact]
   public async Task CreateOwner_Created() {
      // Arrange
      var owner1Dto = _mapper.Map<OwnerDto>(_seed.Owner1);
      Owner? addedOwner = null;
      // mock the repository's Add method
      _mockOwnersRepository.Setup(repository => 
            repository.Add(It.IsAny<Owner>()))
               .Callback<Owner>(owner => addedOwner = owner);
      // mock the data context's SaveAllChangesAsync method
      _mockDataContext.Setup(context => 
            context.SaveAllChangesAsync())
               .ReturnsAsync(true);

      // Act
      var actionResult = await _ownersController.CreateOwner(owner1Dto);

      // Assert
      THelper.IsCreated(actionResult!, owner1Dto);
      // Verify that the repository's Add method was called once
      _mockOwnersRepository.Verify(repository => 
         repository.Add(It.IsAny<Owner>()), Times.Once);
      // Verify that the data context's SaveAllChangesAsync method was called once
      _mockDataContext.Verify(dataContext => 
         dataContext.SaveAllChangesAsync(), Times.Once);
   }

   [Fact]
   public async Task CreateOwner_Conflict() {
      // Arrange
      var owner1Dto = _mapper.Map<OwnerDto>(_seed.Owner1);
      // mock the repository's FindByIdAsync method to return an existing owner
      _mockOwnersRepository.Setup(repository => 
            repository.FindByIdAsync(owner1Dto.Id))
                      .ReturnsAsync(_seed.Owner1);

      // Act
      var actionResult = await _ownersController.CreateOwner(owner1Dto);

      // Assert
      THelper.IsConflict(actionResult!);
      // Verify that the repository's Add method was not called
      _mockOwnersRepository
         .Verify(repository => repository.Add(It.IsAny<Owner>()), Times.Never);
      // Verify that the data context's SaveAllChangesAsync method was not called
      _mockDataContext
         .Verify(dataContext => dataContext.SaveAllChangesAsync(), Times.Never);
   }

   [Fact]
   public async Task UpdateOwner_Created() {
      // Arrange
      var owner1Dto = _mapper.Map<OwnerDto>(_seed.Owner1);
      Owner? addedOwner = null;
      // mock the repository's FindByIdAsync method to return an existing owner
      _mockOwnersRepository.Setup(repository => 
            repository.FindByIdAsync(_seed.Owner1.Id))
               .ReturnsAsync(_seed.Owner1);
      // mock the repository's Update method
      _mockOwnersRepository.Setup(repository => 
            repository.UpdateAsync(It.IsAny<Owner>()))
                      .Callback<Owner>(owner => addedOwner = owner);
      // mock the data context's SaveAllChangesAsync method
      _mockDataContext.Setup(context => 
            context.SaveAllChangesAsync())
                   .ReturnsAsync(true);

      // Act
      var actionResult = await _ownersController.UpdateOwner(owner1Dto.Id, owner1Dto);

      // Assert
      THelper.IsOk(actionResult!, owner1Dto);
      // Verify that the repository's Update method was called once
      _mockOwnersRepository.Verify(repository => 
         repository.UpdateAsync(It.IsAny<Owner>()), Times.Once);
      // Verify that the data context's SaveAllChangesAsync method was called once
      _mockDataContext.Verify(dataContext => 
         dataContext.SaveAllChangesAsync(), Times.Once);
   }

   [Fact]
   public async Task DeleteOwner_NoContent() {
      var owner = _seed.Owner1;
      var id = owner.Id;
      
      _mockOwnersRepository.Setup(repository => 
            repository.FindByIdAsync(id))
               .ReturnsAsync(owner);
      _mockOwnersRepository.Setup(repository => 
            repository.Remove(owner))
               .Callback<Owner>(ownerToRemove => { ownerToRemove = owner; });
      _mockDataContext.Setup(context => 
            context.SaveAllChangesAsync())
               .ReturnsAsync(true);

      // Act
      var result = await _ownersController.DeleteOwner(id);

      // Assert
      _mockOwnersRepository.Verify(repo => repo.Remove(owner), Times.Once);
      _mockDataContext.Verify(context => context.SaveAllChangesAsync(), Times.Once);
   }
      
}