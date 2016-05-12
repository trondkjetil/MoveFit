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
    [Activity(Label = "Around Me")]
    public class UsersNearby : Activity
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private List<User> users;
		

		SwipeRefreshLayout mSwipeRefreshLayout;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            // Set our view from the "main" layout resource
			SetContentView(Resource.Layout.UsersNearby);

			mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swp);
			mSwipeRefreshLayout.SetColorSchemeColors(Color.Orange, Color.Green, Color.Yellow, Color.Turquoise,Color.Turquoise);
			mSwipeRefreshLayout.Refresh += mSwipeRefreshLayout_Refresh;



            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recycleUserNearby);
            //Create our layout manager
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
          
            


            users = new List<User>();
            List<User> userList = await Azure.getPeople();

            foreach (User x in userList)
            {

                users.Add(x);
            }

            mAdapter = new UsersAdapter(users, mRecyclerView, this);
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

    public class UsersAdapter : RecyclerView.Adapter
    {
        private List<User> mUsers;
        private RecyclerView mRecyclerView;
        private Context mContext;
        private int mCurrentPosition = -1;

		public UsersAdapter(List<User> users, RecyclerView recyclerView, Context context)
        {
            mUsers = users;
            mRecyclerView = recyclerView;
            mContext = context;

        }

        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mUserName { get; set; }
            public TextView mStatus { get; set; }
            public TextView mText { get; set; }
            public ImageView mProfilePicture { get; set; }

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

                MyView view = new MyView(peopleNearbyContent) { mUserName = name, mStatus = status, mText = text, mProfilePicture = profile };
                return view;
  
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
                Bitmap userImage;
                //First view
                MyView myHolder = holder as MyView;
                myHolder.mMainView.Click += mMainView_Click;
                myHolder.mUserName.Text = mUsers[position].UserName;

                userImage = IOUtilz.GetImageBitmapFromUrl(mUsers[position].ProfilePicture);

            if (mUsers[position].Online)
            {
                myHolder.mStatus.Text = "Online";
            }
            else
            {
                myHolder.mStatus.Text = "Offline";
            }
               

                myHolder.mText.Text = mUsers[position].Text;


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
            int position = mRecyclerView.GetChildPosition((View)sender);
            Console.WriteLine(mUsers[position].UserName);
        }

        public override int ItemCount
        {
            get { return mUsers.Count; }
        }
    }
}

