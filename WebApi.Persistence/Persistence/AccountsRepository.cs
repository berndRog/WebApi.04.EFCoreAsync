using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;

//[assembly: InternalsVisibleTo("WebApiTest")]
namespace WebApi.Persistence;
internal  class AccountsRepository(
   DataContext dataContext
) : AGenericRepository<Account>(dataContext), IAccountsRepository {
   
   // public async Task<IEnumerable<Account>> SelectAsync() =>
   //     await dataContext.Accounts.ToListAsync();
   //
   // public async Task<Account?> FindByIdAsync(Guid id) => 
   //    await dataContext.Accounts.FindAsync(id);
   //
   // public void Add(Account account) =>
   //    dataContext.Accounts.Add(account);
   //
   // public async Task UpdateAsync(Account account) {
   //    var fetchedAccount = 
   //       await dataContext.Accounts.FirstOrDefaultAsync(i => i.Id == account.Id)
   //       ?? throw new ApplicationException("Update failed, item not found");
   //    dataContext.Entry(fetchedAccount).CurrentValues.SetValues(account);
   //    dataContext.Entry(fetchedAccount).State = EntityState.Modified;
   // }
   //
   // public void Remove(Account account) =>
   //    dataContext.Accounts.Remove(account);
   //
   // public async Task<IEnumerable<Account>> SelectByOwnerIdAsync(Guid ownerId) =>
   //    await dataContext.Owners
   //       .Where(o => o.Id == ownerId)
   //       .SelectMany(o => o.Accounts)
   //       .ToListAsync();
   //
   // public async Task<Account?> FindByIbanAsync(string iban) =>
   //    await dataContext.Accounts
   //       .FirstOrDefaultAsync(a => a.Iban == iban);
   
}
