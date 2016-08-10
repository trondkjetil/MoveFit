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
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
namespace TestApp
{
    [Activity(Label = "RouteOverview", Theme = "@style/Theme2")]
    public class RouteOverview : AppCompatActivity, IOnMapReadyCallback
    {
        Intent myIntent;
        GoogleMap mMap;
        SupportToolbar toolbar;

        public static IMenuItem goBack;
        public static IMenuItem goHome;
        public List<User> me;

        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
        }

        protected async override void OnCreate(Bundle savedInstanceState)
        {
           // RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.routesOverview);

            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            me = await Azure.getUserInstanceByName(MainStart.userName);

            SupportActionBar.SetDisplayShowTitleEnabled(false);
          //  SupportActionBar.SetIcon(icon);


            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mMap = mapFrag.Map;

            if (mMap != null)
            {
                mMap.MapType = GoogleMap.MapTypeTerrain;  // The GoogleMap object is ready to go.
            }

            mMap.UiSettings.ZoomControlsEnabled = true;
            mMap.UiSettings.RotateGesturesEnabled = true;
            mMap.UiSettings.ScrollGesturesEnabled = true;

            List<Route> routes = await Azure.getRoutes();
            var test = await Azure.nearbyRoutes();

            foreach (var item in routes)
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

        public void setMarker(Route route)
        {
            //if (user.Lat == 0 && user.Lon == 0)
            //    return;

            //string[] location = loc.Location.Split(',');


           // LatLng myPosition = new LatLng(Convert.ToInt32(location[0]), Convert.ToInt32(location[1]));
            LatLng myPosition = new LatLng(route.Lat, route.Lon);


            //   mMap.AddMarker(new MarkerOptions()
            //  .SetPosition(new LatLng(10,10))
            ////  .SetSnippet("Points: " + user.Points)
            //  .SetTitle("title")).SetIcon(image);//.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));

            float[] result = new float[1];
            Location.DistanceBetween(me[0].Lat, me[0].Lon, route.Lat,route.Lon,result);
            int dist = Convert.ToInt32(result[0]);

            mMap.AddMarker(new MarkerOptions()
           .SetPosition(myPosition)
           .SetTitle(route.Name + " Diff:"+ route.Difficulty )
           .SetSnippet("Distance from me: " +dist.ToString() + " meters"));

            //    markerOpt1 = new MarkerOptions();
            //    markerOpt1.SetPosition(myPosition);
            //    markerOpt1.SetTitle(user.UserName + " Position");
            //    markerOpt1.SetSnippet("Points: " + user.Points);
            ////  BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(pic); //(Resource.Drawable.test);
            //    markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan)); //;
            //    mMap.AddMarker(markerOpt1);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)

        {
            MenuInflater.Inflate(Resource.Menu.action_menu_nav, menu);

            //itemGender = menu.FindItem(Resource.Id.gender);
            //itemAge = menu.FindItem(Resource.Id.age);
            //itemProfilePic = menu.FindItem(Resource.Id.profilePicture);
            //itemExit = menu.FindItem(Resource.Id.exit);


            //goHome.SetIcon(Resource.Drawable.eexit);
            //goBack.SetIcon(Resource.Drawable.ic_menu_back);


            return base.OnCreateOptionsMenu(menu);
        }

        public override void OnBackPressed()
        {

                base.OnBackPressed();
            Finish();

            }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {

                case Resource.Id.exit:
                    Finish();
                    return true;

                case Resource.Id.back:
                    OnBackPressed();
                    return true;

                case Resource.Id.home:

                    //Intent myIntent = new Intent(this, typeof(WelcomeScreen));
                    //StartActivity(myIntent);

                    OnBackPressed();
                    Finish();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }


        }

    }
}