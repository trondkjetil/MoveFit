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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Locations;

namespace TestApp
{
    [Activity(Label = "FriendsOverview", Theme = "@style/Theme2", ScreenOrientation = ScreenOrientation.Portrait)]
    public class FriendsOverview : AppCompatActivity, IOnMapReadyCallback
    {


        Intent myIntent;
        GoogleMap mMap;
        MarkerOptions markerOpt1;
        SupportToolbar toolbar;
        public List<User> me;
        string distanceUnit;
        int[] unit;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.friendsOverview);

            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            

            me = await Azure.getUserInstanceByName(MainStart.userName);

            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mMap = mapFrag.Map;

            if (mMap != null)
            {
                mMap.MapType = GoogleMap.MapTypeTerrain;  // The GoogleMap object is ready to go.
            }

        
            mMap.UiSettings.ZoomControlsEnabled = true;
            mMap.UiSettings.RotateGesturesEnabled = true;
            mMap.UiSettings.ScrollGesturesEnabled = true;

            List<User> people = null;


            unit = IOUtilz.LoadPreferences();
            if (unit[1] == 0)
            {
                distanceUnit = " km away";
            }
            else
                distanceUnit = " miles away";

            try
            {

                people = await Azure.getPeople();

                foreach (var item in people)
                {
                    setMarker(item);
                }



                Typeface tf = Typeface.CreateFromAsset(Assets,
                 "english111.ttf");
            TextView tv = (TextView)FindViewById(Resource.Id.textFriends);
            tv.Text = "Meet people in your area, and go do some activities together!";
            tv.TextSize = 28;
            tv.Typeface = tf;

            }
            catch (Exception)
            {

              
            }


            Button myFriends = (Button)FindViewById(Resource.Id.myFriends);
            Button friendRequests = (Button)FindViewById(Resource.Id.friendRequests);
            Button findFriends = (Button)FindViewById(Resource.Id.findFriends);

         

            myFriends.Click += (sender, e) => {
                myIntent = new Intent(this, typeof(UsersFriends));
                StartActivity(myIntent);

            };
            friendRequests.Click += (sender, e) => {
                myIntent = new Intent(this, typeof(UserFriendRequest));
                StartActivity(myIntent);
            };
            findFriends.Click += (sender, e) =>
            {
                myIntent = new Intent(this, typeof(UsersNearby));
                StartActivity(myIntent);

            };






        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            Finish();
        }

        public override void OnBackPressed()
        {


                base.OnBackPressed();
                 Finish();
        }
        public void setMarker(User user)
        {
            if (user.Lat == 0 && user.Lon == 0)
                return;

          
            LatLng myPosition = new LatLng(user.Lat, user.Lon);

            Bitmap pic = IOUtilz.GetImageBitmapFromUrl(user.ProfilePicture);
            BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(pic); //(Resource.Drawable.test);


            var onlineStatus = " [Offline]";

            if (user.Online)
                onlineStatus = " [Online]";

            float[] result = new float[1];
            Location.DistanceBetween(me[0].Lat, me[0].Lon, user.Lat, user.Lon, result);
            int dist = Convert.ToInt32(result[0]);

            if (unit[1] == 0)
            {
                dist = dist / 1000;
            }
            else
            {

                int conv = dist / 1000;


                dist = (int) IOUtilz.ConvertKilometersToMiles(conv);
            }
             

            mMap.AddMarker(new MarkerOptions()
           .SetPosition(myPosition)
           .SetTitle(user.UserName + onlineStatus)
           .SetSnippet(dist + distanceUnit)
           .SetIcon(image));//BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan)));
            
            
      



            //    markerOpt1 = new MarkerOptions();
            //    markerOpt1.SetPosition(myPosition);
            //    markerOpt1.SetTitle(user.UserName + " Position");
            //    markerOpt1.SetSnippet("Points: " + user.Points);
            ////  BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(pic); //(Resource.Drawable.test);
            //    markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan)); //;
            //    mMap.AddMarker(markerOpt1);




        }
        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)

        {
            MenuInflater.Inflate(Resource.Menu.action_menu_nav, menu);

            //itemGender = menu.FindItem(Resource.Id.gender);
            //itemAge = menu.FindItem(Resource.Id.age);
            //itemProfilePic = menu.FindItem(Resource.Id.profilePicture);
            //itemExit = menu.FindItem(Resource.Id.exit);


            //goHome.SetIcon(Resource.Drawable.eexit);
            //goBack.SetIcon(Resource.Drawable.ic_menu_back);




            return base.OnCreateOptionsMenu(menu);
        }
      
        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {

                //case Resource.Id.exit:
                //    Finish();
                //    return true;

                //case Resource.Id.back:
                //    OnBackPressed();
                //    return true;
                case Android.Resource.Id.Home:// Resource.Id.back:
                    OnBackPressed();
                    return true;

                case Resource.Id.home:

                    //Intent myIntent = new Intent(this, typeof(MainStart));

                    //StartActivity(myIntent);
                    OnBackPressed();
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }


        }

    }
}