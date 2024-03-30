using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;

[assembly: InternalsVisibleToAttribute("WebApiTest")]
namespace WebApi.Persistence;

internal abstract class AGenericRepository<T> : IGenericRepository<T>
   where T : AEntity {

   #region fields
   private   readonly DataContext _dbContext;
   protected readonly DbSet<T> TypeDbSet;
   #endregion

   #region properties
   public DataContext DatabaseContext {
      get => _dbContext;
   }
   #endregion
   
   #region ctor
   protected AGenericRepository(
      DataContext dbContext
   ){
      _dbContext = dbContext;
      TypeDbSet = _dbContext.Set<T>();
   }
   #endregion

   #region methods
   /// <summary>
   /// Select all items asynchronously
   /// </summary>
   /// <param name="withTracking">== false -> NoTracking, i.e. items are not loaded into the repository</param>
   /// <returns>IEnumerable<T></returns>
   public virtual async Task<IEnumerable<T>> SelectAsync(bool withTracking = false) {
      IQueryable<T> query = TypeDbSet;
      if(!withTracking) query = query.AsNoTracking();
      return await query.ToListAsync(); 
   }

   /// <summary>
   /// Find an item by Id asynchronously
   /// </summary>
   /// <param name="id">Guid</param>
   /// <returns>T?</returns>
   public virtual async Task<T?> FindByIdAsync(Guid id) =>
      await TypeDbSet.FindAsync(id);

   /// <summary>
   /// Select items by LINQ expression asynchronously
   /// </summary>
   /// <param name="predicate"></param>
   /// <returns></returns>
   public async Task<IEnumerable<T>> FilterByAsync(Expression<Func<T, bool>> predicate) {
      return await TypeDbSet
         .Where(predicate)
         .ToListAsync();
   }

   /// <summary>
   /// Find an item by LINQ expression asynchronously
   /// </summary>
   /// <param name="predicate">LINQ expression tree used as filter</param>
   /// <returns>T?</returns>
   public virtual async Task<T?> FindByAsync(Expression<Func<T, bool>> predicate) =>
      await TypeDbSet
         .FirstOrDefaultAsync(predicate);

   /// <summary>
   /// Write an item to the repository
   /// </summary>
   /// <param name="item">item to add</param>
   public void Add(T item) =>
      TypeDbSet.Add(item);
   
   /// <summary>
   /// Add a range of items to the repository
   /// </summary>
   /// <param name="items">items to add</param>
   public virtual void AddRange(IEnumerable<T> items) =>
      TypeDbSet.AddRange(items);
   
   /// <summary>
   /// Update an exiting item asynchronously, item with item.id must exist
   /// </summary>
   /// <param name="item">Item to update</param>
   /// <exception cref="ApplicationException">item with given id not found</exception>
   public async Task UpdateAsync(T item){
      var foundItem = await TypeDbSet.FindAsync(item.Id)
         ?? throw new ApplicationException($"Update failed, item not found");
      _dbContext.Entry(foundItem).CurrentValues.SetValues(item);
      _dbContext.Entry(foundItem).State = EntityState.Modified;
   }
   
   /// <summary>
   /// Remove an item from the repository
   /// </summary>
   /// <param name="item">item to remove</param>
   public virtual void Remove(T item){
      var entityEntry = TypeDbSet.Remove(item);
   }

   /// <summary>
   /// Attach an item to the repository
   /// </summary>
   /// <param name="item">item to attach</param>
   /// <returns>T? the attached item</returns>
   public T? Attach(T item) {
      EntityEntry<T> entityEntry = _dbContext.Attach<T>(item);
      return entityEntry.Entity;
   }
   #endregion
}