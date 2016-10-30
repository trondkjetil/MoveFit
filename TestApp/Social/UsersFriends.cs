using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Animation;
using Android.Graphics;
using Android.Support.V4.Widget;
using System.ComponentModel;
using System.Threading;
using Android.Locations;

namespace TestApp
{
    [Activity(Label = "Friends", Theme = "@style/Theme2")]
    public class UsersFriends : Activity
    {

        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        public RecyclerView.Adapter mAdapter;
        public Activity act;
        public List<User> me;
        SwipeRefreshLayout mSwipeRefreshLayout;
        List<User> userList;
        protected override void OnResume()
        {
            base.OnResume();
           
        }
        protected override void OnStop()
        {
            base.OnStop();
            // Recreate();
        }

        protected async override void OnStart()
        {
            base.OnStart();

           // mAdapter = null;
          //if(mAdapter != null && mAdapter.ItemCount > 0 )
          //  {
          //      mAdapter.UnregisterFromRuntime();
          //      mAdapter.Dispose();
          //  }
          //  mAdapter.

            userList = await Azure.getUsersFriends(MainStart.userId);

            if(me == null)
            {
                me = new List<User>();

                me.Add(MainStart.userInstanceOne);

            }
            if (mAdapter != null)
                mAdapter.Dispose();

            mAdapter = new UserMessageFriendsAdapter(userList, mRecyclerView, this, this, mAdapter, me);
            mRecyclerView.SetAdapter(mAdapter);
          // mAdapter.NotifyDataSetChanged();
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Recreate();
        }
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UsersFriendsMessages);

            act = this;

            //mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.userFriends);
            //mSwipeRefreshLayout.SetColorSchemeColors(Color.Orange, Color.Green, Color.Yellow, Color.Turquoise, Color.Turquoise);
            //mSwipeRefreshLayout.Refresh += mSwipeRefreshLayout_Refresh;

      

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recycleUserFriends);
  
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            //List<User> userList = await Azure.getPeople();

            //userList = await Azure.getUsersFriends(MainStart.userId);
            //me = await Azure.getUserByAuthId(MainStart.userId);
            userList = await Azure.getUsersFriends(MainStart.userId);
            me = await Azure.getUserByAuthId(MainStart.userId);

            if (userList.Count == 0)
            {
                Toast.MakeText(this, "Your friendlist is empty!", ToastLength.Short).Show();

                //Intent myInt = new Intent(this, typeof(RouteOverview));
                //StartActivity(myInt);
                Finish();
            }

            //userList = await Azure.getUsersFriends(MainStart.userId);
            //me = await Azure.getUserByAuthId(MainStart.userId);



            //mAdapter = new UserMessageFriendsAdapter(userList, mRecyclerView, this, this, mAdapter, me);
            //mRecyclerView.SetAdapter(mAdapter);
           

        }


        void mSwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            //BackgroundWorker worker = new BackgroundWorker();
            //worker.DoWork += worker_DoWork;
            //worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            //worker.RunWorkerAsync();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //RunOnUiThread(() => { mSwipeRefreshLayout.Refreshing = false; });
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Will run on separate thread
          //  Thread.Sleep(3000);
        }

        public override void OnBackPressed()
        {
            Finish();
        }
    }




    

}

