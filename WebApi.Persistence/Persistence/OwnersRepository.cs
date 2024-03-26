using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;

namespace WebApi.Persistence;

internal class OwnersRepository(
   DataContext dataContext
) : IOwnersRepository {
   
   public async Task<IEnumerable<Owner>> SelectAsync() =>
      await dataContext.Owners.ToListAsync();

   public async Task<Owner?> FindByIdAsync(Guid id) =>
      await dataContext.Owners.FindAsync(id);

   public void Add(Owner owner) =>
      dataContext.Owners.Add(owner);
   
   public async Task UpdateAsync(Owner owner) {
      var fetchedOwner = 
         await dataContext.Owners.FirstOrDefaultAsync(i => i.Id == owner.Id)
         ?? throw new ApplicationException("Update failed, item not found");
      dataContext.Entry(fetchedOwner).CurrentValues.SetValues(owner);
      dataContext.Entry(fetchedOwner).State = EntityState.Modified;
   }

   public void Remove(Owner owner) =>
      dataContext.Owners.Remove(owner);

   public async Task<IEnumerable<Owner>> SelectByNameAsync(string name) =>
      await dataContext.Owners
         .Where(owner => owner.Name.Contains(name))
         .ToListAsync();

   public async Task <Owner?> FindByEmailAsync(string email) =>
      await dataContext.Owners
         .FirstOrDefaultAsync(owner => owner.Email == email);

   public async Task<IEnumerable<Owner>> SelectByBirthDateAsync(DateTime from, DateTime to) =>
      await dataContext.Owners
         .Where(owner =>
            owner.Birthdate >= from &&
            owner.Birthdate <= to)
         .ToListAsync();
}