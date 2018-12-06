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

            TreeView treeView = FindViewById<TreeView>(Resource.Id.list);
            TreeViewAdapter adapter = new TreeViewAdapter(Resource.Layout.treeviewitem);
            treeView.SetAdapter(adapter);
            TreeViewItem[] items =
            {
                new TreeViewItem(),
                new TreeViewItem(),
                new TreeViewItem(),
            };
            items[1].AddChild(new TreeViewItem());
            TreeViewItem item = new TreeViewItem();
            item.AddChild(new TreeViewItem());
            items[2].AddChild(item);
            adapter.AddItems(items);
            adapter.NotifyDataSetChanged();
        }
    }
}