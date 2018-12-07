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
    [Register("treeview.TreeView", DoNotGenerateAcw = false)]
    public class TreeView : RecyclerView
    {
        private IList<ITreeViewNode> Nodes { get; set; } = null;

        private protected int NodeMargin { get; set; }
        private protected int HeadMargin { get; set; }

        public TreeView(Context context) : base(context) => Initialize(context);
        public TreeView(Context context, IAttributeSet attrs) : base(context, attrs) => Initialize(context);
        public TreeView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) => Initialize(context);

        private void Initialize(Context context)
        {
            LayoutManager layoutManager = new LinearLayoutManager(context, Vertical, false);
            this.SetLayoutManager(layoutManager);

            NodeMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 10, context.Resources.DisplayMetrics);
            HeadMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 5, context.Resources.DisplayMetrics);
        }

        [Obsolete("Use SetAdapter(TreeView.Adapter adapter) instead.", true)]
        public sealed override void SetAdapter(RecyclerView.Adapter adapter)
        {
            if(adapter is Adapter)
            {
                SetAdapter(adapter as Adapter);
            }
            else
            {
                throw new NotSupportedException("Use SetAdapter(TreeView.Adapter adapter) instead.");
            }
        }

        public void SetAdapter(Adapter adapter)
        {
            base.SetAdapter(adapter);
            if (this.Nodes != null)
            {
                adapter.AddNodes(this.Nodes);
                this.Nodes = null;
            }
            adapter.NodeMargin = this.NodeMargin;
            adapter.HeadMargin = this.HeadMargin;
        }

        [Obsolete("Use SwapAdapter(TreeView.Adapter adapter, bool removeAndRecycleExistingViews) instead.", true)]
        public sealed override void SwapAdapter(RecyclerView.Adapter adapter, bool removeAndRecycleExistingViews)
        {
            if (adapter is Adapter)
            {
                SwapAdapter(adapter as Adapter, removeAndRecycleExistingViews);
            }
            else
            {
                throw new NotSupportedException("Use SwapAdapter(TreeView.Adapter adapter, bool removeAndRecycleExistingViews) instead.");
            }
        }

        public void SwapAdapter(Adapter adapter, bool removeAndRecycleExistingViews)
        {
            base.SwapAdapter(adapter, removeAndRecycleExistingViews);
            if (this.Nodes != null)
            {
                adapter.AddNodes(this.Nodes);
                this.Nodes = null;
            }
            adapter.NodeMargin = this.NodeMargin;
            adapter.HeadMargin = this.HeadMargin;
        }

        new public Adapter GetAdapter() => (base.GetAdapter() as Adapter);

        public new abstract class Adapter : RecyclerView.Adapter
        {
            public const int WrapContent = ViewGroup.LayoutParams.WrapContent;
            public const int MatchParent = ViewGroup.LayoutParams.MatchParent;

            public event EventHandler<ClickEventArgs> Click;
            public event EventHandler<ClickEventArgs> LongClick;
            private IList<ITreeViewNode> nodes;
            protected IList<ITreeViewNode> Nodes
            {
                get
                {
                    return nodes;
                }
                set
                {
                    nodes.Clear();
                    foreach (ITreeViewNode node in value) nodes.Add(node);
                    NotifyDataSetChanged();
                }
            }
            protected int Layout { get; } = Resource.Layout.treeview_listitem_default;

            public int NodeMargin { get; set; }
            public int HeadMargin { get; set; }

            protected Adapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) => this.nodes = new List<ITreeViewNode>();
            public Adapter() : base() => this.nodes = new List<ITreeViewNode>();
            public Adapter(IList<ITreeViewNode> nodes) : base() => this.nodes = nodes;
            public Adapter(int layout) : base()
            {
                this.nodes = new List<ITreeViewNode>();
                this.Layout = layout;
            }
            public Adapter(int layout, IList<ITreeViewNode> nodes) : base()
            {
                this.nodes = nodes;
                this.Layout = layout;
            }

            public void AddNode(ITreeViewNode node)
            {
                this.nodes.Add(node);
                NotifyItemChanged(this.nodes.Count - 1);
            }
            public void AddNodes(params ITreeViewNode[] nodes)
            {
                int from = this.nodes.Count;
                foreach (ITreeViewNode node in nodes) this.nodes.Add(node);
                NotifyItemRangeChanged(from, nodes.Length);
            }
            public void AddNodes(IList<ITreeViewNode> nodes)
            {
                int from = this.nodes.Count;
                foreach (ITreeViewNode node in nodes) this.nodes.Add(node);
                NotifyItemRangeChanged(from, nodes.Count);
            }
            public void RemoveNode(ITreeViewNode node)
            {
                int position = this.nodes.IndexOf(node);
                this.nodes.Remove(node);
                NotifyItemRemoved(position);
            }
            public void RemoveNodes(params ITreeViewNode[] nodes)
            {
                foreach(ITreeViewNode node in nodes)
                {
                    this.nodes.Remove(node);
                }
                NotifyDataSetChanged();
            }
            public void RemoveNodes(IList<ITreeViewNode> nodes)
            {
                foreach (ITreeViewNode node in nodes)
                {
                    this.nodes.Remove(node);
                }
                NotifyDataSetChanged();
            }

            public sealed override int ItemCount => this.nodes.Count;
            public sealed override long GetItemId(int position) => this.nodes[position].Id;
            public sealed override int GetItemViewType(int position) => this.nodes[position].Children.Count > 0 ? (int)ViewType.TreeNode : (int)ViewType.Node;

            public void OnClick(ClickEventArgs args) => Click?.Invoke(this, args);
            public void OnLongClick(ClickEventArgs args) => LongClick?.Invoke(this, args);

            public enum ViewType{
                None = 0,
                Node = 1,
                TreeNode = 2,
            }

            public sealed override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View itemView = LayoutInflater.From(parent.Context).Inflate(Layout, parent, false);

                switch (viewType)
                {
                    case (int)ViewType.Node:
                        return OnCreateViewHolder(parent, itemView);
                    case (int)ViewType.TreeNode:
                        return OnCreateViewHolder(parent, new TreeView(parent.Context){
                                LayoutParameters = new LayoutParams(MatchParent, WrapContent)
                            }, itemView);
                    default:
                        goto case (int)ViewType.Node;
                }
            }

            public abstract ViewHolder OnCreateViewHolder(ViewGroup parent, TreeView tree, View itemView);
            public abstract ViewHolder OnCreateViewHolder(ViewGroup parent, View itemView);

            public sealed override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
            {
                if(position != 0)
                {
                    (viewHolder.ItemView.LayoutParameters as MarginLayoutParams).TopMargin = NodeMargin;
                }

                if(viewHolder is TreeViewHolder)
                {
                    TreeViewHolder vh = viewHolder as TreeViewHolder;
                    Adapter adapter = vh.Tree.GetAdapter();
                    if(adapter != null)
                    {
                        adapter.AddNodes(this.nodes[position].Children);
                    }
                    else
                    {
                        vh.Tree.Nodes = this.nodes[position].Children;
                    }
                    (vh.Tree.LayoutParameters as MarginLayoutParams).TopMargin = HeadMargin;
                    OnBindViewHolder(vh, position);
                }
                else if (viewHolder is NodeViewHolder)
                {
                    OnBindViewHolder(viewHolder as NodeViewHolder, position);
                }
            }

            public sealed override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position, IList<Java.Lang.Object> payloads)
            {
                base.OnBindViewHolder(holder, position, payloads);
            }

            public abstract void OnBindViewHolder(TreeViewHolder viewHolder, int position);
            public abstract void OnBindViewHolder(NodeViewHolder viewHolder, int position);
        }

        public new abstract class ViewHolder : RecyclerView.ViewHolder
        {
            public ViewHolder(View itemView, Action<ClickEventArgs> clickListener, Action<ClickEventArgs> longClickListener) : base(itemView)
            {
                itemView.Click += (sender, e) => clickListener(new ClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new ClickEventArgs { View = itemView, Position = AdapterPosition });
            }

            protected ViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer){}
        }

        public abstract class TreeViewHolder : ViewHolder
        {
            public TreeView Tree { get; }

            public TreeViewHolder(TreeView tree, View itemView, Action<ClickEventArgs> clickListener, Action<ClickEventArgs> longClickListener) : base(WrapView(tree, itemView), clickListener, longClickListener)
            {
                this.Tree = tree;
            }

            private static View WrapView(TreeView tree, View itemView)
            {
                LinearLayout RootView = LayoutInflater.From(itemView.Context).Inflate(Resource.Layout.treeview_listitem_wrapper, tree, false).FindViewById<LinearLayout>(Resource.Id.treeview_listitem_wrapper_root);
                RootView.LayoutParameters = itemView.LayoutParameters;
                RootView.LayoutParameters.Height = ViewGroup.LayoutParams.WrapContent;

                MarginLayoutParams parameters = (itemView as ViewGroup).GetChildAt(0).LayoutParameters as MarginLayoutParams;
                parameters.Width = ViewGroup.LayoutParams.MatchParent;
                parameters.SetMargins(0, 0, 0, 0);
                RootView.AddView(itemView, 0, parameters);

                LinearLayout wrapper = RootView.FindViewById<LinearLayout>(Resource.Id.treeview_listitem_wrapper_container);
                parameters = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1);
                wrapper.AddView(tree, parameters);
                return RootView;
            }
        }

        public abstract class NodeViewHolder : ViewHolder
        {
            public NodeViewHolder(View itemView, Action<ClickEventArgs> clickListener, Action<ClickEventArgs> longClickListener) : base(itemView, clickListener, longClickListener)
            {
                (itemView.LayoutParameters as MarginLayoutParams).SetMargins(0, 0, 0, 0);
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