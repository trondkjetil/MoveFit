using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace TestApp
{
	[Activity(Label = "SimpleService")]
	public class StartSimpleService : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.simpleServiceLayout);

			Button start = FindViewById<Button>(Resource.Id.startService);
			start.Click += (sender, args) => { StartService(new Intent(this, typeof(SimpleService))); };

			Button stop = FindViewById<Button>(Resource.Id.stopService);
			stop.Click += (sender, args) => { StopService(new Intent(this, typeof(SimpleService))); };
		}

		protected override void OnStop()
		{
			base.OnStop();
			// Clean up: shut down the service when the Activity is no longer visible.
			StopService(new Intent(this, typeof (SimpleService)));
		}
	}
}