using System;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Graphics;
using Android.Gms.Maps.Model;
using System.Collections.Generic;

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

        protected async override void OnCreate(Bundle savedInstanceState)
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


            List<User> people = await Azure.getPeople();

            foreach (var item in people)
            {
                setMarker(item);
            }

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

        public void setMarker(User user)
        {
            //if (user.Lat == 0 && user.Lon == 0)
            //    return;
              LatLng myPosition = new LatLng(user.Lat, user.Lon);

            Bitmap pic = IOUtilz.GetImageBitmapFromUrl(user.ProfilePicture);
            BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(pic); //(Resource.Drawable.test);

            //   mMap.AddMarker(new MarkerOptions()
            //  .SetPosition(new LatLng(10,10))
            ////  .SetSnippet("Points: " + user.Points)
            //  .SetTitle("title")).SetIcon(image);//.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));


            var onlineStatus = "offline";

            if (user.Online)
                onlineStatus = "online";

            mMap.AddMarker(new MarkerOptions()
           .SetPosition(myPosition)
           .SetTitle(user.UserName)
           .SetSnippet("Online status: " + onlineStatus)
           .SetIcon(image));



            //    markerOpt1 = new MarkerOptions();
            //    markerOpt1.SetPosition(myPosition);
            //    markerOpt1.SetTitle(user.UserName + " Position");
            //    markerOpt1.SetSnippet("Points: " + user.Points);
            ////  BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(pic); //(Resource.Drawable.test);
            //    markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan)); //;
            //    mMap.AddMarker(markerOpt1);




        }
    }
}