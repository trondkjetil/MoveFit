using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;

namespace TestApp
{
	[Service]
	public class LocationService : Service, ILocationListener
	{
		public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
		public event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
		public event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };
		public event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };

        public string locationProvider;

        Location currentLocation;

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
		}

		// This gets called when StartService is called in our App class
		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			Log.Debug (logTag, "LocationService started");

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
		public void StartLocationUpdates () 
		{
            //we can set different location criteria based on requirements for our app -
            //for example, we might want to preserve power, or get extreme accuracy



            LocMgr = (LocationManager)GetSystemService(LocationService);


          

            IList<string> acceptableLocationProviders = null;

            try
            {

                Criteria criteriaForLocationService = new Criteria
                {
                    Accuracy = Accuracy.Fine
                    //PowerRequirement = Power.High


                };

                Criteria low = new Criteria
                {
                    Accuracy = Accuracy.Medium,
                    PowerRequirement = Power.Medium


                };

                var list = LocMgr.GetProviders(criteriaForLocationService, true);

                if (list.Count > 0)
                {
                    acceptableLocationProviders = LocMgr.GetProviders(criteriaForLocationService, true);

                }
                else
                {
                    
                    acceptableLocationProviders = LocMgr.GetProviders(low, true);


                }




            }
            catch (Exception)
            {

               
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
                LocMgr.RequestLocationUpdates(locationProvider, 15000, 0, this);
            }
            catch (Exception)
            {

               
            }
           




            //         var locationCriteria = new Criteria();

            //locationCriteria.Accuracy = Accuracy.Medium;
            //locationCriteria.PowerRequirement = Power.Medium;

            //// get provider: GPS, Network, etc.
            //var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);

            //Log.Debug (logTag, string.Format ("You are about to get location updates via {0}", locationProvider));

            //// Get an initial fix on location (MODIFIED FOR LONGER BREAKS)
            //LocMgr.RequestLocationUpdates(locationProvider, 10000, 0, this);

            Log.Debug (logTag, "Now sending location updates");
		}



		public override void OnDestroy ()
		{
			base.OnDestroy ();
			Log.Debug (logTag, "Service has been terminated");
		}

		#region ILocationListener implementation
		// ILocationListener is a way for the Service to subscribe for updates
		// from the System location Service

		public void OnLocationChanged (Location location)
		{
            currentLocation = location;

            this.LocationChanged (this, new LocationChangedEventArgs (location));

			// This should be updating every time we request new location updates
			// both when teh app is in the background, and in the foreground
			Log.Debug (logTag, String.Format ("Latitude is {0}", location.Latitude));
			Log.Debug (logTag, String.Format ("Longitude is {0}", location.Longitude));
			Log.Debug (logTag, String.Format ("Altitude is {0}", location.Altitude));
			Log.Debug (logTag, String.Format ("Speed is {0}", location.Speed));
			Log.Debug (logTag, String.Format ("Accuracy is {0}", location.Accuracy));
			Log.Debug (logTag, String.Format ("Bearing is {0}", location.Bearing));
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
			StatusChanged (this, new StatusChangedEventArgs (provider, status, extras));
		} 

		#endregion



        public Location getLastKnownLocation()
        {
         
            return LocMgr.GetLastKnownLocation(locationProvider);
        }

	}
}

