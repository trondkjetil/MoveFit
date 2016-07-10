using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Android.Content.PM;

namespace TestApp
{
	[Activity (Label = "Activity",ScreenOrientation = ScreenOrientation.Portrait)]			
	public class ActivityLevelTracker : Activity

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
	
	
       
        protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.accelerometer);
	

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

	

		

		public void stopAlarm(object sender, EventArgs e){
			player.Stop ();
			webLoadingIcon.Destroy();
			stopbtn.Enabled = false;

			// Has been added for test***
			StopService(new Intent(this, typeof(SimpleService)));
	
			Finish();
		}


		public void Alarm(){
			

			player = MediaPlayer.Create (this, Resource.Raw.moveIt);
			player.SetVolume (100, 100);
			player.Start ();
		}


			
		protected override void OnDestroy()
		{
            if(SimpleService.Status == true || SimpleService.status == true)
            {
              
            }
			
			base.OnDestroy();

           

        }
	


	}
}

