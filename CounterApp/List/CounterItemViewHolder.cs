using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CounterApp.Model;
using System;

namespace CounterApp.List
{
    public class CounterItemViewHolder : RecyclerView.ViewHolder
    {
        private readonly TextView NameTextView, ValueTextView, DateTextView;

        private CounterItem CounterItem;

        public delegate void CounterItemChangeDelegate(CounterItem item, int postion);
        public event CounterItemChangeDelegate CounterChange;

        public CounterItemViewHolder(View view) : base(view)
        {
            NameTextView = view.FindViewById<TextView>(Resource.Id.CounterItemName);
            ValueTextView = view.FindViewById<TextView>(Resource.Id.CounterItemValue);
            DateTextView = view.FindViewById<TextView>(Resource.Id.CounterItemDate);

            view.FindViewById<Button>(Resource.Id.SubtractCountButton).Click += CounterButtonClick;
            view.FindViewById<Button>(Resource.Id.AddCountButton).Click += CounterButtonClick;
            view.FindViewById<Button>(Resource.Id.SubtractTenCountButton).Click += CounterButtonClick;
            view.FindViewById<Button>(Resource.Id.AddTenCountButton).Click += CounterButtonClick;
        }

        public void SetCounterItem(CounterItem counterItem) {
            CounterItem = counterItem;

            NameTextView.Text = counterItem.Name;
            ValueTextView.Text = $"{counterItem.Count}";
            DateTextView.Text = counterItem.CreatedDate.ToLongDateString();
        }

        private void CounterButtonClick(object sender, EventArgs e)
        {
            var view = sender as View;
            switch (view.Id)
            {
                case Resource.Id.SubtractCountButton:
                    CounterItem.Count--;        
                    break;
                case Resource.Id.AddCountButton:
                    CounterItem.Count++;
                    break;
                case Resource.Id.SubtractTenCountButton:
                    CounterItem.Count -= 10;
                    break;
                case Resource.Id.AddTenCountButton:
                    CounterItem.Count += 10;
                    break;
            }

            SetCounterItem(CounterItem);

            CounterChange?.Invoke(CounterItem, AdapterPosition);
        }
    }
}