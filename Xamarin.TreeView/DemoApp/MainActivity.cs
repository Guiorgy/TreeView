using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Xamarin.TreeView;

namespace DemoApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

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
            items[1].AddChild(new TreeViewNode());
            items[1].AddChild(new TreeViewNode());
            items[1][1].AddChild(new TreeViewNode());
            items[1].AddChild(new TreeViewNode());
            TreeViewNode item = new TreeViewNode();
            item.AddChild(new TreeViewNode());
            items[2].AddChild(item);
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
        }
    }
}