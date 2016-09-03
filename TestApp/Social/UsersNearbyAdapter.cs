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

    public class UsersNearbyAdapter : RecyclerView.Adapter
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

        public UsersNearbyAdapter(List<User> users, RecyclerView recyclerView, Context context, Activity act, RecyclerView.Adapter adapter)
        {
            mUsers = users;
            mRecyclerView = recyclerView;
            mContext = context;
            mActivity = act;
            mAdapter = adapter;


            if (mUsers.Count == 0 && FriendsOverview.viewPager.CurrentItem == 1)
            {

                TextView txt = act.FindViewById<TextView>(Resource.Id.empty);
                mRecyclerView.Visibility = ViewStates.Invisible;
                txt.Visibility = ViewStates.Visible;
            }
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

            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //card view
            View usersNearby = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.peopleNearByContent, parent, false);

            TextView name = usersNearby.FindViewById<TextView>(Resource.Id.nameId);
            TextView status = usersNearby.FindViewById<TextView>(Resource.Id.statusId);
            TextView text = usersNearby.FindViewById<TextView>(Resource.Id.textId);

            ImageView profile = usersNearby.FindViewById<ImageView>(Resource.Id.ppt);

            ImageButton addToFriends = usersNearby.FindViewById<ImageButton>(Resource.Id.sendFriendRequest);
            addToFriends.Focusable = false;
            addToFriends.FocusableInTouchMode = false;
            addToFriends.Clickable = true;
            ImageButton gender = usersNearby.FindViewById<ImageButton>(Resource.Id.gender);


            //  addToFriends.Click += MSendFriendRequest_Click;

            MyView view = new MyView(usersNearby) { mUserName = name, mStatus = status, mText = text, mProfilePicture = profile, mSendFriendRequest = addToFriends, mGender = gender };

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
            else
            {
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



        void MSendFriendRequest_Click(object sender, EventArgs e)
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
                mAdapter = new UsersNearbyAdapter(mUsers, mRecyclerView, mActivity, mActivity, mAdapter);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();


                if (mUsers.Count == 0)
                {
                    mAdapter = new UsersNearbyAdapter(mUsers, mRecyclerView, mActivity, mActivity, mAdapter);
                    mRecyclerView.SetAdapter(mAdapter);
                    mAdapter.NotifyDataSetChanged();
                    //mActivity.Finish();
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
            // int position = mRecyclerView.GetChildAdapterPosition((View)sender);

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

                Intent myIntent = new Intent(mContext, typeof(UserProfile));
                myIntent.PutExtras(b);
                mContext.StartActivity(myIntent);


            }
            catch (Exception)
            {


            }


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