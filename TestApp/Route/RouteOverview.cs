using System;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;

namespace TestApp
{
    [Activity(Label = "RouteOverview")]
    public class RouteOverview : Activity, IOnMapReadyCallback
    {
        Intent myIntent;
        GoogleMap mMap;
        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.routesOverview);



            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mMap = mapFrag.Map;

            if (mMap != null)
            {
                mMap.MapType = GoogleMap.MapTypeTerrain;  // The GoogleMap object is ready to go.
            }

            mMap.UiSettings.ZoomControlsEnabled = true;
            mMap.UiSettings.RotateGesturesEnabled = true;
            mMap.UiSettings.ScrollGesturesEnabled = true;


            Button myRoutes = (Button)FindViewById(Resource.Id.myRoutes);
            Button findRoute = (Button)FindViewById(Resource.Id.findRoutes);
            Button createRoute = (Button)FindViewById(Resource.Id.createRoutes);


            myRoutes.Click += (sender, e) => {
                myIntent = new Intent(this, typeof(UserMyRoutes));
                StartActivity(myIntent);

            };
            findRoute.Click += (sender, e) => {
                myIntent = new Intent(this, typeof(UsersRoutes));
                StartActivity(myIntent);
            };
            createRoute.Click += (sender, e) =>
            {
                myIntent = new Intent(this, typeof(CreateRoute));
                StartActivity(myIntent);

            };


        }

      
    }
}