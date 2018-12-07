using System.Collections.Generic;

namespace Xamarin.TreeView
{
    public class TreeViewNode : TreeViewNodeAbstract {}

    public abstract class TreeViewNodeAbstract : ITreeViewNode
    {
        private static int count = 0;
        public int Id { get; set; } = count++;
        public ITreeViewNode Parent { get; set; }
        public IList<ITreeViewNode> Children { get; set; } = new List<ITreeViewNode>();

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

    public interface ITreeViewNode
    {
        int Id { get; set; }
        ITreeViewNode Parent { get; set; }
        IList<ITreeViewNode> Children { get; set; }

        void AddChild(ITreeViewNode item);
        void RemoveChild(ITreeViewNode item);
        void ClearChildren();
    }
}