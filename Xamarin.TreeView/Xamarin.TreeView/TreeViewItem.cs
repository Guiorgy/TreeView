using System.Collections.Generic;

namespace Xamarin.TreeView
{
    public class TreeViewItem : TreeViewItemAbstract {}

    public abstract class TreeViewItemAbstract : ITreeViewItem
    {
        private static int count = 0;
        public int Id { get; set; } = count++;
        public ITreeViewItem Parent { get; set; }
        public IList<ITreeViewItem> Children { get; set; } = new List<ITreeViewItem>();

        public void AddChild(ITreeViewItem child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public void ClearChildren()
        {
            foreach (ITreeViewItem child in Children)
            {
                child.Parent = null;
            }
            Children.Clear();
        }

        public void RemoveChild(ITreeViewItem child)
        {
            if (Children.Remove(child)) child.Parent = null;
        }
    }

    public interface ITreeViewItem
    {
        int Id { get; set; }
        ITreeViewItem Parent { get; set; }
        IList<ITreeViewItem> Children { get; set; }

        void AddChild(ITreeViewItem item);
        void RemoveChild(ITreeViewItem item);
        void ClearChildren();
    }
}