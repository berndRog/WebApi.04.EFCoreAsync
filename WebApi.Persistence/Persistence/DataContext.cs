using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;
namespace WebApi.Persistence; 

internal class DataContext: DbContext, IDataContext  {

   #region fields
   private readonly ILogger<DataContext>? _logger;
   public DbSet<Owner> Owners => Set<Owner>();
   public DbSet<Account> Accounts => Set<Account>();
   #endregion

   #region ctor
   // ctor for migration only
   public DataContext(DbContextOptions<DataContext> options) : base(options) { }

   public DataContext(
      DbContextOptions<DataContext> options, 
      ILogger<DataContext> logger
   ) : base(options) {
      _logger = logger;
   }
   #endregion

   #region methods
   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      optionsBuilder.EnableSensitiveDataLogging(true);
   }
   
   public async Task<bool> SaveAllChangesAsync() {
      
      // log repositories before transfer to the database
      _logger?.LogDebug("\n{output}",ChangeTracker.DebugView.LongView);
      
      // save all changes to the database, returns the number of rows affected
      var result = await SaveChangesAsync();
      
      // log repositories after transfer to the database
      _logger?.LogDebug("SaveChanges {result}",result);
      _logger?.LogDebug("\n{output}",ChangeTracker.DebugView.LongView);
      return result > 0;
   }
   
   public void ClearChangeTracker() =>
      ChangeTracker.Clear();

   public void LogChangeTracker(string text) =>
      _logger?.LogDebug("{Text}\n{Tracker}", text, ChangeTracker.DebugView.LongView);
   #endregion
   
   #region static methods
// "UseDatabase": "Sqlite",
// "ConnectionStrings": {
//    "LocalDb": "WebApi04",
//    "SqlServer": "Server=localhost,2433; Database=WebApi04; User=sa; Password=P@ssword_geh1m;",
//    "Sqlite": "WebApi04"
// },
   public static (string useDatabase, string dataSource) EvalDatabaseConfiguration(
      IConfiguration configuration
   ) {

      // read active database configuration from appsettings.json
      string useDatabase = configuration.GetSection("UseDatabase").Value ??
         throw new Exception("UseDatabase is not available");

      // read connection string from appsettings.json
      string connectionString = configuration.GetSection("ConnectionStrings")[useDatabase]
         ?? throw new Exception("ConnectionStrings is not available"); 
      
      // /users/documents/WebApi
      string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      string pathDocuments = Path.Combine(directory,"WebApi");
      Directory.CreateDirectory(pathDocuments);
      
      switch (useDatabase) {
         case "LocalDb":
            var dbFile = $"{Path.Combine(pathDocuments, connectionString)}.mdf";
            var dataSourceLocalDb =
               $"Data Source = (LocalDB)\\MSSQLLocalDB; " +
               $"Initial Catalog = {connectionString}; Integrated Security = True; " +
               $"AttachDbFileName = {dbFile};";
            return (useDatabase, dataSourceLocalDb);

         case "SqlServer":
            return (useDatabase, connectionString);

         case "Sqlite":
            var dataSourceSqlite =
               "Data Source=" + Path.Combine(pathDocuments, connectionString) + ".db";
            return (useDatabase, dataSourceSqlite);
         default:
            throw new Exception("appsettings.json Problems with database configuration");
      }
   }
   #endregion
   
}