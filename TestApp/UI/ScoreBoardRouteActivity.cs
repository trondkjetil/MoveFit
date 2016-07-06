using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Views.InputMethods;
using System.Linq;

namespace TestApp
{
	[Activity(Label = "Scoreboard Routes", Icon = "@drawable/tt")]
	public class ScoreBoardRouteActivity : Activity
	{
		private List<Route> mRoutes;
		private ListView mListView;
		private EditText mSearch;
		private LinearLayout mContainer;
		private bool mAnimatedDown;
		private bool mIsAnimating;
		private RouteAdapterScoreboard mAdapter;

		private TextView mTxtHeaderFirstName;
		private TextView mTxtHeaderLastName;
		private TextView mTxtHeaderAge;
		private TextView mTxtHeaderGender;
		private TextView mTxtHeaderScore;

		private bool mFirstNameAscending;
		private bool mLastNameAscending;
		private bool mAgeAscending;
		private bool mGenderAscending;
		private bool mScoreAscending;

		protected async override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.scoreBoardRoutes);
			mListView = FindViewById<ListView>(Resource.Id.listView);
			mSearch = FindViewById<EditText>(Resource.Id.search);
			mContainer = FindViewById<LinearLayout>(Resource.Id.container);

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


            try
            {
                mRoutes = new List<Route>();
                //mRoutes.Add(new Route { CreatedAt = "", Name = "Test", Id = "Test", Difficulty = "33", Distance = "Male", Trips = 10, Info = "test", Review = "5", RouteType = "Walking", Time = "10", User_id = "121" });

                mRoutes = await Azure.getRoutes();

                if (mRoutes.Count > 0)
                {
                    mAdapter = new RouteAdapterScoreboard(this, Resource.Layout.row_route, mRoutes);
                    mListView.Adapter = mAdapter;
                }
                else
                {
                    //Intent myIntent = new Intent(this, typeof(MainStart));
                    //  StartActivity(myIntent);
                    Toast.MakeText(this, "No Routes found!", ToastLength.Long).Show();
                    Finish();
                }


             
                
                // mRoutes = await Azure.getPeople();
            }
            catch (Exception)
            {

              
            }








        }


        //Route names
		void mTxtHeaderScore_Click (object sender, EventArgs e)
		{
			List<Route> filteredFriends;

			if (!mScoreAscending)
			{
				filteredFriends = (from friend in mRoutes
					orderby friend.Name
					select friend).ToList<Route>();

				//Refresh the listview
				mAdapter = new RouteAdapterScoreboard(this, Resource.Layout.row_route, filteredFriends);
				mListView.Adapter = mAdapter;


			}

			else
			{
				filteredFriends = (from friend in mRoutes
					orderby friend.Name descending
					select friend).ToList<Route>();

				//Refresh the listview
				mAdapter = new RouteAdapterScoreboard(this, Resource.Layout.row_route, filteredFriends);
				mListView.Adapter = mAdapter;
			}

			mFirstNameAscending = !mFirstNameAscending;
		}


        //Route names
        void mTxtHeaderFirstName_Click(object sender, EventArgs e)
		{
			List<Route> filteredFriends;

			if (!mFirstNameAscending)
			{
				filteredFriends = (from friend in mRoutes
					orderby friend.Name
                                   select friend).ToList<Route>();

				//Refresh the listview
				mAdapter = new RouteAdapterScoreboard(this, Resource.Layout.row_route, filteredFriends);
				mListView.Adapter = mAdapter;


			}

			else
			{
				filteredFriends = (from friend in mRoutes
					orderby friend.Name descending
					select friend).ToList<Route>();

				//Refresh the listview
				mAdapter = new RouteAdapterScoreboard(this, Resource.Layout.row_route, filteredFriends);
				mListView.Adapter = mAdapter;
			}

			mFirstNameAscending = !mFirstNameAscending;
		}

		void mTxtHeaderLastName_Click(object sender, EventArgs e)
		{
			List<Route> filteredFriends;

			if (!mLastNameAscending)
			{
				filteredFriends = (from friend in mRoutes
					orderby friend.Name
                                   select friend).ToList<Route>();

				//Refresh the listview
				mAdapter = new RouteAdapterScoreboard(this, Resource.Layout.row_route, filteredFriends);
				mListView.Adapter = mAdapter;


			}

			else
			{
				filteredFriends = (from friend in mRoutes
					orderby friend.Name descending
					select friend).ToList<Route>();

				//Refresh the listview
				mAdapter = new RouteAdapterScoreboard(this, Resource.Layout.row_route, filteredFriends);
				mListView.Adapter = mAdapter;
			}

			mLastNameAscending = !mLastNameAscending;
		}







        //Route reviews
		void mTxtHeaderAge_Click(object sender, EventArgs e)
		{
			List<Route> filteredFriends;

			if (!mAgeAscending)
			{
				filteredFriends = (from friend in mRoutes
					orderby friend.Review.ToString()
					select friend).ToList<Route>();

				//Refresh the listview
				mAdapter = new RouteAdapterScoreboard(this, Resource.Layout.row_route, filteredFriends);
				mListView.Adapter = mAdapter;


			}

			else
			{
				filteredFriends = (from friend in mRoutes
					orderby friend.Review descending
					select friend).ToList<Route>();

				//Refresh the listview
				mAdapter = new RouteAdapterScoreboard(this, Resource.Layout.row_route, filteredFriends);
				mListView.Adapter = mAdapter;
			}

			mAgeAscending = !mAgeAscending;
		}


        // Route distance
		void mTxtHeaderGender_Click(object sender, EventArgs e)
		{
			List<Route> filteredFriends;

			if (!mGenderAscending)
			{
				filteredFriends = (from friend in mRoutes
					orderby friend.Distance
					select friend).ToList<Route>();

				//Refresh the listview
				mAdapter = new RouteAdapterScoreboard(this, Resource.Layout.row_route, filteredFriends);
				mListView.Adapter = mAdapter;


			}

			else
			{
				filteredFriends = (from friend in mRoutes
					orderby friend.Distance descending
					select friend).ToList<Route>();

				//Refresh the listview
				mAdapter = new RouteAdapterScoreboard(this, Resource.Layout.row_route, filteredFriends);
				mListView.Adapter = mAdapter;
			}

			mGenderAscending = !mGenderAscending;
		}

		void mSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
		{
			List<Route> searchedFriends = (from friend in mRoutes
				where friend.Name.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) || friend.Name.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
				|| friend.Review.ToString().Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) || friend.Distance.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
				select friend).ToList<Route>();

			//Refreshes the listview
			mAdapter = new RouteAdapterScoreboard(this, Resource.Layout.row_route, searchedFriends);
			mListView.Adapter = mAdapter;
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.actionbarRouteScoreBoard, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{

                case Resource.Id.switchFriends:
                     Intent myIntent = new Intent(this, typeof(ScoreBoardPersonActivity));
                      StartActivity(myIntent);
                    Finish();
                    return true;


                case Resource.Id.search:
				//Search icon has been clicked

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

