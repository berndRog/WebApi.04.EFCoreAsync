﻿using System;
using System.Collections.Generic;
namespace WebApi.Core.DomainModel.Entities; 

public class Owner: AEntity {
   
   #region properties
   public override Guid Id  { get; init; } = Guid.Empty;
   public string   Name     { get; set; } = string.Empty;
   public DateTime Birthdate{ get; init; } = DateTime.UtcNow;
   public string   Email    { get; set; } = string.Empty;

   // Navigation property
   // One-to-many relationship Owner -> Account
   public ICollection<Account> Accounts { get; set; } = new List<Account>();
   #endregion
   
   #region ctor
   public Owner(): base() {
      Id = Guid.NewGuid();
   }
   #endregion
   
   #region methods
   public void Update (string name, string email) {
      Name = name;
      Email = email;
   }
   
   public void Add(Account account) {
      if (account.OwnerId != Id)
         throw new ApplicationException("Account is already asigned to another owner");
      account.Owner = this;
      account.OwnerId = Id;
      Accounts.Add(account);
   }
   #endregion
   
}
