using System;
namespace WebApi.Core.DomainModel.Dto;

public record AccountDto {
   public Guid    Id       { get; init; } = Guid.Empty;
   public string  Iban     { get; init; } = string.Empty;
   public double  Balance  { get; init; } = 0;
   
   // Navigation property
   public Guid  OwnerId { get; init; } = Guid.Empty;
}