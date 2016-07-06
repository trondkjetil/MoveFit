using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using System.Threading.Tasks;
using Android.Locations;
using System.Diagnostics;
using System.Collections.Generic;

namespace TestApp
{
    [Service]
	public class StartRouteService : Service
	{

		static readonly string TAG = "YYY:" + typeof (StartRouteService).Name;

        public static bool serviceIsRunning;



        Stopwatch stopWatch;
        public LocationServiceConnection con;
        public IBinder binder;

        public Location currentLocation;
        Notification.Builder builder;
        Notification notification;
        NotificationManager notificationManager;

        public static List<float> points;



        void HandleCustomEvent(object e, LocationChangedEventArgs a)
        {


            currentLocation = a.Location;

            points.Add(a.Location.Speed);
          
               
    }
        

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
			{

            new Task (() => {
                points = new List<float>();
                serviceIsRunning = true;
                Log.Debug(TAG, "OnStartCommand called at {2}, flags={0}, startid={1}", flags, startId, DateTime.UtcNow);

              
                App.Current.LocationService.LocationChanged += HandleCustomEvent;

                Intent serviceIntent = new Intent(LocationService);
                con = new LocationServiceConnection(new LocationServiceBinder(App.Current.LocationService));
                BindService(serviceIntent, con, Bind.AutoCreate);
                binder = OnBind(serviceIntent);

               
             

                Intent newIntent = new Intent(this, typeof(StartRouteService));
                // Pass some information to SecondActivity:
                newIntent.PutExtra("message", "testData");

                // Create a task stack builder to manage the back stack:
                TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);

                // Add all parents of SecondActivity to the stack: 
                stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(StartRouteService)));

                // Push the intent that starts SecondActivity onto the stack:
                stackBuilder.AddNextIntent(newIntent);

                const int pendingIntentId = 0;

                PendingIntent pendingIntent =
                PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.OneShot);



                 builder = new Notification.Builder(this)
                .SetContentIntent(pendingIntent)
                .SetContentTitle("MoveFit")
                .SetContentText(StartRoute.routeName + " is active")
                .SetDefaults(NotificationDefaults.Sound)
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


            }).Start();

				return StartCommandResult.NotSticky;
			}



    

        public override void OnDestroy()
		{
            
            base.OnDestroy();

            Log.Debug(TAG, "Destroyed at {0}",  DateTime.UtcNow);
            serviceIsRunning = false;
            if(notificationManager != null)
            {
                notificationManager.CancelAll();
                notificationManager.Dispose();
            }
          
            App.Current.LocationService.LocationChanged -= HandleCustomEvent;
          

       //     StopService(new Intent(this, typeof(StartRouteService)));



        }

	

		public override IBinder OnBind(Intent intent)
		{
			// This example isn't of a bound service, so we just return NULL.
			return null;
		}


		
		}


	

	
}

