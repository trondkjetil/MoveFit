


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

namespace TestApp
{

    public class StartMapFragment : Fragment, IOnMapReadyCallback
    {

        MapView mMapView;
        private GoogleMap mMap;
        int[] unit;    
        List<User> me;
     

        public override  View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.startMapFragment, container, false);

            ImageButton myRoutes = view.FindViewById<ImageButton>(Resource.Id.myRoutes);
            ImageButton createRoute = view.FindViewById<ImageButton>(Resource.Id.createRoute);
            Bitmap icon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.startFlag);
            createRoute.SetImageBitmap(IOUtilz.getRoundedShape(icon));


            createRoute.Click += (sender, e) =>
            {
               Intent myIntent = new Intent(this.Activity, typeof(CreateRoute));
                StartActivity(myIntent);
            };
            myRoutes.Click += (sender, e) =>
            {

                RouteOverview.act.Finish();
            };

            me = RouteOverview.me;
            unit = RouteOverview.unit;

            mMapView = view.FindViewById<MapView>(Resource.Id.map);
            mMapView.OnCreate(savedInstanceState);
            mMapView.OnResume();


            try
            {

                MapsInitializer.Initialize(this.Activity.ApplicationContext);

            }
            catch (Exception e)
            {

                     }

            mMapView.GetMapAsync(this);


            return view;

        }



        public void setMarker(Route route)
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


            BitmapDescriptor image = BitmapDescriptorFactory.FromResource(Resource.Drawable.compass_base); //(Resource.Drawable.test);

            mMap.AddMarker(new MarkerOptions()
           .SetPosition(myPosition)
           .SetTitle(route.Name + "(" + route.Difficulty + ")")
           .SetSnippet(dist.ToString() + RouteOverview.distanceUnit).SetIcon(image));
            }
            catch (Exception a)
            {
                Toast.MakeText(this.Activity, a.ToString() + "ooooooooooooooooooooooo", ToastLength.Long).Show();


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

         



            foreach (var item in RouteOverview.routes)
            {
                setMarker(item);
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