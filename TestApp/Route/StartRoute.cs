using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using System.Collections;
using Android.Content.PM;

namespace TestApp
{
    [Activity(Label = "StartRoute", ScreenOrientation = ScreenOrientation.Portrait)]
    public class StartRoute : Activity, ILocationListener
    {

        LocationManager locationManager;
        string locationProvider;
        MarkerOptions markerOpt1;
        MarkerOptions markerOpt2;
        GoogleMap mMap;

        public static String[] array;
        public List<Locations> locationPointsForRoute;

        private string routeName;
        private string routeInfo;
        private string routeRating;
        private string routeDifficulty;
        private string routeLength;
        private string routeType;
        private string routeTrips;
        private string routeId;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.startRoute);

            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.mapForStartingRoute);
            mMap = mapFrag.Map;

            if (mMap != null)
            {
                mMap.MapType = GoogleMap.MapTypeTerrain;  // The GoogleMap object is ready to go.
            }
            {
                mMap.UiSettings.ZoomControlsEnabled = true;
                mMap.UiSettings.CompassEnabled = true;
                mMap.UiSettings.MapToolbarEnabled = true;
                mMap.UiSettings.MyLocationButtonEnabled = true;

                array = Intent.GetStringArrayExtra("MyData");
                routeId = array[7];
                locationPointsForRoute = await Azure.getLocationsForRoute(routeId);


                TextView name = FindViewById<TextView>(Resource.Id.startRouteName);
                TextView description = FindViewById<TextView>(Resource.Id.startRouteDesc);
                TextView length = FindViewById<TextView>(Resource.Id.startRouteLength);
                TextView difficulty = FindViewById<TextView>(Resource.Id.startRouteDiff);
                TextView rating = FindViewById<TextView>(Resource.Id.startRouteRating);
                TextView trips = FindViewById<TextView>(Resource.Id.startRouteTrips);
                TextView type = FindViewById<TextView>(Resource.Id.startRouteType);
                Button start = FindViewById<Button>(Resource.Id.startRoute);
                Button end = FindViewById<Button>(Resource.Id.endRoute);
                Button cancel = FindViewById<Button>(Resource.Id.cancelRoute);
                RatingBar ratingbar = FindViewById<RatingBar>(Resource.Id.ratingbar);
                ratingbar.Clickable = false;
                ratingbar.Visibility = ViewStates.Visible;

                routeName = array[0];
                routeInfo = array[1];
                routeDifficulty = array[2];
                routeLength = array[3];
                routeType = array[4];
                routeRating = array[5];
                routeTrips = array[6];

               
                name.Text = "Name:" + routeName;
                description.Text = "Description: " + routeInfo;
                difficulty.Text = "Difficulty: " + routeDifficulty;
                length.Text = "Length: " + routeLength;
                type.Text = "Type: " + routeType;
                trips.Text = "Trips: " + routeTrips;

                try
                {
                    if (Convert.ToInt32(routeRating) == 0)
                    {
                        rating.Text = "Rating: Not rated yet!";
                    }
                    else
                        rating.Text = "Rating: " + routeRating;

                       ratingbar.Rating = Convert.ToInt32(routeRating);
                   
                }
                catch (Exception)
                {

                }
               
                drawRoute();

                start.Click += (sender, e) =>
                {

                    InitializeLocationManager();

                    long minTime = 10 * 1000; // Minimum time interval for update in seconds, i.e. 5 seconds.
                    long minDistance = 5; // Minimum distance change for update in meters, i.e. 10 meters.
                    locationManager.RequestLocationUpdates(this.locationProvider, minTime, minDistance, this);


                    double distance = 0;
                    Location myLocation = locationManager.GetLastKnownLocation(locationProvider);
                    Locations firstElement = locationPointsForRoute.First();
                    float[] results = new float[1];
                    string[] LatLngFirst = firstElement.Location.Split(',');
                    Location.DistanceBetween(myLocation.Latitude, myLocation.Longitude, Convert.ToDouble(LatLngFirst[0]), Convert.ToDouble(LatLngFirst[1]), results);
                    distance = (int)results[0];


                    if (distance <= 100)
                    {
                        Toast.MakeText(this, "Starting route...", ToastLength.Short).Show();
                    }
                    else
                        Toast.MakeText(this, "Please move closer to the starting point!", ToastLength.Short).Show();

                };

                end.Click += (sender, e) =>
                {


                    double distance = 0;
                    Location myLocation = locationManager.GetLastKnownLocation(locationProvider);
                    Locations firstElement = locationPointsForRoute.LastOrDefault();
                    float[] results = new float[1];
                    string[] LatLngFirst = firstElement.Location.Split(',');
                    Location.DistanceBetween(myLocation.Latitude, myLocation.Longitude, Convert.ToDouble(LatLngFirst[0]), Convert.ToDouble(LatLngFirst[1]), results);
                    distance = (int)results[0];


                    if (distance <= 60)
                    {
                        Toast.MakeText(this, "Congratulations! You have finished the route", ToastLength.Short).Show();
                        startDialogNameRoute();
                        if (locationManager != null)
                            locationManager.RemoveUpdates(this);

                    }
                    else
                        Toast.MakeText(this, "Please move closer to the finish-line!" + "Distance to finish - point is: "+ distance, ToastLength.Long).Show();



                };

                cancel.Click += (sender, e) =>
                {
                    Toast.MakeText(this, "Route ended! You will not get any points", ToastLength.Long).Show();

                    Finish();
                };


            }

        }


        public void startDialogNameRoute()
        {

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            DialogEndRoute newDialog = new DialogEndRoute();
            newDialog.DialogClosed += OnDialogClosed;
            newDialog.Show(transaction, "End Route");
        }

     
         async void OnDialogClosed(object sender, DialogEndRoute.DialogEventArgs e)
        {

            routeRating = e.ReturnValue.ToString();
            await Azure.giveRouteRating(routeId, "TestUserID", routeRating);


        }

        private bool isGooglePlayServicesAvailable()
        {
            int status = Android.Gms.Common.GooglePlayServicesUtil.IsGooglePlayServicesAvailable(this);
            if (999999 == status)
            {
                return true;
            }
            else
            {
                Android.Gms.Common.GooglePlayServicesUtil.GetErrorDialog(status, this, 0).Show();
                return false;
            }
        }


        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
        }

        void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Medium
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


        protected override void OnResume()
        {
            base.OnResume();
            if(locationManager != null)
            locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (locationManager != null)
                locationManager.RemoveUpdates(this);
        }



        public void OnProviderDisabled(string provider)
        {

        }

        public void OnProviderEnabled(string provider)
        {

        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {

        }

        public void OnLocationChanged(Location location)
        {


            try { 
         
              //  mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
             //   mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(location.Latitude, location.Longitude), 14));

            }
            catch (Exception e)
            {
                // throw;
            }

        }



        public override void OnBackPressed()
        {

            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Exit route creation");
            alert.SetMessage("Do you want to abort the current route creation?");
            alert.SetPositiveButton("Yes", (senderAlert, args) =>
            {
               
                base.OnBackPressed();
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
             

            });
            //run the alert in UI thread to display in the screen
            RunOnUiThread(() =>
            {
                alert.Show();
            });


        }


        public  void drawRoute() {

          mMap.UiSettings.RotateGesturesEnabled = false;
          mMap.UiSettings.ScrollGesturesEnabled = false;

            try
            {
              

            if(locationPointsForRoute.Count == 0)
            {
                Toast.MakeText(this, "No routes found for: " + routeId, ToastLength.Long).Show();

                return;
            }


            Locations lastItem = locationPointsForRoute.LastOrDefault();
            Locations firstElement = locationPointsForRoute.First();

            string[] LatLngLast = lastItem.Location.Split(',');
            string[] LatLngFirst = firstElement.Location.Split(',');

         
            markerOpt1 = new MarkerOptions();
            markerOpt1.SetPosition(new LatLng(Convert.ToDouble(LatLngFirst[0]), Convert.ToDouble(LatLngFirst[1])));
            markerOpt1.SetTitle("Starting Point");
            markerOpt1.Draggable(false);
            markerOpt1.SetSnippet("Starting point of route");
            markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueGreen));
            mMap.AddMarker(markerOpt1);

            markerOpt2 = new MarkerOptions();
            markerOpt2.SetPosition(new LatLng(Convert.ToDouble(LatLngLast[0]), Convert.ToDouble(LatLngLast[1])));
            markerOpt2.SetTitle("Ending Point");
            markerOpt2.Draggable(false);
            markerOpt2.SetSnippet("End point of route");
            markerOpt2.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
            mMap.AddMarker(markerOpt2);

            mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
            mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(Convert.ToDouble(LatLngFirst[0]), Convert.ToDouble(LatLngFirst[1])), 14));

            PolylineOptions opt = new PolylineOptions();

            string[] loc;
            foreach (var item in locationPointsForRoute)
            {
                loc = null;
                loc = item.Location.Split(',');
                opt.Add(new LatLng(Convert.ToDouble(loc[0]), Convert.ToDouble(loc[1])));
            }

            mMap.AddPolyline(opt);


            }catch(Exception e)
            {

                throw;
            }


        }


    





    }
}