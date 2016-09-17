
using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using System.Threading;
using Android.Hardware;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TestApp
{

     [Service(IsolatedProcess = true)]
    [IntentFilter(new String[] { "com.xamarin.DemoService" })]
    public class LogoutService : Service
    {


        public static Stopwatch timer;
        public TimeSpan start;
        public TimeSpan end;
        public string userId;

        public double timeInterval;


        public override void OnCreate()
        {
            base.OnCreate();

        }


        public override void OnTaskRemoved(Intent rootIntent)
        {

            //unregister listeners
            //do any other cleanup if required
            logout();
            //stop service
            StopSelf();
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            new Task(() => {

                Log.Debug("LogoutService", "OnStartCommand called at {2}, flags={0}, startid={1}", flags, startId, DateTime.UtcNow);

                timer = new Stopwatch();
                timer.Start();
                try
                {
                    userId = MainStart.userId;
                }
                catch (Exception)
                {
                    userId = "-facebook|10153939416251346";

                }
                if (userId == null)
                {
                    userId = "-facebook|10153939416251346";
                }


                Log.Debug("LogoutService", "Counter: {0}, startid={1}", timer.Elapsed, DateTime.UtcNow);
                //  startf
                updateLocationTimer();

            }).Start();

            //return StartCommandResult.NotSticky;
            //return StartCommandResult.Sticky;
            return StartCommandResult.RedeliverIntent;
        }


        public void updateLocationTimer()
        {
            var timer = new Timer((e) =>
            {
                User timerTest = null;
                try
                {
                    timerTest = MainStart.userInstanceOne;
                }
                catch (Exception)
                {
                    logout();

                }

                if (timerTest == null)
                {
                    logout();
                    StopService(new Intent(this, typeof(LogoutService)));
                }


            }, null, 0, Convert.ToInt32(TimeSpan.FromMinutes(1).TotalMilliseconds));

        }
        public async void logout()
        {
            try
            {
                var user = await Azure.SetUserOnline(userId, false);
            }
            catch (Exception)
            {

            }
        }

        public override void OnDestroy()
        {
            // childThread.Abort();
            StopService(new Intent(this, typeof(LogoutService)));

            Log.Debug("LogoutService", "SimpleService destroyed at {0}.", DateTime.UtcNow);

            base.OnDestroy();

        }



        public override IBinder OnBind(Intent intent)
        {
            // This example isn't of a bound service, so we just return NULL.
            return null;
        }




    }
}

