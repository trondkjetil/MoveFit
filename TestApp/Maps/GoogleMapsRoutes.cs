using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Gms.Common;

namespace TestApp
{
	[Activity (Label = "Maps",Theme = "@android:style/Theme.NoTitleBar")]
	public class GoogleMapsActivity : Android.Support.V4.App.FragmentActivity,IOnMapReadyCallback, GoogleMap.IInfoWindowAdapter, GoogleMap.IOnMarkerClickListener, GoogleMap.IOnInfoWindowClickListener, GoogleMap.IOnMarkerDragListener,ILocationListener 
	{
		
		GoogleMap mMap;
		static readonly string TAG = "X:" + typeof (GoogleMapsActivity).Name;
		TextView addressText;
		Location currentLocation;
		LocationManager locationManager;

		string locationProvider;
		TextView locationText;

		MarkerOptions markerOpt1;

		LatLng currentPos;
	

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.map);
			//Hides title bar
		//	RequestWindowFeature(WindowFeatures.NoTitle);
			SetUpMapIfNeeded ();	

			//GPS setup

			addressText = FindViewById<TextView> (Resource.Id.address_text);
			locationText = FindViewById<TextView> (Resource.Id.location_text);
			FindViewById<TextView> (Resource.Id.get_address_button).Click += AddressButton_OnClick;

			InitializeLocationManager ();

			Button mark = FindViewById<Button> (Resource.Id.setmark);
			Button pos = FindViewById<Button> (Resource.Id.mypos);


			FragmentTransaction transaction = FragmentManager.BeginTransaction();
			DialogStartRoute newDialog = new DialogStartRoute();
			newDialog.Show(transaction,"Start Route");

		

			Spinner spinner = FindViewById<Spinner> (Resource.Id.spinner);
			spinner.ItemSelected += spinner_ItemSelected;
			var adapter = ArrayAdapter.CreateFromResource (this, Resource.Array.planets_array, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;
		


			mMap.SetOnMarkerClickListener (this);
			mMap.SetOnInfoWindowClickListener (this);
			mMap.SetOnMarkerDragListener (this);
			//	mMap.SetInfoWindowAdapter (new CustomInfoWindowAdapter (this));


			startUpMapConfigs ();

			mark.Click += (sender, e) => {

			//	Bitmap scaledBitmap = UtilResizeImage.scaleDown(
			//	scaleDown(realImage, MAX_IMAGE_SIZE, true);


				markerOpt1 = null;
				markerOpt1 = new MarkerOptions();
				markerOpt1.SetPosition(currentPos);
				markerOpt1.SetTitle("My Position");
				markerOpt1.SetSnippet ("This is where i am");
				BitmapDescriptor image = BitmapDescriptorFactory.FromResource(Resource.Drawable.bike);
				markerOpt1.SetIcon (image); //BitmapDescriptorFactory.DefaultMarker (BitmapDescriptorFactory.HueCyan));
				mMap.AddMarker (markerOpt1);

		};
			pos.Click += (sender, e) => mMap.MoveCamera (CameraUpdateFactory.NewLatLngZoom (currentPos, 13));

		}


		void InitializeLocationManager()
		{
			locationManager = (LocationManager) GetSystemService(LocationService);
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};
			IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
			{
				locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				locationProvider = string.Empty;
			}
			Log.Debug(TAG, "Using " + locationProvider + ".");
		}



		void startUpMapConfigs ()
		{
			LatLng start = new LatLng (60.3894, 5.3300);
			mMap.UiSettings.ZoomControlsEnabled = true;
			mMap.UiSettings.CompassEnabled = true;
			mMap.MoveCamera (CameraUpdateFactory.ZoomIn ());
			mMap.MoveCamera (CameraUpdateFactory.NewLatLngZoom (start, 13));
			//setMarker();
			//setCustomGroundOverLay();

			markerOpt1 = new MarkerOptions ();
			markerOpt1.SetPosition (new LatLng (60.390964, 5.326234));
			markerOpt1.SetTitle ("Bergen By");
			markerOpt1.Draggable (true);
			markerOpt1.SetIcon (BitmapDescriptorFactory.DefaultMarker (BitmapDescriptorFactory.HueCyan));
			mMap.AddMarker (markerOpt1);

			mMap.MapType = GoogleMap.MapTypeTerrain;
		}


		void routeOne(){
			markerOpt1 = new MarkerOptions();
			markerOpt1.SetPosition(new LatLng(60.390964, 5.326234));
			markerOpt1.SetTitle("Bergen By");
			markerOpt1.Draggable (true);
			markerOpt1.SetSnippet ("Starting point of route");
			markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker (BitmapDescriptorFactory.HueCyan));
			mMap.AddMarker (markerOpt1);

			LatLng pos1 = new LatLng(60.391474, 5.328355);
			LatLng pos2 = new LatLng(60.390822,5.330437);
			LatLng pos3 = new LatLng(60.390098,5.330415);
			LatLng pos4 = new LatLng(60.389674,5.328929);
			LatLng pos5 = new LatLng(60.390248,5.326473);


			mMap.AddMarker (new MarkerOptions ().SetPosition (pos3).SetIcon (BitmapDescriptorFactory.FromResource (Resource.Drawable.sign)));
			mMap.AddMarker (new MarkerOptions ().SetPosition (pos2).SetIcon (BitmapDescriptorFactory.FromResource (Resource.Drawable.sign)));
			mMap.AddMarker (new MarkerOptions ().SetPosition (pos4).SetIcon (BitmapDescriptorFactory.FromResource (Resource.Drawable.sign)));
			mMap.AddMarker (new MarkerOptions ().SetPosition (pos5).SetIcon (BitmapDescriptorFactory.FromResource (Resource.Drawable.sign)));
			mMap.AddMarker (new MarkerOptions ().SetPosition (pos1).SetIcon (BitmapDescriptorFactory.FromResource (Resource.Drawable.sign)));

			mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
			mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(pos1,14));

			LatLng startPos = new LatLng(60.390964, 5.326234);
						PolylineOptions opt = new PolylineOptions();
						opt.Add (startPos);
						opt.Add (pos1);
						opt.Add (pos2);
						opt.Add(pos3);
						opt.Add(pos4);
						opt.Add(pos5);
						opt.Add(startPos);
						mMap.AddPolyline(opt);
			

		}

		void routeTwo(){
			markerOpt1 = new MarkerOptions();
			markerOpt1.SetPosition(new LatLng(60.385400,5.341000));
			markerOpt1.SetTitle("Bergen By");
			markerOpt1.Draggable (true);
			markerOpt1.SetSnippet ("Starting point of route x");
			markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker (BitmapDescriptorFactory.HueGreen));
			mMap.AddMarker (markerOpt1);


			LatLng start = new LatLng(60.385400,5.341000);
			mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
			mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(start,14));

			LatLng pos1 = new LatLng(60.385429,5.341739);
			LatLng pos2 = new LatLng(60.384051,5.350286);
			LatLng pos3 = new LatLng(60.381164,5.352899);
			LatLng pos4 = new LatLng(60.379631,5.348631);
			LatLng pos5 = new LatLng(60.380017,5.335668);
			LatLng pos6 = new LatLng(60.384462,5.335869);


			PolylineOptions opt = new PolylineOptions();
			opt.Add (pos1);
			opt.Add (pos2);
			opt.Add(pos3);
			opt.Add(pos4);
			opt.Add(pos5);
			opt.Add(pos6);
			opt.Add(pos1);
			mMap.AddPolyline(opt);

		}



		void routeThree(){
			markerOpt1 = new MarkerOptions();
			markerOpt1.SetPosition(new LatLng(60.384624,5.363317));
			markerOpt1.SetTitle("Route two");
			markerOpt1.SetSnippet ("Starting point of route x. This route give you x points!");
			markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker (BitmapDescriptorFactory.HueGreen));
			mMap.AddMarker (markerOpt1);
		
			LatLng start = new LatLng(60.384624,5.363317);
			mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
			mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(start,14));

			LatLng pos1 = new LatLng(60.38852,5.362763);
			LatLng pos2 = new LatLng(60.395789, 5.378944);
			LatLng pos3 = new LatLng(60.394584, 5.385093);



			PolylineOptions opt = new PolylineOptions();
			opt.Add (start);
			opt.Add (pos1);
			opt.Add(pos2);
			opt.Add(pos3);
			mMap.AddPolyline(opt);
		}

		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;

			if (e.Position == 0) {

				string toast = string.Format ("The route is {0}", spinner.GetItemAtPosition (e.Position));
				Toast.MakeText (this, toast, ToastLength.Long).Show ();

				mMap.Clear ();
				routeOne ();

			} else if (e.Position == 1) {
				string toast = string.Format ("The route is {0}", spinner.GetItemAtPosition (e.Position));
				Toast.MakeText (this, toast, ToastLength.Long).Show ();

				mMap.Clear ();
				routeTwo ();
			} else if (e.Position == 2) {
				string toast = string.Format ("The route is {0}", spinner.GetItemAtPosition (e.Position));
				Toast.MakeText (this, toast, ToastLength.Long).Show ();

				mMap.Clear ();
				routeThree ();


			} else 
			Toast.MakeText (this, "No route for this!", ToastLength.Long).Show ();
		}


		public void setMarker(LatLng myPosition){
			markerOpt1.Dispose ();
			markerOpt1 = null;
			markerOpt1 = new MarkerOptions();
			markerOpt1.SetPosition(myPosition);
			markerOpt1.SetTitle("My Position");
			markerOpt1.SetSnippet ("This is where i am");
			BitmapDescriptor image = BitmapDescriptorFactory.FromResource(Resource.Drawable.test);
			markerOpt1.SetIcon (image); //BitmapDescriptorFactory.DefaultMarker (BitmapDescriptorFactory.HueCyan));
			mMap.AddMarker (markerOpt1);

		}


		 void MapOnMarkerClick(object sender, GoogleMap.MarkerClickEventArgs markerClickEventArgs)
		{
			markerClickEventArgs.Handled = true;
			Marker marker = markerClickEventArgs.Marker;

			//if (marker.Id.Equals(MyMarkerId)) // The ID of a specific marker the user clicked on.
			if (marker.Id.Equals(this))
			{
				mMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(20.72110, -156.44776), 13));
			}
			else
			{
				Toast.MakeText(this, String.Format("You clicked on Marker ID {0}", marker.Id), ToastLength.Short).Show();
			}
		}


		protected override void OnResume() 
		{
			base.OnResume ();
			SetUpMapIfNeeded ();

			//gps
			locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
		}

		protected override void OnPause()
		{
			base.OnPause();
			locationManager.RemoveUpdates(this);
		}


		public void setCustomGroundOverLay(){
			LatLng pos = new LatLng(60.3894, 5.3300);
			BitmapDescriptor image = BitmapDescriptorFactory.FromResource(Resource.Drawable.Icon);
			GroundOverlayOptions groundOverlayOptions = new GroundOverlayOptions()
				.Position(pos, 150, 200)
				.InvokeImage(image);
			mMap.AddGroundOverlay (groundOverlayOptions);
			//mMap.AddGroundOverlay(groundOverlayOptions);
		}


		public void createPolylineCircle(){
			CircleOptions circleOptions = new CircleOptions ();
			circleOptions.InvokeCenter (new LatLng(37.4, -122.1));
			circleOptions.InvokeRadius (1000);
			//	mMap.AddCircle (CircleOptions);

		}

		public void createPolylineRoute(){

			PolylineOptions opt = new PolylineOptions();
			opt.Add(new LatLng(60.391506, 5.328491));
			opt.Add(new LatLng(60.391541,5.328513));
			opt.Add(new LatLng(60.390822,5.330437));
			opt.Add(new LatLng(60.390098,5.330415));
			opt.Add(new LatLng(60.389674,5.328929));
			opt.Add(new LatLng(60.391506, 5.328491)); // close the polyline - this makes a rectangle.
			mMap.AddPolyline(opt);


			/*
			PolylineOptions rectOptions = new PolylineOptions();
			rectOptions.Add(new LatLng(37.35, -122.0));
			rectOptions.Add(new LatLng(37.45, -122.0));
			rectOptions.Add(new LatLng(37.45, -122.2));
			rectOptions.Add(new LatLng(37.35, -122.2));
			rectOptions.Add(new LatLng(37.35, -122.0)); // close the polyline - this makes a rectangle.
			mMap.AddPolyline(rectOptions);



			PolygonOptions rectOptions = new PolygonOptions();
			rectOptions.Add(new LatLng(37.35, -122.0));
			rectOptions.Add(new LatLng(37.45, -122.0));
			rectOptions.Add(new LatLng(37.45, -122.2));
			rectOptions.Add(new LatLng(37.35, -122.2));
			// notice we don't need to close off the polygon
			mMap.AddPolygon(rectOptions);
*/
		}


	
		private void SetUpMapIfNeeded() 
		{
			// Do a null check to confirm that we have not already instantiated the map.
			if (mMap == null) {
				// Try to obtain the map from the SupportMapFragment.
			//	mMap = (SupportFragmentManager.FindFragmentById (Resource.Id.map) as SupportMapFragment).Map; //Map


				mMap = (SupportFragmentManager.FindFragmentById (Resource.Id.map) as SupportMapFragment).Map;

	

			//EDITED OBS OBS OBS
				if (mMap != null)
				{
					//mMap.InfoWindowClick += MapOnMarkerClick;

				}


				//mMap = MapFragment.NewInstance;

				//mMap = ((SupportMapFragment) SupportFragmentManager.FindFragmentById (Resource.Id.map)).Map;
				// Check if we were successful in obtaining the map.
				if (mMap != null) {
					SetUpMap ();
				}
			}
		}

		/**
     * This is where we can add markers or lines, add listeners or move the camera. In this case, we
     * just add a marker near Africa.
     * <p>
     * This should only be called once and when we are sure that {@link #mMap} is not null.
     */
		private void SetUpMap() {
			mMap.AddMarker (new MarkerOptions ().SetPosition (new LatLng (0, 0)).SetTitle ("Marker"));


		}
			
		public View GetInfoContents (Marker marker)
		{
			return null;	
		}

		public View GetInfoWindow (Marker marker)
		{

			return null;
		}




		public bool OnMarkerClick (Marker marker)
		{
			marker.ShowInfoWindow ();
			marker.Snippet = marker.Position.Latitude.ToString () + ", " + marker.Position.Longitude.ToString ();
			return true;
		}

		public void OnMarkerDrag (Marker marker)
		{
			//Toast.MakeText (this, marker.Position.ToString(), ToastLength.Short).Show();
			//marker.SetInfoWindowAnchor ((float)marker.Position.Latitude, (float)marker.Position.Longitude);

		}

		public void OnMarkerDragEnd (Marker marker)
		{

		}

		public void OnMarkerDragStart (Marker marker)
		{

		}

		public void OnInfoWindowClick (Marker marker)
		{

			marker.Snippet = marker.Position.Latitude.ToString () + ", " + marker.Position.Longitude.ToString ();
			marker.ShowInfoWindow ();
		}


		async void AddressButton_OnClick(object sender, EventArgs eventArgs)
		{
			if (currentLocation == null)
			{
				addressText.Text = "Can't determine the current address. Try again in a few minutes.";
				return;
			}

			Address address = await ReverseGeocodeCurrentLocation();
			DisplayAddress(address);
		}

		async Task<Address> ReverseGeocodeCurrentLocation()
		{
			Geocoder geocoder = new Geocoder(this);
			IList<Address> addressList =
				await geocoder.GetFromLocationAsync(currentLocation.Latitude, currentLocation.Longitude, 10);

			Address address = addressList.FirstOrDefault();
			return address;
		}

		void DisplayAddress(Address address)
		{
			if (address != null)
			{
				StringBuilder deviceAddress = new StringBuilder();
				for (int i = 0; i < address.MaxAddressLineIndex; i++)
				{
					deviceAddress.AppendLine(address.GetAddressLine(i));
				}
				// Remove the last comma from the end of the address.
				addressText.Text = deviceAddress.ToString();
			}
			else
			{
				addressText.Text = "Unable to determine the address. Try again in a few minutes.";
			}
		}


		public async void OnLocationChanged(Location location)
		{
			currentLocation = location;
			if (currentLocation == null)
			{
				locationText.Text = "Unable to determine your location. Try again in a short while.";
			}
			else
			{
				locationText.Text = string.Format("{0:f6},{1:f6}", currentLocation.Latitude, currentLocation.Longitude);
				Address address = await ReverseGeocodeCurrentLocation();
				DisplayAddress(address);
				currentPos = new LatLng (currentLocation.Latitude, currentLocation.Longitude);
				setMarker (currentPos);


			}
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



		public void OnMapReady (GoogleMap googleMap)
		{
			
		}



		}




}
