using System;
using Android.Views;
using System.Collections.Generic;
using Android.Widget;

namespace Xamarin.TreeView
{
    public class TreeViewAdapter : TreeView.Adapter
    {
        public TreeViewAdapter() : base()
        {}
        public TreeViewAdapter(IList<ITreeViewNode> items) : base(items)
        {}

        public override TreeView.ViewHolder OnCreateViewHolder(ViewGroup parent, TreeView tree, View itemView)
        {
            TreeViewAdapter adapter = new TreeViewAdapter();
            tree.SetAdapter(adapter);
            return new TreeViewHolder(tree, itemView, OnClick, OnLongClick);
        }

        public override TreeView.ViewHolder OnCreateViewHolder(ViewGroup parent, View itemView)
        {
            return new NodeViewHolder(itemView, OnClick, OnLongClick);
        }

        public override void OnBindViewHolder(TreeView.TreeViewHolder viewHolder, int position)
        {
            ITreeViewNode node = Nodes[position];
            var holder = viewHolder as TreeViewHolder;

            if (holder.TextView != null)
                holder.TextView.Text = $"{position}, {node.Children.Count}";
        }

        public override void OnBindViewHolder(TreeView.NodeViewHolder viewHolder, int position)
        {
            ITreeViewNode node = Nodes[position];
            var holder = viewHolder as NodeViewHolder;

            if (holder.TextView != null)
                holder.TextView.Text = $"{position}, {node.Children.Count}";
        }

        private class NodeViewHolder : TreeView.NodeViewHolder
        {
            public TextView TextView { get; }

            public NodeViewHolder(View itemView, Action<TreeView.ClickEventArgs> clickListener, Action<TreeView.ClickEventArgs> longClickListener) : base(itemView, clickListener, longClickListener)
            {
                this.TextView = itemView.FindViewById<TextView>(Resource.Id.text);
            }
        }

        private class TreeViewHolder : TreeView.TreeViewHolder
        {
            public TextView TextView { get; }

            public TreeViewHolder(TreeView tree, View itemView, Action<TreeView.ClickEventArgs> clickListener, Action<TreeView.ClickEventArgs> longClickListener) : base(tree, itemView, clickListener, longClickListener)
            {
                this.TextView = itemView.FindViewById<TextView>(Resource.Id.text);
            }
        }
    }
}