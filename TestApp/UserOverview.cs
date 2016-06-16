using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using System.Collections.Generic;
using Android.Util;
using Android.Locations;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Content.PM;
using System.Net;
using Android.Graphics.Drawables;

namespace TestApp
{

	[Activity (Label = "User",Theme = "@style/MyTheme",ScreenOrientation = ScreenOrientation.Portrait)]
	public class UserOverview : ActionBarActivity,ILocationListener
	{
        //
		private SupportToolbar mToolbar;
		private MyActionBarDrawerToggle mDrawerToggle;
		private DrawerLayout mDrawerLayout;
		private ListView mLeftDrawer;
		private ListView mRightDrawer;
		private ArrayAdapter mLeftAdapter;
		private ArrayAdapter mRightAdapter;
		private List<string> mLeftDataSet;
		private List<string> mRightDataSet;
		public string[] table;
		public string text;

		public string userID;

		ImageView profilePicture;
		Bitmap profilePic;
		readonly string logTag = "MainActivity";
		TextView latText;
		TextView longText;
		TextView altText;
		TextView speedText;
		TextView bearText;
		TextView accText;

		LatLng start;
		Intent myIntent;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.drawerLayout);
		


			Switch location = FindViewById<Switch> (Resource.Id.switch1);
			//Toast.MakeText (this, "Two background services are started here! One that tracks the users location data, and one that tracks the users movement", ToastLength.Long).Show();

			TextView textview1 = FindViewById<TextView> (Resource.Id.textView1);
			profilePicture = FindViewById<ImageView> (Resource.Id.profilePicture);


			String[] array = Intent.GetStringArrayExtra ("MyData");
			textview1.Text = "Greetings "+array[0]+"!";
			getProfileImage (array[1]);
			userID = array [2];

			//PersonTracker service
			App.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
				Log.Debug (logTag, "ServiceConnected Event Raised");
				// notifies us of location changes from the system
				App.Current.LocationService.LocationChanged += HandleLocationChanged;
				//notifies us of user changes to the location provider (ie the user disables or enables GPS)
				App.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
				App.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
				// notifies us of the changing status of a provider (ie GPS no longer available)
				App.Current.LocationService.StatusChanged += HandleStatusChanged;
			};

			//Starts the person activity tracker
			StartService(new Intent(this, typeof(LocationService)));

			latText = FindViewById<TextView> (Resource.Id.lat);
			longText = FindViewById<TextView> (Resource.Id.longx);
			altText = FindViewById<TextView> (Resource.Id.alt);
			speedText = FindViewById<TextView> (Resource.Id.speed);
			bearText = FindViewById<TextView> (Resource.Id.bear);
			accText = FindViewById<TextView> (Resource.Id.acc);

			profilePicture = FindViewById<ImageView> (Resource.Id.profilePicture);

			//	String[] array = Intent.GetStringArrayExtra ("MyData");
			//	textview1.Text = "Logged in as: "+array[0];

			location.CheckedChange += delegate(object sender, CompoundButton.CheckedChangeEventArgs e) {
				if(e.IsChecked == true){
					Toast.MakeText(this, "Tracking...", ToastLength.Long).Show();
					//StartService(new Intent(this, typeof(LocationService)));
					//	StartService(new Intent(this, typeof(SimpleService)));

					Android.App.AlertDialog alertMessage = new Android.App.AlertDialog.Builder(this).Create();
					alertMessage.SetTitle("User location tracking");
					alertMessage.SetMessage("Your location tracking has been turned on");
					alertMessage.Show();

				}else {
					Toast.MakeText(this, "Tracking stopped!", ToastLength.Long).Show();
					//	StopService(new Intent(this, typeof(LocationService)));
					// StopService(new Intent(this, typeof(SimpleService)));

					Android.App.AlertDialog alertMessage = new Android.App.AlertDialog.Builder(this).Create();
					alertMessage.SetTitle("User location tracking");
					alertMessage.SetMessage("Your location tracking has been turned off");
					alertMessage.Show();
				}
			};


		}





	
	


		protected override void OnPause()
		{
			Log.Debug (logTag, "Location app is moving to background");
			base.OnPause();
		}

		protected override void OnResume()
		{
			Log.Debug (logTag, "Location app is moving into foreground");
			base.OnPause();
		}

		protected override void OnDestroy ()
		{
			Log.Debug (logTag, "Location app is becoming inactive");
			base.OnDestroy ();

		}




		///<summary>
		/// Updates UI with location data
		/// </summary>
		public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
		{
			Location location = e.Location;
			Log.Debug (logTag, "Foreground updating");
			start = new LatLng (60.3894, 5.3300);

			if( (e.Location.Latitude > 60.3891 &&  e.Location.Latitude < 60.3897) && (e.Location.Longitude > 5.3295  && e.Location.Longitude < 5.3325 )){

				FragmentTransaction transaction = FragmentManager.BeginTransaction();
				DialogStartRoute newDialog = new DialogStartRoute();
				newDialog.Show(transaction,"Start Route");

				//	StopService (new Intent (this, typeof(LocationService)));
			}


			// these events are on a background thread, need to update on the UI thread
			RunOnUiThread (() => {

				latText.Text = String.Format ("Latitude: {0}", location.Latitude);
				longText.Text = String.Format ("Longitude: {0}", location.Longitude);
				altText.Text = String.Format ("Altitude: {0}", location.Altitude);
				speedText.Text = String.Format ("Speed: {0}", location.Speed);
				accText.Text = String.Format ("Accuracy: {0}", location.Accuracy);
				bearText.Text = String.Format ("Bearing: {0}", location.Bearing);
			});

		}

		public void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
		{
			Log.Debug (logTag, "Location provider disabled event raised");
		}

		public void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
		{
			Log.Debug (logTag, "Location provider enabled event raised");
		}

		public void HandleStatusChanged(object sender, StatusChangedEventArgs e)
		{
			Log.Debug (logTag, "Location status changed, event raised");
		}


		public void OnLocationChanged (Location location)
		{

		}
		public void OnProviderDisabled (string provider)
		{

		}
		public void OnProviderEnabled (string provider)
		{

		}
		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{

		}


		void getProfileImage(string url){

			string url_profilePic = url;
			// Custom profile pic test
			//"https://fbcdn-profile-a.akamaihd.net/hprofile-ak-xtp1/v/t1.0-1/p50x50/10416996_10153771292926346_4989196998867714535_n.jpg?oh=f11bfe614cb72d234c29b0b632fa83a1&oe=57B3DAD3&__gda__=1471520543_00f991a2704c9f1b3e4994e56e32eb0f";

			//profilePicture = GetImageBitmapFromUrl(url_profilePic);

			WebClient web = new WebClient();
			web.DownloadDataCompleted += new DownloadDataCompletedEventHandler(web_DownloadDataCompleted);
			web.DownloadDataAsync(new Uri(url_profilePic)); 


		}

		void web_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				RunOnUiThread(() =>
					Toast.MakeText(this, e.Error.Message, ToastLength.Short).Show());
			}
			else
			{

				Bitmap bm = BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length);
				bm = IOUtilz.scaleDown (bm,180,false);

				bm = IOUtilz.getRoundedShape (bm);

				RunOnUiThread(() =>
					{
						profilePicture = FindViewById<ImageView>(Resource.Id.profilePicture);

						//profilePicture.SetImageResource(Resource.Drawable.bike);
						profilePicture.SetImageBitmap(bm);

						// Allows us to get bitmap from imageview
						var bitmapDrawable = profilePicture.Drawable as BitmapDrawable;
						if(bitmapDrawable != null)
						{
							profilePic = bitmapDrawable.Bitmap;

						}



					});
			}

		}






	}
}


