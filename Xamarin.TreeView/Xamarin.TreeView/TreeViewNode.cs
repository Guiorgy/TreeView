using System;
using System.Collections.Generic;

namespace Xamarin.TreeView
{
    public class TreeViewNode : TreeViewNodeAbstract { }
    public class TreeViewLeaf : TreeViewLeafAbstract { }

    public abstract class TreeViewNodeAbstract : ITreeViewNode
    {
        private static Random Random = new Random();
        public int Id { get; set; } = Random.Next();
        public ITreeViewNode Parent { get; set; }
        public IList<ITreeViewNode> Children { get; } = new List<ITreeViewNode>();

        public ITreeViewNode this[int index]
        {
            get => Children[index];
            set => Children[index] = value;
        }

        public void AddChild(ITreeViewNode child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public void ClearChildren()
        {
            foreach (ITreeViewNode child in Children)
            {
                child.Parent = null;
            }
            Children.Clear();
        }

        public void RemoveChild(ITreeViewNode child)
        {
            if (Children.Remove(child)) child.Parent = null;
        }
    }

    public abstract class TreeViewLeafAbstract : ITreeViewNode
    {
        private static Random Random = new Random();
        public int Id { get; set; } = Random.Next();
        public ITreeViewNode Parent { get; set; }
        public IList<ITreeViewNode> Children { get; } = null;

        public ITreeViewNode this[int index]
        {
            get => null;
            set { }
        }

        public void AddChild(ITreeViewNode child) { }

        public void ClearChildren() { }

        public void RemoveChild(ITreeViewNode child) { }
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