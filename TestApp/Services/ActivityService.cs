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
	public class SimpleService : Service,ISensorEventListener
	{

		static readonly string TAG = "X:" + typeof (SimpleService).Name;
     
        public static bool isChecked;
        public static bool isRunning;
        SensorManager sensorManager;

		private float[] mGravity;
		private float mAccel;
		private float mAccelCurrent;
		private float mAccelLast;
        public static Stopwatch timer;
       
      


     


			public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
			{

			new Task (() => {

                isChecked = true;
                isRunning = true;

                Log.Debug(TAG, "OnStartCommand called at {2}, flags={0}, startid={1}", flags, startId, DateTime.UtcNow);
				
				sensorManager = (SensorManager) GetSystemService(Context.SensorService);
				mAccel = 0.00f;
				mAccelCurrent = SensorManager.GravityEarth;
				mAccelLast = SensorManager.GravityEarth;

				    sensorManager.RegisterListener(this,
					sensorManager.GetDefaultSensor(SensorType.Accelerometer),
					SensorDelay.Ui);

                timer = new Stopwatch();

                timer.Start();

                // th = new ThreadStart (Counter);
                // childThread = new Thread(th);
                // childThread.Start();

                //Thread.Sleep (1000);

                Log.Debug(TAG, "Counter: {0}, startid={1}", timer.Elapsed, DateTime.UtcNow);


            }).Start();

				return StartCommandResult.NotSticky;
			}
       

	
		
		
		public override void OnDestroy()
		{

            // childThread.Abort();

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



		public void OnSensorChanged (SensorEvent e)
		{
			mGravity = e.Values.ToArray();
		
			// Shake detection
			float x = mGravity[0];
			float y = mGravity[1];
			float z = mGravity[2];
			mAccelLast = mAccelCurrent;
			mAccelCurrent = (float) Math.Sqrt(x*x + y*y + z*z);
			float delta = mAccelCurrent - mAccelLast;
			mAccel = mAccel * 0.9f + delta;




            if (mAccel >= 7)
            {
                timer.Restart();
            }

            if (mAccel < 5 && timer.Elapsed.Minutes == 10) { //TIME_FOR_ALARM > 10 ){  //1800
                mAccel = 0.00f;

                if(timer.Elapsed.Seconds == 10 && isChecked == true)
                {
                    var inte = new Intent(this, typeof(ActivityLevelTracker));
                    inte.AddFlags(ActivityFlags.NewTask);
                    StartActivity(inte);
                    isChecked = false;

                }

                
                timer.Stop();
                timer.Reset();

              
               


            }
		
		}

		public void OnAccuracyChanged (Sensor sensor, SensorStatus accuracy)
		{

		}

	}
}

