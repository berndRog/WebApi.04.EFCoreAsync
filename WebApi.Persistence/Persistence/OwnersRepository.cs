using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;
namespace WebApi.Persistence;

internal class OwnersRepository(
   DataContext dataContext
) : AGenericRepository<Owner>(dataContext), IOwnersRepository {
   
   // public async Task<IEnumerable<Owner>> SelectAsync() =>
   //    await dataContext.Owners.ToListAsync();
   //
   // public async Task<Owner?> FindByIdAsync(Guid id) =>
   //    await dataContext.Owners.FindAsync(id);
   //
   // public void Add(Owner owner) =>
   //    dataContext.Owners.Add(owner);
   //
   // public async Task UpdateAsync(Owner owner) {
   //    var fetchedOwner = 
   //       await dataContext.Owners.FirstOrDefaultAsync(i => i.Id == owner.Id)
   //       ?? throw new ApplicationException("Update failed, item not found");
   //    dataContext.Entry(fetchedOwner).CurrentValues.SetValues(owner);
   //    dataContext.Entry(fetchedOwner).State = EntityState.Modified;
   // }
   //
   // public void Remove(Owner owner) =>
   //    dataContext.Owners.Remove(owner);
   //
   // public async Task<IEnumerable<Owner>> SelectByNameAsync(string name) =>
   //    await DatabaseContext.Owners
   //       .Where(owner => owner.Name.Contains(name))
   //       .ToListAsync();
   //
   // public async Task <Owner?> FindByEmailAsync(string email) =>
   //    await DatabaseContext.Owners
   //       .FirstOrDefaultAsync(owner => owner.Email == email);
   //
   // public async Task<IEnumerable<Owner>> SelectByBirthDateAsync(DateTime from, DateTime to) =>
   //    await DatabaseContext.Owners
   //       .Where(owner =>
   //          owner.Birthdate >= from &&
   //          owner.Birthdate <= to)
   //       .ToListAsync();
   
   
   public async Task<IEnumerable<Owner>> SelectByJoinAsync(
      bool withTracking, 
      Expression<Func<Owner, bool>>? predicate,
      bool joinAccounts
   ) {
      IQueryable<Owner> query = TypeDbSet;
      if(!withTracking)     query = query.AsNoTracking();
      if(predicate != null) query = query.Where(predicate);
      if (joinAccounts)     query = query.Include(o => o.Accounts);
      return await query.ToListAsync(); 
      
   }
   
}