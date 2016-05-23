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
    [Activity(Label = "Route")]
    public class Route : Activity, ILocationListener
    {

       
        LocationManager locationManager;
        string locationProvider;
        MarkerOptions markerOpt1;
        public static GoogleMap mMap;
        ArrayList locationPoints;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.createRoute);

            locationPoints  = new ArrayList();


            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            GoogleMap mMap = mapFrag.Map;

            if (mMap != null)
            {
                mMap.MapType = GoogleMap.MapTypeNormal;  // The GoogleMap object is ready to go.
            }

            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinnerRouteTypes);
            spinner.ItemSelected += spinner_ItemSelected;
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.activity_routeTypes, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;


            Button start = FindViewById<Button>(Resource.Id.startRoute);
            Button end = FindViewById<Button>(Resource.Id.endRoute);
            Button cancel = FindViewById<Button>(Resource.Id.cancelRoute);

          


            //FragmentTransaction transaction = FragmentManager.BeginTransaction();
            //DialogStartRoute newDialog = new DialogStartRoute();
            //newDialog.Show(transaction, "Start Route");


            start.Click += (sender, e) =>
            {
                Toast.MakeText(this,"Starting route...",ToastLength.Short).Show();

                InitializeLocationManager();
            };
            end.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Ending route...", ToastLength.Short).Show();
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

            long minTime = 10 * 1000; // Minimum time interval for update in seconds, i.e. 5 seconds.
            long minDistance = 5; // Minimum distance change for update in meters, i.e. 10 meters.

            locationManager.RequestLocationUpdates(this.locationProvider, minTime,
                      minDistance, this);

        }


        protected override void OnResume()
        {
            base.OnResume();
            //locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
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
        
        public void OnLocationChanged(Location location)
        {

            locationPoints.Add(location.ToString());
            Toast.MakeText(this, "Location point taken", ToastLength.Long).Show();

          //  CameraUpdate center = CameraUpdateFactory.NewLatLng(new LatLng(location.Latitude, location.Longitude));
           // mMap.MoveCamera(center);

          //  CameraUpdate zoom = CameraUpdateFactory.ZoomTo(15);
         //   mMap.AnimateCamera(zoom);

        }


        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            if (e.Position == 0)
            {
                Toast.MakeText(this, "Walking", ToastLength.Short).Show();
            }
            else if (e.Position == 1)
            {
                Toast.MakeText(this, "Running", ToastLength.Short).Show();
            }
            else if (e.Position == 2)
            {
                Toast.MakeText(this, "Hiking", ToastLength.Short).Show();

            }
            else if (e.Position == 3)
            {
                Toast.MakeText(this, "Bicycling", ToastLength.Short).Show();

            }
            else if (e.Position == 4)
            {
                Toast.MakeText(this, "Skiing", ToastLength.Short).Show();

            }
        }



    }
}