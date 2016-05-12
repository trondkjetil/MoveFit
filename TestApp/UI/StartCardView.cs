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
    [Activity(Label = "People Around Me")]
    public class StartCardView : Activity
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private List<Email> mEmails;
		public static Bitmap pic { get; set; }

		SwipeRefreshLayout mSwipeRefreshLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
			SetContentView(Resource.Layout.cardViewMain);


			mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeLayout);
			mSwipeRefreshLayout.SetColorSchemeColors(Color.Orange, Color.Green, Color.Yellow, Color.Turquoise,Color.Turquoise);
			mSwipeRefreshLayout.Refresh += mSwipeRefreshLayout_Refresh;


			pic = (Bitmap) Intent.GetParcelableExtra("MyData");

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            mEmails = new List<Email>();
            mEmails.Add(new Email() { Name = "Ola", Subject = "Socializing", Message = "Lets do a hike together!" });

            //Create our layout manager
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new RecyclerAdapter(mEmails, mRecyclerView, this,pic);
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


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar_cardview, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch(item.ItemId)
            {
                case Resource.Id.add:
                    //Add button clicked
                    mEmails.Add(new Email() { Name = "New Name", Subject = "New Subject", Message = "New Message" });
                    mAdapter.NotifyItemInserted(mEmails.Count - 1);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }




    }

    public class RecyclerAdapter : RecyclerView.Adapter
    {
        private List<Email> mEmails;
        private RecyclerView mRecyclerView;
        private Context mContext;
        private int mCurrentPosition = -1;

	
		public Bitmap pic;

		public RecyclerAdapter(List<Email> emails, RecyclerView recyclerView, Context context, Bitmap pic)
        {
            mEmails = emails;
            mRecyclerView = recyclerView;
            mContext = context;

			this.pic = pic;


        }

        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mName { get; set; }
            public TextView mSubject { get; set; }
            public TextView mMessage { get; set; }

            public MyView (View view) : base(view)
            {
                mMainView = view;
            }
        }

        public class MyView2 : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }


            public MyView2(View view) : base(view)
            {
                mMainView = view;
            }
        }

        public override int GetItemViewType(int position)
        {
            if ((position % 2) == 0)
            {
                //Even number
				return  Resource.Layout.row;
            }

            else
            {
                //Odd number
                return Resource.Layout.row2;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {




            if (viewType == Resource.Layout.row)
            {
                //First card view
                View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.row, parent, false);
			
				ImageView profile = row.FindViewById<ImageView>(Resource.Id.profilePicture);
				//profile.SetImageResource (Resource.Drawable.tt);

				if (pic == null) {
					profile.SetImageResource (Resource.Drawable.tt);
				}else
					profile.SetImageBitmap (pic);




                TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
                TextView txtSubject = row.FindViewById<TextView>(Resource.Id.txtSubject);
                TextView txtMessage = row.FindViewById<TextView>(Resource.Id.txtMessage);


                MyView view = new MyView(row) { mName = txtName, mSubject = txtSubject, mMessage = txtMessage };
                return view;
            }

            else
            {
                //Second card view
                View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.row2, parent, false);
                MyView2 view = new MyView2(row);
                return view;
            }
            
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is MyView)
            {
                //First view
                MyView myHolder = holder as MyView;
                myHolder.mMainView.Click += mMainView_Click;
                myHolder.mName.Text = mEmails[position].Name;
                myHolder.mSubject.Text = mEmails[position].Subject;
                myHolder.mMessage.Text = mEmails[position].Message;

                if (position > mCurrentPosition)
                {
                    int currentAnim = Resource.Animation.slide_left_to_right;
                    SetAnimation(myHolder.mMainView, currentAnim);
                    mCurrentPosition = position;
                }
            }

            else
            {
                //Second View
                MyView2 myHolder = holder as MyView2;
                if (position > mCurrentPosition)
                {
                    int currentAnim = Resource.Animation.slide_right_to_left;
                    SetAnimation(myHolder.mMainView, currentAnim);
                    mCurrentPosition = position;
                }
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
            Console.WriteLine(mEmails[position].Name);
        }

        public override int ItemCount
        {
            get { return mEmails.Count; }
        }
    }
}

