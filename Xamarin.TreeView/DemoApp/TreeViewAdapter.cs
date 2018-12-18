using Android.Views;
using System.Collections.Generic;
using Android.Widget;
using Xamarin.TreeView;
using static Xamarin.TreeView.TreeView;
using Android.Graphics;

namespace DemoApp
{
    public class TreeViewNode : TreeViewNodeAbstract
    {
        public bool? Collapsed { get; set; } = true;
    }
    public class TreeViewLeaf : TreeViewLeafAbstract { }

    public class TreeViewAdapter : TreeView.Adapter
    {
        public TreeViewAdapter() : base()
        {}
        public TreeViewAdapter(IList<ITreeViewNode> items) : base(items)
        {}

        public override int GetLayout(NodeType nodeType, int viewType) => Resource.Layout.treeview_node;

        public override TreeView.NodeViewHolder OnCreateViewHolder(ViewGroup parent, TreeView tree, View itemView, int viewType)
        {
            TreeViewAdapter adapter = new TreeViewAdapter();
            tree.SetAdapter(adapter);
            return new NodeViewHolder(tree, itemView);
        }

        public override TreeView.LeafViewHolder OnCreateViewHolder(ViewGroup parent, View itemView, int viewType)
        {
            return new LeafViewHolder(itemView);
        }

        public override void OnBindViewHolder(TreeView.NodeViewHolder viewHolder, int position)
        {
            ITreeViewNode node = Nodes[position];
            var holder = viewHolder as NodeViewHolder;

            if(node is TreeViewNode && (node as TreeViewNode).Collapsed == true)
            {
                holder.TreeContainer.Visibility = ViewStates.Gone;
                holder.Collapsed = true;
                (node as TreeViewNode).Collapsed = null;
            }

            if (holder.TextView != null)
                holder.TextView.Text = $"{holder.Level}, {position}, {node.Id}";
        }

        public override void OnBindViewHolder(TreeView.LeafViewHolder viewHolder, int position)
        {
            ITreeViewNode node = Nodes[position];
            var holder = viewHolder as LeafViewHolder;

            holder.ItemView.SetBackgroundColor(Color.Green);

            if (holder.TextView != null)
                holder.TextView.Text = $"{holder.Level}, {position}, {node.Id}";
        }

        public override short GetViewType(int position) => 1;

        private class NodeViewHolder : TreeView.NodeViewHolder
        {
            protected ExpandCollapseAnimation ExpandCollapseAnimations;
            public bool Collapsed { get; set; }
            public TextView TextView { get; }

            public NodeViewHolder(TreeView tree, View itemView) : base(tree, itemView)
            {
                this.TextView = itemView.FindViewById<TextView>(Resource.Id.text);

                this.Head.Click += (object sender, System.EventArgs e) =>
                {
                    if (ExpandCollapseAnimations != null && !ExpandCollapseAnimations.HasEnded) return;
                    if (Collapsed) ExpandCollapseAnimations = new ExpandCollapseAnimation(this.TreeContainer, ExpandCollapseAnimation.AnimationMode.Expand, doFade: true, duration: 60, dynamic: true);
                    else ExpandCollapseAnimations = new ExpandCollapseAnimation(this.TreeContainer, ExpandCollapseAnimation.AnimationMode.Collapse, doFade: true, duration: 60, dynamic: true);
                    Collapsed = !Collapsed;
                };
            }
        }

        private class LeafViewHolder : TreeView.LeafViewHolder
        {
            public TextView TextView { get; }

            public LeafViewHolder(View itemView) : base(itemView)
            {
                this.TextView = itemView.FindViewById<TextView>(Resource.Id.text);
            }
        }
    }
}