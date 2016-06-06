
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

namespace TestApp
{
    [Activity(Label = "MyRoutes")]
    public class UserMyRoutes : Activity
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private string UserID;
        List<Route> routeList;

        SwipeRefreshLayout mSwipeRefreshLayout;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.UsersMyRoutes);

            //mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swp);
            //mSwipeRefreshLayout.SetColorSchemeColors(Color.Orange, Color.Green, Color.Yellow, Color.Turquoise, Color.Turquoise);
            //mSwipeRefreshLayout.Refresh += mSwipeRefreshLayout_Refresh;


            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recycleUserMyRoutes);
            //Create our layout manager
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            UserID = "";


            List<User> user = await Azure.getUserId(MainStart.userName);
            UserID = user[0].Id;
          
            routeList = await Azure.getMyRoutes(UserID);
            if (routeList.Count != 0)
            {
                mAdapter = new MyRoutesAdapter(routeList, mRecyclerView, this);
                mRecyclerView.SetAdapter(mAdapter);
            }
            else { 
                Toast.MakeText(this, "Could not find any routes!", ToastLength.Long).Show();
                
                Intent myInt = new Intent(this, typeof(RouteOverview));
                StartActivity(myInt);
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
            RunOnUiThread(() => { mSwipeRefreshLayout.Refreshing = false; });
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Will run on separate thread
            Thread.Sleep(2000);
        }


        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.actionbar_cardview, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    switch(item.ItemId)
        //    {
        //        case Resource.Id.add:
        //            //Add button clicked
        //          //  users.Add(new Email() { Name = "New Name", Subject = "New Subject", Message = "New Message" });
        //            mAdapter.NotifyItemInserted(users.Count - 1);
        //            return true;
        //    }
        //    return base.OnOptionsItemSelected(item);
        //}




    }

    public class MyRoutesAdapter : RecyclerView.Adapter
    {
        private List<Route> mMyRoutes;
        private RecyclerView mRecyclerView;
        private Context mContext;
        private int mCurrentPosition = -1;

        public MyRoutesAdapter(List<Route> routes, RecyclerView recyclerView, Context context)
        {
            mMyRoutes = routes;
            mRecyclerView = recyclerView;
            mContext = context;

        }

        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mUserName { get; set; }
            public TextView mStatus { get; set; }
            public TextView mText { get; set; }
            public TextView mReview { get; set; }
            public ImageView mProfilePicture { get; set; }
            public ImageButton mStartRouteFlag { get; set; }

            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //card view
            View userMyRoutes = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.userMyRoutesContent, parent, false);

            ImageButton startRoute = userMyRoutes.FindViewById<ImageButton>(Resource.Id.startRoute);
            ImageView routeIcon = userMyRoutes.FindViewById<ImageView>(Resource.Id.profilePicture);
            TextView routeName = userMyRoutes.FindViewById<TextView>(Resource.Id.nameId);
            TextView status = userMyRoutes.FindViewById<TextView>(Resource.Id.statusId);
            TextView routeInfo = userMyRoutes.FindViewById<TextView>(Resource.Id.textId);
            TextView review = userMyRoutes.FindViewById<TextView>(Resource.Id.txtTime);


            MyView view = new MyView(userMyRoutes) { mUserName = routeName, mStatus = status, mText = routeInfo, mProfilePicture = routeIcon, mStartRouteFlag = startRoute, mReview = review };

            return view;

        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {

            // First view
            Bitmap userImage;
            MyView myHolder = holder as MyView;
            myHolder.mMainView.Click += mMainView_Click;
            myHolder.mUserName.Text = mMyRoutes[position].Name;
            myHolder.mStartRouteFlag.Click += StartRouteFlag_Click;
            myHolder.mText.Text = mMyRoutes[position].Info;
            myHolder.mStatus.Text = mMyRoutes[position].RouteType;
            myHolder.mReview.Text = mMyRoutes[position].Review;

            userImage = null;
            if (userImage == null)
            {
                myHolder.mProfilePicture.SetImageResource(Resource.Drawable.maps);
            }
            else
                myHolder.mProfilePicture.SetImageBitmap(userImage);

            if (position > mCurrentPosition)
            {
                int currentAnim = Resource.Animation.slide_left_to_right;
                SetAnimation(myHolder.mMainView, currentAnim);
                mCurrentPosition = position;
            }



        }

        private void StartRouteFlag_Click(object sender, EventArgs e)
        {


            string test = "test";

            //test = myHolder.mUserName.Text;
            Bundle b = new Bundle();
            b.PutStringArray("MyData", new String[] {
            test

                 });

            Intent myIntent = new Intent(mContext, typeof(StartRoute));
            myIntent.PutExtras(b);
            mContext.StartActivity(myIntent);

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
            int position = mRecyclerView.GetChildPosition((View)sender);

        }

        public override int ItemCount
        {
            get { return mMyRoutes.Count; }
        }
    }
}

