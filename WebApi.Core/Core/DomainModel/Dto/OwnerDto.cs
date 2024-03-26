using System;
namespace WebApi.Core.DomainModel.Dto; 
public record OwnerDto {
   public Guid     Id       { get; init; } = Guid.Empty;
   public string   Name     { get; init; } = string.Empty;
   public DateTime Birthdate{ get; init; } = DateTime.UtcNow;
   public string   Email    { get; init; } = string.Empty;
   // no Navigation property
   
}
