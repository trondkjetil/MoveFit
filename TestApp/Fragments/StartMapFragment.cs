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
using static Android.Gms.Maps.GoogleMap;
using Java.Util;
using System.Threading.Tasks;

namespace TestApp
{

    public class StartMapFragment : Fragment, IOnMapReadyCallback, IOnMarkerClickListener
    {

        MapView mMapView;
        private GoogleMap mMap;
        int[] unit;    
        List<User> me;
        //   public SupportToolbar toolbar;
        HashMap mHashMapRoute;
        HashMap mHashMapUser;

        public string routeName;
        public string routeInfo;
        public string routeRating;
        public string routeDifficulty;
        public string routeLength;
        public string routeType;
        public int routeTrips;
        public string routeId;
        public string routeTime;
        public string routeUserId;

        public int activeScreen;

        public string userName;
        public string userGender;
        public int userAge;
        public string userProfileImage;
        public int userPoints;
        public string userAboutMe;
        public string userID;
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
           // ImageButton myRoutes = view.FindViewById<ImageButton>(Resource.Id.myRoutes);
            TextView createRouteLabel = view.FindViewById<TextView>(Resource.Id.textRoute);

        

            if (this.Activity.Class.SimpleName != "FriendsOverview")
            {
                 Bitmap icon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.startFlag);
                createRoute.SetImageBitmap(IOUtilz.scaleDown(icon,160,false));
              //  createRouteLabel.Text = "Create Route";
                createRoute.Click += (sender, e) =>
                {
                    Intent myIntent = new Intent(this.Activity, typeof(CreateRoute));
                    StartActivity(myIntent);
                };
                //myRoutes.Click += (sender, e) =>
                //{

                //    RouteOverview.act.Finish();
                //};

                // myRoutes.Visibility = ViewStates.Invisible;
                mHashMapRoute = new HashMap();
                me = RouteOverview.me;
                unit = RouteOverview.unit;
                activeScreen = 0;
            }
            else
            {

                activeScreen = 1;
                mHashMapUser = new HashMap();
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



                Bitmap largeIcon = null;
                BitmapDescriptor image = null;
                if (route.RouteType == "Walking")
                {
                    largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.wa);
                    image = BitmapDescriptorFactory.FromBitmap(IOUtilz.getRoundedShape(largeIcon)); //(Resource.Drawable.test);

                    
                }
                else if (route.RouteType == "Running")
                {
                    largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.ru);
                    image = BitmapDescriptorFactory.FromBitmap(IOUtilz.getRoundedShape(largeIcon)); //(Resource.Drawable.test);

                }
                else if (route.RouteType == "Hiking")
                {
                    largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.tr);
                    image = BitmapDescriptorFactory.FromBitmap(IOUtilz.getRoundedShape(largeIcon)); //(Resource.Drawable.test);


                }
                else if (route.RouteType == "Bicycling")
                {
                    largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.cy);
                    image = BitmapDescriptorFactory.FromBitmap(IOUtilz.getRoundedShape(largeIcon)); //(Resource.Drawable.test);


                }
                else if (route.RouteType == "Skiing")
                {
                    largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.sk);
                    image = BitmapDescriptorFactory.FromBitmap(IOUtilz.getRoundedShape(largeIcon)); //(Resource.Drawable.test);


                }
                else if (route.RouteType == "Kayaking")
                {
                    largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.ka);
                    image = BitmapDescriptorFactory.FromBitmap(IOUtilz.getRoundedShape(largeIcon)); //(Resource.Drawable.test);


                }






                Marker m = mMap.AddMarker(new MarkerOptions()
           .SetPosition(myPosition)
           .SetTitle(route.Name + "(" + route.Difficulty + ")")
           .SetSnippet(dist.ToString() + RouteOverview.distanceUnit).SetIcon(image));
           
                mHashMapRoute.Put(m, route.Id);



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

                string online = "Online";
                if (!user.Online)
                {
                    online = "Offline";
                }

               Marker mark =   mMap.AddMarker(new MarkerOptions()
               .SetPosition(myPosition)
               .SetTitle(user.UserName + "(" + online + ")")
               .SetSnippet(dist.ToString() + FriendsOverview.distanceUnit).SetIcon(image));
                mHashMapUser.Put(mark, user.Id);
              
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
            mMap.SetOnMarkerClickListener(this);

            mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
            var myPos = new LatLng(MainStart.currentLocation.Latitude, MainStart.currentLocation.Longitude);
            mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(myPos, 11));
            
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


        public bool OnMarkerClick(Marker marker)
        {
            Activity.RunOnUiThread( async () =>
            {

           
           
           
                if(activeScreen == 0)
                {

                    string routeIdentification = (string)mHashMapRoute.Get(marker);
                    List<Route> routeList = await Azure.getRouteById(routeIdentification);
                    Route routeInstance = routeList[0];


                    routeName = routeInstance.Name;
                    routeInfo = routeInstance.Info;
                    routeDifficulty = routeInstance.Difficulty;
                    routeLength = routeInstance.Distance;
                    routeType = routeInstance.RouteType;
                    routeRating = routeInstance.Review;
                    routeTrips = routeInstance.Trips;
                    routeId = routeInstance.Id;
                    routeTime = routeInstance.Time;
                    routeUserId = routeInstance.User_id;



                    Bundle b = new Bundle();
                    b.PutStringArray("MyData", new String[] {

            routeName,
            routeInfo,
            routeDifficulty,
            routeLength,
            routeType,
            routeRating,
            routeTrips.ToString(),
            routeId,
            routeTime,
            routeUserId

        });

                    Intent myIntent = new Intent(this.Activity, typeof(StartRoute));
                    myIntent.PutExtras(b);
                    Activity.StartActivity(myIntent);
                }


                else
                {

                    string userIdentiication = (string)mHashMapUser.Get(marker);

                    User userInstance =  await Azure.getUser(userIdentiication);

                    userName = userInstance.UserName;
                    userGender = userInstance.Sex;
                    userAge = userInstance.Age;
                    userProfileImage = userInstance.ProfilePicture;
                    userPoints = userInstance.Points;
                    userAboutMe = userInstance.AboutMe;
                    userID = userInstance.Id;

                    Bundle ba = new Bundle();
                    ba.PutStringArray("MyData", new String[] {

                    userName,
                    userGender,
                    userAge.ToString(),
                    userProfileImage,
                    userPoints.ToString(),
                    userAboutMe,
                    userID

                });

                    Intent myIntent = new Intent(this.Activity, typeof(UserProfile));
                    myIntent.PutExtras(ba);
                    this.Activity.StartActivity(myIntent);


                }




          

            });


            return true;

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