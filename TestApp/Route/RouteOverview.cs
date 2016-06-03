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
    [Activity(Label = "RouteOverview")]
    public class RouteOverview : Activity
    {


        Intent myIntent;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.routesOverview);


            Button myRoutes = (Button)FindViewById(Resource.Id.myRoutes);
            Button findRoute = (Button)FindViewById(Resource.Id.findRoutes);
            Button createRoute = (Button)FindViewById(Resource.Id.createRoutes);


            myRoutes.Click += (sender, e) => {
                myIntent = new Intent(this, typeof(UserMyRoutes));
                StartActivity(myIntent);

            };
            findRoute.Click += (sender, e) => {
                myIntent = new Intent(this, typeof(UsersRoutes));
                StartActivity(myIntent);
            };
            createRoute.Click += (sender, e) =>
            {
                myIntent = new Intent(this, typeof(CreateRoute));
                StartActivity(myIntent);

            };


        }

      
    }
}