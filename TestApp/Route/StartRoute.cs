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
namespace TestApp
{
    [Activity(Label = "StartRoute")]
    public class StartRoute : Activity, ILocationListener
    {

        Location currentLocation;
        LocationManager locationManager;
        string locationProvider;
        TextView locationText;
        MarkerOptions markerOpt1;
        public static GoogleMap mMap;

        public static String[] array;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.startRoute);
            
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
           


            //FragmentTransaction transaction = FragmentManager.BeginTransaction();
            //DialogStartRoute newDialog = new DialogStartRoute();
            //newDialog.Show(transaction, "Start Route");


            start.Click += (sender, e) =>
            {
                Toast.MakeText(this,"Starting route...",ToastLength.Short).Show();
            };
            cancel.Click += (sender, e) =>
            {
               
                Finish();
            };


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

        public async void OnLocationChanged(Location location)
        {
            
            
        }


    }
}