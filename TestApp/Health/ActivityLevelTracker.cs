using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Android.Content.PM;
using Android.Hardware;
using System.Linq;

namespace TestApp
{
	[Activity (Label = "Activity",ScreenOrientation = ScreenOrientation.Portrait)]			
	public class ActivityLevelTracker : Activity, ISensorEventListener

    {
        private float[] mGravity;
        private float mAccel;
        private float mAccelCurrent;
        private float mAccelLast;


        static readonly object syncLock = new object ();
		TextView sensorTextView;
		Random rand;

		MediaPlayer player;
		public WebView webLoadingIcon;
		Button stopbtn;

		public bool inactive;
		public float inactiveTime;

		public string startTime;
		public string endTime;
		public TimeSpan timeOfday;
		public int level;
        public TextView healthFacts;
        SensorManager sensorManager;
        static int counter { get; set; }

        protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.accelerometer);
	

			sensorTextView = FindViewById<TextView>(Resource.Id.accelerometer_text);
			rand = new Random ();
			inactive = true;

			timeOfday = DateTime.Now.TimeOfDay;
			startTime = DateTime.Now.ToString ("h:mm:ss tt");

             healthFacts = FindViewById<TextView>(Resource.Id.facts);

			stopbtn = FindViewById<Button> (Resource.Id.stop);
			stopbtn.Click += stopAlarm;



            sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            mAccel = 0.00f;
            mAccelCurrent = SensorManager.GravityEarth;
            mAccelLast = SensorManager.GravityEarth;

            sensorManager.RegisterListener(this,
            sensorManager.GetDefaultSensor(SensorType.Accelerometer),
            SensorDelay.Ui);


            Alarm ();
			stopbtn.Click += stopAlarm;

		}

	

		public void showFacts()
        {

            
            string[] facts = new string[11];
            facts[0] = "Regular physical activity can help you prevent or manage a wide range of " +
                        "health problems and concerns, including stroke, metabolic syndrome, type 2 " +
                         " diabetes, depression, certain types of cancer, arthritis and falls.";


            facts[1] = "Physical activity stimulates various brain chemicals that may leave you feeling" +
                        " happier and more relaxed.";

            facts[2] = "You may feel better about your appearance and yourself when you exercise" +
                        " regularly, which can boost your confidence and improve your self - esteem";

            facts[3] = "Regular physical activity can improve your muscle strength and boost your endurance.";

            facts[4] = "Exercise and physical activity deliver oxygen and nutrients to your tissues and help " +
                " your cardiovascular system work more efficiently. And when your heart and lungs" +
                "work more efficiently, you have more energy to go about your daily chores.";

            facts[5] = "Regular physical activity can help you fall asleep faster and deepen your sleep. Just" +
                    "don't exercise too close to bedtime, or you may be too energized to fall asleep.";

            facts[6] = "Less than 5% of adults participate in 30 minutes of physical activity each day;2 only one in three adults receive the recommended amount of physical activity each week.";

            facts[7] = "Only 35 – 44% of adults 75 years or older are physically active, and 28-34% of adults ages 65-74 are physically active.";

            facts[8] = "More than 80% of adults do not meet the guidelines for both aerobic and muscle-strengthening activities, and more than 80% of adolescents do not do enough aerobic physical activity to meet the guidelines for youth.";

            facts[9] = "Physical activity can help you connect with family or friends in a fun social" +
                
            "setting.So, take a dance class, hit the hiking trails or join a soccer team.Find" +

            "a physical activity you enjoy, and just do it.If you get bored, try something new.";


            facts[10] = "Exercise and physical activity are a great way to feel better, gain health"+

                "benefits and have fun. As a general goal, aim for at least 30 minutes of"+

                 "physical activity every day.";


       //var d =     "A lack of exercise is now causing as many deaths as smoking across the world, study suggests";

       //     d = "Sitting for more than three hours a day can cut two years of a persons life expectancy";
            
            healthFacts.Text = facts[new Random().Next(11)];

        }

		public void stopAlarm(object sender, EventArgs e){
            stopTheAlarm(false);
		}

        private async void stopTheAlarm(bool moving)
        {

            if (moving)
            {
                var uploadPoints = await Azure.addToMyPoints(MainStart.userId, 5);
                Toast.MakeText(this, "You just earned 5 points!", ToastLength.Long).Show();


            }

            player.Stop();
         
            stopbtn.Enabled = false;

            // Has been added for test***
            //  StopService(new Intent(this, typeof(SimpleService)));
            sensorManager.UnregisterListener(this);
            Finish();
        }

        public void Alarm(){

            showFacts();
            player = MediaPlayer.Create (this, Resource.Raw.moveIt);
			player.SetVolume (100, 100);
			player.Start ();
		}


			
		protected override void OnDestroy()
		{
            
			
			base.OnDestroy();
           
            SimpleService.isChecked = true;
            SimpleService.timer.Restart();


        }



        public void OnSensorChanged(SensorEvent e)
        {

            //Log.Debug(TAG, "Counter {0}, startid={1}",counter, DateTime.UtcNow);
            mGravity = e.Values.ToArray();
            
            // Shake detection
            float x = mGravity[0];
            float y = mGravity[1];
            float z = mGravity[2];
            mAccelLast = mAccelCurrent;
            mAccelCurrent = (float)Math.Sqrt(x * x + y * y + z * z);
            float delta = mAccelCurrent - mAccelLast;
            mAccel = mAccel * 0.9f + delta;

         

            if (mAccel >  6)
            {
                counter = 0;
                mAccel = 0.00f;

                stopTheAlarm(true);
                
            }

        }
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {

        }

    }
}

