using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Core.DomainModel.Entities;
namespace WebApi.Core;

public interface IAccountsRepository {
   Task<IEnumerable<Account>> SelectAsync();
   Task<Account?> FindByIdAsync(Guid id);
   void Add(Account account);
   Task UpdateAsync(Account account);
   void Remove(Account account);

   Task<IEnumerable<Account>> SelectByOwnerIdAsync(Guid ownerId);
   Task<Account?> FindByIbanAsync(string iban);
}