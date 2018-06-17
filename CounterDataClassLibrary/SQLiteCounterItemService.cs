using CounterApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CounterApp
{
    public class SQLiteCounterItemService : ICounterItemService
    {
        private readonly SQLiteAsyncConnection Db;

        public SQLiteCounterItemService()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CounterItemDB.db");

            var db = new SQLiteAsyncConnection(databasePath);

            Db = db;
        }

        public async Task InitialiseAsync()
        {
            await Db.CreateTableAsync<CounterItem>();
        }

        public async Task<bool> InsertCounterItemAsync(CounterItem item)
        {
            return await Db.InsertAsync(item) > 0;
        }

        public async Task<List<CounterItem>> GetCounterItemsAsync()
        {
            var items = Db.Table<CounterItem>().Where(c => true).OrderByDescending(c => c.CreatedDate);
            return await items.ToListAsync();
        }

        public async Task<bool> UpdateCounterItemAsync(CounterItem item)
        {
            return await Db.UpdateAsync(item) > 0;
        }

        public async Task<bool> DeleteCounterItemAsync(CounterItem item)
        {
            return await Db.DeleteAsync(item) > 0;
        }
    }
}
