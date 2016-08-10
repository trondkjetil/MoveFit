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
using Android.Util;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace TestApp
{
    [Activity (Label = "Maps",Theme = "@android:style/Theme.NoTitleBar")]
	public class GoogleMapsPeople : Activity,IOnMapReadyCallback, GoogleMap.IInfoWindowAdapter, GoogleMap.IOnMarkerClickListener, GoogleMap.IOnInfoWindowClickListener, GoogleMap.IOnMarkerDragListener,ILocationListener 
	{
        //Android.Support.V4.App.FragmentActivity,
        GoogleMap mMap;
		string addressText;
		Location currentLocation;
		LocationManager locationManager;

		string locationProvider;
		string locationText;
		MarkerOptions markerOpt1;
        MarkerOptions markerOpt2;
        LatLng currentPos;
        MapFragment mapFrag;

     
        public List<User> users;
        public List<Route> routes;
        public List<Marker> userMarkers;
        public List<Marker> routeMarkers;
        ProgressBar bar;

        protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);
			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView (Resource.Layout.mapPeople);	

            CheckBox peopleCheck = FindViewById<CheckBox>(Resource.Id.checkbox1);
            CheckBox routeCheck = FindViewById<CheckBox>(Resource.Id.checkbox2);
            bar = FindViewById<ProgressBar>(Resource.Id.progressBar);

            mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mMap = mapFrag.Map;

			mMap.SetOnMarkerClickListener (this);
			mMap.SetOnInfoWindowClickListener (this);
			mMap.SetOnMarkerDragListener (this);

            userMarkers = new List<Marker>();
            routeMarkers = new List<Marker>();


            peopleCheck.Click += async(o, e) => {

                            
                if (!peopleCheck.Checked && userMarkers.Count != 0)
                {
       
                        foreach (var item in userMarkers)
                        {
                            item.Remove();
                        
                      
                        }
                       
                }


                    if (peopleCheck.Checked)
                {

                    if (userMarkers.Count > 0)
                        {
                        userMarkers.Clear();
                    }
                     

                     bar.BringToFront();
                     bar.Visibility = ViewStates.Visible;


                        // routes = await Azure.getRoutes();
                        // users = await Azure.getImagesOnMap();
                        users = await Azure.getPeople();
                        RunOnUiThread(  () =>
                        {
                           
                            if (users.Count != 0)
                            {
                                foreach (User user in users)
                                {

                                    setMarker2(user);

                                }
                            }
                        });


                        bar.Visibility = ViewStates.Invisible;
                        Toast.MakeText(this, "Showing Friends", ToastLength.Short).Show();


                  

                }

              

            };

            routeCheck.Click += async (o, e) => {

                if (!routeCheck.Checked && routeMarkers.Count != 0)
                {
                
                        foreach (var item in routeMarkers)
                        {
                        item.Remove();
                    }

                    }
           

                    if (routeCheck.Checked)
                {


                    if (routeMarkers.Count > 0)
                    {
                        routeMarkers.Clear();
                    }
                      

                    bar.BringToFront();
                    bar.Visibility = ViewStates.Visible;

                            // routes = await Azure.getRoutes();
                            users = await Azure.getPeople();
                        if(users.Count != 0)
                        {

                       
                            foreach (User user in users)
                            {
                               
                                setMarker(user);

                            }

                        }

                        bar.Visibility = ViewStates.Invisible;
                        Toast.MakeText(this, "Showing Nearby Routes", ToastLength.Short).Show();

                   
                }

            };



		}

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
        }

        protected override void OnStop()
        {
            base.OnStop();
           
        }


        public void setMarker(User user){
			
            var myPosition =  new LatLng(user.Lat, user.Lon);

            Bitmap pic = IOUtilz.GetImageBitmapFromUrl(user.ProfilePicture);
                
            markerOpt1 = new MarkerOptions();
			markerOpt1.SetPosition(myPosition);
			markerOpt1.SetTitle(user.UserName + " Position");
			markerOpt1.SetSnippet ("Points: " +user.Points);
			BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap (pic); //(Resource.Drawable.test);
			markerOpt1.SetIcon (image); //BitmapDescriptorFactory.DefaultMarker (BitmapDescriptorFactory.HueCyan));
		    var marker = mMap.AddMarker (markerOpt1);
            userMarkers.Add(marker);

           
               }

        public void setMarker2(User user)
        {
            //if (markerOpt1 != null)
            //    markerOpt1.Dispose();


            var myPosition = new LatLng(user.Lat, user.Lon);
            markerOpt2 = new MarkerOptions();
           
            markerOpt2.SetPosition(myPosition);
            markerOpt2.SetTitle("Route name");
            markerOpt2.SetSnippet("test" + System.Environment.NewLine + "test");
            markerOpt2.SetIcon(BitmapDescriptorFactory.DefaultMarker (BitmapDescriptorFactory.HueCyan));
            var marker = mMap.AddMarker(markerOpt2);
            routeMarkers.Add(marker);

            //mMap.MoveCamera (CameraUpdateFactory.NewLatLngZoom (myPosition, 14));

           
        }
        void InitializeLocationManager()
		{
			locationManager = (LocationManager) GetSystemService(LocationService);
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.High
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
	
		}
			

		 void MapOnMarkerClick(object sender, GoogleMap.MarkerClickEventArgs markerClickEventArgs)
		{
			markerClickEventArgs.Handled = true;
			Marker marker = markerClickEventArgs.Marker;

			//if (marker.Id.Equals(MyMarkerId)) // The ID of a specific marker the user clicked on.
			//if (marker.Id.Equals(this))
			//{
			//	mMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(20.72110, -156.44776), 13));
			//}
			//else
			//{
			//	Toast.MakeText(this, String.Format("You clicked on Marker ID {0}", marker.Id), ToastLength.Short).Show();
			//}
		}


		protected override void OnResume() 
		{
			base.OnResume ();
            if(mMap == null)
            {
                mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
                mMap = mapFrag.Map;
            }
    

            //gps
           // locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
		}

		protected override void OnPause()
		{
			base.OnPause();

   //         if(locationManager != null)
			//locationManager.RemoveUpdates(this);
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


		async void getUsersAddress(Location loc)
		{

            currentLocation = loc;

            if (currentLocation == null)
			{
				addressText = "Can't determine the current address. Try again in a few minutes.";
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
				addressText = deviceAddress.ToString();
			}
			else
			{
				addressText = "Unable to determine the address. Try again in a few minutes.";
			}
		}


		public async void OnLocationChanged(Location location)
		{
			currentLocation = location;
			if (currentLocation == null)
			{
				locationText = "Unable to determine your location. Try again in a short while.";
			}
			else
			{
				locationText = string.Format("{0:f6},{1:f6}", currentLocation.Latitude, currentLocation.Longitude);
				Address address = await ReverseGeocodeCurrentLocation();
				DisplayAddress(address);
				currentPos = new LatLng (currentLocation.Latitude, currentLocation.Longitude);



				


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



        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
        }

     
    }




}
