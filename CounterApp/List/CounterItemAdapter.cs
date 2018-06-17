using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using CounterApp.Model;

namespace CounterApp.List
{
    public class CounterItemAdapter : RecyclerView.Adapter
    {
        private List<CounterItem> CounterItems;
        public override int ItemCount => CounterItems.Count;

        public delegate void CounterItemChangeDelegate(CounterItem item, int postion);
        public event CounterItemChangeDelegate CounterChange;

        public delegate void CounterItemDeleteDelegate(CounterItem item, int postion);
        public event CounterItemDeleteDelegate CounterDelete;

        public CounterItemAdapter(List<CounterItem> counterItems) {
            CounterItems = counterItems;
        }

        public override void OnAttachedToRecyclerView(RecyclerView recyclerView)
        {
            base.OnAttachedToRecyclerView(recyclerView);

            var counterItemTouchHelper = new CounterItemTouchHelper();
            counterItemTouchHelper.ItemSwipe += CounterItemSwipe;

            new ItemTouchHelper(counterItemTouchHelper).AttachToRecyclerView(recyclerView);
        }

        public void AddCounter(CounterItem counterItem)
        {
            CounterItems.Insert(0, counterItem);
            NotifyItemInserted(0);
        }

        private void CounterItemSwipe(int postion)
        {
            CounterDelete?.Invoke(CounterItems[postion], postion);

            CounterItems.RemoveAt(postion);
            NotifyItemRangeRemoved(postion, 1);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var counterItemViewHolder = holder as CounterItemViewHolder;
            counterItemViewHolder.SetCounterItem(CounterItems[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = parent.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
            var view = inflater.Inflate(Resource.Layout.CounterItemListItem, parent, false);

            var counterItemViewHolder = new CounterItemViewHolder(view);

            counterItemViewHolder.CounterChange += OnCounterChange;

            return counterItemViewHolder;
        }

        private void OnCounterChange(CounterItem item, int postion)
        {
            CounterChange?.Invoke(item, postion);    
        }
    }
}
