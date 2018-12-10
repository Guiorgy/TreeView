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
    public abstract class TreeViewWrapper : RecyclerView
    {
        internal TreeViewWrapper(Context context) : base(context) { }
        internal TreeViewWrapper(Context context, IAttributeSet attrs) : base(context, attrs) { }
        internal TreeViewWrapper(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { }

        [Obsolete("Use SetAdapter(TreeView.Adapter adapter) instead.", true)]
        public sealed override void SetAdapter(RecyclerView.Adapter adapter)
        {
            if (adapter is Adapter)
            {
                SetAdapter(adapter as Adapter);
            }
            else
            {
                throw new NotSupportedException("Use SetAdapter(TreeView.Adapter adapter) instead.");
            }
        }

        public virtual void SetAdapter(TreeView.Adapter adapter)
        {
            base.SetAdapter(adapter);
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

        public virtual void SwapAdapter(TreeView.Adapter adapter, bool removeAndRecycleExistingViews)
        {
            base.SwapAdapter(adapter, removeAndRecycleExistingViews);
        }

        public new abstract class Adapter : RecyclerView.Adapter
        {
            internal protected Adapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
            internal Adapter() : base() { }

            public sealed override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                return this.OnCreateViewHolder(parent, (TreeView.Adapter.NodeType)viewType);
            }

            public abstract TreeView.ViewHolder OnCreateViewHolder(ViewGroup parent, TreeView.Adapter.NodeType nodeType);

            public sealed override void OnBindViewHolder(ViewHolder viewHolder, int position)
            {
                this.OnBindViewHolder(viewHolder as TreeView.ViewHolder, position);
            }

            public abstract void OnBindViewHolder(TreeView.ViewHolder viewHolder, int position);
        }
    }

    [Register("treeview.TreeView", DoNotGenerateAcw = false)]
    public class TreeView : TreeViewWrapper
    {
        private IList<ITreeViewNode> Nodes { get; set; } = null;
        private void SetClickListeners(EventHandler<ClickEventArgs> Click, EventHandler<ClickEventArgs> LongClick)
        {
            if (this.GetAdapter() == null) return;
            this.GetAdapter().Click += Click;
            this.GetAdapter().LongClick += LongClick;
        }

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

        public override void SetAdapter(Adapter adapter)
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

        public override void SwapAdapter(Adapter adapter, bool removeAndRecycleExistingViews)
        {
            if (this.Nodes != null)
            {
                adapter.AddNodes(this.Nodes);
                this.Nodes = null;
            }
            base.SwapAdapter(adapter, removeAndRecycleExistingViews);
            adapter.NodeMargin = this.NodeMargin;
            adapter.HeadMargin = this.HeadMargin;
        }

        new public Adapter GetAdapter() => (base.GetAdapter() as Adapter);
        
        public new abstract class Adapter : TreeViewWrapper.Adapter
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
            public sealed override int GetItemViewType(int position) => this.nodes[position].Children.Count > 0 ? (int)NodeType.TreeNode : (int)NodeType.SingleNode;

            public void OnClick(ClickEventArgs args) => Click?.Invoke(this, args);
            public void OnLongClick(ClickEventArgs args) => LongClick?.Invoke(this, args);

            public enum NodeType{
                None = 0,
                SingleNode = 1,
                TreeNode = 2,
            }

            public override ViewHolder OnCreateViewHolder(ViewGroup parent, NodeType nodeType)
            {
                View itemView = LayoutInflater.From(parent.Context).Inflate(Layout, parent, false);

                switch (nodeType)
                {
                    case NodeType.SingleNode:
                        NodeViewHolder vh = OnCreateViewHolder(parent, itemView);
                        vh.SetClickListeners(OnClick, OnLongClick);
                        return vh;
                    case NodeType.TreeNode:
                        TreeViewHolder tvh = OnCreateViewHolder(parent, new TreeView(parent.Context)
                        {
                            LayoutParameters = new LayoutParams(MatchParent, WrapContent)
                        }, itemView);
                        tvh.SetClickListeners(OnClick, OnLongClick, Click, LongClick);
                        return tvh;
                    default:
                        goto case NodeType.SingleNode;
                }
            }

            public abstract TreeViewHolder OnCreateViewHolder(ViewGroup parent, TreeView tree, View itemView);
            public abstract NodeViewHolder OnCreateViewHolder(ViewGroup parent, View itemView);

            public override void OnBindViewHolder(ViewHolder viewHolder, int position)
            {
                if (position != 0)
                {
                    (viewHolder.ItemView.LayoutParameters as MarginLayoutParams).TopMargin = NodeMargin;
                }

                if (viewHolder is TreeViewHolder)
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

        new public abstract class ViewHolder : RecyclerView.ViewHolder
        {
            internal ViewHolder(View itemView) : base(itemView) { }

            internal protected ViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        }

        public abstract class TreeViewHolder : ViewHolder
        {
            public TreeView Tree { get; }
            public View Head { get; }

            public TreeViewHolder(TreeView tree, View itemView) : base(WrapView(tree, itemView))
            {
                this.Tree = tree;
                this.Head = itemView;
            }

            private static View WrapView(TreeView tree, View itemView)
            {
                LinearLayout RootView = LayoutInflater.From(itemView.Context).Inflate(Resource.Layout.treeview_listitem_wrapper, tree, false).FindViewById<LinearLayout>(Resource.Id.treeview_listitem_wrapper_root);
                RootView.LayoutParameters = new LayoutParams(itemView.LayoutParameters);
                RootView.LayoutParameters.Height = ViewGroup.LayoutParams.WrapContent;

                MarginLayoutParams parameters = itemView.LayoutParameters as MarginLayoutParams;
                parameters.Width = ViewGroup.LayoutParams.MatchParent;
                parameters.SetMargins(0, 0, 0, 0);
                RootView.AddView(itemView, 0, parameters);

                LinearLayout wrapper = RootView.FindViewById<LinearLayout>(Resource.Id.treeview_listitem_wrapper_container);
                wrapper.AddView(tree, new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1));

                return RootView;
            }

            internal void SetClickListeners(Action<ClickEventArgs> OnClick, Action<ClickEventArgs> OnLongClick, EventHandler<ClickEventArgs> Click, EventHandler<ClickEventArgs> LongClick)
            {
                this.Head.Click += (sender, e) => OnClick(new ClickEventArgs { View = this.Head, NodeType = Adapter.NodeType.TreeNode, Position = AdapterPosition });
                this.Head.LongClick += (sender, e) => OnLongClick(new ClickEventArgs { View = this.Head, NodeType = Adapter.NodeType.TreeNode, Position = AdapterPosition });
                this.Tree.SetClickListeners(Click, LongClick);
            }
        }

        public abstract class NodeViewHolder : ViewHolder
        {
            public NodeViewHolder(View itemView) : base(itemView)
            {
                (itemView.LayoutParameters as MarginLayoutParams).SetMargins(0, 0, 0, 0);
            }

            internal void SetClickListeners(Action<ClickEventArgs> OnClick, Action<ClickEventArgs> OnLongClick)
            {
                this.ItemView.Click += (sender, e) => OnClick(new ClickEventArgs { View = this.ItemView, NodeType = Adapter.NodeType.SingleNode, Position = AdapterPosition });
                this.ItemView.LongClick += (sender, e) => OnLongClick(new ClickEventArgs { View = this.ItemView, NodeType = Adapter.NodeType.SingleNode, Position = AdapterPosition });
            }
        }

        public class ClickEventArgs : EventArgs
        {
            public View View { get; set; }
            public Adapter.NodeType NodeType { get; set; }
            public int Position { get; set; }
        }
    }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
}