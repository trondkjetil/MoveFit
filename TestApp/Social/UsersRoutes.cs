using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Views.Animations;
using Android.Animation;
using Android.Graphics;
using Android.Support.V4.Widget;
using System.ComponentModel;
using System.Threading;
using Android.Locations;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using System.Linq;
namespace TestApp
{
    [Activity(Label = "Routes Nearby", Theme = "@style/Theme2")]
    public class UsersRoutes : AppCompatActivity
    {

        public SupportToolbar toolbar;
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;

        public List<User> me;
        SwipeRefreshLayout mSwipeRefreshLayout;
        public List<Route> routeList;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           // RequestWindowFeature(WindowFeatures.NoTitle);
            // Set our view from the "main" layout resource
			SetContentView(Resource.Layout.UsersRoutes);

			//mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.userRoutes);
			//mSwipeRefreshLayout.SetColorSchemeColors(Color.Orange, Color.Green, Color.Yellow, Color.Turquoise,Color.Turquoise);
			//mSwipeRefreshLayout.Refresh += mSwipeRefreshLayout_Refresh;

          

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recycleUserRoutes);
            //Create our layout manager
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
        

            toolbar = FindViewById<SupportToolbar>(Resource.Id.tbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            routeList = await Azure.getRoutes();
            me = await Azure.getUserInstanceByName(MainStart.userName);

            if (routeList.Count == 0)
            {
                Toast.MakeText(this, "Could not find any routes!", ToastLength.Long).Show();

                //Intent myInt = new Intent(this, typeof(RouteOverview));
                //StartActivity(myInt);
                Finish();
            }
          
            mAdapter = new UsersRoutesAdapter(routeList, mRecyclerView, this,me);
            mRecyclerView.SetAdapter(mAdapter);



        }


        //void mSwipeRefreshLayout_Refresh(object sender, EventArgs e)
        //{
        //	BackgroundWorker worker = new BackgroundWorker();
        //	worker.DoWork += worker_DoWork;
        //	worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        //	worker.RunWorkerAsync();
        //}

        //void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //	RunOnUiThread(() => { mSwipeRefreshLayout.Refreshing = false; });
        //}

        //void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //	//Will run on separate thread
        //	Thread.Sleep(3000);
        //}

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


                //case Resource.Id.back:
                //    OnBackPressed();
                //    return true;
                case Android.Resource.Id.Home:// Resource.Id.back:
                    OnBackPressed();
                    return true;

                case Resource.Id.home:

                    OnBackPressed();


                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }



        }
        public override bool OnCreateOptionsMenu(IMenu menu)

        {
            MenuInflater.Inflate(Resource.Menu.action_menu_nav_routes, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            //MoveTaskToBack(true);

        }

        void sortRating()
        {
            List<Route> orderedRoutes;
            orderedRoutes = (from route in routeList
                             orderby route.Review
                             select route).ToList<Route>();

            ////Refresh the listview
            //mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
            //mListView.Adapter = mAdapter;
            mAdapter = new UsersRoutesAdapter(orderedRoutes, mRecyclerView, this, me);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();


        }
        void sortDistance()
        {
            List<Route> orderedRoutes;
            orderedRoutes = (from route in routeList
                             orderby route.Distance
                               select route).ToList<Route>();

            ////Refresh the listview
            //mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
            //mListView.Adapter = mAdapter;
            mAdapter = new UsersRoutesAdapter(orderedRoutes, mRecyclerView, this, me);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();


        }
        void sortDifficulty()
        {
            List<Route> orderedRoutes;
            orderedRoutes = (from route in routeList
                             orderby route.Difficulty
                             select route).ToList<Route>();

            ////Refresh the listview
            //mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
            //mListView.Adapter = mAdapter;
            mAdapter = new UsersRoutesAdapter(orderedRoutes, mRecyclerView, this, me);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();


        }

        void sortType()
        {
            List<Route> orderedRoutes;
            orderedRoutes = (from route in routeList
                             orderby route.RouteType
                             select route).ToList<Route>();

            ////Refresh the listview
            //mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
            //mListView.Adapter = mAdapter;
            mAdapter = new UsersRoutesAdapter(orderedRoutes, mRecyclerView, this, me);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();


        }





    }

    public class UsersRoutesAdapter : RecyclerView.Adapter
    {
        private List<Route> mRoutes;
        private RecyclerView mRecyclerView;
        private Context mContext;
        private int mCurrentPosition = -1;
        private List<User> mMyInstance;
        public string routeName;
        public string routeInfo;
        public string routeRating;
        public string routeDifficulty;
        public string routeLength;
        public string routeType;
        public int routeTrips;
        public string routeId;
        public string routeTime;



        public UsersRoutesAdapter(List<Route> routes, RecyclerView recyclerView, Context context, List<User> myInstance )
        {
            mRoutes = routes;
            mRecyclerView = recyclerView;
            mContext = context;
            mMyInstance = myInstance;
        }

        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mRouteName { get; set; }
            public TextView mStatus { get; set; }
            public TextView mRouteInfo { get; set; }
            public TextView mDistanceAway { get; set; }
            public ImageView mIconForRoute { get; set; }
            public ImageButton mStartRouteFlag { get; set; }
            public ImageButton mRouteDifficulty { get; set; }

            public MyView (View view) : base(view)
            {
                mMainView = view;
            }
          
        }

     
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

                //card view
                View userRoutes = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.userRouteContent, parent, false);

                ImageView profile = userRoutes.FindViewById<ImageView>(Resource.Id.profilePicture);
                TextView name = userRoutes.FindViewById<TextView>(Resource.Id.nameId);
                TextView status = userRoutes.FindViewById<TextView>(Resource.Id.statusId);
                TextView text = userRoutes.FindViewById<TextView>(Resource.Id.textId);
                TextView distanceFromMe = userRoutes.FindViewById<TextView>(Resource.Id.distAway);

            ImageButton startRoute = userRoutes.FindViewById<ImageButton>(Resource.Id.startRoute);
                ImageButton routeDifficultyImage = userRoutes.FindViewById<ImageButton>(Resource.Id.imageButton3);
              

            MyView view = new MyView(userRoutes) { mRouteName = name, mStatus = status, mRouteInfo = text, mIconForRoute = profile, mStartRouteFlag = startRoute, mRouteDifficulty = routeDifficultyImage,mDistanceAway = distanceFromMe};

            return view;
  
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {


            routeName = "";
            routeInfo = "";
            routeRating = "";
            routeDifficulty = "";
            routeLength = "";
            routeType = "";
            routeTrips = 1;
            routeId = "";
            routeTime = "";

            MyView myHolder = holder as MyView;
                myHolder.mMainView.Click += mMainView_Click;
                myHolder.mRouteName.Text = mRoutes[position].Name;
              //  myHolder.mStartRouteFlag.Click += StartRouteFlag_Click;
                myHolder.mRouteInfo.Text = "Length " + mRoutes[position].Distance + " meters";
                myHolder.mStatus.Text = mRoutes[position].RouteType;


            // Calculate distance to User
            float[] result = null;
            result = new float[1];
            Location.DistanceBetween(mRoutes[position].Lat, mRoutes[position].Lon, mMyInstance[0].Lat, mMyInstance[0].Lon, result);

            var res = Convert.ToInt32(result[0]);


            string unit = " km";
            double dist = 0;
            var test = IOUtilz.LoadPreferences();
            if (test[1] == 1)
            {
                unit = " miles";
                dist =(int) IOUtilz.ConvertKilometersToMiles(res / 1000);
            }
            else
            {
                dist = res/ 1000;
            }


            myHolder.mDistanceAway.Text = dist.ToString() + unit + " away";


            //    myHolder.mDistanceAway.Text = res.ToString() + " meters away";


            if (mRoutes[position].Difficulty == "Easy")
            {
                myHolder.mRouteDifficulty.SetImageResource(Resource.Drawable.green);
            }
            else if(mRoutes[position].Difficulty == "Medium")
            {
                myHolder.mRouteDifficulty.SetImageResource(Resource.Drawable.orange);

            }
            else if (mRoutes[position].Difficulty == "Hard")
            {
                myHolder.mRouteDifficulty.SetImageResource(Resource.Drawable.red);

            }


                myHolder.mIconForRoute.SetImageResource(Resource.Drawable.maps);
          
            if (position > mCurrentPosition)
            {
                int currentAnim = Resource.Animation.slide_left_to_right;
                SetAnimation(myHolder.mMainView, currentAnim);
                mCurrentPosition = position;
            }

        }

       

        private void SetAnimation(View view, int currentAnim)
        {
            Animator animator = AnimatorInflater.LoadAnimator(mContext, Resource.Animation.flip);
            animator.SetTarget(view);
           
            animator.Start();
            

             //Animation anim = AnimationUtils.LoadAnimation(mContext, currentAnim);
             //view.StartAnimation(anim);
        }

        void mMainView_Click(object sender, EventArgs e)
        {
        
          
            try
            {

            int position = mRecyclerView.GetChildAdapterPosition((View)sender);
             
            routeName = mRoutes[position].Name;
            routeInfo = mRoutes[position].Info;
            routeDifficulty = mRoutes[position].Difficulty;
            routeLength = mRoutes[position].Distance;
            routeType = mRoutes[position].RouteType;
            routeRating = mRoutes[position].Review;
            routeTrips = mRoutes[position].Trips;
            routeId = mRoutes[position].Id;
            routeTime = mRoutes[position].Time;
          



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
            routeTime

        });

            Intent myIntent = new Intent(mContext, typeof(StartRoute));
            myIntent.PutExtras(b);
            mContext.StartActivity(myIntent);

            }
            catch (Exception )
            {

              
            }


        }

        public override int ItemCount
        {
            get { return mRoutes.Count; }
        }






        }





    }

