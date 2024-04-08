using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Dto;
using Xunit;
namespace WebApiTest.Controllers;
[Collection(nameof(SystemTestCollectionDefinition))]
public class OwnersControllerTest: BaseControllerTest {

   [Fact]
   public async Task GetByIdTest() {
      // Arrange
      await _arrangeTest.OwnersAsync(_seed);
      var owner1Dto = _mapper.Map<OwnerDto>(_seed.Owner1); 
      
      // Act
      var response = await _ownersController.GetOwnerById(_seed.Owner1.Id);
      
      // Assert
      var(success, result, value) = 
         Helper.ResultFromResponse<OkObjectResult, OwnerDto>(response);
      success.Should().BeTrue();
      
      result.StatusCode.Should().Be(200);
      value.Should().NotBeNull().And
         .BeEquivalentTo(owner1Dto);
   }

   [Fact]
   public async Task GetOwnersByNameTest() {
      // Arrange
      await _arrangeTest.OwnersAsync(_seed);
      var expected = new List<OwnerDto> {
         _mapper.Map<OwnerDto>(_seed.Owner1),
         _mapper.Map<OwnerDto>(_seed.Owner2)
      };
      var expectedDtos = _mapper.Map<IEnumerable<OwnerDto>>(expected); 
      
      // Act
      var response = await _ownersController.GetOwnersByName("Mustermann");
     
      // Assert
      var(success, result,value) = 
         Helper.ResultFromResponse<OkObjectResult, IEnumerable<OwnerDto>>(response);
      success.Should().BeTrue();

      result.StatusCode.Should().Be(200);
      value.Should().NotBeNull().And
         .BeEquivalentTo(expectedDtos);
   }


   [Fact]
   public async Task GetByIdNotFoundTest() {
      // Arrange
      await _arrangeTest.OwnersAsync(_seed);
      var idError = new Guid("12345678-0000-0000-0000-000000000000");
      
      // Act
      var response = await _ownersController.GetOwnerById(idError);
      
      // Assert
      var (success, result, value) = 
         Helper.ResultFromResponse<NotFoundObjectResult, OwnerDto>(response);
      success.Should().BeFalse();

      result.StatusCode.Should().Be(404);
   }
   
   [Fact]
   public async Task CreateOwnerTest() {
      // Arrange
      var owner1Dto = _mapper.Map<OwnerDto>(_seed.Owner1); 
      
      // Act
      var response = await _ownersController.CreateOwner(owner1Dto);
      
      // Assert
      var(success, result, value) = 
         Helper.ResultFromResponse<CreatedResult, OwnerDto>(response);
      success.Should().BeTrue();

      result.StatusCode.Should().Be(201);
      value.Should().NotBeNull().And
         .BeEquivalentTo(owner1Dto);
   }
   
   [Fact]
   public async Task CreateOwnerConflictTest() {
      // Arrange
      var owner1Dto = _mapper.Map<OwnerDto>(_seed.Owner1); 
      await _ownersController.CreateOwner(owner1Dto);
      
      // Act
      var response = await _ownersController.CreateOwner(owner1Dto);
      
      // Assert
      var(success,result,value) = 
         Helper.ResultFromResponse<ConflictObjectResult, OwnerDto>(response);
      success.Should().BeFalse();

      result.StatusCode.Should().Be(409);
      
   }
}