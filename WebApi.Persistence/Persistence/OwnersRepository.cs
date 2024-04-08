using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;

[assembly: InternalsVisibleTo("WebApiTest.Persistence")]
[assembly: InternalsVisibleTo("WebApiTest.Controllers")] 
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
   
   
   public async Task<IEnumerable<Owner>> FilterByJoinAsync(
      Expression<Func<Owner, bool>>? predicate,
      bool joinAccounts,
      bool withTracking
   ) {
      // convert DbSet into an IQueryable
      IQueryable<Owner> query = _dbContext.Owners;
      
      // switch off tracking if not needed
      if(!withTracking)     query = query.AsNoTracking();
      
      // filter by predicate
      if(predicate != null) query = query.Where(predicate);
      
      // join accounts with owner
      if (joinAccounts)     query = query.Include(o => o.Accounts);
      
      // eager evaluation of results
      return await query.ToListAsync(); 
      
   }
}