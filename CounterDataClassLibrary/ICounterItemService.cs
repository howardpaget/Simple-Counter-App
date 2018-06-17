using CounterApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CounterApp
{
    public interface ICounterItemService
    {
        Task InitialiseAsync();
        Task<bool> InsertCounterItemAsync(CounterItem item);
        Task<List<CounterItem>> GetCounterItemsAsync();
        Task<bool> UpdateCounterItemAsync(CounterItem item);
        Task<bool> DeleteCounterItemAsync(CounterItem item);
    }
}
