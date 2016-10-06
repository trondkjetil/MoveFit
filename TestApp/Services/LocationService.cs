using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TestApp
{
    [Service]
    public class LocationService : Service, ILocationListener
    {
        public string userId;
        //private static readonly int UPDATE_INTERVAL = 1000 * 30 * 1;
        //private static readonly int MIN_DISTANCE = 10; // 20

        private static readonly int UPDATE_INTERVAL = 1000 * 35 * 1;
        private static readonly int MIN_DISTANCE = 18; // 20
        bool firstFix;
        public  bool accuracyHigh;
        public  bool AccuracyHigh
        {
            get { return accuracyHigh; }
            set { accuracyHigh = value; }
        }


        public static int distance;
        public static int timeInterval;
        public static int TimeInterval {

            get{ return timeInterval;}
            set { timeInterval = value; }
            }

        public static int Distance
        {

            get { return distance; }
            set { distance = value; }
        }


        public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
		public event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
		public event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };
		public event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };

        public string locationProvider;

        Location currentLocation;
        Location currentBestLocation;
        public LocationService() 
		{
		}

        // Set our location manager as the system location service
        protected LocationManager LocMgr; //= Application.Context.GetSystemService ("location") as LocationManager;

		readonly string logTag = "LocationService";
		IBinder binder;

		public override void OnCreate ()
		{
			base.OnCreate ();
			Log.Debug (logTag, "OnCreate called in the Location Service");

            LocMgr = (LocationManager)GetSystemService(LocationService);
            AccuracyHigh = false;
            firstFix = false;
        }
      
     
        // This gets called when StartService is called in our App class
        public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			Log.Debug (logTag, "LocationService started");

            //userId = MainStart.userId;
                    
            //updateLocationTimer();

            return StartCommandResult.Sticky;
		}

		// This gets called once, the first time any client bind to the Service
		// and returns an instance of the LocationServiceBinder. All future clients will
		// reuse the same instance of the binder
		public override IBinder OnBind (Intent intent)
		{
			Log.Debug (logTag, "Client now bound to service");

			binder = new LocationServiceBinder (this);
			return binder;
		}

        // Handle location updates from the location manager
        public void StartLocationUpdates()
        {


            try
            {
                LocMgr.RemoveUpdates(this);
            }
            catch (Exception)
            {
                      
            }
          

            IList<string> acceptableLocationProviders = null;
            Criteria fine = new Criteria
            {
                Accuracy = Accuracy.Fine
            };

            Criteria coarse = new Criteria
            {
                Accuracy = Accuracy.Coarse

            };

            Criteria medium = new Criteria
            {
                Accuracy = Accuracy.Medium

            };

            Criteria low = new Criteria
            {
                Accuracy = Accuracy.Low

            };


            if (AccuracyHigh == true)
            {


                try
                {
                    acceptableLocationProviders = LocMgr.GetProviders(fine, true);

                }
                catch (Exception)
                {

                }

                if (acceptableLocationProviders.Count == 0)
                {
                    try
                    {
                        acceptableLocationProviders = LocMgr.GetProviders(coarse, true);

                    }
                    catch (Exception)
                    {


                    }

                }


                if (acceptableLocationProviders.Any())
                {
                    locationProvider = acceptableLocationProviders.First();
                }
                else
                {
                    locationProvider = string.Empty;
                }

                try
                {
                    LocMgr.RequestLocationUpdates(locationProvider, UPDATE_INTERVAL, MIN_DISTANCE, this);
                }
                catch (Exception)
                {

                }

            }else if (!AccuracyHigh)
            {
           
                       if(LocMgr.IsProviderEnabled(LocationManager.NetworkProvider))
                    {


                        try
                        {
                            LocMgr.RequestLocationUpdates(LocationManager.NetworkProvider, 30, 0, this);

                        }
                        catch (Exception)
                        {


                        }


                    }
                else
                {
             
                    try
                    {

                        acceptableLocationProviders = LocMgr.GetProviders(coarse, true);
                        if (acceptableLocationProviders.Any())
                        {
                            locationProvider = acceptableLocationProviders.First();
                            LocMgr.RequestLocationUpdates(locationProvider, UPDATE_INTERVAL, MIN_DISTANCE, this);

                        }else
                        {
                            acceptableLocationProviders = LocMgr.GetProviders(medium, true);
                            locationProvider = acceptableLocationProviders.First();
                            LocMgr.RequestLocationUpdates(locationProvider, UPDATE_INTERVAL, MIN_DISTANCE, this);

                        }
                    }
                    catch (Exception)
                    {

                        
                    }

                }





            }







        }



        public override void OnDestroy ()
		{
			base.OnDestroy ();
            logout();
			Log.Debug (logTag, "Service has been terminated");
		}

		#region ILocationListener implementation
		// ILocationListener is a way for the Service to subscribe for updates
		// from the System location Service

		public void OnLocationChanged (Location location)
		{
            if(currentBestLocation == null)
            {
                currentBestLocation = location;
            }


            currentLocation = location;

            if (!firstFix)
            {
                this.LocationChanged(this, new LocationChangedEventArgs(currentLocation));
                firstFix = true;
               
                accuracyHigh = true;
                StartLocationUpdates();
            }
            else
            {
          
                 if (isBetterLocation(currentLocation, currentBestLocation))
            {
                currentBestLocation = currentLocation;
                this.LocationChanged(this, new LocationChangedEventArgs(currentLocation));
            }

            }




        }

        public void OnProviderDisabled (string provider)
		{
			ProviderDisabled (this, new ProviderDisabledEventArgs (provider));
		}

		public void OnProviderEnabled (string provider)
		{
			ProviderEnabled (this, new ProviderEnabledEventArgs (provider));
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{

            
            //if (!isSameProvider(provider, locationProvider))
            //{
            //    LocMgr.RequestLocationUpdates(provider, UPDATE_INTERVAL, MIN_DISTANCE, this);
            //    LocMgr.RemoveUpdates(this);
            //    StatusChanged(this, new StatusChangedEventArgs(provider, status, extras));

            //}

           // StatusChanged(this, new StatusChangedEventArgs(provider, status, extras));



        }

        #endregion

        public Location getLastKnownLocation()
        {
         if(currentLocation != null)
            {
             return currentLocation;

            }else

            return LocMgr.GetLastKnownLocation(locationProvider);
        }



        protected bool isBetterLocation(Location location, Location currentBestLocation)
        {
            if (currentBestLocation == null)
            {
                // A new location is always better than no location
                return true;
            }

            // Check whether the new location fix is newer or older
            //long timeDelta = location.Time - currentBestLocation.Time;
            //bool isSignificantlyNewer = timeDelta > UPDATE_INTERVAL;
            //bool isSignificantlyOlder = timeDelta < -UPDATE_INTERVAL;
            //bool isNewer = timeDelta > 0;

            // If it's been more than two minutes since the current location, use the new location
            // because the user has likely moved


            //if (isSignificantlyNewer)
            //{
            //    return true;
            //    // If the new location is more than two minutes older, it must be worse
            //}
            //else if (isSignificantlyOlder)
            //{
            //    return false;
            //}

            // Check whether the new location fix is more or less accurate
            int accuracyDelta = (int)(location.Accuracy - currentBestLocation.Accuracy);
            bool isLessAccurate = accuracyDelta > 0;
            bool isMoreAccurate = accuracyDelta <= 0;
            bool isSignificantlyLessAccurate = accuracyDelta > 200;

            // Check if the old and new location are from the same provider
            bool isFromSameProvider = isSameProvider(location.Provider,
                    currentBestLocation.Provider);

            //if (isMoreAccurate)
            //{
            //    return true;
            //}
            //else if (isNewer && !isLessAccurate)
            //{
            //    return true;
            //}
            //else if (isNewer && !isSignificantlyLessAccurate && isFromSameProvider)
            //{
            //    return true;
            //}
            // Determine location quality using a combination of timeliness and accuracy
            if (isMoreAccurate)
            {
                return true;
            }
            else if (!isLessAccurate)
            {
                return true;
            }
            else if (!isSignificantlyLessAccurate && isFromSameProvider)
            {
                return true;
            }
            return false;
        }

        /** Checks whether two providers are the same */
        private bool isSameProvider(String provider1, String provider2)
        {
            if (provider1 == null)
            {
                return provider2 == null;
            }
            return provider1.Equals(provider2);
        }


        public async void logout()
        {
            try
            {
                Azure.SetUserOnline(userId, false);
            }
            catch (Exception)
            {

            }
        }

    }
}

