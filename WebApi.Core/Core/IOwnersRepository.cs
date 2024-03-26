using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Core.DomainModel.Entities;
namespace WebApi.Core;

public interface IOwnersRepository {
   Task<IEnumerable<Owner>> SelectAsync();
   Task<Owner?> FindByIdAsync(Guid id);
   void Add(Owner owner);
   Task UpdateAsync(Owner owner);
   void Remove(Owner owner);

   Task<IEnumerable<Owner>> SelectByNameAsync(string name);
   Task<Owner?> FindByEmailAsync(string email);
   Task<IEnumerable<Owner>> SelectByBirthDateAsync(DateTime from, DateTime to);
}