using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;

namespace CounterApp.List
{
    public class CounterItemTouchHelper : ItemTouchHelper.Callback
    {
        public delegate void ItemSwipeDelegate(int postion);
        public event ItemSwipeDelegate ItemSwipe;

        public CounterItemTouchHelper() {}

        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            //int dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down;
            int swipeFlags = ItemTouchHelper.Start | ItemTouchHelper.End;
            return MakeMovementFlags(0, swipeFlags);
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return true;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            ItemSwipe?.Invoke(viewHolder.AdapterPosition);
        }
    }
}