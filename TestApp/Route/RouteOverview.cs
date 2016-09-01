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
using System.Linq;
using Android.Support.V7.Widget;

namespace TestApp
{
    [Activity(Label = "RouteOverview", Theme = "@style/Theme2")]
    public class RouteOverview : AppCompatActivity  //, IOnMapReadyCallback
    {
     
        public static IMenuItem goBack;
        public static IMenuItem goHome;
       
        public static string distanceUnit;
        public static int[] unit;
        public static Activity act;

        public static List<Route> routes;
        //public List<Route> routeList;
        public static List<User> me;
        public static List<Route> myRoutes;

        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private int currentPage;
        private SupportToolbar toolbar;
        private TabLayout tabLayout;
        private ViewPager viewPager;
        private int[] tabIcons = {
            Resource.Drawable.maps,
            Resource.Drawable.rsz_running,
             //Resource.Drawable.startFlag,
            Resource.Drawable.test
    };


        private void setupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
            adapter.addFragment(new StartMapFragment(), "Map");
            //adapter.addFragment(new RouteListFragment(), "Create Routes");
            adapter.addFragment(new RouteListFragment(), "Find Routes");
            adapter.addFragment(new MyRoutesFragment(), "My Routes");
            viewPager.Adapter = adapter;
        }


        private void setupTabIcons()
        {
            tabLayout.GetTabAt(0).SetIcon(tabIcons[0]);
            tabLayout.GetTabAt(1).SetIcon(tabIcons[1]);
            tabLayout.GetTabAt(2).SetIcon(tabIcons[2]);
           // tabLayout.GetTabAt(3).SetIcon(tabIcons[3]);

        }
        protected async override void OnCreate(Bundle savedInstanceState)
        {
           // RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.routesOverview);
            act = this;
            routes = await Azure.getRoutes();
           // routeList = routes;
            me = new List<User>();
            me.Add(MainStart.userInstanceOne);
            unit = IOUtilz.LoadPreferences();
            myRoutes = await Azure.getMyRoutes(MainStart.userId);
           
            toolbar = FindViewById<SupportToolbar>(Resource.Id.tools);
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

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            menu.Clear();
            MenuInflater.Inflate(Resource.Menu.action_menu_nav_routes, menu);
            return base.OnPrepareOptionsMenu(menu);
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

                case Resource.Id.type:
                    sortType();
                    return true;

                case Resource.Id.rating:
                    sortRating();
                    return true;

                case Resource.Id.nearbyRoutes:
                    sortDistance();
                    return true;

                case Resource.Id.difficulty:
                    sortDifficulty();
                    return true;
   
                case Android.Resource.Id.Home:// Resource.Id.back:
                    this.OnBackPressed();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }
        }


        void sortRating()
        {
            List<Route> orderedRoutes;
            currentPage = viewPager.CurrentItem;
            if (currentPage == 1)
            {
                mRecyclerView = RouteListFragment.mRecyclerView;
                orderedRoutes = (from route in routes
                                 orderby route.Review
                                 select route).ToList<Route>();


                mAdapter = new UsersRoutesAdapterFragment(orderedRoutes, mRecyclerView, this, me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }else if(currentPage == 2)
            {
                mRecyclerView = MyRoutesFragment.mRecyclerView;
                orderedRoutes = (from route in myRoutes
                                 orderby route.Review
                                 select route).ToList<Route>();


                mAdapter = new UsersRoutesAdapterFragment(orderedRoutes, mRecyclerView, this, me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }

        }
        void sortDistance()
        {
            List<Route> orderedRoutes;
            currentPage = viewPager.CurrentItem;
            if (currentPage == 1)
            {
                mRecyclerView = RouteListFragment.mRecyclerView;
                orderedRoutes = (from route in routes
                                 orderby route.Review
                                 select route).ToList<Route>();


                mAdapter = new UsersRoutesAdapterFragment(orderedRoutes, mRecyclerView, this, me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }
            else if (currentPage == 2)
            {
                mRecyclerView = MyRoutesFragment.mRecyclerView;
                orderedRoutes = (from route in myRoutes
                                 orderby route.Distance
                                 select route).ToList<Route>();


                mAdapter = new UsersRoutesAdapterFragment(orderedRoutes, mRecyclerView, this, me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }



        }
        void sortDifficulty()
        {
            List<Route> orderedRoutes;
            currentPage = viewPager.CurrentItem;
            if (currentPage == 1)
            {
                mRecyclerView = RouteListFragment.mRecyclerView;
                orderedRoutes = (from route in routes
                                 orderby route.Review
                                 select route).ToList<Route>();


                mAdapter = new UsersRoutesAdapterFragment(orderedRoutes, mRecyclerView, this, me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }
            else if (currentPage == 2)
            {
                mRecyclerView = MyRoutesFragment.mRecyclerView;
                orderedRoutes = (from route in myRoutes
                                 orderby route.Difficulty
                                 select route).ToList<Route>();


                mAdapter = new UsersRoutesAdapterFragment(orderedRoutes, mRecyclerView, this, me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }


        }

        void sortType()
        {
            List<Route> orderedRoutes;
            currentPage = viewPager.CurrentItem;
            if (currentPage == 1)
            {
                mRecyclerView = RouteListFragment.mRecyclerView;
                orderedRoutes = (from route in routes
                                 orderby route.Review
                                 select route).ToList<Route>();


                mAdapter = new UsersRoutesAdapterFragment(orderedRoutes, mRecyclerView, this, me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }
            else if (currentPage == 2)
            {
                mRecyclerView = MyRoutesFragment.mRecyclerView;
                orderedRoutes = (from route in myRoutes
                                 orderby route.RouteType
                                 select route).ToList<Route>();


                mAdapter = new UsersRoutesAdapterFragment(orderedRoutes, mRecyclerView, this, me);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

            }


        }
    }




}