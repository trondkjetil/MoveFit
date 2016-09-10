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
using Android.Locations;
using Android.Animation;

namespace TestApp
{

        public class UsersMyRoutesAdapterFragment : RecyclerView.Adapter
        {
            private List<Route> mRoutes;
            private RecyclerView mRecyclerView;
            private Context mContext;
            private int mCurrentPosition = -1;
            private List<User> mMyInstance;
            public string routeName;
            public string routeInfo;
            public string routeRating;
            public string routeDifficulty;
            public string routeLength;
            public string routeType;
            public int routeTrips;
            public string routeId;
            public string routeTime;
            public string routeUserId;

            private RecyclerView.Adapter mAdapter;

      

            public UsersMyRoutesAdapterFragment(List<Route> routes, RecyclerView recyclerView, Context context, List<User> myInstance, int sender)
            {
                mRoutes = routes;
                mRecyclerView = recyclerView;
                mContext = context;
                mMyInstance = myInstance;
          
                mAdapter = this;
            }

            public class MyView : RecyclerView.ViewHolder
            {
                public View mMainView { get; set; }
                public TextView mRouteName { get; set; }
                public TextView mStatus { get; set; }
                public TextView mRouteInfo { get; set; }
                 public ImageButton mDeleteRoute { get; set; }
                 public TextView mDistanceAway { get; set; }
                public ImageView mIconForRoute { get; set; }
                public ImageButton mStartRouteFlag { get; set; }
                public ImageButton mRouteDifficulty { get; set; }

                public MyView(View view) : base(view)
                {
                    mMainView = view;
                }

            }


            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
        




               View userRoutes = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.userMyRoutesContent, parent, false);
           


                ImageView profile = userRoutes.FindViewById<ImageView>(Resource.Id.profilePicture);
                TextView name = userRoutes.FindViewById<TextView>(Resource.Id.nameId);
                TextView status = userRoutes.FindViewById<TextView>(Resource.Id.statusId);
                TextView text = userRoutes.FindViewById<TextView>(Resource.Id.textId);
                TextView distanceFromMe = userRoutes.FindViewById<TextView>(Resource.Id.distAway);


                ImageButton startRoute = userRoutes.FindViewById<ImageButton>(Resource.Id.startRoute);
                ImageButton routeDifficultyImage = userRoutes.FindViewById<ImageButton>(Resource.Id.imageButton3);


            ImageButton deleteRoute = userRoutes.FindViewById<ImageButton>(Resource.Id.deleteRoute);

            deleteRoute.Visibility = ViewStates.Visible;
            deleteRoute.Focusable = false;
            deleteRoute.FocusableInTouchMode = false;
            deleteRoute.Clickable = true;

            MyView view = new MyView(userRoutes) { mRouteName = name, mStatus = status, mRouteInfo = text, mIconForRoute = profile, mStartRouteFlag = startRoute, mRouteDifficulty = routeDifficultyImage, mDistanceAway = distanceFromMe, mDeleteRoute = deleteRoute };

                return view;


            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {


                routeName = "";
                routeInfo = "";
                routeRating = "";
                routeDifficulty = "";
                routeLength = "";
                routeType = "";
                routeTrips = 1;
                routeId = "";
                routeTime = "";
                routeUserId = "";

                MyView myHolder = holder as MyView;
                myHolder.mMainView.Click += mMainView_Click;
                myHolder.mRouteName.Text = mRoutes[position].Name;
                //  myHolder.mStartRouteFlag.Click += StartRouteFlag_Click;
                myHolder.mStatus.Text = mRoutes[position].RouteType;

                myHolder.mDeleteRoute.SetTag(Resource.Id.deleteRoute, position);
             

                myHolder.mDeleteRoute.Click +=async (sender, args) =>
                {

                    int pos = (int)(((ImageButton)sender).GetTag(Resource.Id.deleteRoute));

                    //     var pos = ((View)sender).Tag;

                    Toast.MakeText(mContext, mRoutes[pos].Name + " Route Deleted", ToastLength.Long).Show();

                    //   var wait = Azure.deleteFriend(MainStart.userId, (mUsers[(int)pos].Id));
                    await Azure.deleteRoute(mRoutes[pos].Id);
                    mRoutes.RemoveAt((int)pos);
                    mAdapter = new UsersMyRoutesAdapterFragment(mRoutes, mRecyclerView, mContext, mMyInstance,2);
                    mRecyclerView.SetAdapter(mAdapter);
                    mAdapter.NotifyDataSetChanged();

                };
                



            // Calculate distance to User
            float[] result = null;
            result = new float[1];
            try
            {


                Location.DistanceBetween(mRoutes[position].Lat, mRoutes[position].Lon, mMyInstance[0].Lat, mMyInstance[0].Lon, result);
            }
            catch (Exception)
            {


            }
            var res = Convert.ToInt32(result[0]);

                int routeDistanceMeters = Convert.ToInt32(mRoutes[position].Distance);
                string unit = " km";
                double dist = 0;
                var test = IOUtilz.LoadPreferences();

                if (test[1] == 1)
                {
                    unit = " miles";
                    dist = (int)IOUtilz.ConvertKilometersToMiles(res / 1000);
                    myHolder.mRouteInfo.Text = "Length " + IOUtilz.ConvertKilometersToMiles(routeDistanceMeters / 1000) + " miles";

                }
                else
                {
                    myHolder.mRouteInfo.Text = "Length " + routeDistanceMeters / 1000 + " km";

                    dist = res / 1000;
                }


                myHolder.mDistanceAway.Text = dist.ToString() + unit + " away";


                //    myHolder.mDistanceAway.Text = res.ToString() + " meters away";


                if (mRoutes[position].Difficulty == "Easy")
                {
                    myHolder.mRouteDifficulty.SetImageResource(Resource.Drawable.green);
                }
                else if (mRoutes[position].Difficulty == "Medium")
                {
                    myHolder.mRouteDifficulty.SetImageResource(Resource.Drawable.orange);

                }
                else if (mRoutes[position].Difficulty == "Hard")
                {
                    myHolder.mRouteDifficulty.SetImageResource(Resource.Drawable.red);

                }


              //  myHolder.mIconForRoute.SetImageResource(Resource.Drawable.maps);

            if (mRoutes[position].RouteType == "Walking")
            {
                myHolder.mIconForRoute.SetImageResource(Resource.Drawable.wa);
            }
            else if (mRoutes[position].RouteType == "Running")
            {
                myHolder.mIconForRoute.SetImageResource(Resource.Drawable.ru);

            }
            else if (mRoutes[position].RouteType == "Hiking")
            {
                myHolder.mIconForRoute.SetImageResource(Resource.Drawable.tr);

            }
            else if (mRoutes[position].RouteType == "Bicycling")
            {
                myHolder.mIconForRoute.SetImageResource(Resource.Drawable.cy);

            }
            else if (mRoutes[position].RouteType == "Skiing")
            {
                myHolder.mIconForRoute.SetImageResource(Resource.Drawable.sk);

            }
            else if (mRoutes[position].RouteType == "Kayaking")
            {
                myHolder.mIconForRoute.SetImageResource(Resource.Drawable.ka);

            }






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


            try
            {

                int position = mRecyclerView.GetChildAdapterPosition((View)sender);

                routeName = mRoutes[position].Name;
                routeInfo = mRoutes[position].Info;
                routeDifficulty = mRoutes[position].Difficulty;
                routeLength = mRoutes[position].Distance;
                routeType = mRoutes[position].RouteType;
                routeRating = mRoutes[position].Review;
                routeTrips = mRoutes[position].Trips;
                routeId = mRoutes[position].Id;
                routeTime = mRoutes[position].Time;
                routeUserId = mRoutes[position].User_id;



                Bundle b = new Bundle();
                b.PutStringArray("MyData", new String[] {

            routeName,
            routeInfo,
            routeDifficulty,
            routeLength,
            routeType,
            routeRating,
            routeTrips.ToString(),
            routeId,
            routeTime,
            routeUserId

        });

                Intent myIntent = new Intent(mContext, typeof(StartRoute));
                myIntent.PutExtras(b);
                mContext.StartActivity(myIntent);

            }
            catch (Exception)
            {


            }


        }

        public override int ItemCount
        {
            get { return mRoutes.Count; }
        }


    }
}