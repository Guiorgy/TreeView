using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Xamarin.TreeView
{
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class TreeViewWrapper : RecyclerView
    {
        internal TreeViewWrapper(Context context) : base(context) { }
        internal TreeViewWrapper(Context context, IAttributeSet attrs) : base(context, attrs) { }
        internal TreeViewWrapper(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { }

        [EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never)]
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

            [EditorBrowsable(EditorBrowsableState.Never)]
            public sealed override void OnBindViewHolder(ViewHolder holder, int position, IList<Java.Lang.Object> payloads)
            {
                base.OnBindViewHolder(holder, position, payloads);
            }

            public sealed override void OnBindViewHolder(ViewHolder viewHolder, int position)
            {
                this.OnBindViewHolder(viewHolder as TreeView.ViewHolder, position);
            }

            public abstract void OnBindViewHolder(TreeView.ViewHolder viewHolder, int position);
        }
    }

    [Register("treeview.TreeView", DoNotGenerateAcw = true)]
    public class TreeView : TreeViewWrapper
    {
        private IList<ITreeViewNode> Nodes { get; set; } = null;
        private void SetClickListeners(EventHandler<ClickEventArgs> Click, EventHandler<ClickEventArgs> LongClick)
        {
            if (this.GetAdapter() == null) return;
            this.GetAdapter().Click += Click;
            this.GetAdapter().LongClick += LongClick;
        }

        #region Attributes
        [EditorBrowsable(EditorBrowsableState.Never)]
        public sealed class TreeViewAttributes
        {
            public ViewStates TraceVisibility;
            public int TraceColor;
            public int TraceWidth;
            public int TraceMargin;
            public int NodeMargin;
            public int HeadMargin;
            public int MaxDepth;
        }
        internal TreeViewAttributes Attributes = new TreeViewAttributes();

        public bool IsTraceVisible
        {
            get => Attributes.TraceVisibility == ViewStates.Visible;
            set
            {
                Attributes.TraceVisibility = value ? ViewStates.Visible : ViewStates.Invisible;
                Adapter adapter = this.GetAdapter();
                if (adapter != null) adapter.Attributes.TraceVisibility = Attributes.TraceVisibility;
            }
        }
        public int TraceColor
        {
            get => Attributes.TraceColor;
            set
            {
                Attributes.TraceColor = value;
                Adapter adapter = this.GetAdapter();
                if (adapter != null) adapter.Attributes.TraceColor = value;
            }
        }
        public int TraceWidth
        {
            get => Attributes.TraceWidth;
            set
            {
                Attributes.TraceWidth = value;
                Adapter adapter = this.GetAdapter();
                if (adapter != null) adapter.Attributes.TraceWidth = value;
            }
        }
        public int TraceMargin
        {
            get => Attributes.TraceMargin;
            set
            {
                Attributes.TraceMargin = value;
                Adapter adapter = this.GetAdapter();
                if (adapter != null) adapter.Attributes.TraceMargin = value;
            }
        }
        public int NodeMargin
        {
            get => Attributes.NodeMargin;
            set
            {
                Attributes.NodeMargin = value;
                Adapter adapter = this.GetAdapter();
                if (adapter != null) adapter.Attributes.NodeMargin = value;
            }
        }
        public int HeadMargin
        {
            get => Attributes.HeadMargin;
            set
            {
                Attributes.HeadMargin = value;
                Adapter adapter = this.GetAdapter();
                if (adapter != null) adapter.Attributes.HeadMargin = value;
            }
        }
        public int MaxDepth
        {
            get => Attributes.MaxDepth;
            set
            {
                Attributes.MaxDepth = value;
                Adapter adapter = this.GetAdapter();
                if (adapter != null) adapter.Attributes.MaxDepth = value;
            }
        }
        #endregion

        public TreeView(Context context) : base(context) => Initialize(context, null);
        public TreeView(Context context, IAttributeSet attrs) : base(context, attrs) => Initialize(context, attrs);
        public TreeView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) => Initialize(context, attrs);

        private void Initialize(Context context, IAttributeSet set)
        {
            if (IsInEditMode) return;

            LayoutManager layoutManager = new LinearLayoutManager(context, Vertical, false);
            this.SetLayoutManager(layoutManager);

            if (set == null)
            {
                Attributes.TraceVisibility = ViewStates.Visible;
                Attributes.TraceColor = unchecked((int)0xff00ff00);
                Attributes.TraceWidth = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 3, context.Resources.DisplayMetrics);
                Attributes.TraceMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 6, context.Resources.DisplayMetrics);
                Attributes.NodeMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 7, context.Resources.DisplayMetrics);
                Attributes.HeadMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 4, context.Resources.DisplayMetrics);
                Attributes.MaxDepth = int.MaxValue;
                return;
            }

            TypedArray attrs = context.ObtainStyledAttributes(set, Resource.Styleable.TreeView);
            try
            {
                Attributes.TraceVisibility = attrs.GetInt(Resource.Styleable.TreeView_trace_visibility, 0)
                    == 0 ? ViewStates.Visible : ViewStates.Invisible;
                Attributes.TraceColor = attrs.GetColor(Resource.Styleable.TreeView_trace_color, unchecked((int)0xffb3b3b3));
                Attributes.TraceWidth = (int)attrs.GetDimension(Resource.Styleable.TreeView_trace_width,
                    (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 3, context.Resources.DisplayMetrics));
                Attributes.TraceMargin = (int)attrs.GetDimension(Resource.Styleable.TreeView_trace_margin,
                    (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 6, context.Resources.DisplayMetrics));
                Attributes.NodeMargin = (int)attrs.GetDimension(Resource.Styleable.TreeView_node_margin,
                    (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 7, context.Resources.DisplayMetrics));
                Attributes.HeadMargin = (int)attrs.GetDimension(Resource.Styleable.TreeView_head_margin,
                    (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 4, context.Resources.DisplayMetrics));
                Attributes.MaxDepth = attrs.GetInt(Resource.Styleable.TreeView_maxDepth, int.MaxValue);
            }
            finally
            {
                attrs.Recycle();
            }
        }

        public override void SetAdapter(Adapter adapter)
        {
            base.SetAdapter(adapter);
            if (this.Nodes != null)
            {
                adapter.Nodes = this.Nodes;
                this.Nodes = null;
            }

            adapter.Attributes = Attributes;
        }

        public override void SwapAdapter(Adapter adapter, bool removeAndRecycleExistingViews)
        {
            if (this.Nodes != null)
            {
                adapter.Nodes = this.Nodes;
                this.Nodes = null;
            }
            base.SwapAdapter(adapter, removeAndRecycleExistingViews);

            adapter.Attributes = Attributes;
        }

        new public Adapter GetAdapter() => (base.GetAdapter() as Adapter);

        public new abstract class Adapter : TreeViewWrapper.Adapter
        {
            internal const int WrapContent = ViewGroup.LayoutParams.WrapContent;
            internal const int MatchParent = ViewGroup.LayoutParams.MatchParent;

            public event EventHandler<ClickEventArgs> Click;
            public event EventHandler<ClickEventArgs> LongClick;
            private IList<ITreeViewNode> nodes;
            internal protected IList<ITreeViewNode> Nodes
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
            protected int Layout { get; }
            protected internal TreeViewAttributes Attributes { get; set; }

            protected Adapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) => this.nodes = new List<ITreeViewNode>();
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

            #region Node Manipulations
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
            /*public void RemoveNode(ITreeViewNode node)
            {
                int position = this.nodes.IndexOf(node);
                this.nodes.Remove(node);
                NotifyItemRemoved(position);
            }
            public void RemoveNodes(params ITreeViewNode[] nodes)
            {
                foreach (ITreeViewNode node in nodes)
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
            }*/
            public void ClearNodes()
            {
                this.nodes.Clear();
                NotifyDataSetChanged();
            }
            #endregion

            public sealed override int ItemCount => this.nodes.Count;
            public sealed override long GetItemId(int position) => this.nodes[position].Id;
            public sealed override int GetItemViewType(int position) => this.nodes[position].Children.Count > 0 ? (int)NodeType.Node : (int)NodeType.Leaf;

            public void OnClick(ClickEventArgs args) => Click?.Invoke(this, args);
            public void OnLongClick(ClickEventArgs args) => LongClick?.Invoke(this, args);

            public enum NodeType {
                None = 0,
                Leaf = 1,
                Node = 2,
            }

            private static SparseArray<Color> colors = new SparseArray<Color>();
            private static Color GetColor(int hex)
            {
                Color color = colors.Get(hex, default);
                if (!color.Equals(default(Color))) return color;

                int alpha = Color.GetAlphaComponent(hex);
                int red = Color.GetRedComponent(hex);
                int green = Color.GetGreenComponent(hex);
                int blue = Color.GetBlueComponent(hex);
                color = Color.Argb(alpha, red, green, blue);

                colors.Put(hex, color);
                return color;
            }

            public sealed override ViewHolder OnCreateViewHolder(ViewGroup parent, NodeType nodeType)
            {
                View itemView = LayoutInflater.From(parent.Context).Inflate(Layout, parent, false);

                switch (nodeType)
                {
                    case NodeType.Leaf:
                        NodeViewHolder vh = OnCreateViewHolder(parent, itemView);
                        vh.SetClickListeners(OnClick, OnLongClick);
                        (vh.ItemView.LayoutParameters as MarginLayoutParams).TopMargin = Attributes.NodeMargin;
                        return vh;
                    case NodeType.Node:
                        TreeView tree = new TreeView(parent.Context) { LayoutParameters = new LayoutParams(MatchParent, WrapContent) };
                        tree.Attributes = Attributes;
                        TreeViewHolder tvh = OnCreateViewHolder(parent, tree, itemView);
                        tvh.SetClickListeners(OnClick, OnLongClick, Click, LongClick);
                        tvh.Trace.Visibility = Attributes.TraceVisibility;
                        tvh.Trace.SetBackgroundColor(GetColor(Attributes.TraceColor));
                        tvh.Trace.LayoutParameters.Width = Attributes.TraceWidth;
                        (tvh.Trace.LayoutParameters as MarginLayoutParams).RightMargin = Attributes.TraceMargin;
                        (tvh.ItemView.LayoutParameters as MarginLayoutParams).TopMargin = Attributes.NodeMargin;
                        (tvh.Tree.LayoutParameters as MarginLayoutParams).TopMargin = Attributes.HeadMargin;
                        return tvh;
                    default:
                        goto case NodeType.Leaf;
                }
            }

            public abstract TreeViewHolder OnCreateViewHolder(ViewGroup parent, TreeView tree, View itemView);
            public abstract NodeViewHolder OnCreateViewHolder(ViewGroup parent, View itemView);

            public sealed override void OnBindViewHolder(ViewHolder viewHolder, int position)
            {
                viewHolder.Node = nodes[position];

                if (position == 0)
                {
                    (viewHolder.ItemView.LayoutParameters as MarginLayoutParams).TopMargin = 0;
                }
                else
                {
                    (viewHolder.ItemView.LayoutParameters as MarginLayoutParams).TopMargin = Attributes.NodeMargin;
                }

                if (viewHolder is TreeViewHolder)
                {
                    TreeViewHolder vh = viewHolder as TreeViewHolder;
                    Adapter adapter = vh.Tree.GetAdapter();
                    if (adapter != null)
                    {
                        adapter.Nodes = this.nodes[position].Children;
                    }
                    else
                    {
                        vh.Tree.Nodes = this.nodes[position].Children;
                    }
                    OnBindViewHolder(vh, position);
                }
                else if (viewHolder is NodeViewHolder)
                {
                    OnBindViewHolder(viewHolder as NodeViewHolder, position);
                }
            }

            public abstract void OnBindViewHolder(TreeViewHolder viewHolder, int position);
            public abstract void OnBindViewHolder(NodeViewHolder viewHolder, int position);
        }

        new public abstract class ViewHolder : RecyclerView.ViewHolder
        {
            public ITreeViewNode Node { get; internal set; }

            internal ViewHolder(View itemView) : base(itemView) { }

            internal protected ViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        }

        public abstract class TreeViewHolder : ViewHolder
        {
            public TreeView Tree { get; }
            public View Head { get; }
            public View Trace { get; }

            public TreeViewHolder(TreeView tree, View itemView) : base(WrapView(tree, itemView))
            {
                this.Tree = tree;
                this.Head = itemView;
                this.Trace = this.ItemView.FindViewById(Resource.Id.treeview_listitem_wrapper_trace);
            }

            private static View WrapView(TreeView tree, View itemView)
            {
                ViewGroup RootView = LayoutInflater.From(itemView.Context).Inflate(Resource.Layout.treeview_listitem_wrapper, tree, false) as ViewGroup;
                RootView.LayoutParameters = new LayoutParams(itemView.LayoutParameters) { Height = Adapter.WrapContent };

                MarginLayoutParams parameters = itemView.LayoutParameters as MarginLayoutParams;
                parameters.Width = Adapter.MatchParent;
                parameters.SetMargins(0, 0, 0, 0);
                RootView.AddView(itemView, 0, parameters);

                LinearLayout wrapper = RootView.FindViewById<LinearLayout>(Resource.Id.treeview_listitem_wrapper_container);
                wrapper.AddView(tree, new LinearLayout.LayoutParams(0, Adapter.WrapContent, 1));

                return RootView;
            }

            internal void SetClickListeners(Action<ClickEventArgs> OnClick, Action<ClickEventArgs> OnLongClick, EventHandler<ClickEventArgs> Click, EventHandler<ClickEventArgs> LongClick)
            {
                this.Head.Click += (sender, e) => OnClick(new ClickEventArgs { Node = Node, View = this.Head, NodeType = Adapter.NodeType.Node, Position = AdapterPosition });
                this.Head.LongClick += (sender, e) => OnLongClick(new ClickEventArgs { Node = Node, View = this.Head, NodeType = Adapter.NodeType.Node, Position = AdapterPosition });
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
                this.ItemView.Click += (sender, e) => OnClick(new ClickEventArgs { Node = Node, View = this.ItemView, NodeType = Adapter.NodeType.Leaf, Position = AdapterPosition });
                this.ItemView.LongClick += (sender, e) => OnLongClick(new ClickEventArgs { Node = Node, View = this.ItemView, NodeType = Adapter.NodeType.Leaf, Position = AdapterPosition });
            }
        }

        public class ClickEventArgs : EventArgs
        {
            public ITreeViewNode Node { get; set; }
            public View View { get; set; }
            public Adapter.NodeType NodeType { get; set; }
            public int Position { get; set; }
        }

        #region Expand/Collapse Animation
        protected class CollapseAnimation : Animation
        {
            private View View;
            private readonly int height;

            public CollapseAnimation(View view, int duration)
            {
                this.height = view.MeasuredHeight;

                this.View = view;

                this.SetAnimationListener(new AnimationListener(view));
                this.Interpolator = new AccelerateInterpolator();
                this.Duration = duration;

                view.StartAnimation(this);
            }

            protected override void ApplyTransformation(float interpolatedTime, Transformation t)
            {
                View.LayoutParameters.Height = (int)(height - height * interpolatedTime);
                View.RequestLayout();
            }

            private class AnimationListener : Java.Lang.Object, IAnimationListener
            {
                private View View;

                public AnimationListener(View view)
                {
                    this.View = view;
                }

                public void OnAnimationStart(Animation animation) { }

                public void OnAnimationEnd(Animation animation)
                {
                    View.Visibility = ViewStates.Gone;
                }

                public void OnAnimationRepeat(Animation animation) { }
            }
        }

        protected class ExpandAnimation : Animation
        {
            private View View;
            private readonly int height;

            public ExpandAnimation(View view, int duration)
            {
                view.Measure(Adapter.MatchParent, Adapter.WrapContent);
                this.height = view.MeasuredHeight;
                view.LayoutParameters.Height = 1;
                view.Visibility = ViewStates.Visible;

                this.View = view;
                
                this.Interpolator = new AccelerateInterpolator();
                this.Duration = duration;

                view.StartAnimation(this);
            }

            protected override void ApplyTransformation(float interpolatedTime, Transformation t)
            {
                View.LayoutParameters.Height = (int)(height * interpolatedTime);
                View.RequestLayout();
            }
        }

        protected Animation animation;

        protected bool CollapseView(View view, int duration)
        {
            if (animation != null && !animation.HasEnded) return false;

            animation = new CollapseAnimation(view, duration);

            return true;
        }

        protected bool ExpandView(View view, int duration)
        {
            if (animation != null && !animation.HasEnded) return false;

            animation = new ExpandAnimation(view, duration);

            return true;
        }
        #endregion
    }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
}