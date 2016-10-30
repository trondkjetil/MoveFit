using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Graphics;
using Android.Locations;
using Android.Animation;

namespace TestApp
{
    public class UsersFriendRequestAdapter : RecyclerView.Adapter
    {
        private List<User> mUsers;
        private RecyclerView mRecyclerView;
        private Context mContext;
        private int mCurrentPosition = -1;
        private Activity mActivity;
        private RecyclerView.Adapter mAdapter;
        public string userName;
        public string userGender;
        public int userAge;
        public string userProfileImage;
        public int userPoints;
        public string userAboutMe;
        public string userID;
        TextView txt;
        public static bool check;
        public UsersFriendRequestAdapter(List<User> users, RecyclerView recyclerView, Context context, Activity act, RecyclerView.Adapter adapter)
        {
            mActivity = act;
            mUsers = users;           
            mRecyclerView = recyclerView;
            mContext = context;
            mAdapter = adapter;

            check = false;
            //txt = act.FindViewById<TextView>(Resource.Id.empty);
            //if (mUsers.Count == 0 && FriendsOverview.viewPager.CurrentItem == 3)
            //{                          
            //    mRecyclerView.Visibility = ViewStates.Invisible;
            //    txt.Visibility = ViewStates.Visible;
            //}
            //else
            //{
            //    txt.Visibility = ViewStates.Gone;
            //}

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
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
                      
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
                var waiting = Azure.setFriendAcceptance(MainStart.userId, mUsers[position].Id, false);
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
                    // mActivity.Finish();
                    txt = mActivity.FindViewById<TextView>(Resource.Id.emptyRequest);
                    mRecyclerView.Visibility = ViewStates.Invisible;
                    txt.Visibility = ViewStates.Visible;
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
                    //  mActivity.Finish();
                    txt = mActivity.FindViewById<TextView>(Resource.Id.emptyRequest);
                    mRecyclerView.Visibility = ViewStates.Invisible;
                    txt.Visibility = ViewStates.Visible;

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
            //  int position = mRecyclerView.GetChildAdapterPosition((View)sender);
            if (!check)
            {

           
            try
            {

                int position = mRecyclerView.GetChildAdapterPosition((View)sender);

                userName = mUsers[position].UserName;
                userGender = mUsers[position].Sex;
                userAge = mUsers[position].Age;
                userProfileImage = mUsers[position].ProfilePicture;
                userPoints = mUsers[position].Points;
                userAboutMe = mUsers[position].AboutMe;
                userID = mUsers[position].Id;

                Bundle b = new Bundle();
                b.PutStringArray("MyData", new String[] {

                userName,
                userGender,
                userAge.ToString(),
                userProfileImage,
                userPoints.ToString(),
                userAboutMe,
                userID

            });

                    check = true;
                Intent myIntent = new Intent(mContext, typeof(UserProfile));
                myIntent.PutExtras(b);
                mContext.StartActivity(myIntent);


            }
            catch (Exception)
            {


            }
            }
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