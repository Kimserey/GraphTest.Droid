using Android.App;
using Android.Widget;
using Android.OS;

namespace Graph
{
	[Activity(Label = "Graph", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.GraphLayout);
		}
	}
}

