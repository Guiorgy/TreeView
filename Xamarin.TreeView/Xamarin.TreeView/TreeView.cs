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

    [Register("treeview.TreeView")]
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
        public class TreeViewAttributes
        {
            public ViewStates TraceVisibility;
            public int TraceColor;
            public int TraceWidth;
            public int TraceMargin;
            public int NodeMargin;
            public int HeadMargin;
            public int MaxLevel;
            protected internal int Level;
            protected internal bool Collapsed;

            public TreeViewAttributes() { }
            protected internal TreeViewAttributes(TreeViewAttributes attributes)
            {
                this.TraceVisibility = attributes.TraceVisibility;
                this.TraceColor = attributes.TraceColor;
                this.TraceWidth = attributes.TraceWidth;
                this.TraceMargin = attributes.TraceMargin;
                this.NodeMargin = attributes.NodeMargin;
                this.HeadMargin = attributes.HeadMargin;
                this.MaxLevel = attributes.MaxLevel;
                this.Level = attributes.Level;
                this.Collapsed = attributes.Collapsed;
            }
        }
        internal TreeViewAttributes Attributes;

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
        public int MaxLevel
        {
            get => Attributes.MaxLevel;
            set
            {
                Attributes.MaxLevel = value;
                Adapter adapter = this.GetAdapter();
                if (adapter != null) adapter.Attributes.MaxLevel = value;
            }
        }
        protected internal int Level
        {
            get => Attributes.Level;
            set
            {
                Attributes.Level = value;
                Adapter adapter = this.GetAdapter();
                if (adapter != null) adapter.Attributes.Level = value;
            }
        }
        public bool Collapsed
        {
            get => Attributes.Collapsed;
            protected internal set
            {
                Attributes.Collapsed = value;
                Adapter adapter = this.GetAdapter();
                if (adapter != null) adapter.Attributes.Collapsed = value;
            }
        }
        #endregion

        public TreeView(Context context) : base(context) => Initialize(context, null);
        public TreeView(Context context, IAttributeSet attrs) : base(context, attrs) => Initialize(context, attrs);
        public TreeView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) => Initialize(context, attrs);
        protected internal TreeView(Context context, TreeViewAttributes attributes) : base(context)
        {
            if (this.GetLayoutManager() == null)
            {
                LayoutManager layoutManager = new LinearLayoutManager(context, Vertical, false);
                this.SetLayoutManager(layoutManager);
            }

            this.Attributes = attributes;
            this.Attributes.Level++;
        }

        private void Initialize(Context context, IAttributeSet set)
        {
            if (IsInEditMode) return;

            if (this.GetLayoutManager() == null)
            {
                LayoutManager layoutManager = new LinearLayoutManager(context, Vertical, false);
                this.SetLayoutManager(layoutManager);
            }

            if (set == null)
            {
                if(Attributes == null)
                {
                    Attributes = new TreeViewAttributes
                    {
                        TraceVisibility = ViewStates.Visible,
                        TraceColor = unchecked((int)0xff00ff00),
                        TraceWidth = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 3, context.Resources.DisplayMetrics),
                        TraceMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 6, context.Resources.DisplayMetrics),
                        NodeMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 7, context.Resources.DisplayMetrics),
                        HeadMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 4, context.Resources.DisplayMetrics),
                        Collapsed = false,
                        MaxLevel = int.MaxValue,
                    };
                }
                return;
            }

            TypedArray attrs = context.ObtainStyledAttributes(set, Resource.Styleable.TreeView);
            try
            {
                if (Attributes == null) Attributes = new TreeViewAttributes();

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
                Attributes.Collapsed = attrs.GetBoolean(Resource.Styleable.TreeView_collapsed, false);
                Attributes.MaxLevel = attrs.GetInt(Resource.Styleable.TreeView_maxLevel, int.MaxValue);
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

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            if (Attributes?.Level != 0 && e.Action == MotionEventActions.Move) return true;
            return base.DispatchTouchEvent(e);
        }

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
            internal TreeViewAttributes Attributes { get; set; }

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
            
            public sealed override ViewHolder OnCreateViewHolder(ViewGroup parent, NodeType nodeType)
            {
                View itemView = LayoutInflater.From(parent.Context).Inflate(Layout, parent, false);

                switch (nodeType)
                {
                    case NodeType.Leaf:
                        NodeViewHolder vh = OnCreateViewHolder(parent, itemView);
                        vh.SetClickListeners(OnClick, OnLongClick);
                        return vh;
                    case NodeType.Node:
                        TreeView tree = new TreeView(parent.Context, new TreeViewAttributes(Attributes)) { LayoutParameters = new LayoutParams(MatchParent, WrapContent) };
                        TreeViewHolder tvh = OnCreateViewHolder(parent, tree, itemView);
                        tvh.SetClickListeners(OnClick, OnLongClick, Click, LongClick);
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
                viewHolder.Level = Attributes.Level;

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
            public int Level { get; internal set; }

            internal ViewHolder(View itemView) : base(itemView) { }
            internal protected ViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        }

        public abstract class TreeViewHolder : ViewHolder
        {
            public TreeView Tree { get; }
            public View Head { get; }
            public View Trace { get; }
            public LinearLayout TreeContainer { get; }

            public TreeViewHolder(TreeView tree, View itemView) : base(WrapView(tree, itemView))
            {
                this.Tree = tree;
                this.Head = itemView;
                this.Trace = this.ItemView.FindViewById(Resource.Id.treeview_listitem_wrapper_trace);
                this.TreeContainer = this.ItemView.FindViewById<LinearLayout>(Resource.Id.treeview_listitem_wrapper_container);

                (this.Trace.LayoutParameters as MarginLayoutParams).RightMargin = tree.TraceMargin;
                (this.ItemView.LayoutParameters as MarginLayoutParams).TopMargin = tree.NodeMargin;
                (this.Tree.LayoutParameters as MarginLayoutParams).TopMargin = tree.HeadMargin;

                if(tree.Level > tree.MaxLevel)
                {
                    this.Trace.Visibility = ViewStates.Gone;
                }
                else
                {
                    this.Trace.Visibility = tree.Attributes.TraceVisibility;
                    this.Trace.SetBackgroundColor(GetColor(tree.TraceColor));
                    this.Trace.LayoutParameters.Width = tree.TraceWidth;
                }

                this.TreeContainer.Visibility = tree.Collapsed ? ViewStates.Gone : ViewStates.Visible;
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
                this.Head.Click += (sender, e) => OnClick(new ClickEventArgs { Node = Node, View = this.Head, NodeType = Adapter.NodeType.Node, Level = Level, Position = AdapterPosition });
                this.Head.LongClick += (sender, e) => OnLongClick(new ClickEventArgs { Node = Node, View = this.Head, NodeType = Adapter.NodeType.Node, Level = Level, Position = AdapterPosition });
                this.Tree.SetClickListeners(Click, LongClick);
            }

            protected static SparseArray<Color> colors = new SparseArray<Color>();
            protected static Color GetColor(int hex)
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
        }

        public abstract class NodeViewHolder : ViewHolder
        {
            public NodeViewHolder(View itemView) : base(itemView) { }

            internal void SetClickListeners(Action<ClickEventArgs> OnClick, Action<ClickEventArgs> OnLongClick)
            {
                this.ItemView.Click += (sender, e) => OnClick(new ClickEventArgs { Node = Node, View = this.ItemView, NodeType = Adapter.NodeType.Leaf, Level = Level, Position = AdapterPosition });
                this.ItemView.LongClick += (sender, e) => OnLongClick(new ClickEventArgs { Node = Node, View = this.ItemView, NodeType = Adapter.NodeType.Leaf, Level = Level, Position = AdapterPosition });
            }
        }

        public class ClickEventArgs : EventArgs
        {
            public ITreeViewNode Node { get; set; }
            public View View { get; set; }
            public Adapter.NodeType NodeType { get; set; }
            public int Level { get; set; }
            public int Position { get; set; }
        }



        #region Expand/Collapse Animation
        public class ExpandCollapseAnimation : Animation
        {
            protected readonly View View;
            protected readonly bool Expand;
            protected readonly int Height;
            protected readonly bool Fade;

            public ExpandCollapseAnimation(View view, bool expand, IInterpolator interpolator = null, int duration = DefaultDuration, bool doFade = Defaultfade, bool optimized = DefaultOptimized)
            {
                if (expand)
                {
                    //view.Measure(Adapter.MatchParent, Adapter.WrapContent);
                    view.Measure(MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified), MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
                    view.LayoutParameters.Height = 1;
                }
                this.Height = view.MeasuredHeight;
                view.Visibility = ViewStates.Visible;

                this.View = view;
                this.Fade = doFade;
                this.Expand = expand;

                this.SetAnimationListener(new AnimationListener(view, expand, optimized));
                this.Interpolator = interpolator ?? new AccelerateInterpolator();
                this.Duration = duration;

                view.StartAnimation(this);
            }

            protected override void ApplyTransformation(float interpolatedTime, Transformation t)
            {
                if (Expand)
                {
                    if (Fade) t.Alpha = interpolatedTime == 1 ? 1 : 0.2f + interpolatedTime * 0.8f;
                    View.LayoutParameters.Height = interpolatedTime == 1 ? Adapter.WrapContent : (int)(Height * interpolatedTime);
                }
                else
                {
                    if (Fade) t.Alpha = interpolatedTime == 1 ? 0 : 1 - interpolatedTime * 0.8f;
                    View.LayoutParameters.Height = interpolatedTime == 1 ? 0 : (int)(Height - Height * interpolatedTime);
                }
                View.Invalidate();
                View.RequestLayout();
            }

            private class AnimationListener : Java.Lang.Object, IAnimationListener
            {
                private readonly View View;
                private readonly bool Expand;
                private readonly bool Optimized;

                public AnimationListener(View view, bool expand, bool optimized)
                {
                    this.View = view;
                    this.Optimized = optimized;
                    this.Expand = expand;
                }

                public void OnAnimationStart(Animation animation)
                {
                    if (Optimized && View is ViewGroup)
                    {
                        ViewGroup view = View as ViewGroup;
                        for (int i = view.ChildCount - 1; i >= 0; --i)
                        {
                            view.GetChildAt(i).Visibility = ViewStates.Gone;
                        }
                    }
                }

                public void OnAnimationEnd(Animation animation)
                {
                    if (Optimized && View is ViewGroup)
                    {
                        ViewGroup view = View as ViewGroup;
                        for (int i = view.ChildCount - 1; i >= 0; --i)
                        {
                            view.GetChildAt(i).Visibility = ViewStates.Visible;
                        }
                    }
                    View.Visibility = Expand ? ViewStates.Visible : ViewStates.Gone;
                }

                public void OnAnimationRepeat(Animation animation) { }
            }

            public override bool WillChangeBounds()
            {
                return true;
            }
        }

        protected const int DefaultDuration = 500;
        protected const bool Defaultfade = false;
        protected const bool DefaultOptimized = false;
        #endregion
    }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
}