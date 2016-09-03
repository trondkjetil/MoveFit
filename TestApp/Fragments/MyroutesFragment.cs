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

    public class MyRoutesFragment : Fragment
    {
        public static RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        SwipeRefreshLayout mSwipeRefreshLayout;
        public SupportToolbar toolbar;
        public List<Route> routeList;
        public static List<User> me;
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.myRoutesRecycleView, container, false);
                 
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclemyRoutes);           
            routeList = RouteOverview.myRoutes;
            me = RouteOverview.me;
           
            if (routeList.Count != 0)
            {
                mLayoutManager = new LinearLayoutManager(this.Activity);
                mRecyclerView.SetLayoutManager(mLayoutManager);

                mAdapter = new UsersRoutesAdapterFragment(routeList, mRecyclerView, this.Activity, RouteOverview.me);
                mRecyclerView.SetAdapter(mAdapter);

            }
            else

            {
                TextView txt = view.FindViewById<TextView>(Resource.Id.empty);

                mRecyclerView.Visibility = ViewStates.Invisible;
                txt.Visibility = ViewStates.Visible;
            }
            return view;

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
            Thread.Sleep(2000);
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