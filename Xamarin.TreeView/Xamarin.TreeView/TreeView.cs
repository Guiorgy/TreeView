﻿using Android.Content;
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

            protected Adapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) => this.nodes = new List<ITreeViewNode>();
            public Adapter() => this.nodes = new List<ITreeViewNode>();
            public Adapter(IList<ITreeViewNode> nodes) => this.nodes = nodes;

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
            public sealed override int GetItemViewType(int position) => this.nodes[position].Children.Count > 1 ? (int)ViewType.TreeNode : this.nodes[position].Children.Count;

            public void OnClick(ClickEventArgs args) => Click?.Invoke(this, args);
            public void OnLongClick(ClickEventArgs args) => LongClick?.Invoke(this, args);

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