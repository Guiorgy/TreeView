using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace Xamarin.TreeView
{
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    [Register("treeview.TreeView")]
    public class TreeView : RecyclerView
    {
        public TreeView(Context context) : base(context) => Initialize();
        public TreeView(Context context, IAttributeSet attrs) : base(context, attrs) => Initialize();
        public TreeView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) => Initialize();

        private void Initialize()
        {

        }

        [Obsolete("Use SetAdapter(TieredListView.Adapter adapter) instead.", true)]
        public sealed override void SetAdapter(RecyclerView.Adapter adapter)
        {
            if(adapter is Adapter)
            {
                SetAdapter(adapter as Adapter);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void SetAdapter(Adapter adapter) => base.SetAdapter(adapter);

        [Obsolete("Use SwapAdapter(Adapter adapter, bool removeAndRecycleExistingViews) instead.", true)]
        public sealed override void SwapAdapter(RecyclerView.Adapter adapter, bool removeAndRecycleExistingViews)
        {
            if (adapter is Adapter)
            {
                SwapAdapter(adapter as Adapter, removeAndRecycleExistingViews);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void SwapAdapter(Adapter adapter, bool removeAndRecycleExistingViews) => base.SwapAdapter(adapter, removeAndRecycleExistingViews);

        public new abstract class Adapter : RecyclerView.Adapter
        {
            public event EventHandler<ClickEventArgs> ItemClick;
            public event EventHandler<ClickEventArgs> ItemLongClick;
            private IList<ITreeViewItem> items;
            protected IList<ITreeViewItem> Items
            {
                get
                {
                    return items;
                }
                set
                {
                    items.Clear();
                    foreach (ITreeViewItem item in value) items.Add(item);
                    NotifyDataSetChanged();
                }
            }

            protected Adapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) => this.items = new List<ITreeViewItem>();
            public Adapter() => this.items = new List<ITreeViewItem>();
            public Adapter(IList<ITreeViewItem> items) => this.items = items;

            public void AddItem(ITreeViewItem item)
            {
                this.items.Add(item);
                NotifyItemChanged(this.items.Count - 1);
            }
            public void AddItems(params ITreeViewItem[] items)
            {
                int from = this.items.Count;
                foreach (ITreeViewItem item in items) this.items.Add(item);
                NotifyItemRangeChanged(from, items.Length);
            }
            public void AddItems(IList<ITreeViewItem> items)
            {
                int from = this.items.Count;
                foreach (ITreeViewItem item in items) this.items.Add(item);
                NotifyItemRangeChanged(from, items.Count);
            }
            public void RemoveItem(ITreeViewItem item)
            {
                int position = this.items.IndexOf(item);
                this.items.Remove(item);
                NotifyItemRemoved(position);
            }
            public void RemoveItems(params ITreeViewItem[] items)
            {
                foreach(ITreeViewItem item in items)
                {
                    this.items.Remove(item);
                }
                NotifyDataSetChanged();
            }
            public void RemoveItems(IList<ITreeViewItem> items)
            {
                foreach (ITreeViewItem item in items)
                {
                    this.items.Remove(item);
                }
                NotifyDataSetChanged();
            }

            public sealed override int ItemCount => this.items.Count;
            public sealed override long GetItemId(int position) => this.items[position].Id;
            public sealed override int GetItemViewType(int position) => this.items[position].Children.Count > 1 ? (int)ViewType.TreeNode : this.items[position].Children.Count;

            public void OnClick(ClickEventArgs args) => ItemClick?.Invoke(this, args);
            public void OnLongClick(ClickEventArgs args) => ItemLongClick?.Invoke(this, args);

            public enum ViewType{
                None = 0,
                Node = 1,
                TreeNode = 2,
            }
        }
        
        public new abstract class ViewHolder : RecyclerView.ViewHolder
        {
            public ViewHolder(View itemView, Action<ClickEventArgs> clickListener, Action<ClickEventArgs> longClickListener) : base(WrapView(itemView))
            {
                itemView.Click += (sender, e) => clickListener(new ClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new ClickEventArgs { View = itemView, Position = AdapterPosition });
            }

            private static View WrapView(View itemView)
            {
                return itemView;
                ViewGroup Parent = itemView.Parent as ViewGroup;
                LinearLayout RootView = LayoutInflater.From(itemView.Context).Inflate(Resource.Layout.tieredlistview_listitem, Parent, false) as LinearLayout;
                LinearLayout.LayoutParams parameters = itemView.LayoutParameters as LinearLayout.LayoutParams;
                parameters.Weight = 1;
                RootView.AddView(itemView, parameters);
                return RootView;
            }
        }

        public class ClickEventArgs : EventArgs
        {
            public View View { get; set; }
            public int Position { get; set; }
        }
    }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
}