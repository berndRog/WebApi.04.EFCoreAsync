using System;
using WebApi.Core.DomainModel.NullEntities;
namespace WebApi.Core.DomainModel.Entities;

public class Account: AEntity {
   public override Guid Id { get; init; } = Guid.NewGuid();
   public string  Iban     { get; init; } = string.Empty;
   public double  Balance  { get; set; }
   
   // Navigation property
   public Owner Owner   { get; set; } = NullOwner.Instance;
   public Guid  OwnerId { get; set; } = NullOwner.Instance.Id;
   
}