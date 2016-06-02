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
using Android.Content.PM;


namespace TestApp
{
    [Activity(Label = "Route", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CreateRoute : Activity, ILocationListener, IOnMapReadyCallback
    {
        
        public LocationManager locationManager;
        public string locationProvider;
        public MarkerOptions markerOpt1;
        public MarkerOptions markerOpt2;
        public GoogleMap mMap;
       
        public List<User> me;
        public static string givenRouteName;

        static string routeName;
        static string routeInfo;
        static string routeDistance;
        static string routeReview;
        static int routeTrips;
        static string routeDifficulty;
        static string routeType;
        static string  routeUserId;

        public List<Location> points;

        public static Location startLocation;
        public static Location endLocation;
        public Location currentLocation;

        public static bool isChecked;
        public static bool alreadyDone;
        public static bool isReady;
        public TextView routeStatus;
        public ImageView statusImage;
        public bool Ischecked
        {

            set {

                isChecked = value;

                if (!alreadyDone && isChecked && isReady)
                {
                    startRouteSettings();
                    alreadyDone = true;
                }
            }

            get { return isChecked; }
        }
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.createRoute);

            points = new List<Location>();
            me = await Azure.getUserId(MainStart.userName);

            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mMap = mapFrag.Map;
            markerOpt1 = new MarkerOptions();
            markerOpt2 = new MarkerOptions();

            if (mMap != null)
            {
                mMap.MapType = GoogleMap.MapTypeTerrain;  // The GoogleMap object is ready to go.
            }


            //mMap.SetOnMyLocationChangeListener;


            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinnerRouteTypes);
            spinner.ItemSelected += spinner_ItemSelected;
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.activity_routeTypes, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            statusImage = FindViewById<ImageView>(Resource.Id.imageStatus);
            TextView routeTitle = FindViewById<TextView>(Resource.Id.routeTitle);
            routeStatus = FindViewById<TextView>(Resource.Id.statusRoute);
            routeStatus.Text = "Stauts: Idle";
            Button start = FindViewById<Button>(Resource.Id.startRoute);
            Button end = FindViewById<Button>(Resource.Id.endRoute);
            Button cancel = FindViewById<Button>(Resource.Id.cancelRoute);

            InitializeLocationManager();
       
           


            start.Click += (sender, e) =>
            {
                mMap.Clear();
                // fire an application-specified Intent when the device enters the proximity of a given geographical location.
                //locationManager.RemoveProximityAlert();

                Toast.MakeText(this,"Starting route waiting for position...",ToastLength.Short).Show();

                long minTime = 7 * 1000; // Minimum time interval for update in seconds, i.e. 5 seconds.
                long minDistance = 5; // Minimum distance change for update in meters, i.e. 10 meters.

                locationManager.RequestLocationUpdates(this.locationProvider, minTime,
                          minDistance, this);
                //  InitializeLocationManager();
               
                routeStatus.Text = "Aquiring your position...";

                startDialogNameRoute();
             

            };
            end.Click += (sender, e) =>
            {
                routeStatus.Text = "Stauts: Stopped";
                double dist = 0;
                Toast.MakeText(this, "Ending route...", ToastLength.Short).Show();
                statusImage.SetImageResource(Resource.Drawable.red);
             

                endLocation = locationManager.GetLastKnownLocation(locationProvider);

                try
                {
                    dist = getDistanceForRoute(startLocation, endLocation);


                    if (dist == 0 || dist.Equals(null))
                    {
                        dist = 0;
                    }
                    Azure.AddRoute(givenRouteName, routeInfo, dist.ToString() + " Start to end" + " Whole distance" + calculateDistance().ToString(), "RouteReview", 1, routeDifficulty, routeType, routeUserId);

                    locationManager.RemoveUpdates(this);
                    uploadLocation();
                    drawRoute();

                }
                catch (Exception)
                {

                   
                }
               

          

                
            };
            cancel.Click += (sender, e) =>
            {
                locationManager.RemoveUpdates(this);
                Finish();
            };

        }

       public void uploadLocation()
        {
            String routePoints ="";
            Location lastPoint;
                foreach (var item in points)
            {
                lastPoint = item;
                if (!lastPoint.Equals(item) || !item.Equals(lastPoint))
                {
                    routePoints = routePoints + item.Longitude + "*" + item.Latitude;
                }
                
            }

            Azure.AddLocation(routePoints, routeUserId);

        }
      
       
        public void startDialogNameRoute()
        {

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            DialogStartRoute newDialog = new DialogStartRoute();
            newDialog.DialogClosed += OnDialogClosed;
            newDialog.Show(transaction, "Start Route");
        }
        void OnDialogClosed(object sender, DialogStartRoute.DialogEventArgs e)
        {
            String[] returnData; 
            returnData = e.ReturnValue.Split(',');
            givenRouteName = returnData[0];
            routeInfo = returnData[1];
            routeDifficulty = returnData[2];
            isReady = true;
           // startRouteSettings();
        //    Toast.MakeText(this, givenRouteName, ToastLength.Long).Show();
        }

     public void startRouteSettings()
        {

            try
            {
               
                startLocation = locationManager.GetLastKnownLocation(locationProvider);
                foreach (var item in me)
                {
                    routeUserId = item.Id;
                }

                //CameraUpdate center = CameraUpdateFactory.NewLatLng(new LatLng(startLocation.Latitude, startLocation.Longitude));
                //mMap.MoveCamera(center);

                //CameraUpdate zoom = CameraUpdateFactory.ZoomTo(12);
                //mMap.AnimateCamera(zoom);
                statusImage.SetImageResource(Resource.Drawable.green);
                routeStatus.Text = "Stauts: Creating...";

            }
            catch (Exception)
            {
                throw;
            }
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

            //long minTime = 30 * 1000; // Minimum time interval for update in seconds, i.e. 5 seconds.
            //long minDistance = 100; // Minimum distance change for update in meters, i.e. 10 meters.

            //locationManager.RequestLocationUpdates(this.locationProvider, minTime,
            //          minDistance, this);

        }


        protected override void OnResume()
        {
            base.OnResume();
            //locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
           // locationManager.RemoveUpdates(this);
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

            currentLocation = location;
            Ischecked = true;
            try {
                mMap.UiSettings.ZoomControlsEnabled = true;
                mMap.UiSettings.CompassEnabled = true;
                mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
                mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(location.Latitude, location.Longitude), 14));

                Toast.MakeText(this, "Location point taken", ToastLength.Long).Show();
                Azure.AddLocation(location.Latitude.ToString() + "," + location.Longitude.ToString()
               , routeUserId);
            points.Add(location);

            }catch(Exception e)
            {
                throw;
            }


        }


        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            if (e.Position == 0)
            {
                Toast.MakeText(this, "Walking", ToastLength.Short).Show();
                routeType = "Walking";
            }
            else if (e.Position == 1)
            {
                Toast.MakeText(this, "Running", ToastLength.Short).Show();
                routeType = "Running";
            }
            else if (e.Position == 2)
            {
                Toast.MakeText(this, "Hiking", ToastLength.Short).Show();
                routeType = "Hiking";
            }
            else if (e.Position == 3)
            {
                Toast.MakeText(this, "Bicycling", ToastLength.Short).Show();
                routeType = "Bicycling";
            }
            else if (e.Position == 4)
            {
                Toast.MakeText(this, "Skiing", ToastLength.Short).Show();
                routeType = "Skiing";
            }



        }

       public double getDistanceForRoute(Location start,Location end)
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

        public double calculateDistance() {

            double totalLength = 0;

            for (int i = 0; i < points.Count - 1; i++)
            {
                if(points[i+1]!= null)
                totalLength += calculateDistance(points[i].Latitude, points[i].Longitude, points[i+1].Latitude, points[i+1].Longitude);

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
            catch (Exception e)
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


    
       public void drawRoute()
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
            mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(startLocation.Latitude,startLocation.Longitude), 14));

            PolylineOptions opt = new PolylineOptions();

            foreach (var item in points)
            {
                opt.Add(new LatLng(item.Latitude, item.Longitude));
            }

            mMap.AddPolyline(opt);

        }


    }
}



//public class FragActivity extends SherlockFragmentActivity implements OnMyLocationChangeListener

//private GoogleMap mMap;
//my mMap-setup:

//    if (mMap == null) {
//        // Try to obtain the map from the SupportMapFragment.
//        mMap = customMapFragment.getMap();

//        // Check if we were successful in obtaining the map.
//        if (mMap != null)
//            setUpMap();
//    }
//setUpMap-method:

//private void setUpMap()
//{
//    mMap.setMyLocationEnabled(true);
//    mMap.setOnMyLocationChangeListener(this);
//}
//and my onlocationchange:

//@Override
//public void onMyLocationChange(Location lastKnownLocation)
//{
//    CameraUpdate myLoc = CameraUpdateFactory.newCameraPosition(
//            new CameraPosition.Builder().target(new LatLng(lastKnownLocation.getLatitude(),
//                    lastKnownLocation.getLongitude())).zoom(6).build());
//    mMap.moveCamera(myLoc);
//    mMap.setOnMyLocationChangeListener(null);