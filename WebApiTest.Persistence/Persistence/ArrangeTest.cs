using System.Threading.Tasks;
using WebApi.Core;
namespace WebApiTest.Persistence;
public class ArrangeTest(
   IOwnersRepository ownersRepository,
   IAccountsRepository accountsRepository,
   IDataContext dataContext
) {
   
   public async Task OwnerWith1AccountAsync(Seed seed){
      // Arrange
      // Owner owner = new() {
      //    Id = new Guid("10000000-0000-0000-0000-000000000000"),
      //    Name = "Erika Mustermann",
      //    Birthdate = new DateTime(1988, 2, 1).ToUtcDateTime(),
      //    Email = "erika.mustermann@t-online.de"
      // }; // _seed.Owner1
      // Account account = new() {
      //    Id = new Guid("01000000-0000-0000-0000-000000000000"),
      //    Iban = "DE10 10000000 0000000001",
      //    Balance = 2100.0
      // }; // _seed.Account1
      // owner.Add(account); // DomainModel
      // accountsRepository.Add(account);
      // await dataContext.SaveAllChangesAsync();
      seed.Owner1.Add(seed.Account1);
      accountsRepository.Add(seed.Account1);
      await dataContext.SaveAllChangesAsync();
      dataContext.ClearChangeTracker();
   }

   public async Task OwnersAsync(Seed seed){
      ownersRepository.AddRange(seed.Owners);
      await dataContext.SaveAllChangesAsync();
      dataContext.ClearChangeTracker();
   }
   
   // public async Task Owner1WithAccountAsync(Seed seed){
   //    // DomainModel
   //    seed.Owner1.Add(seed.Account1);
   //    seed.Owner1.Add(seed.Account2);
   //    // Repositories
   //    ownersRepository.Add(seed.Owner1);
   //    accountsRepository.Add(seed.Account1);
   //    accountsRepository.Add(seed.Account2);
   //    // Database
   //    await dataContext.SaveAllChangesAsync();
   // }
   
   public async Task Owner1WithAccountsAsync(Seed seed) {
      
      seed.InitAccountsForOwner1();
      accountsRepository.Add(seed.Account1);
      accountsRepository.Add(seed.Account2);
      await dataContext.SaveAllChangesAsync();
      dataContext.ClearChangeTracker();
   }
   
   public async Task OwnersWithAccountsAsync(Seed seed){
      seed.InitAccounts();
      ownersRepository.AddRange(seed.Owners);  
      accountsRepository.AddRange(seed.Accounts);
      await dataContext.SaveAllChangesAsync();
      dataContext.ClearChangeTracker();
   }
}