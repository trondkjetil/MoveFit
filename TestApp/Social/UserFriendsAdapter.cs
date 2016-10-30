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
    public class UsersFriendsAdapter : RecyclerView.Adapter
    {
        private List<User> mUsers;
        private RecyclerView mRecyclerView;
        private Context mContext;
        private int mCurrentPosition = -1;

        public string userName;
        public string userGender;
        public int userAge;
        public string userProfileImage;
        public int userPoints;
        public string userAboutMe;
        public string userID;
        private RecyclerView.Adapter mAdapter;
        private Activity mActivity;
        List<User> mMyInstance;
        TextView txt;

        public UsersFriendsAdapter(List<User> users, RecyclerView recyclerView, Context context, Activity act, RecyclerView.Adapter adapter, List<User> me)
        {
            mUsers = users;
            mRecyclerView = recyclerView;
            mContext = context;
            mActivity = act;
            mAdapter = adapter;
            mMyInstance = me;

        

        }

        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mUserName { get; set; }
            public TextView mStatus { get; set; }
            public TextView mText { get; set; }
            public TextView mDist { get; set; }
            public ImageView mProfilePicture { get; set; }

            public ImageButton mDeleteFriend { get; set; }
            public ImageButton mSendMessage { get; set; }

            public ImageButton mOnlineStatus { get; set; }
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //card view
            View userFriendsContent = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.userFriendsContent, parent, false);

            ImageView profile = userFriendsContent.FindViewById<ImageView>(Resource.Id.profilePicture);
            TextView name = userFriendsContent.FindViewById<TextView>(Resource.Id.nameId);
            TextView status = userFriendsContent.FindViewById<TextView>(Resource.Id.statusId);
            TextView text = userFriendsContent.FindViewById<TextView>(Resource.Id.textId);
            TextView distanceFromMe = userFriendsContent.FindViewById<TextView>(Resource.Id.distAway);


            ImageButton online = userFriendsContent.FindViewById<ImageButton>(Resource.Id.onlineStatus);
            ImageButton deleteFriend = userFriendsContent.FindViewById<ImageButton>(Resource.Id.imageButton3);
            deleteFriend.Focusable = false;
            deleteFriend.FocusableInTouchMode = false;
            deleteFriend.Clickable = true;

            ImageButton sendMessage = userFriendsContent.FindViewById<ImageButton>(Resource.Id.sendMsg);
            deleteFriend.Focusable = false;
            deleteFriend.FocusableInTouchMode = false;
            deleteFriend.Clickable = true;




            MyView view = new MyView(userFriendsContent) { mUserName = name, mStatus = status, mText = text, mProfilePicture = profile, mDeleteFriend = deleteFriend, mSendMessage = sendMessage, mOnlineStatus = online, mDist = distanceFromMe };
            return view;

        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Bitmap userImage;
            //First view
            MyView myHolder = holder as MyView;
            myHolder.mMainView.Click += mMainView_Click;
            myHolder.mUserName.Text = mUsers[position].UserName;

       //    myHolder.mMainView.SetTag(myHolder.ItemViewType, position); 
           myHolder.mDeleteFriend.SetTag(Resource.Id.imageButton3, position);


            float[] result = null;
            // Calculate distance to User
            result = new float[1];

            try
            {

          
            Location.DistanceBetween(mUsers[position].Lat, mUsers[position].Lon, mMyInstance[0].Lat, mMyInstance[0].Lon, result);
                
            }
            catch (Exception)
            {

               
            }

            var res = Convert.ToInt32(result[0]);
            //  myHolder.mDist.Text = res.ToString()+ " meters away";


            string unit = " km";
            double dist = 0;
            var test = IOUtilz.LoadPreferences();
            if (test[1] == 1)
            {
                unit = " miles";
                dist = (int)IOUtilz.ConvertKilometersToMiles(res / 1000);
            }
            else
            {
                dist = res / 1000;
            }


            myHolder.mDist.Text = dist.ToString() + unit + " away";

            if (mUsers[position].Online)
            {
                myHolder.mOnlineStatus.SetImageResource(Resource.Drawable.greenonline);
            }
            else
                myHolder.mOnlineStatus.SetImageResource(Resource.Drawable.redoffline);



            myHolder.mSendMessage.Click += (sender, args) =>
           {



               Bundle b = new Bundle();
               b.PutStringArray("MyData", new String[] {

                           mUsers[position].UserName.ToString(),
                           mUsers[position].Id.ToString(),
                            mUsers[position].ProfilePicture.ToString()
                   // createNewConversation.Id.ToString()


               });




               Intent myInt = new Intent(mContext, typeof(PrivateChat));
               myInt.PutExtras(b);
               mContext.StartActivity(myInt);



           };



            myHolder.mDeleteFriend.Click += (sender, args) =>
        {
           
            int pos = (int)(((ImageButton)sender).GetTag(Resource.Id.sendFriendRequest));

                    //     var pos = ((View)sender).Tag;


           Toast.MakeText(mContext, mUsers[(int)pos].UserName.ToString() + " Deleted", ToastLength.Long).Show();

            var wait = Azure.deleteFriend(MainStart.userId, (mUsers[(int)pos].Id));

            mUsers.RemoveAt((int)pos);
            mAdapter = new UsersFriendsAdapter(mUsers, mRecyclerView, mActivity, mActivity, mAdapter, mMyInstance);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.NotifyDataSetChanged();

            if (mUsers.Count == 0)
            {

                //Intent myInt = new Intent(mContext, typeof(RouteOverview));
                //mContext.StartActivity(myInt);

                //mAdapter = new UsersFriendsAdapter(mUsers, mRecyclerView, mActivity, mActivity, mAdapter, mMyInstance);
                //mRecyclerView.SetAdapter(mAdapter);
                //mAdapter.NotifyDataSetChanged();
         
                txt = mActivity.FindViewById<TextView>(Resource.Id.emptyFriends);
                mRecyclerView.Visibility = ViewStates.Invisible;
                txt.Visibility = ViewStates.Visible;
            }


        };

            mAdapter = new UsersFriendsAdapter(mUsers, mRecyclerView, mActivity, mActivity, mAdapter, mMyInstance);

            // myHolder.mDeleteFriend.Click += MDeleteFriend_Click;  //+= (e, a) => //{
            // //  Azure.removeUser(mUsers[position].UserName);
            //   AlertDialog.Builder alert = new AlertDialog.Builder(mContext);
            //   alert.SetTitle("Delete friend");
            //   alert.SetMessage("You want to delete " + mUsers[position].UserName + " ?");
            //   alert.SetPositiveButton("Yes", (senderAlert, args) =>
            //   {
            //       //change value write your own set of instructions
            //       //you can also create an event for the same in xamarin
            //       //instead of writing things here
            //       Azure.removeUser(mUsers[position].UserName);
            //   });

            //   alert.SetNegativeButton("Cancel", (senderAlert, args) =>
            //   {
            //       //perform your own task for this conditional button click

            //   });


            //    };// 

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
            //  int position = mRecyclerView.GetChildPosition((View)sender);
            ///    int position = mRecyclerView.GetChildAdapterPosition((View)sender);

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

        void MDeleteFriend_Click(object sender, EventArgs e)
        {
            //int position = mRecyclerView.GetChildPosition((View)sender);
            //  int position = mRecyclerView.GetChildAdapterPosition((View)sender);

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