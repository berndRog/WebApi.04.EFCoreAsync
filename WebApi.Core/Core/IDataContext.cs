using System.Threading.Tasks;
namespace WebApi.Core;

public interface IDataContext {
   Task<bool> SaveAllChangesAsync();
}