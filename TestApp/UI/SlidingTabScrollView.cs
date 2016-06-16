using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Util;

namespace TestApp
{
	/// <summary>
	/// Sliding tab scroll view. Adds tabs and views to the SlidingTabStrip menu tab.
	/// This class holds the TabStrip class, and allows for scrolling the tabs.
	/// </summary>
	public class SlidingTabScrollView : HorizontalScrollView
	{

		private const int TITLE_OFFSET_DIPS = 24;
		private const int TAB_VIEW_PADDING_DIPS = 16;
		private const int TAB_VIEW_TEXT_SIZE_SP = 12;

		private int mTitleOffset;

		private int mTabViewLayoutID;
		private int mTabViewTextViewID;

		// ViewPager located in Android support lib v4. 
		private ViewPager mViewPager;
		private ViewPager.IOnPageChangeListener mViewPagerPageChangeListener;

		private static SlidingTabStrip mTabStrip;

		private int mScrollState;

		public interface TabColorizer
		{
			int GetIndicatorColor(int position);
			int GetDividerColor(int position);
		}


		//Constructors.   :this calls the next constructor with 2 para, and so on
		public SlidingTabScrollView(Context context) : this(context, null) { }

		public SlidingTabScrollView(Context context, IAttributeSet attrs) : this(context, attrs, 0) { }
		// Base is what is inherited by the class ( like super() )
		public SlidingTabScrollView (Context context, IAttributeSet attrs, int defaultStyle) : base(context, attrs, defaultStyle)
		{
			//Disable the scroll bar
			HorizontalScrollBarEnabled = false;

			//Make sure the tab strips fill the view. The tab menu gets the color grey
			FillViewport = true;
			this.SetBackgroundColor(Android.Graphics.Color.Rgb(0xE5, 0xE5, 0xE5)); 

			mTitleOffset = (int)(TITLE_OFFSET_DIPS * Resources.DisplayMetrics.Density);

			mTabStrip = new SlidingTabStrip(context);
			this.AddView(mTabStrip, LayoutParams.MatchParent, LayoutParams.MatchParent);
		}

		public TabColorizer CustomTabColorizer
		{
			set { mTabStrip.CustomTabColorizer = value; }
		}

		public int [] SelectedIndicatorColor
		{
			set { mTabStrip.SelectedIndicatorColors = value; }
		}
			
		public int [] DividerColors
		{
			set { mTabStrip.DividerColors = value; }
		}

		public ViewPager.IOnPageChangeListener OnPageListener
		{
			set { mViewPagerPageChangeListener = value; }
		}

		public ViewPager ViewPager
		{
			set
			{
				mTabStrip.RemoveAllViews();
				mViewPager = value;
				if (value != null)
				{

					//The value of a page changing
					value.PageSelected += value_PageSelected;
					value.PageScrollStateChanged += value_PageScrollStateChanged;
					value.PageScrolled += value_PageScrolled;
					PopulateTabStrip();
				}
			}
		}

	
		void value_PageScrolled(object sender, ViewPager.PageScrolledEventArgs e)
		{
			//nr of tabs
			int tabCount = mTabStrip.ChildCount;
			//If no tabs, too much left, too much right then do nothing
			if ((tabCount == 0) || (e.Position < 0) || (e.Position >= tabCount))
			{
				//if any of these conditions apply, return, no need to scroll
				return;
			}

			// Takes the posistion the user tries to scroll to. Offset is the distance from where the user are, to where he is going
			mTabStrip.OnViewPagerPageChanged(e.Position, e.PositionOffset);

			View selectedTitle = mTabStrip.GetChildAt(e.Position);
			int extraOffset = (selectedTitle != null ? (int)(e.Position * selectedTitle.Width) : 0);

			//Scrolls to the specified tab, takes current position, and the distance to the goal
			ScrollToTab(e.Position, extraOffset);

			if (mViewPagerPageChangeListener != null)
			{
				mViewPagerPageChangeListener.OnPageScrolled(e.Position, e.PositionOffset, e.PositionOffsetPixels);
			}

		}

		void value_PageScrollStateChanged(object sender, ViewPager.PageScrollStateChangedEventArgs e)
		{
			mScrollState = e.State;

			if (mViewPagerPageChangeListener != null)
			{
				mViewPagerPageChangeListener.OnPageScrollStateChanged(e.State);
			}
		}

		void value_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
		{
			if (mScrollState == ViewPager.ScrollStateIdle)
			{
				mTabStrip.OnViewPagerPageChanged(e.Position, 0f);
				ScrollToTab(e.Position, 0);

			}


				
			if (mViewPagerPageChangeListener != null)
			{
				mViewPagerPageChangeListener.OnPageSelected(e.Position);
			}
		}

		private void PopulateTabStrip()
		{
			PagerAdapter adapter = mViewPager.Adapter;

			for (int i = 0; i < adapter.Count; i++)
			{

				if (i == 0) {


				}

				TextView tabView = CreateDefaultTabView (Context);
				tabView.Text = ((SlidingTabsFragment.SamplePagerAdapter)adapter).GetHeaderTitle(i);
				tabView.SetTextColor(Android.Graphics.Color.Black);
				//Tags holds what ever, as object.
				tabView.Tag = i;
				tabView.Click += tabView_Click;
				//adds view to tabstrip
				mTabStrip.AddView(tabView);
			}

		}

		void tabView_Click(object sender, EventArgs e)
		{
			TextView clickTab = (TextView)sender;
			int pageToScrollTo = (int)clickTab.Tag;
			mViewPager.CurrentItem = pageToScrollTo;
		}

		private TextView CreateDefaultTabView(Android.Content.Context context)
		{
			TextView textView = new TextView(context);
			textView.Gravity = GravityFlags.Center;
			textView.SetTextSize(ComplexUnitType.Sp, TAB_VIEW_TEXT_SIZE_SP);
			textView.Typeface = Android.Graphics.Typeface.DefaultBold;

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
			{
				TypedValue outValue = new TypedValue();
				Context.Theme.ResolveAttribute(Android.Resource.Attribute.SelectableItemBackground, outValue, false);
				textView.SetBackgroundResource(outValue.ResourceId);
			}

			if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.IceCreamSandwich)
			{
				textView.SetAllCaps(true);
			}

			int padding = (int)(TAB_VIEW_PADDING_DIPS * Resources.DisplayMetrics.Density);
			textView.SetPadding(padding, padding, padding, padding);

			return textView;
		}

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			if (mViewPager != null)
			{
				ScrollToTab(mViewPager.CurrentItem, 0);
			}
		}

		private void ScrollToTab(int tabIndex, int extraOffset)
		{
			int tabCount = mTabStrip.ChildCount;

			if (tabCount == 0 || tabIndex < 0 || tabIndex >= tabCount)
			{
				//No need to go further, dont scroll
				return;
			}

			View selectedChild = mTabStrip.GetChildAt(tabIndex);
			if (selectedChild != null)
			{
				int scrollAmountX = selectedChild.Left + extraOffset;

				if (tabIndex >0 || extraOffset > 0)
				{
					scrollAmountX -= mTitleOffset;
				}

				this.ScrollTo(scrollAmountX, 0);
			}

		}

	}
}