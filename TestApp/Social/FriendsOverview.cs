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
using Android.Support.V7.Widget;
using System.Linq;
namespace TestApp
{
    [Activity(Label = "FriendsOverview", Theme = "@style/Theme2")]
    public class FriendsOverview : AppCompatActivity  //, IOnMapReadyCallback
    {
       
       
        SupportToolbar toolbar;

        public static IMenuItem goBack;
        public static IMenuItem goHome;
       
        public static string distanceUnit;
        public static int[] unit;
        public static Activity act;

        public static List<User> myFriends;
        public static List<User> friendRequests;
        public static List<User> me;
        public static List<User> users;

        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;

        private TabLayout tabLayout;
        private ViewPager viewPager;
        ViewPagerAdapter adapter;
        private int[] tabIcons = {
            Resource.Drawable.maps,
            Resource.Drawable.perm_group_social_info,
            Resource.Drawable.test,
            Resource.Drawable.ic_menu_allfriends
 };



        private void setupViewPager(ViewPager viewPager)
        {
            adapter = new ViewPagerAdapter(SupportFragmentManager);
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

            toolbar = FindViewById<SupportToolbar>(Resource.Id.tbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            setupViewPager(viewPager);


            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
            setupTabIcons();

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recycleUserRoutes);
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

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

                case Resource.Id.online:
                    showOnline();
                    return true;

                case Resource.Id.male:
                    showMale();
                    return true;

                case Resource.Id.female:
                    showFemale();
                    return true;

                case Resource.Id.all:
                    showAll();
                    return true;

                case Android.Resource.Id.Home:
                   OnBackPressed();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }

        }

        public int checkFragment()
        {


            //Doesnt work
            int tall = 0;
            var current = viewPager.CurrentItem;

            if (adapter.GetItemPosition(1).Equals(current) || adapter.GetItemPosition(1) == current)
            {
                tall = 1;
            }
            else if (adapter.GetItemPosition(2).Equals(current) || adapter.GetItemPosition(1) == current)
            {
                tall = 2;
            }
            else if (adapter.GetItemPosition(3).Equals(current) || adapter.GetItemPosition(1) == current)
            {
                tall = 3;
            }
            else
                tall = 1;

            return tall;
        }

        void showAll()
        {
            List<User> orderedRoutes;
            orderedRoutes = (from user in friendRequests
                             orderby user.Points
                             select user).ToList<User>();
            mAdapter = new UsersFriendRequestAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();

        }
        void showOnline()
        {
            List<User> orderedRoutes;
            if (checkFragment() == 1)
            {
                
                orderedRoutes = (from user in users
                                 where user.Online == true
                                 select user).ToList<User>();

                mAdapter = new UsersNearbyAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();
            }else if(checkFragment() == 2)
            {
                orderedRoutes = (from user in myFriends
                                 where user.Online == true
                                 select user).ToList<User>();

                mAdapter = new UsersFriendsAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter,me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();
            }
            else if (checkFragment() == 3)
            {
                orderedRoutes = (from user in friendRequests
                                 where user.Online == true
                                 select user).ToList<User>();

                mAdapter = new UsersFriendRequestAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();
            }


        }




        void showMale()
        {
            List<User> orderedRoutes;
            orderedRoutes = (from user in friendRequests
                             where user.Sex != "Female"
                             orderby user.Sex
                             select user).ToList<User>();

            mAdapter = new UsersFriendRequestAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();



        }
        void showFemale()
        {
            List<User> orderedRoutes;
            orderedRoutes = (from user in friendRequests
                             where user.Sex != "Male"
                             orderby user.Sex
                             select user).ToList<User>();

            mAdapter = new UsersFriendRequestAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();


        }



    }




}