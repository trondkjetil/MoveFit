using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Felipecsl.GifImageViewLibrary;


namespace TestApp
{
	public class SlidingTabsFragment : Fragment
	{
		private SlidingTabScrollView mSlidingTabScrollView;
		private ViewPager mViewPager;
		public static WebView webLoadingIcon;
		public FragmentTransaction transaction;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.fragment_sample, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			mSlidingTabScrollView = view.FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
			mViewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
			mViewPager.Adapter = new SamplePagerAdapter();

			mSlidingTabScrollView.ViewPager = mViewPager;
		}


		// The page that is being displayed
		public class SamplePagerAdapter : PagerAdapter
		{
			//Headers / titles of each tabs
			List<string> items = new List<string>();

			public SamplePagerAdapter() : base()
			{
				items.Add("Map");
				items.Add("Messages");
				items.Add("Share");
				items.Add("Activity");

			}

			public override int Count
			{
				get { return items.Count; }
			}

			public override bool IsViewFromObject(View view, Java.Lang.Object objectValue)
			{
				return view == objectValue;
			}



			// MÅ bruke listener på view for at view skal vite at noe skjer! Hjelper ikke med ekstern klasse...
			public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
			{
				View view;

			
				if (position == 1) {
					view = LayoutInflater.From (container.Context).Inflate (Resource.Layout.profile, container, false);
					container.AddView (view);
			

				}else if (position == 3) {
					view = LayoutInflater.From (container.Context).Inflate (Resource.Layout.scoreBoardMain, container, false);
					container.AddView (view);


				//starts an activity
				//	Intent myIntent = new Intent (container.Context, typeof(GoogleMapsActivity));
				//container.Context.StartActivity(myIntent);

				}else if (position == 2) {
					view = LayoutInflater.From (container.Context).Inflate (Resource.Layout.accelerometer, container, false);


					Button knappa = view.FindViewById<Button>(Resource.Id.stop);
					knappa.Click += (sender, e) => Toast.MakeText (container.Context, "Bka", ToastLength.Long);
					container.AddView (view);
				

				}else if (position == 4) {
					view = LayoutInflater.From (container.Context).Inflate (Resource.Layout.accelerometer, container, false);
					container.AddView (view);
				} else {
					// Inflates / starts the sample page layout
					view = LayoutInflater.From (container.Context).Inflate(Resource.Layout.profile, container, false);
					//		(Resource.Layout.c  pager_item, container, false);


//					 transaction = FragmentManager.BeginTransaction();
//					Test fragment = new Test();
//					transaction.Replace(Resource.Id.fragment_sample, fragment);
//					transaction.Commit();


					container.AddView (view);




//					TextView txtTitle = view.FindViewById<TextView> (Resource.Id.item_title);
//					//Writes page 1 etc
//					int pos = position + 1;
//					txtTitle.Text = pos.ToString ();
				}

				return view;
			}

			public string GetHeaderTitle (int position)
			{
				return items[position];
			}


			//Removes the view
			public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue)
			{
				container.RemoveView((View)objectValue);
			}
		}
	}
}