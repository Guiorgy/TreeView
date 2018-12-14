﻿using Android.Views;
using System.Collections.Generic;
using Android.Widget;
using Xamarin.TreeView;
using static Xamarin.TreeView.TreeView;

namespace DemoApp
{
    public class TreeViewAdapter : TreeView.Adapter
    {
        public TreeViewAdapter() : base(Resource.Layout.treeview_node)
        {}
        public TreeViewAdapter(IList<ITreeViewNode> items) : base(Resource.Layout.treeview_node, items)
        {}

        public override TreeView.TreeViewHolder OnCreateViewHolder(ViewGroup parent, TreeView tree, View itemView)
        {
            TreeViewAdapter adapter = new TreeViewAdapter();
            tree.SetAdapter(adapter);
            return new TreeViewHolder(tree, itemView);
        }

        public override TreeView.NodeViewHolder OnCreateViewHolder(ViewGroup parent, View itemView)
        {
            return new NodeViewHolder(itemView);
        }

        public override void OnBindViewHolder(TreeView.TreeViewHolder viewHolder, int position)
        {
            ITreeViewNode node = Nodes[position];
            var holder = viewHolder as TreeViewHolder;

            if (holder.TextView != null)
                holder.TextView.Text = $"{position}, {node.Id}";
        }

        public override void OnBindViewHolder(TreeView.NodeViewHolder viewHolder, int position)
        {
            ITreeViewNode node = Nodes[position];
            var holder = viewHolder as NodeViewHolder;

            if (holder.TextView != null)
                holder.TextView.Text = $"{position}, {node.Id}";
        }
        
        private class TreeViewHolder : TreeView.TreeViewHolder
        {
            public TextView TextView { get; }
            private bool collapsed = true;

            public TreeViewHolder(TreeView tree, View itemView) : base(tree, itemView)
            {
                this.TextView = itemView.FindViewById<TextView>(Resource.Id.text);

                this.Head.Click += Head_Click;
            }

            private void Head_Click(object sender, System.EventArgs e)
            {
                if (collapsed) this.ExpandView(this.TreeContainer, fadein: true);
                else this.CollapseView(this.TreeContainer, fadeout: true);
                collapsed = !collapsed;
            }
        }

        private class NodeViewHolder : TreeView.NodeViewHolder
        {
            public TextView TextView { get; }

            public NodeViewHolder(View itemView) : base(itemView)
            {
                this.TextView = itemView.FindViewById<TextView>(Resource.Id.text);
            }
        }
    }
}