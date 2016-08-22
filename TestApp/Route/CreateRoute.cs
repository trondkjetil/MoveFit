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
using System.Threading;
using TestApp.Points;
using Android.Graphics;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;


namespace TestApp
{
  //  [Activity(Label = "Route", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/Theme2")]

    [Activity(AlwaysRetainTaskState = true, ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = Android.Content.PM.LaunchMode.SingleInstance, Label = "Route", Theme = "@style/Theme2")]
  //  [IntentFilter(new[] { Intent.ActionAssist }, Categories = new[] { Intent.CategoryDefault })]

    public class CreateRoute : AppCompatActivity, IOnMapReadyCallback //, ILocationListener
    {
        const long MIN_TIME = 5 * 1000; // Minimum time interval for update in seconds, i.e. 5 seconds.
        const long MIN_DISTANCE = 0;

        SupportToolbar toolbar;
        public LocationManager locationManager;
        public string locationProvider;
        public MarkerOptions markerOpt1;
        public MarkerOptions markerOpt2;
        public GoogleMap mMap;

        public List<User> me;
        public static string givenRouteName;
        static string routeInfo;
        static string routeDifficulty;
        static string routeType;
        static string routeUserId;

        public List<Location> points;

        public static Location startLocation;
        public static Location endLocation;
        public Location currentLocation;

        public static bool isChecked;
        public static bool alreadyDone;
        public static bool isReady;
        public static TextView routeStatus;
        public static ImageView statusImage;
        public static string routeId;

        double dist;

        public Stopwatch stopWatch;
        public string elapsedTime;
        public static Activity activity;
        public Spinner spinner;
        public ToggleButton start;
        public static bool isPaused;

        static bool firstRun;
        ListView list;
        public int typeToDraw;
        public bool Ischecked
        {

            set
            {

                isChecked = value;

                if (!alreadyDone && isChecked && isReady)
                {
                    //  startRouteSettings();
                    alreadyDone = true;
                }
            }

            get { return isChecked; }
        }


        public override void OnBackPressed()
        {
            //base.OnBackPressed();
            MoveTaskToBack(true);

            //AlertDialog.Builder alert = new AlertDialog.Builder(this);
            
            //alert.SetTitle("Exit route creation");
            //alert.SetMessage("Do you want to abort the current route creation?");
            //alert.SetPositiveButton("Yes", (senderAlert, args) =>
            //{
            //    //change value write your own set of instructions
            //    //you can also create an event for the same in xamarin
            //    //instead of writing things here
            //    StopService(new Intent(this, typeof(CreateRouteService)));
            //   
            //    StopService(new Intent(this, typeof(CreateRouteService)));
            //});

            //alert.SetNegativeButton("Cancel", (senderAlert, args) =>
            //{
            //    //perform your own task for this conditional button click

            //});
            ////run the alert in UI thread to display in the screen
            //RunOnUiThread(() =>
            //{
            //    alert.Show();
            //});


        }





        protected async override void OnCreate(Bundle savedInstanceState)
        {
           // RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.createRoute);

            activity = this;
            isPaused = false;
            firstRun = true;

            typeToDraw = 0;

            points = new List<Location>();
            me = await Azure.getUserInstanceByName(MainStart.userName);

            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mMap = mapFrag.Map;


            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);


            SupportActionBar.SetDisplayShowTitleEnabled(false);

            markerOpt1 = new MarkerOptions();
            markerOpt2 = new MarkerOptions();

            if (mMap != null)
            {
                mMap.MapType = GoogleMap.MapTypeTerrain;  // The GoogleMap object is ready to go.
            }
            mMap.UiSettings.ZoomControlsEnabled = true;
            mMap.UiSettings.CompassEnabled = true;
            //mMap.SetOnMyLocationChangeListener;

            //spinner = FindViewById<Spinner>(Resource.Id.createRoute);

            //spinner.ItemSelected += spinner_ItemSelected;

            list = FindViewById<ListView>(Resource.Id.createRoute);
            //  spinner_ItemSelected; 
          //  list.ItemSelected += List_ItemSelected;
           

          String[] array = { "Walking", "Running", "Hiking", "Bicycling", "Skiing" };
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItemChecked, array);
            //spinner.Adapter = adapter;
            //spinner.SetSelection(0);
           // list.SetSelection(0);
            list.Adapter = adapter;
            list.ChoiceMode = Android.Widget.ChoiceMode.Single;
            list.SetItemChecked(0, true);


            list.ItemClick += (a, e) =>
            {
                var item = this.list.GetItemAtPosition(e.Position);

                //Make a toast with the item name just to show it was clicked
              //  Toast.MakeText(this, item.ToString() + " Clicked!", ToastLength.Short).Show();

                if (e.Position == 0)
                {
                    Toast.MakeText(this, "Walking", ToastLength.Short).Show();
                    routeType = "Walking";
                    typeToDraw = 1;
                }
                else if (e.Position == 1)
                {
                    Toast.MakeText(this, "Running", ToastLength.Short).Show();
                    routeType = "Running";
                    typeToDraw = 2;
                }
                else if (e.Position == 2)
                {
                    Toast.MakeText(this, "Hiking", ToastLength.Short).Show();
                    routeType = "Hiking";
                     typeToDraw = 3;
                }
                else if (e.Position == 3)
                {
                    Toast.MakeText(this, "Bicycling", ToastLength.Short).Show();
                    routeType = "Bicycling";
                    typeToDraw = 4;
                }
                else if (e.Position == 4)
                {
                    Toast.MakeText(this, "Skiing", ToastLength.Short).Show();
                    routeType = "Skiing";
                    typeToDraw = 5;
                }
            };


            //android: checkMark = "@drawable/acceptfriend"

            //  android: checkMark = "?android:attr/listChoiceIndicatorSingle"



            //spinner.ItemSelected += spinner_ItemSelected;
            //var adapter = routeTypeadapter;
            //spinner.Adapter = adapter;


            //    spinner.Visibility = ViewStates.Visible;


            statusImage = FindViewById<ImageView>(Resource.Id.imageStatus);
            TextView routeTitle = FindViewById<TextView>(Resource.Id.routeTitle);
            routeStatus = FindViewById<TextView>(Resource.Id.statusRoute);
            routeStatus.Text = "Stauts: Idle";


            // Button start = FindViewById<Button>(Resource.Id.startRoute);
            //Button pause = FindViewById<Button>(Resource.Id.pauseRoute);

            start = FindViewById<ToggleButton>(Resource.Id.toggleStart);

            //   Button end = FindViewById<Button>(Resource.Id.endRoute);
           // Button cancel = FindViewById<Button>(Resource.Id.cancelRoute);


            routeId = "";

            // fire an application-specified Intent when the device enters the proximity of a given geographical location.
            //locationManager.RemoveProximityAlert();




            start.Click += (sender, e) =>
            {

                ////Cannot create and do a route at the same time!
                if (StartRouteService.serviceIsRunning == true)
                {
                    Toast.MakeText(this, "Cannot create a route while doing one!", ToastLength.Long).Show();

                    return;
                }

              

                if (start.Checked)
                {
                    startRouteCreation();
                }
                else
                    finishRoute();



                //if (firstRun)
                //{
                //    firstRun = false;
                //    startRouteCreation();

                //}
                //else
                //{
                //    isPaused = false;

                //    StartService(new Intent(this, typeof(CreateRouteService)));
                //    routeStatus.Text = "Resuming creation";
                //    start.Text = "Pause Route";
                //    start.SetBackgroundColor(Color.Green);
                //}



                //         }

             




                //Resuming a route creation
                //if (!start.Checked)
                //{

                //if (CreateRouteService.serviceIsRunning == true)
                //{
                //    isPaused = true;
                //    StopService(new Intent(this, typeof(CreateRouteService)));
                //    routeStatus.Text = "Route creation paused";
                //    start.SetBackgroundColor(Color.Blue);


                //    //bool val = routeIsRunning();
                //    //if (val)
                //    //    return;


                //}







                //   }


                //Starting route creation for first time






                //    if (points.Count > 0)
                //    {

                //        points.Clear();
                //}



                //routeStatus.Text = "Aquiring your position...";

                //var loc = App.Current.LocationService.getLastKnownLocation();

                //if (loc != null)
                //{
                //    mMap.Clear();
                //    mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
                //    mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(loc.Latitude, loc.Longitude), 14));

                //    MarkerOptions markerMe = new MarkerOptions();
                //    markerMe.SetPosition(new LatLng(loc.Latitude, loc.Longitude));
                //    markerMe.SetTitle("My position");
                //    markerMe.Draggable(false);
                //    markerMe.SetSnippet("My Location");
                //    BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(MainStart.profilePic);

                //    markerMe.SetIcon(image);
                //    mMap.AddMarker(markerMe);
                //}



                //StartService(new Intent(this, typeof(CreateRouteService)));

                ////mMap.Clear();
                //isReady = false;
                //Ischecked = false;
                //alreadyDone = false;


                //stopWatch = new Stopwatch();
                //stopWatch.Start();

                //spinner.Visibility = ViewStates.Invisible;

                //} // en Pause


            };
            //end.Click +=  (sender, e) =>
            //{
            //    if (CreateRouteService.serviceIsRunning == false)
            //    {
            //        Toast.MakeText(this, "Nothing to stop!", ToastLength.Short).Show();
            //        return;

            //    }


            //    points = CreateRouteService.getPoints();


            //    StopService(new Intent(this, typeof(CreateRouteService)));


            //    routeStatus.Text = "Stauts: Stopped";
            //    dist = 0;
            //    Toast.MakeText(this, "Ending route...", ToastLength.Short).Show();
            //    statusImage.SetImageResource(Resource.Drawable.red);

            //    drawRoute();

            //    startDialogNameRoute();


            //    stopWatch.Stop();
            //    TimeSpan ts = stopWatch.Elapsed;
            //    elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    ts.Hours, ts.Minutes, ts.Seconds,
            //    ts.Milliseconds / 10);


            //    firstRun = true;
            //};



            //cancel.Click += (sender, e) =>
            //{
            //    StopService(new Intent(this, typeof(CreateRouteService)));
            //    //  locationManager.RemoveUpdates(this);
            //    Finish();
            //};

        }

        //private void List_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        //{
        //    // var t = list.GetItemAtPosition(e.Position);
           
        //  //  list.SetItemChecked(e.Position, true);

        //    if (e.Position == 0)
        //    {
        //        Toast.MakeText(this, "Walking", ToastLength.Short).Show();
        //        routeType = "Walking";
        //    }
        //    else if (e.Position == 1)
        //    {
        //        Toast.MakeText(this, "Running", ToastLength.Short).Show();
        //        routeType = "Running";
        //    }
        //    else if (e.Position == 2)
        //    {
        //        Toast.MakeText(this, "Hiking", ToastLength.Short).Show();
        //        routeType = "Hiking";
        //    }
        //    else if (e.Position == 3)
        //    {
        //        Toast.MakeText(this, "Bicycling", ToastLength.Short).Show();
        //        routeType = "Bicycling";
        //    }
        //    else if (e.Position == 4)
        //    {
        //        Toast.MakeText(this, "Skiing", ToastLength.Short).Show();
        //        routeType = "Skiing";
        //    }
            
          
        //}

        public void finishRoute()
        {

            if (CreateRouteService.serviceIsRunning == false)
            {
                Toast.MakeText(this, "Nothing to stop!", ToastLength.Short).Show();
                return;

            }

            start.Enabled = false;
            points = CreateRouteService.getPoints();

            try
            {

            points = filterLocationPoints(points);

            }
            catch (Exception)
            {

             
            }

            StopService(new Intent(this, typeof(CreateRouteService)));


            routeStatus.Text = "Stauts: Stopped";
            dist = 0;
            Toast.MakeText(this, "Ending route...", ToastLength.Short).Show();
            statusImage.SetImageResource(Resource.Drawable.red);

            drawRoute(typeToDraw );

            startDialogNameRoute();


            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);


            firstRun = true;

          
        }
        public void startDialogNameRoute()
        {

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            DialogStartRoute newDialog = new DialogStartRoute();
            newDialog.DialogClosed += OnDialogClosed;
            newDialog.Show(transaction, "Start Route");
        }
        async void OnDialogClosed(object sender, DialogStartRoute.DialogEventArgs e)
        {
            String[] returnData;
            returnData = e.ReturnValue.Split(',');
            givenRouteName = returnData[0];
            routeInfo = returnData[1];
            routeDifficulty = returnData[2];



            foreach (var item in me)
            {
                routeUserId = item.Id;
            }

            try
            {


                routeStatus.Text = "Waiting to upload the route...";
                dist = calculateDistance();
                var first = points[0];


                List<Route> routeHere = await Azure.AddRoute(givenRouteName, routeInfo, dist.ToString(), "0", 1, routeDifficulty, routeType, routeUserId, elapsedTime, first.Latitude, first.Longitude);
                Azure.addToMyDistance(MainStart.userId, dist);
                string routeID = "";

                List<Route> te = await Azure.getLatestRouteId(routeUserId);
                foreach (var item in te)
                {
                    routeID = item.Id;
                }


                foreach (var item in points)
                {

                    Azure.AddLocation(item.Latitude, item.Longitude, routeID);
                }

                if (dist == 0)
                    dist = getDistanceForRoute(startLocation, endLocation);

                int mypoints = MyPoints.calculatePoints(routeType, (int)dist);
                var pointAdded = Azure.addToMyPoints(routeUserId, mypoints);

                statusImage.SetImageResource(Resource.Drawable.orange);
                Toast.MakeText(this, "Uploading successful", ToastLength.Long).Show();
                routeStatus.Text = "Status: Idle";
                Toast.MakeText(this, "You have earned " + mypoints + " points on on a " + routeType + " route", ToastLength.Long).Show();

                start.Enabled = true;
                list.Visibility = ViewStates.Visible;

            }
            catch (Exception eg)
            {

                throw eg;
            }


        }
        public void startRouteCreation()
        {
            start.SetBackgroundColor(Color.Blue);
            list.Visibility = ViewStates.Invisible;

            if (points.Count > 0)
            {

                points.Clear();
            }

            start.Enabled = false;

            routeStatus.Text = "Aquiring your position...";

            var loc = App.Current.LocationService.getLastKnownLocation();

            if (loc != null)
            {
                mMap.Clear();
                mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
                mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(loc.Latitude, loc.Longitude), 14));

                MarkerOptions markerMe = new MarkerOptions();
                markerMe.SetPosition(new LatLng(loc.Latitude, loc.Longitude));
                markerMe.SetTitle("My position");
                markerMe.Draggable(false);
                markerMe.SetSnippet("My Location");
                BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(MainStart.profilePic);

                markerMe.SetIcon(image);
                mMap.AddMarker(markerMe);
            }



            StartService(new Intent(this, typeof(CreateRouteService)));

            //mMap.Clear();
            isReady = false;
            Ischecked = false;
            alreadyDone = false;


            stopWatch = new Stopwatch();
            stopWatch.Start();


            start.Enabled = true;

            //  spinner.Visibility = ViewStates.Invisible;

        } // en Pause


        //public void startRouteSettings()
        //{

        //    try
        //    {

        //        startLocation = locationManager.GetLastKnownLocation(locationProvider);
        //        statusImage.SetImageResource(Resource.Drawable.green);
        //       // routeStatus.Text = "Stauts: Creating...";

        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}


            public List<Location> filterLocationPoints(List<Location> points)
        {
            List<Location> sortedLocation = new List<Location>();
            double dist = 0;

                for (int i = 0; i < points.Count; i++)
            {
                if(points[i + 1] != null)
                {

                    var firstPoint = points[i];
                    var secondPoint = points[i + 1];

                    dist = getDistanceForRoute(firstPoint, secondPoint);
                    if(dist >= 10)
                    {
                        sortedLocation.Add(points[i]);

                    }

                }
          

            }





            return sortedLocation;
        }


        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
        }

        // Set very accurate Accuracy for locating
        void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Medium,
                PowerRequirement = Power.Medium


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
          

        }
        protected override void OnPause()
        {
          base.OnPause();
           
        }


        public void OnProviderDisabled(string provider)
        {

        }

        public void OnProviderEnabled(string provider)
        {
            //OnProviderEnabled: called when the user enables a provider, such as GPS or network.
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {

        }

      


        //private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        //{
        //   // Spinner spinner = (Spinner)sender;

          
        //    if (e.Position == 0)
        //    {
        //        Toast.MakeText(this, "Walking", ToastLength.Short).Show();
        //        routeType = "Walking";
        //    }
        //    else if (e.Position == 1)
        //    {
        //        Toast.MakeText(this, "Running", ToastLength.Short).Show();
        //        routeType = "Running";
        //    }
        //    else if (e.Position == 2)
        //    {
        //        Toast.MakeText(this, "Hiking", ToastLength.Short).Show();
        //        routeType = "Hiking";
        //    }
        //    else if (e.Position == 3)
        //    {
        //        Toast.MakeText(this, "Bicycling", ToastLength.Short).Show();
        //        routeType = "Bicycling";
        //    }
        //    else if (e.Position == 4)
        //    {
        //        Toast.MakeText(this, "Skiing", ToastLength.Short).Show();
        //        routeType = "Skiing";
        //    }



        //}

        public double getDistanceForRoute(Location start, Location end)
        {

            double distance = 0;
            try
            {
                distance = calculateDistance(start.Latitude, start.Longitude, end.Latitude, end.Longitude);
            }
            catch (Exception)
            {

                //throw;
            }
            return distance;
        }

        public double calculateDistance()
        {

            double totalLength = 0;

            for (int i = 0; i < points.Count - 1; i++)
            {
                if (points[i + 1] != null)
                    totalLength += calculateDistance(points[i].Latitude, points[i].Longitude, points[i + 1].Latitude, points[i + 1].Longitude);

            }

            return totalLength;
        }

        public double calculateDistance(double fromLatitude, double fromLongitude, double toLatitude, double toLongitude)
        {

            float[] results = new float[1];
            int distance = 0;
            LatLng source = new LatLng(fromLatitude, fromLongitude);
            LatLng destination = new LatLng(toLatitude, toLongitude);

            try
            {
                Location.DistanceBetween(fromLatitude, fromLongitude, toLatitude, toLongitude, results);
            }
            catch (Exception)
            {

            }
            if (source.Equals(destination))
            {
                distance = 0;
            }
            else
            {
                distance = (int)results[0];


            }
            return distance;
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

                //case Resource.Id.exit:
                //    OnBackPressed();
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
                   

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }


        }

        bool routeIsRunning()
        {

            bool val = false;
            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);

            alert.SetTitle("Route is already being created!");
            alert.SetMessage("Route is already being created! Start a new Route?");
            alert.SetPositiveButton("Yes", (senderAlert, args) => {

                StopService(new Intent(this, typeof(CreateRouteService)));
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
        public void drawRoute(int type)
        {
            Location lastItem = points.LastOrDefault();
            Location firstElement = points.First();

            mMap.Clear();

            markerOpt1.SetPosition(new LatLng(firstElement.Latitude, firstElement.Longitude));
            markerOpt1.SetTitle("Starting Point");
            markerOpt1.Draggable(false);
            markerOpt1.SetSnippet("Starting point of route");
            markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueGreen));
            mMap.AddMarker(markerOpt1);

            markerOpt2 = new MarkerOptions();
            markerOpt2.SetPosition(new LatLng(lastItem.Latitude, lastItem.Longitude));
            markerOpt2.SetTitle("Ending Point");
            markerOpt2.Draggable(false);
            markerOpt2.SetSnippet("End point of route");
            markerOpt2.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
            mMap.AddMarker(markerOpt2);

            mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
            mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(firstElement.Latitude, firstElement.Longitude), 14));

            PolylineOptions opt = new PolylineOptions();

           
            if(type == 1)
            {
                opt.InvokeColor(Color.Black);

            }else if( type == 2)
            {
                opt.InvokeColor(Color.IndianRed);
            }else if(type == 3)
            {
                opt.InvokeColor(Color.BlueViolet);

            }
            else if (type == 4)
            {
                opt.InvokeColor(Color.DarkOrchid);
            }else
                opt.InvokeColor(Color.DarkOliveGreen);



            foreach (var item in points)
            {
                opt.Add(new LatLng(item.Latitude, item.Longitude));
            }

            mMap.AddPolyline(opt);


        }






    }







}


