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

        public class UsersRoutesAdapterFragment : RecyclerView.Adapter
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

        public int origin;

            public UsersRoutesAdapterFragment(List<Route> routes, RecyclerView recyclerView, Context context, List<User> myInstance, int sender)
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
                 //public ImageButton mDeleteRoute { get; set; }
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


            View userRoutes = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.userRouteContent, parent, false);
              //  deleteRoute = userRoutes.FindViewById<ImageButton>(Resource.Id.delete);

             
            
           


                ImageView profile = userRoutes.FindViewById<ImageView>(Resource.Id.profilePicture);
                TextView name = userRoutes.FindViewById<TextView>(Resource.Id.nameId);
                TextView status = userRoutes.FindViewById<TextView>(Resource.Id.statusId);
                TextView text = userRoutes.FindViewById<TextView>(Resource.Id.textId);
                TextView distanceFromMe = userRoutes.FindViewById<TextView>(Resource.Id.distAway);


                ImageButton startRoute = userRoutes.FindViewById<ImageButton>(Resource.Id.startRoute);
                ImageButton routeDifficultyImage = userRoutes.FindViewById<ImageButton>(Resource.Id.imageButton3);

  
                MyView view = new MyView(userRoutes) { mRouteName = name, mStatus = status, mRouteInfo = text, mIconForRoute = profile, mStartRouteFlag = startRoute, mRouteDifficulty = routeDifficultyImage, mDistanceAway = distanceFromMe };

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



            //  if(myHolder.mDeleteRoute != null)
        



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


                myHolder.mIconForRoute.SetImageResource(Resource.Drawable.maps);

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