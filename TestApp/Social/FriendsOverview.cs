using System;
using Android.App;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Graphics;
using Android.Gms.Maps.Model;
using System.Collections.Generic;
using Android.Support.V7.App;
using Android.Support.V4.View;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Android.Content;
using System.Threading.Tasks;

namespace TestApp
{
    [Activity(Label = "FriendsOverview", Theme = "@style/Theme2")]
    public class FriendsOverview : AppCompatActivity  //, IOnMapReadyCallback
    {
        //   Intent myIntent;
        GoogleMap mMap;
        // SupportToolbar toolbar;

        public static IMenuItem goBack;
        public static IMenuItem goHome;
       
        public static string distanceUnit;
        public static int[] unit;
        public static Activity act;

        public static List<User> myFriends;
        public static List<User> friendRequests;
        public static List<User> me;
        public static List<User> users;

        private SupportToolbar toolbar;
        private TabLayout tabLayout;
        private ViewPager viewPager;
        private int[] tabIcons = {
            Resource.Drawable.maps,
            Resource.Drawable.perm_group_social_info,
            Resource.Drawable.test,
            Resource.Drawable.ic_menu_allfriends


    };

        private void setupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
            adapter.addFragment(new StartMapFragment(), "Map");
            adapter.addFragment(new FindpeopleFragment(), "Nearby");
            adapter.addFragment(new MyFriendsFragment(), "Friends");
            adapter.addFragment(new FriendRequestFragment(), "Requests");
         
            viewPager.Adapter = adapter;
        }


        private void setupTabIcons()
        {
            tabLayout.GetTabAt(0).SetIcon(tabIcons[0]);
            tabLayout.GetTabAt(1).SetIcon(tabIcons[1]);
           tabLayout.GetTabAt(2).SetIcon(tabIcons[2]);
           tabLayout.GetTabAt(3).SetIcon(tabIcons[3]);

        }

        public async static Task<List<User>> updateFriendList()
        {
            myFriends = await Azure.getFriendRequests(MainStart.userId);
            return myFriends;
        }
        public async static Task<List<User>> updatePeopleNearby()
        {
            users = await Azure.getPeople();
            return myFriends;
        }
        protected async override void OnCreate(Bundle savedInstanceState)
        {
          
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.friendsOverview);
            act = this;
            myFriends = await Azure.getUsersFriends(MainStart.userId);
            me = await Azure.getUserByAuthId(MainStart.userId);
            unit = IOUtilz.LoadPreferences();
            users = await Azure.getPeople();
            friendRequests = await Azure.getFriendRequests(MainStart.userId);

            //toolbar = FindViewById<SupportToolbar>(Resource.Id.tbar);
            //SetSupportActionBar(toolbar);
            //SupportActionBar.SetDisplayShowTitleEnabled(false);
            //SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            //SupportActionBar.SetDisplayShowHomeEnabled(false);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            setupViewPager(viewPager);


            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
            setupTabIcons();


        

            //Button myRoutes = (Button)FindViewById(Resource.Id.myRoutes);
            //Button findRoute = (Button)FindViewById(Resource.Id.findRoutes);
            //Button createRoute = (Button)FindViewById(Resource.Id.createRoutes);

            //Typeface tf = Typeface.CreateFromAsset(Assets,
            //   "english111.ttf");
            //TextView tv = (TextView)FindViewById(Resource.Id.textRoute);
            //tv.Text = "Create your own route, and earn points!";
            //tv.TextSize = 28;
            //tv.Typeface = tf;

            // unit = IOUtilz.LoadPreferences();
            //if (unit[1] == 0)
            //{
            //    distanceUnit = " km away";
            //}
            //else
            //    distanceUnit = " miles away";

            //MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            //mMap = mapFrag.Map;

            //if (mMap != null)
            //{
            //    mMap.MapType = GoogleMap.MapTypeTerrain;  // The GoogleMap object is ready to go.
            //}

            //mMap.UiSettings.ZoomControlsEnabled = true;
            //mMap.UiSettings.RotateGesturesEnabled = true;
            //mMap.UiSettings.ScrollGesturesEnabled = true;

            //myRoutes.Click += (sender, e) => {
            //    //myIntent = new Intent(this, typeof(UserMyRoutes));
            //    //StartActivity(myIntent);

            //};
            //findRoute.Click += (sender, e) => {
            //    //myIntent = new Intent(this, typeof(UsersRoutes));
            //    //StartActivity(myIntent);
            //};
            //createRoute.Click += (sender, e) =>
            //{
            //    //myIntent = new Intent(this, typeof(CreateRoute));
            //    //StartActivity(myIntent);

            //};

        }
        public override bool OnCreateOptionsMenu(IMenu menu)

        {
            MenuInflater.Inflate(Resource.Menu.action_menu_nav_routes, menu);



            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {

                //case Resource.Id.exit:
                //    Finish();
                //    return true;

                //case Resource.Id.back:
                //    OnBackPressed();
                //    return true;
             

                case Resource.Id.home:

                    Finish();



                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }


        }


        //public void OnMapReady(GoogleMap googleMap)
        //{
        //    mMap = googleMap;
        //}
        //public void setMarker(Route route)
        //{
        //      LatLng myPosition = new LatLng(route.Lat, route.Lon);


        //    float[] result = new float[1];
        //    Location.DistanceBetween(me[0].Lat, me[0].Lon, route.Lat,route.Lon,result);
        //    int dist = Convert.ToInt32(result[0]);

        //    if (unit[1] == 0)
        //    {
        //        dist = dist / 1000;
        //    }
        //    else
        //        dist = (int) IOUtilz.ConvertKilometersToMiles(dist / 1000);


        //    BitmapDescriptor image = BitmapDescriptorFactory.FromResource(Resource.Drawable.compass_base); //(Resource.Drawable.test);

        //    mMap.AddMarker(new MarkerOptions()
        //   .SetPosition(myPosition)
        //   .SetTitle(route.Name + "("+ route.Difficulty +")" )
        //   .SetSnippet(dist.ToString() + distanceUnit).SetIcon(image));

        //    markerOpt1 = new MarkerOptions();
        //    markerOpt1.SetPosition(myPosition);
        //    markerOpt1.SetTitle(user.UserName + " Position");
        //    markerOpt1.SetSnippet("Points: " + user.Points);
        ////  BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(pic); //(Resource.Drawable.test);
        //    markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan)); //;
        //    mMap.AddMarker(markerOpt1);
    }




}