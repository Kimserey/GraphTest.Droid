using Android.App;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Core;
using Xamarin.Forms.Platform.Android;

namespace Graph
{
	[Activity(Label = "Graph", Theme = "@style/MyTheme", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
			FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

			base.OnCreate(savedInstanceState);
			Xamarin.Forms.Forms.Init(this, savedInstanceState);
			this.LoadApplication(new App());
		}
	}

	public class GraphAtivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.GraphLayout);
		}
	}
}

