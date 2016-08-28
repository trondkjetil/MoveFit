//using System;
//using Android.App;
//using Android.Content;
//using Android.Views;
//using Android.Widget;
//using Android.OS;
//using Android.Support.V7.Widget;
//using System.Collections.Generic;
//using Android.Animation;
//using Android.Support.V4.Widget;
//using System.ComponentModel;
//using System.Threading;
//using Android.Net;
//using Android.Graphics;

//namespace TestApp
//{
//    [Activity(Label = "MyRoutes")]
//    public class UserMyRoutes : Activity
//    {
//        private RecyclerView mRecyclerView;
//        private RecyclerView.LayoutManager mLayoutManager;
//        private RecyclerView.Adapter mAdapter;
//        SwipeRefreshLayout mSwipeRefreshLayout;

    
//        protected async override void OnCreate(Bundle savedInstanceState)
//        {
//            base.OnCreate(savedInstanceState);
//            RequestWindowFeature(WindowFeatures.NoTitle);
//            // Set our view from the "main" layout resource
//            SetContentView(Resource.Layout.UsersMyRoutes);

//            mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.userRoutes);
//            mSwipeRefreshLayout.SetColorSchemeColors(Color.Orange, Color.Green, Color.Yellow, Color.Turquoise, Color.Turquoise);
//            mSwipeRefreshLayout.Refresh += mSwipeRefreshLayout_Refresh;


//            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recycleUserMyRoutes);
//            mLayoutManager = new LinearLayoutManager(this);
//            mRecyclerView.SetLayoutManager(mLayoutManager);

//            //connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

//            //List<User> user = await Azure.getUserId(MainStart.userName);

//           // UserID = MainStart.userId;

//            var  routeList = await Azure.getMyRoutes(MainStart.userId);
//                if (routeList.Count != 0)
//                {
//                    mAdapter = new MyRoutesAdapter(routeList, mRecyclerView, this);
//                    mRecyclerView.SetAdapter(mAdapter);
//                }
//                else 
//                {
//                    Toast.MakeText(this, "Could not find any routes!", ToastLength.Long).Show();

//                    //Intent myInt = new Intent(this, typeof(RouteOverview));
//                    //StartActivity(myInt);
//                    Finish();
//                }

            
  

//        }


//        void mSwipeRefreshLayout_Refresh(object sender, EventArgs e)
//        {
//            BackgroundWorker worker = new BackgroundWorker();
//            worker.DoWork += worker_DoWork;
//            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
//            worker.RunWorkerAsync();
//        }

//        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
//        {
//            RunOnUiThread(() => { mSwipeRefreshLayout.Refreshing = false; });
//        }

//        void worker_DoWork(object sender, DoWorkEventArgs e)
//        {
//            //Will run on separate thread
//            Thread.Sleep(2000);
//        }
//        public override void OnBackPressed()
//        {
//            Finish();
//        }

//    }

//    public class MyRoutesAdapter : RecyclerView.Adapter
//    {
//        private List<Route> mMyRoutes;
//        private RecyclerView mRecyclerView;
//        private Context mContext;
//        private int mCurrentPosition = -1;
//        public string routeName;
//        public string routeInfo;
//        public string routeRating;
//        public string routeDifficulty;
//        public string routeLength;
//        public string routeType;
//        public int routeTrips;
//        string routeTime;
//        public string routeId;
//        public string routeUserId;

//        public MyRoutesAdapter(List<Route> routes, RecyclerView recyclerView, Context context)
//        {
//            mMyRoutes = routes;
//            mRecyclerView = recyclerView;
//            mContext = context;

//        }
//        public class MyView : RecyclerView.ViewHolder
//        {
//            public View mMainView { get; set; }
//            public TextView mRouteName { get; set; }
//            public TextView mStatus { get; set; }
//            public TextView mRouteInfo { get; set; }
          
//            public ImageView mIconForRoute { get; set; }
//            public ImageButton mStartRouteFlag { get; set; }
//            public ImageButton mRouteDifficulty { get; set; }

//            public MyView(View view) : base(view)
//            {
//                mMainView = view;
//            }

//        }


//        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
//        {

//            //card view
//            View userRoutes = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.userMyRoutesContent, parent, false);

//            ImageView profile = userRoutes.FindViewById<ImageView>(Resource.Id.profilePicture);
//            TextView name = userRoutes.FindViewById<TextView>(Resource.Id.nameId);
//            TextView status = userRoutes.FindViewById<TextView>(Resource.Id.statusId);
//            TextView text = userRoutes.FindViewById<TextView>(Resource.Id.textId);
//            ImageButton startRoute = userRoutes.FindViewById<ImageButton>(Resource.Id.startRoute);
//            ImageButton routeDifficultyImage = userRoutes.FindViewById<ImageButton>(Resource.Id.imageButton3);


//            MyView view = new MyView(userRoutes) { mRouteName = name, mStatus = status, mRouteInfo = text, mIconForRoute = profile, mStartRouteFlag = startRoute, mRouteDifficulty = routeDifficultyImage };

//            return view;

//        }

//        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
//        {

//            routeName = "";
//            routeInfo = "";
//            routeRating = "";
//            routeDifficulty = "";
//            routeLength = "";
//            routeType = "";
//            routeTrips = 1;
//            routeId = "";
//            routeTime = "";
//            routeUserId = "";

//            MyView myHolder = holder as MyView;
//            myHolder.mMainView.Click += mMainView_Click;
//            myHolder.mRouteName.Text = mMyRoutes[position].Name;
//            //  myHolder.mStartRouteFlag.Click += StartRouteFlag_Click;
//            myHolder.mRouteInfo.Text = "Distance " + mMyRoutes[position].Distance + " meters";
//            myHolder.mStatus.Text = mMyRoutes[position].RouteType;

//            if (mMyRoutes[position].Difficulty == "Easy")
//            {
//                myHolder.mRouteDifficulty.SetImageResource(Resource.Drawable.green);
//            }
//            else if (mMyRoutes[position].Difficulty == "Medium")
//            {
//                myHolder.mRouteDifficulty.SetImageResource(Resource.Drawable.orange);

//            }
//            else if (mMyRoutes[position].Difficulty == "Hard")
//            {
//                myHolder.mRouteDifficulty.SetImageResource(Resource.Drawable.red);

//            }



//            myHolder.mIconForRoute.SetImageResource(Resource.Drawable.maps);

//            if (position > mCurrentPosition)
//            {
//                int currentAnim = Resource.Animation.slide_left_to_right;
//                SetAnimation(myHolder.mMainView, currentAnim);
//                mCurrentPosition = position;
//            }

//        }

    
//        private void SetAnimation(View view, int currentAnim)
//        {
//            Animator animator = AnimatorInflater.LoadAnimator(mContext, Resource.Animation.flip);
//            animator.SetTarget(view);
//            animator.Start();
//            //Animation anim = AnimationUtils.LoadAnimation(mContext, currentAnim);
//            //view.StartAnimation(anim);
//        }

//        void mMainView_Click(object sender, EventArgs e)
//        {
//            //   int position = mRecyclerView.GetChildPosition((View)sender);

//            try
//            {

//                int position = mRecyclerView.GetChildAdapterPosition((View)sender);

//                routeName = mMyRoutes[position].Name;
//                routeInfo = mMyRoutes[position].Info;
//                routeDifficulty = mMyRoutes[position].Difficulty;
//                routeLength = mMyRoutes[position].Distance;
//                routeType = mMyRoutes[position].RouteType;
//                routeRating = mMyRoutes[position].Review;
//                routeTrips = mMyRoutes[position].Trips;
//                routeId = mMyRoutes[position].Id;
//                routeTime = mMyRoutes[position].Time;
//                routeUserId = mMyRoutes[position].User_id;

//                Bundle b = new Bundle();
//                b.PutStringArray("MyData", new String[] {

//            routeName,
//            routeInfo,
//            routeDifficulty,
//            routeLength,
//            routeType,
//            routeRating,
//            routeTrips.ToString(),
//            routeId,
//            routeTime,
//            routeUserId


//        });

//                Intent myIntent = new Intent(mContext, typeof(StartRoute));
//                myIntent.PutExtras(b);
//                mContext.StartActivity(myIntent);

//                // Toast.MakeText(mContext, "" + position.ToString() + " " +routeId + " RouteINFo: "+ routeName, ToastLength.Long).Show();

//            }
//            catch (Exception)
//            {


//            }


//        }

//        public override int ItemCount
//        {
//            get { return mMyRoutes.Count; }
//        }
//    }





//}

