using System;
using System.Collections.Generic;

namespace Xamarin.TreeView
{
    public abstract class TreeViewNodeAbstract : ITreeViewNode
    {
        protected static Random Random = new Random();
        public virtual int Id { get; set; } = Random.Next();
        public virtual ITreeViewNode Parent { get; set; }
        public virtual IList<ITreeViewNode> Children { get; } = new List<ITreeViewNode>();

        public virtual ITreeViewNode this[int index]
        {
            get => Children[index];
            set => Children[index] = value;
        }

        public virtual void AddChild(ITreeViewNode child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public virtual void ClearChildren()
        {
            foreach (ITreeViewNode child in Children)
            {
                child.Parent = null;
            }
            Children.Clear();
        }

        public virtual void RemoveChild(ITreeViewNode child)
        {
            if (Children.Remove(child)) child.Parent = null;
        }
    }

    public abstract class TreeViewLeafAbstract : ITreeViewNode
    {
        protected static Random Random = new Random();
        public virtual int Id { get; set; } = Random.Next();
        public virtual ITreeViewNode Parent { get; set; }
        public virtual IList<ITreeViewNode> Children { get; } = null;

        public virtual ITreeViewNode this[int index]
        {
            get => null;
            set { }
        }

        public virtual void AddChild(ITreeViewNode child) { }

        public virtual void ClearChildren() { }

        public virtual void RemoveChild(ITreeViewNode child) { }
    }

    public interface ITreeViewNode
    {
        int Id { get; set; }
        ITreeViewNode Parent { get; set; }
        IList<ITreeViewNode> Children { get; }

        void AddChild(ITreeViewNode child);
        void RemoveChild(ITreeViewNode child);
        void ClearChildren();
    }
}