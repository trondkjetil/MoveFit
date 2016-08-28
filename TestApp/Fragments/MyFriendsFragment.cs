﻿using System.Linq;
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
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using System.ComponentModel;
using System.Threading;

namespace TestApp
{

    public class MyFriendsFragment : Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        SwipeRefreshLayout mSwipeRefreshLayout;
        public SupportToolbar toolbar;
        public List<User> myFriends;
        public static List<User> me;
        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.UsersRoutes, container, false);

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycleUserRoutes);
            //Create our layout manager
            mLayoutManager = new LinearLayoutManager(this.Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            toolbar = view.FindViewById<SupportToolbar>(Resource.Id.tbar);
            AppCompatActivity activity = (AppCompatActivity)this.Activity;
            activity.SetSupportActionBar(toolbar);
            activity.SupportActionBar.SetDisplayShowTitleEnabled(false);
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            activity.SupportActionBar.SetDisplayShowHomeEnabled(false);

            myFriends = FriendsOverview.myFriends;
            me = FriendsOverview.me;
            if (myFriends.Count == 0)
            {
                IOUtilz.notFound(this.Activity);
            }
            else
            {
                mAdapter = new UsersFriendsAdapter(myFriends, mRecyclerView, this.Activity, this.Activity, mAdapter, RouteOverview.me);
                mRecyclerView.SetAdapter(mAdapter);
            }
            return view;

        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)

        {
            //if(menu.Size() != 0)
            //{
            //   
            //}
            menu.Clear();
            inflater.Inflate(Resource.Menu.action_menu_nav_routes, menu);

            base.OnCreateOptionsMenu(menu, inflater);

          
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {

                   case Resource.Id.home:

                    this.Activity.Finish();


                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }



        }


      
        void mSwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Activity.RunOnUiThread(() => { mSwipeRefreshLayout.Refreshing = false; });
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Will run on separate thread
            //  Thread.Sleep(2000);
            myFriends =  FriendsOverview.updateFriendList().Result;
              mAdapter = new UsersFriendsAdapter(myFriends, mRecyclerView, this.Activity, this.Activity, mAdapter, RouteOverview.me);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();
        }

        public override void OnResume()
        {
            base.OnResume();

        }


        public override void OnPause()
        {
            base.OnPause();

        }


        public override void OnDestroy()
        {
            base.OnDestroy();

        }


        public override void OnLowMemory()
        {
            base.OnLowMemory();

        }



        //void sortRating()
        //{
        //    List<Route> orderedRoutes;
        //    orderedRoutes = (from route in routeList
        //                     orderby route.Review
        //                     select route).ToList<Route>();

        //    ////Refresh the listview
        //    //mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
        //    //mListView.Adapter = mAdapter;
        //    mAdapter = new UsersRoutesAdapter(orderedRoutes, mRecyclerView, this.Activity, me);
        //    mRecyclerView.SetAdapter(mAdapter);
        //    mAdapter.NotifyDataSetChanged();


        //}
        //void sortDistance()
        //{
        //    List<Route> orderedRoutes;
        //    orderedRoutes = (from route in routeList
        //                     orderby route.Distance
        //                     select route).ToList<Route>();

        //    ////Refresh the listview
        //    //mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
        //    //mListView.Adapter = mAdapter;
        //    mAdapter = new UsersRoutesAdapter(orderedRoutes, mRecyclerView, this.Activity, me);
        //    mRecyclerView.SetAdapter(mAdapter);
        //    mAdapter.NotifyDataSetChanged();


        //}
        //void sortDifficulty()
        //{
        //    List<Route> orderedRoutes;
        //    orderedRoutes = (from route in routeList
        //                     orderby route.Difficulty
        //                     select route).ToList<Route>();

        //    ////Refresh the listview
        //    //mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
        //    //mListView.Adapter = mAdapter;
        //    mAdapter = new UsersRoutesAdapter(orderedRoutes, mRecyclerView, this.Activity, me);
        //    mRecyclerView.SetAdapter(mAdapter);
        //    mAdapter.NotifyDataSetChanged();


        //}

        //void sortType()
        //{
        //    List<Route> orderedRoutes;
        //    orderedRoutes = (from route in routeList
        //                     orderby route.RouteType
        //                     select route).ToList<Route>();

        //    ////Refresh the listview
        //    //mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
        //    //mListView.Adapter = mAdapter;
        //    mAdapter = new UsersRoutesAdapter(orderedRoutes, mRecyclerView, this.Activity, me);
        //    mRecyclerView.SetAdapter(mAdapter);
        //    mAdapter.NotifyDataSetChanged();


        //}







    }

}