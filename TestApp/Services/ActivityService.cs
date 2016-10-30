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
    [Service]
    public class SimpleService : Service, ISensorEventListener
    {

        static readonly string TAG = "X:" + typeof(SimpleService).Name;

        public static bool isChecked;
        public static bool isRunning;
        SensorManager sensorManager;

        private float[] mGravity;
        private float mAccel;
        private float mAccelCurrent;
        private float mAccelLast;
        public static Stopwatch timer;
        public TimeSpan start;
        public TimeSpan end;
        int[] pref;

        public float timeInterval;


        public override void OnCreate()
        {
            base.OnCreate();

            //start = TimeSpan.Parse("21:00"); // 10 PM
            //end = TimeSpan.Parse("09:00");
            pref = IOUtilz.LoadPreferences();
            start = new TimeSpan(08, 0, 0);
            end = new TimeSpan(22, 0, 0);
        }



        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            timer = new Stopwatch();
            new Task(() =>
            {

                isChecked = true;
                isRunning = true;

                
                timeInterval = pref[2];
                if (timeInterval == 0 || timeInterval == 60)
                    timeInterval = 59;


                Log.Debug(TAG, "OnStartCommand called at {2}, flags={0}, startid={1}", flags, startId, DateTime.UtcNow);

                sensorManager = (SensorManager)GetSystemService(Context.SensorService);
                mAccel = 0.00f;
                mAccelCurrent = SensorManager.GravityEarth;
                mAccelLast = SensorManager.GravityEarth;

                sensorManager.RegisterListener(this,
                sensorManager.GetDefaultSensor(SensorType.Accelerometer),
                SensorDelay.Ui);

              
                timer.Start();
                Log.Debug(TAG, "Counter: {0}, startid={1}", timer.Elapsed, DateTime.UtcNow);


            }).Start();

            return StartCommandResult.NotSticky;
        }


        public override void OnDestroy()
        {
            isRunning = false;
            StopService(new Intent(this, typeof(SimpleService)));
            sensorManager.UnregisterListener(this);
            Log.Debug(TAG, "SimpleService destroyed at {0}.", DateTime.UtcNow);
            isChecked = false;
            base.OnDestroy();

        }



        public override IBinder OnBind(Intent intent)
        {
            // This example isn't of a bound service, so we just return NULL.
            return null;
        }



        public void OnSensorChanged(SensorEvent e)
        {
            mGravity = e.Values.ToArray();

            // Shake detection
            float x = mGravity[0];
            float y = mGravity[1];
            float z = mGravity[2];
            mAccelLast = mAccelCurrent;
            mAccelCurrent = (float)Math.Sqrt(x * x + y * y + z * z);
            float delta = mAccelCurrent - mAccelLast;
            mAccel = mAccel * 0.9f + delta;


            if (mAccel >= 7)
            {
                timer.Restart();
            }

            //  if (mAccel < 5 && (Convert.ToDouble(timer.Elapsed.Seconds) == timeInterval) || (Convert.ToDouble(timer.Elapsed.Seconds) >= timeInterval && Convert.ToDouble(timer.Elapsed.Seconds) <= timeInterval + 8))

                double minutesPassed = Convert.ToDouble(timer.Elapsed.Minutes);
                double hourPassed = Convert.ToDouble(timer.Elapsed.Hours);



            if (pref[2] == 120)
            {

                if ((mAccel < 5 && hourPassed == 2))
                {
                    soundAlarm();
                }

            }
            else
            {

                if ((mAccel < 5 && minutesPassed == timeInterval) || (minutesPassed >= timeInterval && minutesPassed <= timeInterval + 1))
                {
                    mAccel = 0.00f;


                    soundAlarm();


                }

            }


        }
        public void soundAlarm()
        {
            if (isChecked == true && DateTime.Now.TimeOfDay <= end && DateTime.Now.TimeOfDay >= start)
            {

                var inte = new Intent(this, typeof(ActivityLevelTracker));
                inte.AddFlags(ActivityFlags.NewTask);
                StartActivity(inte);
                isChecked = false;

            }

            timer.Stop();
            timer.Reset();
        }
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {

        }

    }
}
























