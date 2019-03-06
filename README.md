# TreeView for Xamarin.Android

## Download
[![NuGet](https://img.shields.io/nuget/v/Xamarin.TreeView.svg?style=flat&max-age=86400)](https://www.nuget.org/packages/Xamarin.TreeView/)

|  |  |
| --------------- | ----------------------------------------------- |
| Package Manager | Install-Package Xamarin.TreeView -Version 1.0.1 |
| .NET CLI | dotnet add package Xamarin.TreeView --version 1.0.1 |

### ScreenShots

<img src="https://github.com/Guiorgy/TreeView/blob/master/Captures/demo.gif?raw=true" width="360"/>

## Usage

* xml
```xml
<treeview.TreeView
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    android:layout_margin="5dp"
    app:trace_visibility="visible"
    app:trace_color="#ffb3b3b3"
    app:trace_width="3dp"
    app:trace_margin="6dp"
    app:node_margin="7dp"
    app:head_margin="4dp"
    app:maxLevel="3"
    android:id="@+id/treeView"/>
```

* Adapter
```cs
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

        if (holder.TextView != null)
            holder.TextView.Text = $"{holder.Level}, {position}, {node.Id}";
    }

    public override void OnBindViewHolder(TreeView.LeafViewHolder viewHolder, int position)
    {
        ITreeViewNode node = Nodes[position];
        var holder = viewHolder as LeafViewHolder;

        if (holder.TextView != null)
            holder.TextView.Text = $"{holder.Level}, {position}, {node.Id}";
    }

    private class NodeViewHolder : TreeView.NodeViewHolder
    {
        public TextView TextView { get; }

        public NodeViewHolder(TreeView tree, View itemView) : base(tree, itemView)
        {
            this.TextView = itemView.FindViewById<TextView>(Resource.Id.text);
        }
    }

    private class LeafViewHolder : TreeView.LeafViewHolder
    {
        public TextView TextView { get; }

        public LeafViewHolder(View itemView) : base(itemView)
        {
            this.TextView = itemView.FindViewById<TextView>(Resource.Id.text);
            itemView.SetBackgroundColor(Color.Green);
        }
    }
}
```

* Activity/Fragment
```cs
TreeView treeView = FindViewById<TreeView>(Resource.Id.treeView);
TreeViewAdapter adapter = new TreeViewAdapter();
treeView.SetAdapter(adapter);
TreeViewNode[] items =
{
    new TreeViewNode(),
    new TreeViewNode(),
    new TreeViewNode(),
    new TreeViewNode(),
};
adapter.AddNodes(items);
adapter.Click += (object sender, TreeView.ClickEventArgs e) =>
{
    System.Console.WriteLine($"Click. NodeId:{e.Node.Id}, Level:{e.Level}, Position:{e.Position}, NodeType{e.NodeType}");
};
adapter.LongClick += (object sender, TreeView.ClickEventArgs e) =>
{
    System.Console.WriteLine($"LongClick. NodeId:{e.Node.Id}, Level:{e.Level}, Position:{e.Position}, NodeType{e.NodeType}");
    e.Node.AddChild(new TreeViewNode());
    adapter.NotifyDataSetChanged();
};
```

## MIT License

Copyright (c) 2018-2019 Guiorgy

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
