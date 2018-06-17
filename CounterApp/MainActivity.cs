using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Android.Views;
using CounterApp.List;
using CounterApp.Model;
using System;
using Android.Widget;

namespace CounterApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ICounterItemService CounterItemDB;
        private CounterItemAdapter Adapter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ActivityMain);

            SupportActionBar.Title = "Simple Counter";

            // Set up SQLite connection
            CounterItemDB = new SQLiteCounterItemService();
            await CounterItemDB.InitialiseAsync();

            // Set up adapter and recycler view
            var recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView);

            Adapter = new CounterItemAdapter(await CounterItemDB.GetCounterItemsAsync());
            Adapter.CounterChange += CounterChangeAsync;
            Adapter.CounterDelete += CounterDeleteAsync;

            var layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);
            
            recyclerView.SetAdapter(Adapter);
        }

        // Handle delete events from the adapter
        private async void CounterDeleteAsync(CounterItem item, int postion)
        {
            await CounterItemDB.DeleteCounterItemAsync(item);
        }

        // Handle update events from the adapter
        private async void CounterChangeAsync(CounterItem item, int postion)
        {
            await CounterItemDB.UpdateCounterItemAsync(item);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            
            // For some reason setting always show in xml doesn't work...
            menu.GetItem(0).SetShowAsAction(ShowAsAction.Always);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                // Open up add counter dialog when add counter is clicked
                case Resource.Id.AddCounter:
                    var builder = new AlertDialog.Builder(this)
                        .SetTitle("Add Counter")
                        .SetView(Resource.Layout.AddCounterDialog)
                        .SetPositiveButton("Add", AddCounterDialogClick)
                        .Show();
                    break;
            }

            return false;
        }

        // Add the counter to the database when the add counter dialog button is clicked
        private async void AddCounterDialogClick(object sender, EventArgs dialogEventClickArgs)
        {
            var dialog = sender as AlertDialog;

            var nameTextView = dialog.FindViewById<EditText>(Resource.Id.AddCounterName);
            var item = new CounterItem { Name = nameTextView.Text, Count = 0, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };

            await CounterItemDB.InsertCounterItemAsync(item);

            Adapter.AddCounter(item);
        }
    }
}

