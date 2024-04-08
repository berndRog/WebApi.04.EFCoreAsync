using AutoMapper;
using FluentAssertions;
using WebApi.Core.DomainModel.Entities;
using WebApi.Core.Dto;
namespace WebApiTest.Core.Mapping;

public class MappingProfileUt {
   private readonly Seed _seed;
   private readonly IMapper _mapper;

   public MappingProfileUt() {
      var config = new MapperConfiguration(config =>
         config.AddProfile(new WebApi.Core.Mapping.MappingProfile())
      );
      _mapper = new Mapper(config);
      _seed = new Seed();
      _seed.InitAccounts();
   }

   [Fact]
   public void Owner2OwnerDtoUt() {
      // Arrange
      // Act
      var actualDto = _mapper.Map<OwnerDto>(_seed.Owner1);
      // Assert
      actualDto.Should().NotBeNull().And
         .BeOfType<OwnerDto>();

      actualDto.Id.Should().Be(_seed.Owner1.Id);
      actualDto.Name.Should().Be(_seed.Owner1.Name);
      actualDto.Birthdate.Should().Be(_seed.Owner1.Birthdate);
      actualDto.Email.Should().Be(_seed.Owner1.Email);
   }

   [Fact]
   public void OwnerDto2OwnerUt() {
      // Arrange
      var ownerDto = _mapper.Map<OwnerDto>(_seed.Owner1);
      // Act
      var actualOwner = _mapper.Map<Owner>(ownerDto);
      // Assert
      actualOwner.Should().NotBeNull().And
         .BeOfType<Owner>();
      actualOwner.Id.Should().Be(_seed.Owner1.Id);
      actualOwner.Name.Should().Be(_seed.Owner1.Name);
      actualOwner.Birthdate.Should().Be(_seed.Owner1.Birthdate);
      actualOwner.Email.Should().Be(_seed.Owner1.Email);
   }

   [Fact]
   public void Account2AccountDtoUt() {
      // Arrange
      // Act
      var actualDto = _mapper.Map<AccountDto>(_seed.Account1);
      // Assert
      actualDto.Should().NotBeNull().And
         .BeOfType<AccountDto>();
      actualDto.Id.Should().Be(_seed.Account1.Id);
      actualDto.Iban.Should().Be(_seed.Account1.Iban);
      actualDto.Balance.Should().Be(_seed.Account1.Balance);
   }

   [Fact]
   public void AccountDto2AccountUt() {
      // Arrange
      var accountDto = _mapper.Map<AccountDto>(_seed.Account1);
      // Act
      var actual = _mapper.Map<Account>(accountDto);
      // Assert
      actual.Should().NotBeNull().And
         .BeOfType<Account>();
      actual.Id.Should().Be(_seed.Account1.Id);
      actual.Iban.Should().Be(_seed.Account1.Iban);
      actual.Balance.Should().Be(_seed.Account1.Balance);
   }
   
}