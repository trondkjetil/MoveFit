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
            me = new List<User>();
            me.Add( MainStart.userInstanceOne); //await Azure.getUserByAuthId(MainStart.userId);
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


        

         

        }
        public override bool OnCreateOptionsMenu(IMenu menu)

        {
            MenuInflater.Inflate(Resource.Menu.action_menu_nav_people, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.home:

                    Finish();



                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }


        }


    
    }




}