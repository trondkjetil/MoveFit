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
    [Activity(Label = "Friend Requests")]
    public class UserFriendRequest : Activity
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
			SetContentView(Resource.Layout.UsersFriendRequest);

            act = this;

			mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swap);
			mSwipeRefreshLayout.SetColorSchemeColors(Color.Orange, Color.Green, Color.Yellow, Color.Turquoise,Color.Turquoise);
			mSwipeRefreshLayout.Refresh += mSwipeRefreshLayout_Refresh;



            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recycleUserFriendRequest);
            //Create our layout manager
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            List<User> userList = null;

            try
            {
                 userList = await Azure.getFriendRequests(MainStart.userId);

                if (userList.Count == 0 || userList == null)
                {
                    Toast.MakeText(this, "Could not find any friend requests!", ToastLength.Long).Show();
                    Finish();
                }

            }
            catch (Exception)
            {

               
            }
            


            mAdapter = new UsersFriendRequestAdapter(userList, mRecyclerView, this, act,mAdapter);
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



    }

    public class UsersFriendRequestAdapter : RecyclerView.Adapter
    {
        private List<User> mUsers;
        private RecyclerView mRecyclerView;
        private Context mContext;
        private int mCurrentPosition = -1;
        private Activity mActivity;
        private RecyclerView.Adapter mAdapter;


        public UsersFriendRequestAdapter(List<User> users, RecyclerView recyclerView, Context context, Activity act, RecyclerView.Adapter adapter)
        {

            mActivity = act;
            mUsers = users;
            mRecyclerView = recyclerView;
            mContext = context;
            mAdapter = adapter;
        }

        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mUserName { get; set; }
            public TextView mStatus { get; set; }
            public TextView mText { get; set; }
            public ImageView mProfilePicture { get; set; }

            public ImageButton mDeleteFriend { get; set; }

            public ImageButton mAcceptFriend { get; set; }
            public MyView (View view) : base(view)
            {
                mMainView = view;
            }
        }

     
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

                //card view
                View userFriendRequest = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.userFriendsRequestContent, parent, false);

                ImageView profile = userFriendRequest.FindViewById<ImageView>(Resource.Id.profilePicture);
                TextView name = userFriendRequest.FindViewById<TextView>(Resource.Id.nameId);
                TextView status = userFriendRequest.FindViewById<TextView>(Resource.Id.statusId);
                TextView text = userFriendRequest.FindViewById<TextView>(Resource.Id.textId);

            ImageButton rejectFriendReq = userFriendRequest.FindViewById<ImageButton>(Resource.Id.rejectFriend);
            rejectFriendReq.Focusable = false;
            rejectFriendReq.FocusableInTouchMode = false;
            rejectFriendReq.Clickable = true;


            ImageButton acceptFriend = userFriendRequest.FindViewById<ImageButton>(Resource.Id.acceptFriend);
            acceptFriend.Focusable = false;
            acceptFriend.FocusableInTouchMode = false;
            acceptFriend.Clickable = true;


            MyView view = new MyView(userFriendRequest) { mUserName = name, mStatus = status, mText = text, mProfilePicture = profile, mDeleteFriend = rejectFriendReq, mAcceptFriend = acceptFriend };
                return view;
  
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
                Bitmap userImage;
            
                MyView myHolder = holder as MyView;
                myHolder.mMainView.Click += mMainView_Click;
                myHolder.mUserName.Text = mUsers[position].UserName;

                userImage = IOUtilz.GetImageBitmapFromUrl(mUsers[position].ProfilePicture);


            myHolder.mDeleteFriend.SetTag(Resource.Id.rejectFriend, position);
            myHolder.mAcceptFriend.SetTag(Resource.Id.acceptFriend, position);

            myHolder.mDeleteFriend.Click += (sender, args) =>
            {

               
                int pos = (int)(((ImageButton)sender).GetTag(Resource.Id.rejectFriend));

                Toast.MakeText(mContext, mUsers[position].UserName.ToString() + " Rejected", ToastLength.Long).Show();

                var waiting = Azure.setFriendAcceptance(MainStart.userId, mUsers[position].Id,false);

                mUsers.RemoveAt(pos);
                mAdapter = new UsersFriendRequestAdapter(mUsers, mRecyclerView, mActivity, mActivity, mAdapter);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();


            };



            myHolder.mAcceptFriend.Click += (sender, args) =>
            {
                if (mUsers.Count == 0)
                {

                    //Intent myInt = new Intent(mContext, typeof(RouteOverview));
                    //mContext.StartActivity(myInt);

                    mActivity.Finish();
                }

                //    var pos = ((View)sender).Tag;

                int pos = (int)(((ImageButton)sender).GetTag(Resource.Id.acceptFriend));

                Toast.MakeText(mContext, mUsers[position].UserName.ToString() + " Added!", ToastLength.Long).Show();

                var waiting = Azure.setFriendAcceptance(MainStart.userId, mUsers[position].Id, true);

                mUsers.RemoveAt(pos);
                mAdapter = new UsersFriendRequestAdapter(mUsers, mRecyclerView, mActivity, mActivity, mAdapter);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

                //deleteIndex(position);
                //NotifyDataSetChanged();

                if (mUsers.Count == 0)
                {

                    //Intent myInt = new Intent(mContext, typeof(RouteOverview));
                    //mContext.StartActivity(myInt);
                    mActivity.Finish();

                }

            };


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
            Console.WriteLine(mUsers[position].UserName);
        }

        public override int ItemCount
        {
            get { return mUsers.Count; }
        }

        public void Clear()
        {
            mUsers.Clear();
        }


        public bool deleteIndex(int position)
        {


            return mUsers.Remove(mUsers[position]);
        }
    }
}

