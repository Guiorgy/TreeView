using System;
using Android.Views;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Graphics;

namespace Xamarin.TreeView
{
    public class TreeViewAdapter : TreeView.Adapter
    {
        private readonly int Layout;

        public TreeViewAdapter(int layout) : base()
        {
            this.Layout = layout;
        }
        public TreeViewAdapter(int layout, IList<ITreeViewNode> items) : base(items)
        {
            this.Layout = layout;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = null;

            switch (viewType)
            {
                case (int)ViewType.Node:
                    itemView = LayoutInflater.From(parent.Context).Inflate(Layout, parent, false);
                    break;
                case (int)ViewType.TreeNode:
                    itemView = new TreeView(parent.Context);
                    TreeViewAdapter adapter = new TreeViewAdapter(Layout);
                    (itemView as TreeView).SetAdapter(adapter);
                    break;
            }

            var vh = new ViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            ITreeViewNode item = Nodes[position];
            var holder = viewHolder as ViewHolder;
        }

        private class ViewHolder : TreeView.ViewHolder
        {
            public ViewHolder(View itemView, Action<TreeView.ClickEventArgs> clickListener, Action<TreeView.ClickEventArgs> longClickListener) : base(itemView, clickListener, longClickListener)
            {
            }
        }
    }
}