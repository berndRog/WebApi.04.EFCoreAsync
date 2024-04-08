using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Controllers;
using WebApi.Core;
using WebApi.Core.Dto;
using WebApi.Persistence;
using WebApiTest.Di;
using WebApiTest.Persistence;
using Xunit;

namespace WebApiTest.Controllers;
[Collection(nameof(SystemTestCollectionDefinition))]
public class AccountsControllerTest :BaseControllerTest {

   [Fact]
   public async Task GetTest() {
      // Arrange
      await _arrangeTest.Owner1WithAccountsAsync(_seed);
      var expected = new List<AccountDto> {
         _mapper.Map<AccountDto>(_seed.Account1),
         _mapper.Map<AccountDto>(_seed.Account2)
      };
     
      // Act
      var response = await _accountsController.GetAccountsByOwnerId(_seed.Owner1.Id);
      
      // Assert
      var (success, result, value) =
         Helper.ResultFromResponse<OkObjectResult, IEnumerable<AccountDto>>(response);
      success.Should().BeTrue();
      
      result.StatusCode.Should().Be(200);
      value.Should().NotBeNull().And
         .HaveCount(2);
      IList<AccountDto> actual = value.ToList(); 
      actual[0].Should().BeEquivalentTo(expected[0]);
      actual[1].Should().BeEquivalentTo(expected[1]);
   }
   [Fact]
   public async Task GetByIdTest() {
      // Arrange
      await _arrangeTest.Owner1WithAccountsAsync(_seed);
      var expected = _mapper.Map<AccountDto>(_seed.Account1);
      
      // Act
      var response = await _accountsController.GetAccountById(_seed.Account1.Id);
      
      // Assert
      var (success, result, value) =
         Helper.ResultFromResponse<OkObjectResult, AccountDto>(response!);
      success.Should().BeTrue();
      
      result.StatusCode.Should().Be(200);
      value.Should().NotBeNull().And
         .BeEquivalentTo(expected);
   }

   [Fact]
   public async Task GetByIbanTest() {
      // Arrange
      await _arrangeTest.OwnersWithAccountsAsync(_seed);
      var account6Dto = _mapper.Map<AccountDto>(_seed.Account6);
      
      // Act
      var response = await _accountsController.GetAccountByIban("DE50 10000000 0000000000");
      
      // Assert
      response.Should().NotBeNull();
      var (success, result, value) =
         Helper.ResultFromResponse<OkObjectResult, AccountDto>(response!);
      success.Should().BeTrue();
      
      result.StatusCode.Should().Be(200);
      value.Should().NotBeNull().And
         .BeEquivalentTo(account6Dto);
   }

   [Fact]
   public async Task GetByAccountByIdNotFoundTest() {
      // Arrange
      await _arrangeTest.Owner1WithAccountsAsync(_seed);
      var idError = new Guid("12345678-0000-0000-0000-000000000000");
      
      // Act
      var response = await _accountsController.GetAccountById(idError);
      
      // Assert
      response.Should().NotBeNull();
      var (success, result, value) =
         Helper.ResultFromResponse<NotFoundObjectResult, AccountDto>(response!);
      success.Should().BeFalse();
      
      result.StatusCode.Should().Be(404);
   }
   
   [Fact]
   public async Task CreateAccountTest() {
      // Arrange
      _ownersRepository.Add(_seed.Owner1);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();   
      var account1Dto = _mapper.Map<AccountDto>(_seed.Account1);
      
      // Act
      var response = await _accountsController.CreateAccount(_seed.Owner1.Id, account1Dto);
      
      // Assert
      var (success, result, value) =
         Helper.ResultFromResponse<CreatedResult, AccountDto>(response);
      success.Should().BeTrue();
      
      result.StatusCode.Should().Be(201);
      account1Dto = account1Dto with { OwnerId=_seed.Owner1.Id};
      value.Should().NotBeNull().And
         .BeEquivalentTo(account1Dto);
   }
   
   [Fact]
   public async Task CreateAccountConflictTest() {
      // Arrange
      _ownersRepository.Add(_seed.Owner1);
      await _dataContext.SaveAllChangesAsync();
      _dataContext.ClearChangeTracker();   
      _seed.Owner1.Add(_seed.Account1);
      var account1Dto = _mapper.Map<AccountDto>(_seed.Account1);
      await _accountsController.CreateAccount(_seed.Owner1.Id, account1Dto);
     
      // Act
      var response = await _accountsController.CreateAccount(_seed.Owner1.Id, account1Dto);
    
      // Assert
      var (success, result, value) = 
         Helper.ResultFromResponse<ConflictObjectResult, AccountDto>(response);
      success.Should().BeFalse();
      
      result.StatusCode.Should().Be(409);
   }
}