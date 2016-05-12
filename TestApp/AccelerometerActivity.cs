using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Android.Content.PM;

namespace TestApp
{
	[Activity (Label = "Activity",ScreenOrientation = ScreenOrientation.Portrait)]			
	public class AccelerometerActivity : Activity

	{

		static readonly object syncLock = new object ();
		TextView sensorTextView;
		Random rand;

		MediaPlayer player;
		public WebView webLoadingIcon;
		Button stopbtn;

		public bool inactive;
		public float inactiveTime;

		public string startTime;
		public string endTime;
		public TimeSpan timeOfday;
		public int level;
	
	
		// Create check for time of day. Eg alarm only between 9 am - 9pm
		public void timing(){
			var start = DateTime.Now;
			var oldDate = DateTime.Parse(timeOfday.ToString()); //DateTime.Parse("08/10/2011 23:50:31"); 

			if(start.Subtract(oldDate) >= TimeSpan.FromMinutes(20)) 
			{
				//20 minutes were passed from start
			}

		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.accelerometer);
			//StopService (new Intent (this, typeof(SimpleService)));


			sensorTextView = FindViewById<TextView>(Resource.Id.accelerometer_text);
			rand = new Random ();
			inactive = true;

			timeOfday = DateTime.Now.TimeOfDay;
			startTime = DateTime.Now.ToString ("h:mm:ss tt");
		
			stopbtn = FindViewById<Button> (Resource.Id.stop);
			stopbtn.Click += stopAlarm;

			Alarm ();
			stopbtn.Click += stopAlarm;

		}

	

		public String getImage(){
			String gif;
			int tall = rand.Next (0, 3);

			if (tall == 1) {
				gif = "file:///android_asset/Lem.gif";
			}else if (tall == 2){
				gif ="file:///android_asset/Hipp.gif";
			}else {
				gif ="file:///android_asset/Afro.gif";
			}
			return gif;
		}


		public void stopAlarm(object sender, EventArgs e){
			player.Stop ();
			webLoadingIcon.Destroy();
			stopbtn.Enabled = false;

			// Has been added for test***
			StopService(new Intent(this, typeof(SimpleService)));
		//	StartService(new Intent(this, typeof(SimpleService)));
			Finish();
		}


		public void Alarm(){
			webLoadingIcon = FindViewById<WebView> (Resource.Id.webLoadingIcon);
			// expects to find the 'loading_icon_small.gif' file in the 'root' of the assets folder, compiled as AndroidAsset.
			webLoadingIcon.LoadUrl (string.Format (getImage()));

			//"file:///android_asset/Lem.gif"

			webLoadingIcon.Settings.SetSupportZoom (true);

			// this makes it transparent so you can load it over a background
			webLoadingIcon.SetBackgroundColor (new Color (0, 0, 0, 0));
			webLoadingIcon.SetLayerType (LayerType.Software, null);

			player = MediaPlayer.Create (this, Resource.Raw.moveIt);
			player.SetVolume (100, 100);
			player.Start ();
		}


			
		protected override void OnDestroy()
		{
			StartService (new Intent (this, typeof(SimpleService)));
			base.OnDestroy();
			//sensorManager.UnregisterListener(this);


		}
	


	}
}

