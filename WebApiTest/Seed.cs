
using System.Runtime.InteropServices.JavaScript;
using WebApi.Core.DomainModel.Entities;
using WebApi.Core.DomainModel.NullEntities;
using WebApi.Core.Misc;

namespace WebApiTest;

public class Seed {

   #region fields
   public Owner Owner1{ get; set; }
   public Owner Owner2{ get; set; }
   public Owner Owner3{ get; set; }
   public Owner Owner4{ get; set; }
   public Owner Owner5{ get; set; }
   public Owner Owner6{ get; set; }

   public Account Account1{ get; set; }
   public Account Account2{ get; set; }
   public Account Account3{ get; set; }
   public Account Account4{ get; set; }
   public Account Account5{ get; set; }
   public Account Account6{ get; set; }
   public Account Account7{ get; set; }
   public Account Account8{ get; set; }


   // not serialized
   public List<Owner> Owners{ get; private set; } = new List<Owner>();
   public List<Account> Accounts{ get; private set; } = new List<Account>();
   #endregion

   public Seed(){

      #region Owners
      Owner1 = new(){
         Id = new Guid("10000000-0000-0000-0000-000000000000"),
         Name = "Erika Mustermann",
         Birthdate = new DateTime(1988, 2, 1).ToUniversalTime(),
         Email = "erika.mustermann@t-online.de"
      };
      Owner2 = new(){
         Id = new Guid("20000000-0000-0000-0000-000000000000"),
         Name = "Max Mustermann",
         Birthdate = new DateTime(1980, 12, 31).ToUniversalTime(),
         Email = "max.mustermann@gmail.com"
      };
      Owner3 = new(){
         Id = new Guid("30000000-0000-0000-0000-000000000000"),
         Name = "Arno Arndt",
         Birthdate = new DateTime(1969, 04, 11).ToUniversalTime(),
         Email = "a.arndt@t-online.com"
      };
      Owner4 = new(){
         Id = new Guid("40000000-0000-0000-0000-000000000000"),
         Name = "Benno Bauer",
         Birthdate = new DateTime(1965, 05, 18).ToUniversalTime(),
         Email = "b.bauer@gmail.com"
      };
      Owner5 = new(){
         Id = new Guid("50000000-0000-0000-0000-000000000000"),
         Name = "Christine Conrad",
         Birthdate = new DateTime(1972, 06, 07).ToUniversalTime(),
         Email = "c.conrad@gmx.de"
      };
      Owner6 = new(){
         Id = new Guid("60000000-0000-0000-0000-000000000000"),
         Name = "Dana Deppe",
         Birthdate = new DateTime(1978, 09, 11).ToUniversalTime(),
         Email = "d.deppe@icloud.com"
      };
      #endregion

      #region Accounts
      Account1 = new(){
         Id = new Guid("01000000-0000-0000-0000-000000000000"),
         Iban = "DE10 10000000 0000000000",
         Balance = 2100.0
      };
      Account2 = new(){
         Id = new Guid("02000000-0000-0000-0000-000000000000"),
         Iban = "DE10 20000000 0000000000",
         Balance = 2000.0
      };
      Account3 = new(){
         Id = new Guid("03000000-0000-0000-0000-000000000000"),
         Iban = "DE20 10000000 0000000000",
         Balance = 3000.0
      };
      Account4 = new(){
         Id = new Guid("04000000-0000-0000-0000-000000000000"),
         Iban = "DE30 10000000 0000000000",
         Balance = 2500.0
      };
      Account5 = new(){
         Id = new Guid("05000000-0000-0000-0000-000000000000"),
         Iban = "DE40 10000000 0000000000",
         Balance = 1900.0
      };
      Account6 = new(){
         Id = new Guid("06000000-0000-0000-0000-000000000000"),
         Iban = "DE50 10000000 0000000000",
         Balance = 3500.0
      };
      Account7 = new(){
         Id = new Guid("07000000-0000-0000-0000-000000000000"),
         Iban = "DE50 20000000 0000000000",
         Balance = 3100.0
      };
      Account8 = new(){
         Id = new Guid("08000000-0000-0000-0000-000000000000"),
         Iban = "DE60 10000000 0000000000",
         Balance = 4300.0
      };
      #endregion
      
      Owners = new List<Owner>{ Owner1, Owner2, Owner3, Owner4, Owner5, Owner6 };
      Accounts = new List<Account>{ Account1, Account2, Account3, Account4, Account5, Account6, Account7, Account8 };
      
   }

   public Seed InitAccounts(){
      Owner1.Add(Account1); // Owner 1 with 2 accounts 1+2
      Owner1.Add(Account2);
      Owner2.Add(Account3); // Owner 2 witn account 3
      Owner3.Add(Account4); // Owner 3 with account 4
      Owner4.Add(Account5); // Owner 4 with account 5
      Owner5.Add(Account6); // Owner 5 with 2 accounts 6+7
      Owner5.Add(Account7);
      Owner6.Add(Account8); // Owner 6 wiht account 8
      return this;
   }

   public Seed InitAccountsForOwner1(){
      Owner1.Add(Account1); // Owner 1 with 2 accounts 1+2
      Owner1.Add(Account2);
      return this;
   }
   

}