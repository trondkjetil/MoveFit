using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Locations;
using System.Diagnostics;
using Android.Content.PM;
using TestApp.Points;
using Android.Graphics;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using System.Text.RegularExpressions;
using Android.Media;

namespace TestApp
{
    [Activity(LaunchMode = LaunchMode.SingleInstance,Label = "StartRoute", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/Theme2")]
    public class StartRoute : AppCompatActivity, ILocationListener
    {

        const long MIN_TIME = 10 * 1000;
        const long MIN_DISTANCE = 5;
        readonly int DISTANCE_AWAY = 100;
        static bool completed;

        SupportToolbar toolbar;
       // LocationManager locationManager;
       // string locationProvider;
        MarkerOptions markerOpt1;
        MarkerOptions markerOpt2;
        GoogleMap mMap;
        List<Route> routes;
        public static String[] array;
        public List<Locations> locationPointsForRoute;
        public List<Locations> locationPointsForRouteVerify;
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
        public static bool record;
        public static string newRecordTime;
        public static string oldRecordTime;
        string userId;
        User instance;
        Marker myMark;

        public RatingBar ratingbar;

        TextView time;
        TextView name;
        TextView description;
        TextView length;
        TextView difficulty;
        TextView rating;
        TextView trips;
        TextView type;
        TextView createdBy;

        MapFragment mapFrag;

        Android.Net.Uri notification;
        Ringtone r;
        List<Location> myRoutePoints;
        static bool alarm;
        public void toggleInfoVisibility(bool visible)
        {
            
            if (visible)
            {
                time.Visibility = ViewStates.Visible;
                 name.Visibility = ViewStates.Visible;
                description.Visibility = ViewStates.Visible;
                length.Visibility = ViewStates.Visible;
                difficulty.Visibility = ViewStates.Visible;
                rating.Visibility = ViewStates.Visible;
                trips.Visibility = ViewStates.Visible;
                type.Visibility = ViewStates.Visible;
                createdBy.Visibility = ViewStates.Visible;

                ratingbar.Visibility = ViewStates.Visible;

                
                            ViewGroup.LayoutParams paramseters = mapFrag.View.LayoutParameters;
                            paramseters.Height = 450;
                            mapFrag.View.LayoutParameters=paramseters;
            }
            else
            {
                time.Visibility = ViewStates.Gone;
                name.Visibility = ViewStates.Gone;
                description.Visibility = ViewStates.Gone;
                length.Visibility = ViewStates.Gone;
                difficulty.Visibility = ViewStates.Gone;
                rating.Visibility = ViewStates.Gone;
                trips.Visibility = ViewStates.Gone;
                type.Visibility = ViewStates.Gone;
                createdBy.Visibility = ViewStates.Gone;
                ratingbar.Visibility = ViewStates.Gone;

                ViewGroup.LayoutParams paramseters = mapFrag.View.LayoutParameters;
                paramseters.Height = 800;
                mapFrag.View.LayoutParameters = paramseters;

            }

        }

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.startRoute);

           notification = RingtoneManager.GetDefaultUri(RingtoneType.Alarm);
            r = RingtoneManager.GetRingtone(ApplicationContext, notification);


            routes = new List<Route>();
             mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.mapForStartingRoute);
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

              
                toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayShowTitleEnabled(false);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);

                 name = FindViewById<TextView>(Resource.Id.startRouteName);
                 description = FindViewById<TextView>(Resource.Id.startRouteDesc);
                 length = FindViewById<TextView>(Resource.Id.startRouteLength);
                 difficulty = FindViewById<TextView>(Resource.Id.startRouteDiff);
                 rating = FindViewById<TextView>(Resource.Id.startRouteRating);
                 trips = FindViewById<TextView>(Resource.Id.startRouteTrips);
                 type = FindViewById<TextView>(Resource.Id.startRouteType);
                 time = FindViewById<TextView>(Resource.Id.startRouteTime);
                 createdBy = FindViewById<TextView>(Resource.Id.createdby);
                 start = FindViewById<ToggleButton>(Resource.Id.toggleStart);

              
                 ratingbar = FindViewById<RatingBar>(Resource.Id.ratingbar);

                 ratingbar.Enabled = false;
                //ratingbar.Clickable = false;
                //ratingbar.Visibility = ViewStates.Visible;
                ratingbar.Rating = 0;


                array = Intent.GetStringArrayExtra("MyData");

                routeId = array[7];

                locationPointsForRoute = await Azure.getLocationsForRoute(routeId);

                locationPointsForRouteVerify = locationPointsForRoute;

                List<Route> route = await Azure.getRouteById(routeId);
                var routeInstance = route.FirstOrDefault();

                routeName = routeInstance.Name;
                routeInfo = routeInstance.Info;
                routeDifficulty = routeInstance.Difficulty;
                routeLength = routeInstance.Distance;
                routeType = routeInstance.RouteType;
                routeRating = routeInstance.Review;
                routeTrips = routeInstance.Trips.ToString();
                routeTime = routeInstance.Time;
                userId = routeInstance.User_id;


                completed = false;
                //userId = array[9];
                //routeName = array[0];
                //routeInfo = array[1];
                //routeDifficulty = array[2];
                //routeLength = array[3];
                //routeType = array[4];
                //routeRating = array[5];
                //routeTrips = array[6];
                //routeTime = array[8];
                //userId = array[9];


                name.Text = "Name: " + routeName;
                description.Text = "Description: " + routeInfo;
                difficulty.Text = "Difficulty: " + routeDifficulty;

                string unit = " km";
                double lengthOfRoute = 0;

                var test = IOUtilz.LoadPreferences();
                if (test[1] == 1)
                {
                    unit = " miles";
                    lengthOfRoute = (int)IOUtilz.ConvertKilometersToMiles(Convert.ToDouble(routeLength) / 1000);

                }
                else
                {
                    lengthOfRoute = Convert.ToDouble(routeLength) / 1000;
                }

                length.Text = "Length: " + lengthOfRoute + unit;  //routeLength;
                type.Text = "Type: " + routeType;
                trips.Text = "Trips: " + routeTrips;
                time.Text = "Best time: " + routeTime;

                instance = await Azure.getUser(userId);
                record = false;

                try
                {

                    createdBy.Text = instance.UserName;
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
                catch (Exception)
                {
                   
                }


                start.Click += (sender, e) =>
                {

                    if (start.Checked)
                    {
                        startRoute();
                    }
                    else if(!start.Checked)
                    {
                        endRoute();
                        
                    }
                      

                };

            }

        }
        private void startRoute()
        {
            if (CreateRouteService.serviceIsRunning == true)
            {
                Toast.MakeText(this, "Cannot start a route while creating one!", ToastLength.Long).Show();

                return;
            }

    

            if (StartRouteService.serviceIsRunning == true)
            {
                var val = routeIsRunning();

                if (val == true)
                    return;

            }

            start.Enabled = false;

            double distance = distanceFromRoutePoint(true);

            if (distance <= DISTANCE_AWAY)

            {


                Toast.MakeText(this, "Starting route...", ToastLength.Long).Show();
                StartService(new Intent(this, typeof(StartRouteService)));
                myRoutePoints = new List<Location>();
                completed = false;
                alarm = true;
                stopWatch = new Stopwatch();
                stopWatch.Start();
                toggleInfoVisibility(false);

                App.Current.LocationService.LocationChanged += HandleCustomEvent;


                SupportActionBar.SetDisplayShowTitleEnabled(true);
                SupportActionBar.Title = "Route Active...";
                
            }
            else
            {
                Toast.MakeText(this, "Please move closer to the starting point!", ToastLength.Short).Show();
                start.Checked = false;
                
            }
           
            start.Enabled = true;
        }

        void HandleCustomEvent(object e, LocationChangedEventArgs a)
        {

            if (myMark != null)
            {
                myMark.Remove();
            }
            myRoutePoints.Add(a.Location);
            Bitmap flagStart = BitmapFactory.DecodeResource(Resources, Resource.Drawable.startF);
            var startPoint = BitmapDescriptorFactory.FromBitmap(IOUtilz.scaleDown(flagStart, 80, false)); //(Resource.Drawable.test);

            MarkerOptions liveTrackingMarker = new MarkerOptions();
            liveTrackingMarker.SetPosition(new LatLng(a.Location.Latitude, a.Location.Longitude));
            liveTrackingMarker.SetTitle("My position");
            liveTrackingMarker.Draggable(false);

            BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(MainStart.profilePic);
            liveTrackingMarker.SetIcon(image);
            myMark = mMap.AddMarker(liveTrackingMarker);


            verifyRoute(a.Location);  
            
                   
        }
        private async void endRoute()
        {
           
            if (StartRouteService.serviceIsRunning == false)
            {
                Toast.MakeText(this, "Nothing to stop!", ToastLength.Short).Show();
                return;

            }

         

            // Distance from finish
            double distance = distanceFromRoutePoint(false);
            bool completed = routeCompleted();

            if (!completed)
            {
                App.Current.LocationService.LocationChanged -= HandleCustomEvent;
                StopService(new Intent(this, typeof(StartRouteService)));
                start.Checked = false;
                start.Enabled = true;
                SupportActionBar.SetDisplayShowTitleEnabled(false);
                SupportActionBar.Title = "";
                toggleInfoVisibility(true);
                alarm = false;
                Toast.MakeText(this, "You didnt reach all the check-points! Try again..", ToastLength.Short).Show();

                return;
            }

            if (distance <= DISTANCE_AWAY && completed)
            {


            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.Title = "";
            alarm = false;
            App.Current.LocationService.LocationChanged -= HandleCustomEvent;
                StopService(new Intent(this, typeof(StartRouteService)));
                toggleInfoVisibility(true);
            

            start.Enabled = false;
            List<float> speedList = StartRouteService.points;

            float sum = 0;
            avgSpeed = 0;

            foreach (var item in speedList)
            {

                sum += item;

            }


            if (sum != 0)
            {
                avgSpeed = sum / speedList.Count();
            }
            else
                avgSpeed = 0;


          

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

          
            newRecordTime = "";
            oldRecordTime = "";
            
         
                DateTime newTime = new DateTime();
                DateTime oldTime = new DateTime();
                try
                {

                    newTime = Convert.ToDateTime(elapsedTime);
                    routeTime = Regex.Replace(routeTime, "[^0-9.+-: ]", "");
                    oldTime = Convert.ToDateTime(routeTime);
                }
                catch (Exception)
                {
                   
                 

                }



                int result = DateTime.Compare(newTime, oldTime);

                if (result <= 0 )
                {
                    Toast.MakeText(this, "Congratulations! You have made a new Record!", ToastLength.Short).Show();
                    record = true;

                    newRecordTime = elapsedTime;
                    oldRecordTime = routeTime;
                  

                    var setRecord = await Azure.updateRouteBestTimeUser(routeId,newRecordTime, MainStart.userName);
                    //newRecordTime.Replace(" ", "");
                    //time.Text = "Best time: " + newRecordTime + " by " + MainStart.userName;
                }
                else
                {
                    Toast.MakeText(this, "Congratulations! You have finished the route!", ToastLength.Long).Show();

                }

                //startDialogNameRoute();

                //if (locationManager != null)
                //    locationManager.RemoveUpdates(this);

                int mypoints = MyPoints.calculatePoints(routeType, Math.Round(distance));

                if (mypoints > 50)
                {
                    var done = Azure.addToMyPoints(routeId, mypoints);
                }
                else
                    mypoints = 0;

                var complete = Azure.increaseTripCount(routeId);
                Azure.addToMyDistance(MainStart.userId, distance);

                startDialogNameRoute();
                Toast.MakeText(this, "You have earned " + mypoints + " points!", ToastLength.Long).Show();

               

            }
            else
            {
                Toast.MakeText(this, "Please move closer to the finish-line!" + "Distance to finishline is: " + distance, ToastLength.Long).Show();

                start.Checked = true;
            }

            
            start.Enabled = true;
        }

        private double distanceFromRoutePoint(bool start)
        {
            double distance = 0;
            try
            {

                Location myLocation = App.Current.LocationService.getLastKnownLocation();
                Locations firstElement = null;
                if (start)
                {
                    firstElement = locationPointsForRoute.First();
                }
                else
                    firstElement = locationPointsForRoute.LastOrDefault();





                float[] results = new float[1];
           // double[] LatLngFirst = firstElement.Location.Split(',');
            Location.DistanceBetween(myLocation.Latitude, myLocation.Longitude, firstElement.Lat, firstElement.Lon, results);
            //Location.DistanceBetween(myLocation.Latitude, myLocation.Longitude, Convert.ToDouble(LatLngFirst[0]), Convert.ToDouble(LatLngFirst[1]), results);

            distance = (int)results[0];

            }
            catch (Exception)
            {
           
            }

            return distance;
        }
        private double calculateDistance(Location location1, Locations location2)
        {
            double distance = 0;
            try
            {

                  float[] results = new float[1];
                // double[] LatLngFirst = firstElement.Location.Split(',');
                Location.DistanceBetween(location1.Latitude, location1.Longitude, location2.Lat, location2.Lon, results);
                //Location.DistanceBetween(myLocation.Latitude, myLocation.Longitude, Convert.ToDouble(LatLngFirst[0]), Convert.ToDouble(LatLngFirst[1]), results);


                distance = (int)results[0];

            }
            catch (Exception)
            {


            }

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
           
            if(routeRating == "" || routeRating == null)
            {
                routeRating = "0";
            }

            var user = MainStart.userInstanceOne;
                if(user == null)
            {
              var listUser =  await Azure.getUserByAuthId(MainStart.userId);
              user = listUser.FirstOrDefault();
            }
               

            var reviewInstance = await Azure.getRouteReview(routeId);
            int amountOfRatings = reviewInstance.Count();


            bool noPreviousReview = false;
            foreach (var item in reviewInstance)
            {
                if(item.UserId == MainStart.userId)
                {
                    noPreviousReview = true;
                }
            }

            if (!noPreviousReview)
            {
                Azure.AddReview(routeId, Convert.ToInt32(routeRating), user.Id);

            }


            reviewInstance = await Azure.getRouteReview(routeId);

            int rating = 0;
            foreach (var item in reviewInstance)
            {
                if(item.UserId != MainStart.userId)
                {
                    rating = rating + item.Rating;
                }
               

            }

            double totalRate = 0;
            if (rating != 0)
            {
                totalRate = rating / amountOfRatings;
                totalRate = Math.Round(totalRate);
            }

            ratingbar.Rating = Convert.ToInt32(totalRate);
            await Azure.giveRouteRating(routeId, totalRate.ToString());

            OnResume();
     
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;

        }

          protected override void OnDestroy()
        {
            base.OnDestroy();
            try
            {
                App.Current.LocationService.LocationChanged -= HandleCustomEvent;
                StopService(new Intent(this, typeof(StartRouteService)));

            }
            catch (Exception)
            {

              
            }
        
        }
        protected async override void OnResume()
        {
            base.OnResume();
            //if (locationManager != null)
            //    locationManager.RequestLocationUpdates(this.locationProvider, MIN_TIME, MIN_DISTANCE, this);

            routes = await Azure.getRouteById(routeId);
            if(routes.Count != 0)
            {
                time.Text = routes.FirstOrDefault().Time;
                ratingbar.Rating = Convert.ToInt32(routes.FirstOrDefault().Review);
                ratingbar.Progress = (int) ratingbar.Rating;
               
            }
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




        }

     public bool routeCompleted()
        {

           // var approvedList = new List<Location>();
            // locationPointsForRoute();
            int count = 0;
            int randomIndex = new Random().Next(1, locationPointsForRouteVerify.Count-1);
            int indexx = 0;

            int middle = 0;
            double mid = 0;

            try
            {
                mid = Math.Round(locationPointsForRouteVerify.Count / 2.0);
                middle = Convert.ToInt32(mid);
            }
            catch (Exception)
            {
                middle = middle - 1;
            }
          


            try
            {
    
            double almostThere = Math.Round(locationPointsForRouteVerify.Count / 3.0);
             indexx = Convert.ToInt32(almostThere);

            }
            catch (Exception)
            {
                indexx = 0;


            }

            bool[] checkList = new bool[5];
            foreach (var myPoint in myRoutePoints)
            {

                if (checkList[0] != true && calculateDistance(myPoint, locationPointsForRouteVerify[0]) <= 200)
                {
                    checkList[0] = true;
                }

                if (checkList[1] != true && calculateDistance(myPoint, locationPointsForRouteVerify[middle]) <= 200)
                {
                    checkList[1] = true;

                }

                if (checkList[2] != true && calculateDistance(myPoint, locationPointsForRouteVerify.LastOrDefault()) <= 200)
                {
                    checkList[2] = true;

                }


                try
                {
             
                if (checkList[3] != true && calculateDistance(myPoint, locationPointsForRouteVerify[randomIndex]) <= 200)
                {
                    checkList[3] = true;

                }

                    if (checkList[4] != true && calculateDistance(myPoint, locationPointsForRouteVerify[indexx]) <= 200)
                    {
                        checkList[4] = true;

                    }
                }
                catch (Exception)
                {

                   
                }

            }


            for(int i = 0; i < checkList.Length; i++)
            {
                if(checkList[i] == true)
                {
                    count++;
                }
            }


            if (count >= 3 && checkList[1] == true)
            {
                return true;
            }
            else
                return false;
               
        }
        public void verifyRoute(Location loc)
        {

            bool drifting = false;      
           
            List<int> checks = new List<int>();

            foreach (var item in locationPointsForRouteVerify)
            {
                if (calculateDistance(loc, item) >= 2)
                {
                   checks.Add(1);
                }
            }

            if(checks.Count == locationPointsForRouteVerify.Count)
            {
                drifting = true;
            }

            //instanceToRemove = locationPointsForRouteVerify.FirstOrDefault();
            //locationPointsForRouteVerify.Remove(instanceToRemove);
            App.Current.LocationService.LocationChanged -= HandleCustomEvent;


            if (drifting && alarm)
            {
               
                  r.Play();

            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle("Out of track");
            alert.SetMessage("You seem to be drifting away from the route! Get back on track to finish route");
            alert.SetPositiveButton("Ok", (senderAlert, args) =>
            {
                      r.Stop();
                App.Current.LocationService.LocationChanged += HandleCustomEvent;
            });

            alert.SetNegativeButton("Deactivate alarm", (senderAlert, args) =>
            {
                //    locationManager.RequestLocationUpdates(this.locationProvider, MIN_TIME, MIN_DISTANCE, this);
                r.Stop();
                alarm = false;
                App.Current.LocationService.LocationChanged += HandleCustomEvent;

            });
         
                alert.Show();
            }

        }




        public override void OnBackPressed()
        {
            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);

            alert.SetTitle("Exit route");
            alert.SetMessage("Do you want to abort the current route?");
            alert.SetPositiveButton("Yes", (senderAlert, args) =>
            {

                base.OnBackPressed();
                Finish();
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
                Toast.MakeText(this, "No routes found for: " + routeName, ToastLength.Long).Show();
            else
            {

                try
                {

                    mMap.UiSettings.RotateGesturesEnabled = true;
                    mMap.UiSettings.ScrollGesturesEnabled = true;

                    Locations lastItem = locationPointsForRoute.LastOrDefault();
                    Locations firstElement = locationPointsForRoute.First();

                    Locations middlePoint = locationPointsForRoute[locationPointsForRoute.Count / 2];

                    markerOpt1 = new MarkerOptions();
                    markerOpt1.SetPosition(new LatLng(firstElement.Lat, firstElement.Lon));
                    // markerOpt1.SetPosition(new LatLng(Convert.ToDouble(LatLngFirst[0]), Convert.ToDouble(LatLngFirst[1])));

                    Bitmap flagStart = BitmapFactory.DecodeResource(Resources, Resource.Drawable.startF);
                    var startPoint = BitmapDescriptorFactory.FromBitmap(IOUtilz.scaleDown(flagStart, 80, false)); //(Resource.Drawable.test);

                    markerOpt1.SetTitle("Starting Point");
                    markerOpt1.Draggable(false);
                    markerOpt1.SetSnippet("Starting point of route");
                    markerOpt1.SetIcon(startPoint);

                    mMap.AddMarker(markerOpt1);

                    Bitmap flagEnd = BitmapFactory.DecodeResource(Resources, Resource.Drawable.finishF);
                    var endPoint = BitmapDescriptorFactory.FromBitmap(IOUtilz.scaleDown(flagEnd, 80, false)); //(Resource.Drawable.test);

                    markerOpt2 = new MarkerOptions();
                    markerOpt2.SetPosition(new LatLng(lastItem.Lat, lastItem.Lon));

                  //  markerOpt2.SetPosition(new LatLng(Convert.ToDouble(LatLngLast[0]), Convert.ToDouble(LatLngLast[1])));
                    markerOpt2.SetTitle("Ending Point");
                    markerOpt2.Draggable(false);
                    markerOpt2.SetSnippet("End point of route");
                  //  markerOpt2.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
                    markerOpt2.SetIcon(endPoint);
                    
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

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {

                         case Android.Resource.Id.Home:// Resource.Id.back:
                    OnBackPressed();
                    return true;

                case Resource.Id.home:
                    OnBackPressed();
                    Finish();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }


        }


    }
}