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

namespace TestApp
{
    [Activity(Label = "FriendsOverview")]
    public class FriendsOverview : Activity
    {


        Intent myIntent;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.friendsOverview);


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






    }
}