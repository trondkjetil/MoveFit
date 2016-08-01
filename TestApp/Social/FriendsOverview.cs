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

namespace TestApp
{
    [Activity(Label = "FriendsOverview")]
    public class FriendsOverview : Activity, IOnMapReadyCallback
    {


        Intent myIntent;
        GoogleMap mMap;
        MarkerOptions markerOpt1;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.friendsOverview);


            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mMap = mapFrag.Map;

            if (mMap != null)
            {
                mMap.MapType = GoogleMap.MapTypeTerrain;  // The GoogleMap object is ready to go.
            }

        
            mMap.UiSettings.ZoomControlsEnabled = true;
            mMap.UiSettings.RotateGesturesEnabled = true;
            mMap.UiSettings.ScrollGesturesEnabled = true;

            List<User> people = await Azure.getPeople();

            foreach (var item in people)
            {
                setMarker(item);
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
            //if (user.Lat == 0 && user.Lon == 0)
            //    return;

          
         LatLng myPosition = new LatLng(user.Lat, user.Lon);

            Bitmap pic = IOUtilz.GetImageBitmapFromUrl(user.ProfilePicture);
            BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(pic); //(Resource.Drawable.test);


            var onlineStatus = "offline";

            if (user.Online)
                onlineStatus = "online";

            mMap.AddMarker(new MarkerOptions()
           .SetPosition(myPosition)
           .SetTitle(user.UserName)
           .SetSnippet("Online status: "+ onlineStatus)
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

    }
}