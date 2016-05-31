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

namespace TestApp
{
    [Activity(Label = "StartRoute")]
    public class StartRoute : Activity, ILocationListener
    {

        Location currentLocation;
        LocationManager locationManager;
        public string locationProvider;
        TextView locationText;
        MarkerOptions markerOpt1;
        public static GoogleMap mMap;
        public static String[] array;

        public ArrayList locationPoints;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.startRoute);


            locationPoints = new ArrayList();

            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.mapForStartingRoute);
            GoogleMap mMap = mapFrag.Map;

            if (mMap != null)
            {
                mMap.MapType = GoogleMap.MapTypeNormal;  // The GoogleMap object is ready to go.
            }

            InitializeLocationManager();

            array = Intent.GetStringArrayExtra("MyData");
            TextView name = FindViewById<TextView>(Resource.Id.startRouteName);
            TextView description = FindViewById<TextView>(Resource.Id.startRouteDesc);
            TextView length = FindViewById<TextView>(Resource.Id.startRouteLength);
            TextView difficulty = FindViewById<TextView>(Resource.Id.startRouteDiff);
            TextView rating = FindViewById<TextView>(Resource.Id.startRouteRating);
            Button start = FindViewById<Button>(Resource.Id.startRoute);
            Button cancel = FindViewById<Button>(Resource.Id.cancelRoute);
            RatingBar ratingbar = FindViewById<RatingBar>(Resource.Id.ratingbar);


            name.Text ="Name: " +  array[0];

            ratingbar.RatingBarChange += (o, e) => {
                Toast.MakeText(this, "New Rating: " + ratingbar.Rating.ToString(), ToastLength.Short).Show();
            };
           




            start.Click += (sender, e) =>
            {
                Toast.MakeText(this,"Starting route...",ToastLength.Short).Show();


                long minTime = 2 * 1000; // Minimum time interval for update in seconds, i.e. 5 seconds.
                long minDistance = 1; // Minimum distance change for update in meters, i.e. 10 meters.
                
                 locationManager.RequestLocationUpdates(this.locationProvider, minTime,
                        minDistance,this);
                
                
            };
            cancel.Click += (sender, e) =>
            {
               
                Finish();
            };


        }

        public double calculateDistance(double fromLatitude, double fromLongitude, double toLatitude, double toLongitude)
        {

            float[] results = new float[1];
            int distance = 0;
            LatLng source = new LatLng(fromLatitude,fromLongitude);
            LatLng destination = new LatLng(toLatitude,toLongitude);
            

            try
            {
                Location.DistanceBetween(fromLatitude, fromLongitude, toLatitude, toLongitude, results);
            }
            catch (Exception e)
            {
                if (e != null)
                   Console.Write( e.Message);
            }
            if (source.Equals(destination))
            {
                distance = 0;
            }
            else
            {
                int dist = (int)results[0];
                if (dist <= 0)
                    return 0D;

                //DecimalFormat decimalFormat = new DecimalFormat("#.##");
                //results[0] /= 1000D;
                //distance = decimalFormat.format(results[0]);
                //double d = Double.parseDouble(distance);
                //double speed = 40;
                //double time = d / speed;
                //ts = manual(time);
              


            }
            return distance;
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

        public void startDialog()
        {

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            DialogStartRoute newDialog = new DialogStartRoute();
            newDialog.Show(transaction, "End route");
        }
        void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);
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
            Location location = locationManager.GetLastKnownLocation(locationProvider);

            ////initialize the location
            //if (location != null)
            //{

            //    OnLocationChanged(location);
            //}
        }


        protected override void OnResume()
        {
            base.OnResume();
            locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
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

        public  void OnLocationChanged(Location location)
        {


            //locationPoints.Add(location.ToString());
            //Toast.MakeText(this, "Location point taken", ToastLength.Long).Show();

            //CameraUpdate center = CameraUpdateFactory.NewLatLng(new LatLng(location.Latitude, location.Longitude));
            ////mMap.MoveCamera(center);

            //CameraUpdate zoom = CameraUpdateFactory.ZoomTo(15);
           // mMap.AnimateCamera(zoom);

        }


    }
}