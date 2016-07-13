using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using System.Threading;
using Android.Hardware;
using System.Linq;
using System.Threading.Tasks;


namespace TestApp
{
	[Service]
	public class SimpleService : Service,ISensorEventListener
	{

		static readonly string TAG = "X:" + typeof (SimpleService).Name;
		SensorManager sensorManager;

		private float[] mGravity;
		private float mAccel;
		private float mAccelCurrent;
		private float mAccelLast;

        public static bool status;
        public static bool Status {

            get { return status; }
            set
            {
                status = value;
            }
        }
		static int counter { get; set;}
	
	public ThreadStart th;
	public Thread childThread;

		// Equivalent to oncreate method
			public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
			{

			new Task (() => {

				Log.Debug(TAG, "OnStartCommand called at {2}, flags={0}, startid={1}", flags, startId, DateTime.UtcNow);
				counter = 0;
				sensorManager = (SensorManager) GetSystemService(Context.SensorService);
				mAccel = 0.00f;
				mAccelCurrent = SensorManager.GravityEarth;
				mAccelLast = SensorManager.GravityEarth;

				    sensorManager.RegisterListener(this,
					sensorManager.GetDefaultSensor(SensorType.Accelerometer),
					SensorDelay.Ui);
	
				 th = new ThreadStart (Counter);
				 childThread = new Thread(th);
				 childThread.Start();

				Thread.Sleep (1000);

				//childThread.Abort ();

				
			}).Start();

				return StartCommandResult.NotSticky;
			}
       

	    public static void Counter(){
			Log.Debug (TAG, "Starting at: {0}", DateTime.UtcNow);

			try{
			        counter = 0;
				for (int teller = 0; teller <= 150; teller++){
				Thread.Sleep(1000);
					counter++;
				Log.Debug (TAG, "Counter: {0}, startid={1}", counter, DateTime.UtcNow);

			}
                Thread.CurrentThread.Abort();
            }
            catch (ThreadAbortException e){
				Console.WriteLine("Thread Abort Exception" + e.Message);
				}
				finally {
                Thread.CurrentThread.Abort();
            }
       
			//Only use thread abort() to quit
//				Thread.CurrentThread.IsBackground = true; 

		
		}


		public override void OnDestroy()
		{

            childThread.Abort();
            StopService(new Intent(this, typeof(SimpleService)));
            sensorManager.UnregisterListener(this);
            Log.Debug(TAG, "SimpleService destroyed at {0}.", DateTime.UtcNow);


            base.OnDestroy();
           
			
		}

	

		public override IBinder OnBind(Intent intent)
		{
			// This example isn't of a bound service, so we just return NULL.
			return null;
		}



		public void OnSensorChanged (SensorEvent e)
		{
	
		//	Log.Debug(TAG, "DATA  {2}, flags={0}, startid={1}",  e.Values[0], e.Values[1], DateTime.UtcNow);
			//Log.Debug(TAG, "Counter {0}, startid={1}",counter, DateTime.UtcNow);
			mGravity = e.Values.ToArray();
			//	mGravity = event.values.clone();
			// 	mGravity =
			// Shake detection
			float x = mGravity[0];
			float y = mGravity[1];
			float z = mGravity[2];
			mAccelLast = mAccelCurrent;
			mAccelCurrent = (float) Math.Sqrt(x*x + y*y + z*z);
			float delta = mAccelCurrent - mAccelLast;
			mAccel = mAccel * 0.9f + delta;

			// Make this higher or lower according to how much
			// motion you want to detect

			if(mAccel < 8 && counter > 1800 ){ 
				counter = 0;
                mAccel = 0.00f;
                var inte = new Intent(this, typeof(ActivityLevelTracker));
                inte.AddFlags(ActivityFlags.NewTask);
                StartActivity(inte);


            }
		
		}


		public void OnAccuracyChanged (Sensor sensor, SensorStatus accuracy)
		{

		}

	}
}

