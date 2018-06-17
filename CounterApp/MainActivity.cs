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

            CounterItemDB = new SQLiteCounterItemService();
            //await s.InitialiseAsync();

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recycler_view);

            Adapter = new CounterItemAdapter(await CounterItemDB.GetCounterItemsAsync());
            Adapter.CounterChange += CounterChangeAsync;
            Adapter.CounterDelete += CounterDeleteAsync;

            var layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);
            
            recyclerView.SetAdapter(Adapter);
        }

        private async void CounterDeleteAsync(CounterItem item, int postion)
        {
            await CounterItemDB.DeleteCounterItemAsync(item);
        }

        private async void CounterChangeAsync(CounterItem item, int postion)
        {
            await CounterItemDB.UpdateCounterItemAsync(item);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            menu.GetItem(0).SetShowAsAction(ShowAsAction.Always);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.AddCounter:
                    var builder = new AlertDialog.Builder(this)
                        .SetTitle("Add Counter")
                        .SetView(Resource.Layout.AddCounterDialog)
                        .SetPositiveButton("Add", AddCounterClick)
                        .Show();
                    break;
            }

            return false;
        }

        private async void AddCounterClick(object sender, EventArgs dialogEventClickArgs)
        {
            var dialog = sender as AlertDialog;

            var nameTextView = dialog.FindViewById<EditText>(Resource.Id.AddCounterName);
            var item = new CounterItem { Name = nameTextView.Text, Count = 0, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };

            await CounterItemDB.InsertCounterItemAsync(item);

            Adapter.AddCounter(item);
        }
    }
}

