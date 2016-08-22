using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using System.Collections;
using System.Diagnostics;
using Android.Content.PM;
using TestApp.Points;
using Android.Graphics.Drawables;
using Android.Graphics;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
namespace TestApp
{
    [Activity(Label = "StartRoute", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/Theme2")]
    public class StartRoute : AppCompatActivity, ILocationListener
    {


        const long MIN_TIME = 10 * 1000; // Minimum time interval for update in seconds, i.e. 5 seconds.
        const long MIN_DISTANCE = 5;

        SupportToolbar toolbar;
        LocationManager locationManager;
        string locationProvider;
        MarkerOptions markerOpt1;
        MarkerOptions markerOpt2;
        GoogleMap mMap;

        public static String[] array;
        public List<Locations> locationPointsForRoute;

        public static string routeName;

        private string routeInfo;
        private string routeRating;
        private string routeDifficulty;
        private string routeLength;
        private string routeType;
        private string routeTrips;
        private string routeId;
        private string routeTime;

        public Stopwatch stopWatch;
        public string elapsedTime;

        public static float avgSpeed;
        public ToggleButton start;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.startRoute);

            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.mapForStartingRoute);
            mMap = mapFrag.Map;

            if (mMap != null)
            {
                mMap.MapType = GoogleMap.MapTypeTerrain;  // The GoogleMap object is ready to go.
            }
            {
                mMap.UiSettings.ZoomControlsEnabled = true;
                mMap.UiSettings.CompassEnabled = true;
                mMap.UiSettings.MapToolbarEnabled = true;
                mMap.UiSettings.MyLocationButtonEnabled = true;

                array = Intent.GetStringArrayExtra("MyData");
                routeId = array[7];

                locationPointsForRoute = await Azure.getLocationsForRoute(routeId);

                toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayShowTitleEnabled(false);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                TextView name = FindViewById<TextView>(Resource.Id.startRouteName);
                TextView description = FindViewById<TextView>(Resource.Id.startRouteDesc);
                TextView length = FindViewById<TextView>(Resource.Id.startRouteLength);
                TextView difficulty = FindViewById<TextView>(Resource.Id.startRouteDiff);
                TextView rating = FindViewById<TextView>(Resource.Id.startRouteRating);
                TextView trips = FindViewById<TextView>(Resource.Id.startRouteTrips);
                TextView type = FindViewById<TextView>(Resource.Id.startRouteType);
                TextView time = FindViewById<TextView>(Resource.Id.startRouteTime);

                start = FindViewById<ToggleButton>(Resource.Id.toggleStart);


                RatingBar ratingbar = FindViewById<RatingBar>(Resource.Id.ratingbar);

                LayerDrawable stars = (LayerDrawable)ratingbar.ProgressDrawable;
                stars.GetDrawable(2).SetColorFilter(Color.Yellow, PorterDuff.Mode.SrcAtop);
                stars.GetDrawable(0).SetColorFilter(Color.Yellow, PorterDuff.Mode.SrcAtop);
                stars.GetDrawable(1).SetColorFilter(Color.Yellow, PorterDuff.Mode.SrcAtop);
                ratingbar.Clickable = false;
                ratingbar.Visibility = ViewStates.Visible;
                ratingbar.Rating = 0;


                routeName = array[0];
                routeInfo = array[1];
                routeDifficulty = array[2];
                routeLength = array[3];
                routeType = array[4];
                routeRating = array[5];
                routeTrips = array[6];

                routeTime = array[8];


                name.Text = "Name:" + routeName;
                description.Text = "Description: " + routeInfo;
                difficulty.Text = "Difficulty: " + routeDifficulty;
                length.Text = "Length: " + routeLength;
                type.Text = "Type: " + routeType;
                trips.Text = "Trips: " + routeTrips;
                time.Text = "Best time: " + routeTime;

                try
                {

                    List<Review> rate = await Azure.getRouteReview(routeId);

                    if (rate.Count != 0)
                    routeRating = rate.First().Rating.ToString();

                    if (Convert.ToInt32(routeRating) == 0)
                    {
                        rating.Text = "Rating: Not rated yet!";
                    }
                    else
                    {
                        rating.Text = "Rating: " + routeRating;
                        ratingbar.Rating = Convert.ToInt32(routeRating);

                    }



                    

                    drawRoute();

                }
                catch (Exception ex)
                {
                   
                }




                start.Click += (sender, e) =>
                {

                    if (start.Checked)
                    {
                        startRoute();
                    }
                    else
                        endRoute();

                };






                //    start.Click += (sender, e) =>
                //{
                    
                //};

                //end.Click += (sender, e) =>
                //{
                  

                //};

            


            }

        }
        private void startRoute()
        {
            if (CreateRouteService.serviceIsRunning == true)
            {
                Toast.MakeText(this, "Cannot start a route while creating one!", ToastLength.Long).Show();

                return;
            }

            start.Enabled = false;

            if (StartRouteService.serviceIsRunning == true)
            {
                var val = routeIsRunning();

                if (val == true)
                    return;

            }

            double distance = getDistanceFromStart();

            if (distance <= 100)
            {
                Toast.MakeText(this, "Starting route...", ToastLength.Long).Show();
                StartService(new Intent(this, typeof(StartRouteService)));

                stopWatch = new Stopwatch();
                stopWatch.Start();
            }
            else
                Toast.MakeText(this, "Please move closer to the starting point!", ToastLength.Short).Show();



            start.Enabled = true;

            //InitializeLocationManager();
            //locationManager.RequestLocationUpdates(this.locationProvider, MIN_TIME, MIN_DISTANCE, this);



        }
        private void endRoute()
        {
            if (StartRouteService.serviceIsRunning == false)
            {
                Toast.MakeText(this, "Nothing to stop!", ToastLength.Short).Show();
                return;

            }

            start.Enabled = false;
            List<float> speedList = StartRouteService.points;

            float sum = 0;
            avgSpeed = 0;

            foreach (var item in speedList)
            {

                sum += item;

            }

            avgSpeed = sum / speedList.Count();

            StopService(new Intent(this, typeof(StartRouteService)));

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);


            double distance = getDistanceFromStart();


            if (distance <= 70)
            {
                Toast.MakeText(this, "Congratulations! You have finished the route", ToastLength.Short).Show();
                startDialogNameRoute();
                if (locationManager != null)
                    locationManager.RemoveUpdates(this);



                int mypoints = MyPoints.calculatePoints(routeType, (int)Math.Round(distance));
                var done = Azure.addToMyPoints(routeId, mypoints);
                var complete = Azure.increaseTripCount(routeId);
                Azure.addToMyDistance(MainStart.userId, distance);

                Toast.MakeText(this, "You have earned " + mypoints + " points!", ToastLength.Long).Show();


            }
            else
                Toast.MakeText(this, "Please move closer to the finish-line!" + "Distance to finish - point is: " + distance, ToastLength.Long).Show();




            start.Enabled = true;

        }

        private double getDistanceFromStart()
        {
            double distance = 0;
            Location myLocation = App.Current.LocationService.getLastKnownLocation();// locationManager.GetLastKnownLocation(locationProvider);
            Locations firstElement = locationPointsForRoute.First();

            float[] results = new float[1];
           // double[] LatLngFirst = firstElement.Location.Split(',');
            Location.DistanceBetween(myLocation.Latitude, myLocation.Longitude, firstElement.Lat, firstElement.Lon, results);
            //Location.DistanceBetween(myLocation.Latitude, myLocation.Longitude, Convert.ToDouble(LatLngFirst[0]), Convert.ToDouble(LatLngFirst[1]), results);


            distance = (int)results[0];

            return distance;
        }

        public void startDialogNameRoute()
        {

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            DialogEndRoute newDialog = new DialogEndRoute();
            newDialog.DialogClosed += OnDialogClosed;
            newDialog.Show(transaction, "End Route");
        }


        async void OnDialogClosed(object sender, DialogEndRoute.DialogEventArgs e)
        {

            routeRating = e.ReturnValue.ToString();
            //  await Azure.giveRouteRating(routeId, "TestUserID", routeRating);

            var user = await Azure.getUserInstanceByName(MainStart.userName);
            var userName = user.First().UserName;

            Azure.AddReview(routeId, Convert.ToInt32(routeRating), userName);

            // Gets all the reviews for a route
            var reviewInstance = await Azure.getRouteReview(routeId);
            int amountOfRatings = reviewInstance.Count();

            int rating = 0;
            foreach (var item in reviewInstance)
            {
                rating = rating + item.Rating;

            }

            int totalRate = rating / amountOfRatings;
            await Azure.giveRouteRating(routeId, totalRate.ToString());

        }

        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
        }

        void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Medium
            };
            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                locationProvider = string.Empty;
            }


        }


        protected override void OnResume()
        {
            base.OnResume();
            if (locationManager != null)
                locationManager.RequestLocationUpdates(this.locationProvider, MIN_TIME, MIN_DISTANCE, this);

        }

        protected override void OnPause()
        {
            base.OnPause();
            //if (locationManager != null)
            //    locationManager.RemoveUpdates(this);
        }
        protected override void OnStop()
        {
            base.OnStop();
            //if (locationManager != null)
            //    locationManager.RemoveUpdates(this);
        }



        public void OnProviderDisabled(string provider)
        {

        }

        public void OnProviderEnabled(string provider)
        {

        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {

        }

        public void OnLocationChanged(Location location)
        {


            try
            {

                //  mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
                //   mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(location.Latitude, location.Longitude), 14));

            }
            catch (Exception)
            {
               
            }

        }



        public override void OnBackPressed()
        {
            // MoveTaskToBack(true);
            // base.OnBackPressed();

            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);

            alert.SetTitle("Exit route creation");
            alert.SetMessage("Do you want to abort the current route creation?");
            alert.SetPositiveButton("Yes", (senderAlert, args) =>
            {

                base.OnBackPressed();
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) =>
            {


            });
            //run the alert in UI thread to display in the screen
            RunOnUiThread(() =>
            {
                alert.Show();
            });


        }


        public void drawRoute()
        {


            if (locationPointsForRoute.Count == 0)
                Toast.MakeText(this, "No routes found for: " + routeId, ToastLength.Long).Show();
            else
            {


                try
                {


                    mMap.UiSettings.RotateGesturesEnabled = false;
                    mMap.UiSettings.ScrollGesturesEnabled = false;




                    Locations lastItem = locationPointsForRoute.LastOrDefault();
                    Locations firstElement = locationPointsForRoute.First();

                    Locations middlePoint = locationPointsForRoute[locationPointsForRoute.Count / 2];

                   // string[] LatLngLast = lastItem.Location.Split(',');
                  //  string[] LatLngFirst = firstElement.Location.Split(',');
                  //  string[] LatMid= middlePoint.Location.Split(',');
                    


                    markerOpt1 = new MarkerOptions();
                    markerOpt1.SetPosition(new LatLng(firstElement.Lat, firstElement.Lon));
                     // markerOpt1.SetPosition(new LatLng(Convert.ToDouble(LatLngFirst[0]), Convert.ToDouble(LatLngFirst[1])));
                   

                    markerOpt1.SetTitle("Starting Point");
                    markerOpt1.Draggable(false);
                    markerOpt1.SetSnippet("Starting point of route");
                    markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueGreen));
                    mMap.AddMarker(markerOpt1);

                    

                    markerOpt2 = new MarkerOptions();
                    markerOpt2.SetPosition(new LatLng(lastItem.Lat, lastItem.Lon));

                  //  markerOpt2.SetPosition(new LatLng(Convert.ToDouble(LatLngLast[0]), Convert.ToDouble(LatLngLast[1])));
                    markerOpt2.SetTitle("Ending Point");
                    markerOpt2.Draggable(false);
                    markerOpt2.SetSnippet("End point of route");
                    markerOpt2.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
                    mMap.AddMarker(markerOpt2);

                    mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
                    // mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(Convert.ToDouble(LatMid[0]), Convert.ToDouble(LatMid[1])), 14));
                    mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(middlePoint.Lat, middlePoint.Lon), 14));

                    PolylineOptions opt = new PolylineOptions();

                   // string[] loc;
                    foreach (var item in locationPointsForRoute)
                    {
                       // loc = null;
                        // loc = item.Location.Split(',');
                      //  loc = item.lo
                        opt.Add(new LatLng(item.Lat, item.Lon));
                        //opt.Add(new LatLng(Convert.ToDouble(loc[0]), Convert.ToDouble(loc[1])));
                        

                    }

                    mMap.AddPolyline(opt);


                }
                catch (Exception)
                {

                   
                }


            }
        }


        bool routeIsRunning()
        {
            bool val = false;
            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);

            alert.SetTitle("Route is already active!");
            alert.SetMessage("Route is already active! Start a new Route?");
            alert.SetPositiveButton("Yes", (senderAlert, args) => {

                StopService(new Intent(this, typeof(StartRouteService)));
                val = true;

            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                val = false;

            });
            //run the alert in UI thread to display in the screen
            RunOnUiThread(() => {
                alert.Show();
            });

            return val;
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

                    //Intent myIntent = new Intent(this, typeof(WelcomeScreen));
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