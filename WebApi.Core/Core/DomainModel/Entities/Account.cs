using System;
using WebApi.DomainModel.NullEntities;
namespace WebApi.Core.DomainModel.Entities;

public class Account: IEntity {
   public Guid    Id       { get; init; } = Guid.Empty;
   public string  Iban     { get; init; } = string.Empty;
   public double  Balance  { get; private set; }
   
   // Navigation property
   public Owner Owner   { get; set; } = NullOwner.Instance;
   public Guid  OwnerId { get; set; } = NullOwner.Instance.Id;
   
   #region ctor
   public Account () {
      Id = Guid.NewGuid();
   }
   #endregion
   
}