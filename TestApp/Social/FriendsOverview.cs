using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
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
    [Activity(Label = "FriendsOverview", Theme = "@style/Theme2", ScreenOrientation = ScreenOrientation.Portrait)]
    public class FriendsOverview : AppCompatActivity, ViewPager.IOnPageChangeListener  //, IOnMapReadyCallback
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

        private int currentPage;
        LinearLayout layout;
        private TabLayout tabLayout;
        public static ViewPager viewPager;
        ViewPagerAdapter adapter;

        public static int currentFragment;

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
            users = await Azure.nearbyPeople();
            return myFriends;
        }
        protected async override void OnCreate(Bundle savedInstanceState)
        {
          
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.friendsOverview);
            act = this;
           
            me = new List<User>();
            me.Add(MainStart.userInstanceOne); //await Azure.getUserByAuthId(MainStart.userId);
            unit = IOUtilz.LoadPreferences();
            myFriends = await Azure.getUsersFriends(MainStart.userId);
            users = await Azure.nearbyPeople();
            friendRequests = await Azure.getFriendRequests(MainStart.userId);

            distanceUnit = "km";


            toolbar = FindViewById<SupportToolbar>(Resource.Id.tbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            setupViewPager(viewPager);

            viewPager.AddOnPageChangeListener(this);

            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);            
            setupTabIcons();


            currentPage = viewPager.CurrentItem;
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

    
        void showAll()
        {
            mAdapter = null;
            currentPage = viewPager.CurrentItem;
            List<User> orderedRoutes;

            if (currentPage == 1)
            {
                mRecyclerView = FindpeopleFragment.mRecyclerView;

                orderedRoutes = (from user in users
                             
                                 select user).ToList<User>();
                mAdapter = new UsersNearbyAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();
                
            }
            else if (currentPage == 2)
            {
                mRecyclerView = MyFriendsFragment.mRecyclerView;
                orderedRoutes = (from user in myFriends
                                
                                 select user).ToList<User>();
           
                mAdapter = new UsersFriendsAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter, me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }
            else if (currentPage == 3)
            {

                mRecyclerView = FriendRequestFragment.mRecyclerView;
                orderedRoutes = (from user in friendRequests
                                
                                 select user).ToList<User>();           
                    mAdapter = new UsersFriendRequestAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter);
                    mRecyclerView.SetAdapter(mAdapter);
                    mAdapter.NotifyDataSetChanged();
                
                
            }

        }
        void showOnline()
        {
            List<User> orderedRoutes;
            currentPage = viewPager.CurrentItem;

            if (currentPage == 1)
            {
                mRecyclerView = FindpeopleFragment.mRecyclerView;
                orderedRoutes = (from user in users
                                 where user.Online == true
                                 select user).ToList<User>();

                mAdapter = new UsersNearbyAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();
             
            }
            else if(currentPage == 2)
            {
                mRecyclerView = MyFriendsFragment.mRecyclerView;
                orderedRoutes = (from user in myFriends
                                 where user.Online == true
                                 select user).ToList<User>();

                mAdapter = new UsersFriendsAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter,me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }
            else if (currentPage == 3)
            {
                mRecyclerView = FriendRequestFragment.mRecyclerView;
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
            currentPage = viewPager.CurrentItem;


            if (currentPage == 1)
            {
                mRecyclerView = FindpeopleFragment.mRecyclerView;
                orderedRoutes = (from user in users
                                 where user.Sex != "Female"
                                 orderby user.Sex
                                 select user).ToList<User>();
                
                mAdapter = new UsersNearbyAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }
            else if (currentPage == 2)
            {
                mRecyclerView = MyFriendsFragment.mRecyclerView;
                orderedRoutes = (from user in myFriends
                                 where user.Sex != "Female"
                                 orderby user.Sex
                                 select user).ToList<User>();

                mAdapter = new UsersFriendsAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter, me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }
            else if (currentPage == 3)
            {
                mRecyclerView = FriendRequestFragment.mRecyclerView;
                orderedRoutes = (from user in friendRequests
                                 where user.Sex != "Female"
                                 orderby user.Sex
                                 select user).ToList<User>();
                mAdapter = new UsersFriendRequestAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();
            }



        }
        void showFemale()
        {
            List<User> orderedRoutes;
            currentPage = viewPager.CurrentItem;


            if (currentPage == 1)
            {
                mRecyclerView = FindpeopleFragment.mRecyclerView;
                orderedRoutes = (from user in users
                                 where user.Sex != "Male"
                                 orderby user.Sex
                                 select user).ToList<User>();

                mAdapter = new UsersNearbyAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }
            else if (currentPage == 2)
            {
                mRecyclerView = MyFriendsFragment.mRecyclerView;
                orderedRoutes = (from user in myFriends
                                 where user.Sex != "Male"
                                 orderby user.Sex
                                 select user).ToList<User>();

                mAdapter = new UsersFriendsAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter, me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }
            else if (currentPage == 3)
            {
                mRecyclerView = FriendRequestFragment.mRecyclerView;
                orderedRoutes = (from user in friendRequests
                                 where user.Sex != "Male"
                                 orderby user.Sex
                                 select user).ToList<User>();

                mAdapter = new UsersFriendRequestAdapter(orderedRoutes, mRecyclerView, this, this, mAdapter);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();
            }

        }

        public void OnPageScrollStateChanged(int state)
        {
            
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
          
        }

        public async void OnPageSelected(int position)
        {
            TextView txt = null;
            if (position == 1)
            {
                mRecyclerView = FindpeopleFragment.mRecyclerView;
               
                users = await Azure.nearbyPeople();
                txt = act.FindViewById<TextView>(Resource.Id.emptyNearby);
                if (users.Count != 0)
                {
                    mRecyclerView.Visibility = ViewStates.Visible;
                    txt.Visibility = ViewStates.Gone;                 
                    mAdapter = new UsersNearbyAdapter(users, mRecyclerView, this, this, mAdapter);
                    mRecyclerView.SetAdapter(mAdapter);
                     mAdapter.NotifyDataSetChanged();

                }
                else
                {
                    
                        mRecyclerView.Visibility = ViewStates.Invisible;
                        txt.Visibility = ViewStates.Visible;
                    }
                

            }else if(position == 2)
            {
                 myFriends = await Azure.getUsersFriends(MainStart.userId);
                txt = act.FindViewById<TextView>(Resource.Id.emptyFriends);
                mRecyclerView = MyFriendsFragment.mRecyclerView;
                if (myFriends.Count != 0)
                {
                   txt.Visibility = ViewStates.Gone;
                    mRecyclerView.Visibility = ViewStates.Visible;
                    mAdapter = new UsersFriendsAdapter(myFriends, mRecyclerView, this, this, mAdapter, me);
                   mRecyclerView.SetAdapter(mAdapter);
                       mAdapter.NotifyDataSetChanged();
                    
                }else
                {
                               
                        mRecyclerView.Visibility = ViewStates.Invisible;                   
                         txt.Visibility = ViewStates.Visible;
                    }
                        

            }
            else if(position == 3)
            {
                 friendRequests = await Azure.getFriendRequests(MainStart.userId);
                txt = act.FindViewById<TextView>(Resource.Id.emptyRequest);
                mRecyclerView = FriendRequestFragment.mRecyclerView;
                if (friendRequests.Count != 0) 
                {
                    mRecyclerView.Visibility = ViewStates.Visible;
                    txt.Visibility = ViewStates.Gone;
                    mAdapter = new UsersFriendRequestAdapter(friendRequests, mRecyclerView, this, this, mAdapter);
                    mRecyclerView.SetAdapter(mAdapter);
                   mAdapter.NotifyDataSetChanged();
                }else
                {
                    mRecyclerView.Visibility = ViewStates.Invisible;
                    txt.Visibility = ViewStates.Visible;
                }

            }
        }



        public override void OnBackPressed()
        {

            base.OnBackPressed();
            Finish();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();


        }

    }




}