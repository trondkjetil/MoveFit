using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Views.InputMethods;
using System.Linq;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

namespace TestApp
{
	[Activity(Label = "Scoreboard People", Theme = "@style/Theme2")]
	public class ScoreBoardPersonActivity : AppCompatActivity
    {
		private List<User> mUsers;
		private ListView mListView;
		private EditText mSearch;
		private LinearLayout mContainer;
		private bool mAnimatedDown;
		private bool mIsAnimating;
        private UserAdapterScoreboard mAdapter;
		
		private TextView mTxtHeaderLastName;
		private TextView mTxtHeaderAge;
		private TextView mTxtHeaderGender;
		private TextView mTxtHeaderScore;
        SupportToolbar toolbar;
        private bool mFirstNameAscending;
		private bool mLastNameAscending;
		private bool mAgeAscending;
		private bool mGenderAscending;
		private bool mScoreAscending;

		protected async override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.scoreBoardMain);
			mListView = FindViewById<ListView>(Resource.Id.listView);
			mSearch = FindViewById<EditText>(Resource.Id.etSearch);
			mContainer = FindViewById<LinearLayout>(Resource.Id.llContainer);

			//mTxtHeaderFirstName = FindViewById<TextView>(Resource.Id.txtHeaderFirstName);
			mTxtHeaderLastName = FindViewById<TextView>(Resource.Id.txtHeaderLastName);
			mTxtHeaderAge = FindViewById<TextView>(Resource.Id.txtHeaderAge);
			mTxtHeaderGender = FindViewById<TextView>(Resource.Id.txtHeaderGender);
			mTxtHeaderScore = FindViewById<TextView>(Resource.Id.txtHeaderScore);

			//mTxtHeaderFirstName.Click += mTxtHeaderFirstName_Click;
			mTxtHeaderLastName.Click += mTxtHeaderLastName_Click;
			mTxtHeaderAge.Click += mTxtHeaderAge_Click;
			mTxtHeaderGender.Click += mTxtHeaderGender_Click;
			mTxtHeaderScore.Click += mTxtHeaderScore_Click;

			mSearch.Alpha = 0;
			mContainer.BringToFront();
			mSearch.TextChanged += mSearch_TextChanged;

           // ActionBar.SetDisplayShowHomeEnabled(true);
            //ActionBar.SetDisplayHomeAsUpEnabled(true);
            //ActionBar.SetDisplayShowTitleEnabled(true);
            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);

            try
            {
                //   mUsers = new List<User>();
                mUsers = await Azure.nearbyPeopleScoreboard();

                if (mUsers.Count > 0)
                {
                    mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, mUsers);
                    mListView.Adapter = mAdapter;
                }
                else
                {
                    Intent myIntent = new Intent(this, typeof(ScoreBoardRouteActivity));
                    StartActivity(myIntent);
                    Toast.MakeText(this, "No People found!", ToastLength.Long).Show();
                    Finish();
                }

            }
            catch (Exception)
            {

               
            }
        }

		void mTxtHeaderScore_Click (object sender, EventArgs e)
		{
			List<User> filteredFriends;

			if (!mScoreAscending)
			{
				filteredFriends = (from friend in mUsers
					orderby friend.Points
					select friend).ToList<User>();

				//Refresh the listview
				mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
				mListView.Adapter = mAdapter;


			}

			else
			{
				filteredFriends = (from friend in mUsers
					orderby friend.UserName descending
					select friend).ToList<User>();

				//Refresh the listview
				mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
				mListView.Adapter = mAdapter;
			}

			mFirstNameAscending = !mFirstNameAscending;


            //EDITED UNKOWN EFFECT!
            mScoreAscending = !mScoreAscending;

        }

		void mTxtHeaderFirstName_Click(object sender, EventArgs e)
		{
			List<User> filteredFriends;

			if (!mFirstNameAscending)
			{
				filteredFriends = (from friend in mUsers
					orderby friend.UserName
					select friend).ToList<User>();

				//Refresh the listview
				mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
				mListView.Adapter = mAdapter;


			}

			else
			{
				filteredFriends = (from friend in mUsers
					orderby friend.UserName descending
					select friend).ToList<User>();

				//Refresh the listview
				mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
				mListView.Adapter = mAdapter;
			}

			mFirstNameAscending = !mFirstNameAscending;
		}

		void mTxtHeaderLastName_Click(object sender, EventArgs e)
		{
			List<User> filteredFriends;

			if (!mLastNameAscending)
			{
				filteredFriends = (from friend in mUsers
					orderby friend.UserName
					select friend).ToList<User>();

				//Refresh the listview
				mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
				mListView.Adapter = mAdapter;


			}

			else
			{
				filteredFriends = (from friend in mUsers
					orderby friend.UserName descending
					select friend).ToList<User>();

				//Refresh the listview
				mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
				mListView.Adapter = mAdapter;
			}

			mLastNameAscending = !mLastNameAscending;
		}

		void mTxtHeaderAge_Click(object sender, EventArgs e)
		{
			List<User> filteredFriends;

			if (!mAgeAscending)
			{
				filteredFriends = (from friend in mUsers
					orderby friend.Age.ToString()
					select friend).ToList<User>();

				//Refresh the listview
				mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
				mListView.Adapter = mAdapter;


			}

			else
			{
				filteredFriends = (from friend in mUsers
					orderby friend.Age descending
					select friend).ToList<User>();

				//Refresh the listview
				mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
				mListView.Adapter = mAdapter;
			}

			mAgeAscending = !mAgeAscending;
		}

		void mTxtHeaderGender_Click(object sender, EventArgs e)
		{
			List<User> filteredFriends;

			if (!mGenderAscending)
			{
				filteredFriends = (from friend in mUsers
					orderby friend.Sex
					select friend).ToList<User>();

				//Refresh the listview
				mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
				mListView.Adapter = mAdapter;


			}

			else
			{
				filteredFriends = (from friend in mUsers
					orderby friend.Sex descending
					select friend).ToList<User>();

				//Refresh the listview
				mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, filteredFriends);
				mListView.Adapter = mAdapter;
			}

			mGenderAscending = !mGenderAscending;
		}

		void mSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
		{
			List<User> searchedFriends = (from friend in mUsers
				where friend.UserName.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) || friend.UserName.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
				|| friend.Age.ToString().Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) || friend.Sex.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
				select friend).ToList<User>();

			//Refreshes the listview
			mAdapter = new UserAdapterScoreboard(this, Resource.Layout.row_friend, searchedFriends);
			mListView.Adapter = mAdapter;
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.actionbar, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{

                case Resource.Id.switchRoutes:
                    Intent myIntent = new Intent(this, typeof(ScoreBoardRouteActivity));
                    StartActivity(myIntent);
                    Finish();
                    return true;

                case Android.Resource.Id.Home:// Resource.Id.back:
                    OnBackPressed();
                    return true;

                   

                case Resource.Id.search:
				mSearch.Visibility = ViewStates.Visible;

                    if (mIsAnimating)
                    {
                        return true;
                    }

                    if (!mAnimatedDown)
                    {
                        //Listview is up
                        MyAnimation anim = new MyAnimation(mListView, mListView.Height - mSearch.Height);
                        anim.Duration = 500;
                        mListView.StartAnimation(anim);
                        anim.AnimationStart += anim_AnimationStartDown;
                        anim.AnimationEnd += anim_AnimationEndDown;
                        mContainer.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();
                    }

                    else
                    {
                        //Listview is down
                        MyAnimation anim = new MyAnimation(mListView, mListView.Height + mSearch.Height);
                        anim.Duration = 500;
                        mListView.StartAnimation(anim);
                        anim.AnimationStart += anim_AnimationStartUp;
                        anim.AnimationEnd += anim_AnimationEndUp;
                        mContainer.Animate().TranslationYBy(-mSearch.Height).SetDuration(500).Start();
                    }

                    mAnimatedDown = !mAnimatedDown;
                    return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}

		void anim_AnimationEndUp(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
		{
			mIsAnimating = false;
			mSearch.ClearFocus();
			InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
			inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
		}

		void anim_AnimationEndDown(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
		{
			mIsAnimating = false;
		}

		void anim_AnimationStartDown(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
		{
			mIsAnimating = true;
			mSearch.Animate().AlphaBy(1.0f).SetDuration(500).Start();
		}

		void anim_AnimationStartUp(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
		{
			mIsAnimating = true;
			mSearch.Animate().AlphaBy(-1.0f).SetDuration(300).Start();
		}
	}
}

