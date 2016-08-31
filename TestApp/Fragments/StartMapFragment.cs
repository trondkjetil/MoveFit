


using System;
using Android;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using System.Collections.Generic;
using Android.Graphics;
using Android.Content;
//using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

namespace TestApp
{

    public class StartMapFragment : Fragment, IOnMapReadyCallback
    {

        MapView mMapView;
        private GoogleMap mMap;
        int[] unit;    
        List<User> me;
     //   public SupportToolbar toolbar;
 
        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;

        }
        public override  View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.startMapFragment, container, false);


            //toolbar = view.FindViewById<SupportToolbar>(Resource.Id.tbar);
            //AppCompatActivity activity = (AppCompatActivity)this.Activity;
            //activity.SetSupportActionBar(toolbar);
            //activity.SupportActionBar.SetDisplayShowTitleEnabled(false);
            //activity.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            //activity.SupportActionBar.SetDisplayShowHomeEnabled(false);
            //toolbar.Visibility = ViewStates.Invisible;


            ImageButton createRoute = view.FindViewById<ImageButton>(Resource.Id.createRoute);
            ImageButton myRoutes = view.FindViewById<ImageButton>(Resource.Id.myRoutes);
            TextView createRouteLabel = view.FindViewById<TextView>(Resource.Id.textRoute);

        

            if (this.Activity.Class.SimpleName != "FriendsOverview")
            {
                 Bitmap icon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.startFlag);
                createRoute.SetImageBitmap(IOUtilz.getRoundedShape(icon));
                createRouteLabel.Text = "Create Route";
                createRoute.Click += (sender, e) =>
                {
                    Intent myIntent = new Intent(this.Activity, typeof(CreateRoute));
                    StartActivity(myIntent);
                };
                myRoutes.Click += (sender, e) =>
                {

                    RouteOverview.act.Finish();
                };

                myRoutes.Visibility = ViewStates.Invisible;

                me = RouteOverview.me;
                unit = RouteOverview.unit;
            }else
            {
                me = FriendsOverview.me;
                unit = FriendsOverview.unit;
                createRoute.Visibility = ViewStates.Invisible;
                createRouteLabel.Text = "Find your self a partner to do activities with!";
            }

           

            mMapView = view.FindViewById<MapView>(Resource.Id.map);
            mMapView.OnCreate(savedInstanceState);
            mMapView.OnResume();


            try
            {

                MapsInitializer.Initialize(this.Activity.ApplicationContext);

            }
            catch (Exception)
            {

                     }

            mMapView.GetMapAsync(this);


            return view;

        }

        //public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)

        //{
        //    try
        //    {
        //      menu.Clear();
        //       menu.FindItem(Resource.Id.type).SetVisible(false);
        //    }
        //    catch (Exception)
        //    {


        //    }
        //    inflater.Inflate(Resource.Menu.action_menu_nav_routes, menu);

        //    base.OnCreateOptionsMenu(menu, inflater);


        //}
        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{

        //    switch (item.ItemId)
        //    {


        //        case Resource.Id.type:
                    
        //            return true;

             
        //        case Resource.Id.home:

        //            this.Activity.Finish();


        //            return true;

        //        default:
        //            return base.OnOptionsItemSelected(item);

        //    }



        //}



        public void setMarkerRoutes(Route route)
        {


            try
            {

          
            LatLng myPosition = new LatLng(route.Lat, route.Lon);
            float[] result = new float[1];
            Location.DistanceBetween(me[0].Lat, me[0].Lon, route.Lat, route.Lon, result);
            int dist = Convert.ToInt32(result[0]);

            if (unit[1] == 0)
            {
                dist = dist / 1000;
            }
            else if(dist == 0)
                {
                    dist = 0;
                }
                else
                dist = Convert.ToInt32(IOUtilz.ConvertKilometersToMiles(dist / 1000));

              

                Bitmap largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.compass_base);     
                BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(IOUtilz.getRoundedShape( largeIcon)); //(Resource.Drawable.test);
               
            mMap.AddMarker(new MarkerOptions()
           .SetPosition(myPosition)
           .SetTitle(route.Name + "(" + route.Difficulty + ")")
           .SetSnippet(dist.ToString() + RouteOverview.distanceUnit).SetIcon(image));
            }
            catch (Exception a)
            {
              

            }

        }


        public void setMarkerUser(User user)
        {


            try
            {


                LatLng myPosition = new LatLng(user.Lat, user.Lon);
                float[] result = new float[1];
                Location.DistanceBetween(me[0].Lat, me[0].Lon, user.Lat, user.Lon, result);
                int dist = Convert.ToInt32(result[0]);

                if (unit[1] == 0)
                {
                    dist = dist / 1000;
                }
                else if (dist == 0)
                {
                    dist = 0;
                }
                else
                    dist = Convert.ToInt32(IOUtilz.ConvertKilometersToMiles(dist / 1000));


                Bitmap pic = IOUtilz.GetImageBitmapFromUrl(user.ProfilePicture);
                BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(pic); //(Resource.Drawable.test);

                mMap.AddMarker(new MarkerOptions()
               .SetPosition(myPosition)
               .SetTitle(user.UserName + "(" + user.Online + ")")
               .SetSnippet(dist.ToString() + FriendsOverview.distanceUnit).SetIcon(image));
            }
            catch (Exception a)
            {
               

            }

        }

        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;

          //  mMap = googleMap;//mMapView.Map;
            mMap.MapType = GoogleMap.MapTypeTerrain;  // The GoogleMap object is ready to go.
            mMap.UiSettings.ZoomControlsEnabled = true;
            mMap.UiSettings.RotateGesturesEnabled = true;
            mMap.UiSettings.ScrollGesturesEnabled = true;


            if (this.Activity.Class.SimpleName != "FriendsOverview")
            {
                foreach (var item in RouteOverview.routes)
                {
                    setMarkerRoutes(item);
                }

            }else
            {

                foreach (var item in FriendsOverview.myFriends)
                {
                    setMarkerUser(item);
                }

            }



        }

        //public override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //    mMapView.OnCreate(savedInstanceState);
        //}
        public override void OnResume()
        {
            base.OnResume();
            mMapView.OnResume();
        }

       
        public override void OnPause()
        {
            base.OnPause();
            mMapView.OnPause();
        }

        
     public override void OnDestroy()
        {
            base.OnDestroy();
            mMapView.OnDestroy();
        }

      
            public override void OnLowMemory()
        {
            base.OnLowMemory();
            mMapView.OnLowMemory();
        }
    }

}