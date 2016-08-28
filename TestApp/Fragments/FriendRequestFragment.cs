using System.Linq;
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

    public class FriendRequestFragment : Fragment
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

            myFriends = FriendsOverview.friendRequests;
            me = FriendsOverview.me;
            if (myFriends.Count == 0)
            {
                IOUtilz.notFound(this.Activity);
            }
            else
            {
                mAdapter = new UsersFriendRequestAdapter(myFriends, mRecyclerView, this.Activity, this.Activity, mAdapter);
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
            inflater.Inflate(Resource.Menu.action_menu_nav_people, menu);

            base.OnCreateOptionsMenu(menu, inflater);
 
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
                case Resource.Id.home:

                    this.Activity.Finish();


                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }



        }

        void showAll()
        {
            List<User> orderedRoutes;
            orderedRoutes = (from user in myFriends
                             orderby user.Points
                             select user).ToList<User>();
            mAdapter = new UsersFriendRequestAdapter(orderedRoutes, mRecyclerView, this.Activity, this.Activity, mAdapter);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();

        }
        void showOnline()
        {
            List<User> orderedRoutes;
            orderedRoutes = (from user in myFriends
                             where user.Online == true
                             select user).ToList<User>();

            mAdapter = new UsersFriendRequestAdapter(orderedRoutes, mRecyclerView, this.Activity, this.Activity, mAdapter);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();


        }




        void showMale()
        {
            List<User> orderedRoutes;
            orderedRoutes = (from user in myFriends
                             where user.Sex != "Female"
                             orderby user.Sex
                             select user).ToList<User>();

            mAdapter = new UsersFriendRequestAdapter(orderedRoutes, mRecyclerView, this.Activity, this.Activity, mAdapter);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();



        }
        void showFemale()
        {
            List<User> orderedRoutes;
            orderedRoutes = (from user in myFriends
                             where user.Sex != "Male"
                             orderby user.Sex
                             select user).ToList<User>();

            mAdapter = new UsersFriendRequestAdapter(orderedRoutes, mRecyclerView, this.Activity, this.Activity, mAdapter);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();


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


    }

}