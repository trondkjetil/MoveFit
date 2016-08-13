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
using System.Threading.Tasks;

namespace TestApp
{
    [Activity(Label = "People Nearby")]
    public class UsersNearby : Activity
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        public RecyclerView.Adapter mAdapter;
        public static Activity act;
       

        SwipeRefreshLayout mSwipeRefreshLayout;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        //    RequestWindowFeature(WindowFeatures.NoTitle);
            // Set our view from the "main" layout resource
			SetContentView(Resource.Layout.UsersNearby);

			mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swp);
			mSwipeRefreshLayout.SetColorSchemeColors(Color.Orange, Color.Green, Color.Yellow, Color.Turquoise,Color.Turquoise);
			mSwipeRefreshLayout.Refresh += mSwipeRefreshLayout_Refresh;
            act = this;


            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recycleUserNearby);
            //Create our layout manager
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            List<User> userList = await Azure.nearbyPeople(); // getPeople();
            if (userList.Count == 0)
            {
                Toast.MakeText(this, "Could not find anyone nearby!", ToastLength.Long).Show();
                Finish();
            }
           

            mAdapter = new UsersAdapter(userList, mRecyclerView, this,act, mAdapter);
            mRecyclerView.SetAdapter(mAdapter);

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
			Thread.Sleep(3000);
		}

        public override void OnBackPressed()
        {
            Finish();
        }
    }

    public class UsersAdapter : RecyclerView.Adapter
    {
        private List<User> mUsers;
        private RecyclerView mRecyclerView;
        private Context mContext;
        private int mCurrentPosition = -1;
        private Activity mActivity;
        private RecyclerView.Adapter mAdapter;

        public UsersAdapter(List<User> users, RecyclerView recyclerView, Context context, Activity act, RecyclerView.Adapter adapter)
        {
            mUsers = users;
            mRecyclerView = recyclerView;
            mContext = context;
            mActivity = act;
            mAdapter = adapter;
        }

        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mUserName { get; set; }
            public TextView mStatus { get; set; }
            public TextView mText { get; set; }
            public ImageView mProfilePicture { get; set; }
            public ImageButton mSendFriendRequest { get; set; }
            public ImageButton mGender { get; set; }

            public MyView (View view) : base(view)
            {
                mMainView = view;
            }
        }

     
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

                //card view
                View peopleNearbyContent = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.peopleNearByContent, parent, false);

                ImageView profile = peopleNearbyContent.FindViewById<ImageView>(Resource.Id.profilePicture);
                TextView name = peopleNearbyContent.FindViewById<TextView>(Resource.Id.nameId);
                TextView status = peopleNearbyContent.FindViewById<TextView>(Resource.Id.statusId);
                TextView text = peopleNearbyContent.FindViewById<TextView>(Resource.Id.textId);
                ImageButton addToFriends = peopleNearbyContent.FindViewById<ImageButton>(Resource.Id.sendFriendRequest);
                addToFriends.Focusable = false;
                addToFriends.FocusableInTouchMode = false;
                addToFriends.Clickable = true;
                ImageButton gender = peopleNearbyContent.FindViewById<ImageButton>(Resource.Id.gender);


              //  addToFriends.Click += MSendFriendRequest_Click;

            MyView view = new MyView(peopleNearbyContent) { mUserName = name, mStatus = status, mText = text, mProfilePicture = profile, mSendFriendRequest = addToFriends , mGender = gender};

            return view;
  
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {


            Bitmap userImage;
            MyView myHolder = holder as MyView;
            myHolder.mMainView.Click += mMainView_Click;
            myHolder.mUserName.Text = mUsers[position].UserName;

            myHolder.mSendFriendRequest.SetTag(Resource.Id.sendFriendRequest, position);

            myHolder.mSendFriendRequest.Click += MSendFriendRequest_Click;


            if (mUsers[position].Sex == "Male")
            {
                myHolder.mGender.SetImageResource(Resource.Drawable.male);
            }
            else {
                myHolder.mGender.SetImageResource(Resource.Drawable.female);
            }

            userImage = IOUtilz.GetImageBitmapFromUrl(mUsers[position].ProfilePicture);

            if (mUsers[position].Online)
            {
                myHolder.mStatus.Text = "Online";
            }
            else
            {
                myHolder.mStatus.Text = "Offline";
            }


                myHolder.mText.Text = "Age " + mUsers[position].Age;


            if (userImage == null)
            {
                myHolder.mProfilePicture.SetImageResource(Resource.Drawable.tt);
            }
            else
                myHolder.mProfilePicture.SetImageBitmap(userImage);

            if (position > mCurrentPosition)
            {
                int currentAnim = Resource.Animation.slide_left_to_right;
                SetAnimation(myHolder.mMainView, currentAnim);
                mCurrentPosition = position;
            }


            //myHolder.mSendFriendRequest.Click += (object sender, EventArgs e) =>
            //{

            //    try
            //    {


            //        int pos = (int)(((ImageButton)sender).GetTag(Resource.Id.sendFriendRequest));

            //        Toast.MakeText(mContext, "Friend request is sent to " + mUsers[pos].UserName.ToString(), ToastLength.Long).Show();

            //        Azure.AddFriendShip(MainStart.userId, mUsers[pos].Id);
            //        deleteIndex(pos);
            //        NotifyDataSetChanged();

            //        //   mAdapter.NotifyDataSetChanged();



            //        if (mUsers.Count == 0)
            //        {

            //            mActivity.Finish();
            //        }


            //    }
            //    catch (Exception)
            //    {


            //    }



            //};



            }



       void  MSendFriendRequest_Click(object sender, EventArgs e)
        {

        


            try
            {

              
                int pos = (int)(((ImageButton)sender).GetTag(Resource.Id.sendFriendRequest));

                Toast.MakeText(mContext, "Friend request is sent to " + mUsers[pos].UserName.ToString(), ToastLength.Long).Show();

                Azure.AddFriendShip(MainStart.userId, mUsers[pos].Id);
                mUsers.RemoveAt(pos);
                //deleteIndex(pos);
                //NotifyDataSetChanged();

                //   mAdapter.NotifyDataSetChanged();
                mAdapter = new UsersAdapter(mUsers, mRecyclerView, mActivity, mActivity, mAdapter);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();


                if (mUsers.Count == 0)
                {

                    mActivity.Finish();
                }


            }
            catch (Exception)
            {

               
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
            //   int position = mRecyclerView.GetChildPosition((View)sender);
            int position = mRecyclerView.GetChildAdapterPosition((View)sender);
            Toast.MakeText(mContext, "Position " + position.ToString(), ToastLength.Long).Show();



        }
        public bool deleteIndex(int position)
        {
            return mUsers.Remove(mUsers[position]);
        }
        public override int ItemCount
        {
            get { return mUsers.Count; }
        }
    }
}

