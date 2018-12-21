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
        public bool Collapsed { get; set; } = false;
        public TreeViewNode() { }
        public TreeViewNode(bool collapsed) { Collapsed = collapsed; }
    }
    public class TreeViewLeaf : TreeViewLeafAbstract { }

    public class TreeViewAdapter : TreeView.Adapter
    {
        public TreeViewAdapter() : base() { }
        public TreeViewAdapter(IList<ITreeViewNode> items) : base(items) { }

        public override short GetViewType(int position) => 1;
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

            if(node is TreeViewNode)
            {
                holder.TreeContainer.Visibility = (node as TreeViewNode).Collapsed ? ViewStates.Gone : ViewStates.Visible;
            }

            if (holder.TextView != null)
                holder.TextView.Text = $"{holder.Level}, {position}, {node.Id}";
        }

        public override void OnBindViewHolder(TreeView.LeafViewHolder viewHolder, int position)
        {
            ITreeViewNode node = Nodes[position];
            var holder = viewHolder as LeafViewHolder;

            holder.ItemView.SetBackgroundColor(Color.Green);
            if (node is TreeViewNode) (node as TreeViewNode).Collapsed = false;

            if (holder.TextView != null)
                holder.TextView.Text = $"{holder.Level}, {position}, {node.Id}";
        }

        private class NodeViewHolder : TreeView.NodeViewHolder
        {
            //protected ExpandCollapseAnimation ExpandCollapseAnimations;
            public TextView TextView { get; }

            public NodeViewHolder(TreeView tree, View itemView) : base(tree, itemView)
            {
                this.TextView = itemView.FindViewById<TextView>(Resource.Id.text);

                this.Head.Click += (object sender, System.EventArgs e) =>
                {
                    var node = Node as TreeViewNode;
                    /*if (ExpandCollapseAnimations != null && !ExpandCollapseAnimations.HasEnded) return;
                    if (node.Collapsed) ExpandCollapseAnimations = new ExpandCollapseAnimation(this.TreeContainer, ExpandCollapseAnimation.AnimationMode.Expand, doFade: true, duration: 60, dynamic: true);
                    else ExpandCollapseAnimations = new ExpandCollapseAnimation(this.TreeContainer, ExpandCollapseAnimation.AnimationMode.Collapse, doFade: true, duration: 60, dynamic: true);*/
                    if (node.Collapsed) ExpandView(this.TreeContainer, duration: 60, dynamic: true, doFade: true);
                    else CollapseView(this.TreeContainer, duration: 60, dynamic: true, doFade: true);
                    node.Collapsed = !node.Collapsed;
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