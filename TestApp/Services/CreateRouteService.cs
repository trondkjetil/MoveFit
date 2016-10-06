using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using System.Threading.Tasks;
using System.Collections.Generic;
using Android.Locations;
using Android.Widget;

namespace TestApp
{
    [Service]
	public class CreateRouteService : Service
	{

		static readonly string TAG = "XXX:" + typeof (CreateRouteService).Name;

        public static bool serviceIsRunning;

        bool locationFound;
   
        public static List<Location> points;
        public LocationServiceConnection con;
        public IBinder binder;
        public Location currentLocation;
        Notification.Builder builder;
        Notification notification;
        NotificationManager notificationManager;
        
        public static  List<Location> getPoints()
        {
            return points;
        }

 
        void HandleCustomEvent(object e, LocationChangedEventArgs a)
        {

            currentLocation = a.Location;
            points.Add(a.Location);

            if (!locationFound)
            {
                CreateRoute.routeStatus.Text = "Stauts: Creating...";
                locationFound = true;
                CreateRoute.statusImage.SetImageResource(Resource.Drawable.green);
            }

        }
        

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
			{

            //App.Current.LocationService.AccuracyHigh = true;
            //App.Current.LocationService.StartLocationUpdates();

            new Task (() => {


                serviceIsRunning = true;
                Log.Debug(TAG, "OnStartCommand called at {2}, flags={0}, startid={1}", flags, startId, DateTime.UtcNow);

                points = new List<Location>();
                points.Clear();

              

                App.Current.LocationService.LocationChanged += HandleCustomEvent;
              
                locationFound = false;
                Intent serviceIntent = new Intent(LocationService);
                con = new LocationServiceConnection(new LocationServiceBinder(App.Current.LocationService));
                BindService(serviceIntent, con, Bind.AutoCreate);
                binder = OnBind(serviceIntent);


                messageNotification();


            }).Start();

				return StartCommandResult.NotSticky;
			}




        public void messageNotification()
        {

            Notification.Builder builder;
            Notification notification;
            NotificationManager notificationManager;

            Intent newIntent = new Intent(this, typeof(CreateRoute));
            // Pass some information to SecondActivity:
             newIntent.PutExtra("Route Creating", "Route creating");

            // Create a task stack builder to manage the back stack:
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);

            // Add all parents of SecondActivity to the stack: 
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(RouteOverview)));
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(MainStart)));

            // Push the intent that starts SecondActivity onto the stack:
            stackBuilder.AddNextIntent(newIntent);

            const int pendingIntentId = 0;

            PendingIntent pendingIntent =
            PendingIntent.GetActivity(this, pendingIntentId, newIntent, PendingIntentFlags.OneShot);


            builder = new Notification.Builder(this)
              //   .SetContentIntent(pendingIntent)
              .SetContentTitle("MoveFit")
              .SetContentText("Creating route....")
              .SetDefaults(NotificationDefaults.Sound)
              .SetContentIntent(pendingIntent)
              .SetAutoCancel(true)
              // .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
              .SetSmallIcon(Resource.Drawable.tt);

            // Build the notification:
            notification = builder.Build();


            // Get the notification manager:
            notificationManager =
           GetSystemService(Context.NotificationService) as NotificationManager;



            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);


        }
        public override void OnDestroy()
		{
            StopService(new Intent(this, typeof(CreateRouteService)));
            base.OnDestroy();


            Log.Debug(TAG, "Destroyed at {0}",  DateTime.UtcNow);
            serviceIsRunning = false;

            if (notificationManager != null)
            {
                notificationManager.CancelAll();
                notificationManager.Dispose();
            }

            //App.Current.LocationService.AccuracyHigh = false;
            //App.Current.LocationService.StartLocationUpdates();


            App.Current.LocationService.LocationChanged -= HandleCustomEvent;
            //App.Current.LocationService.LocationChanged -= (ae, e) =>
            // {

            // };
            // App.Current.LocationService.UnbindService(con);


            StopService(new Intent(this, typeof(CreateRouteService)));



        }

	

		public override IBinder OnBind(Intent intent)
		{
			// This example isn't of a bound service, so we just return NULL.
			return null;
		}


		
		}


	

	
}

