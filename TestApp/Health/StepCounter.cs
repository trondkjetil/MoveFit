using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Hardware;
using System.ComponentModel;
using Android.Graphics;

namespace TestApp
{
    [Activity(Label = "StepCounter")]
    public class StepCounter : Activity, ISensorEventListener, INotifyPropertyChanged
    {

        public bool isRunning;
        public event PropertyChangedEventHandler PropertyChanged;

        private int stepCounter = 0;
        private int counterSteps = 0;
        private int stepDetector = 0;
        TextView resultView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.stepCounter);


             resultView = FindViewById<TextView>(Resource.Id.stepCounter);
             RegisterListeners(SensorType.StepCounter);

          if (!IOUtilz.IsKitKatWithStepCounter(PackageManager))
            {
                Toast.MakeText(this, "Device not compatible", ToastLength.Short).Show();
                resultView.Text = "Sorry! Your device is not compatible with this step counter";
                resultView.SetBackgroundColor(Color.Red);

            }
            
                //RunOnUiThread(() =>
                //{
                //    resultView.Text = stepCounter.ToString();

                //});
            


            }




        protected override void OnStart()
        {
            base.OnStart();

            //if (isRunning || !IOUtilz.IsKitKatWithStepCounter(PackageManager))
            //    return;

            RegisterListeners(SensorType.StepCounter);
        }




        void RegisterListeners(SensorType sensorType)
        {


            var sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            var sensor = sensorManager.GetDefaultSensor(sensorType);

            sensorManager.RegisterListener(this, sensor, SensorDelay.Normal);
            
        }


        void UnregisterListeners()
        {

            var sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            sensorManager.UnregisterListener(this);
          

            isRunning = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterListeners();
            isRunning = false;
        }








        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            Toast.MakeText(this,"accuracy changed", ToastLength.Short).Show();
        }

        public void OnSensorChanged(SensorEvent e)
        {


            Toast.MakeText(this, "Step taken!", ToastLength.Short).Show();

            switch (e.Sensor.Type)
            {
                case SensorType.StepDetector:
                    stepDetector++;
                    Toast.MakeText(this, stepDetector, ToastLength.Short).Show();
                    break;
                case SensorType.StepCounter:
                    //Since it will return the total number since we registered we need to subtract the initial amount
                    //for the current steps since we opened app
                    if (counterSteps < 1)
                    {
                        // initial value
                        counterSteps = (int)e.Values[0];
                    }

                    // Calculate steps taken based on first counter value received.
                    stepCounter = (int)e.Values[0] - counterSteps;
                    break;
            }

            RunOnUiThread(() =>
            {
                resultView.Text = stepCounter.ToString();

            });


        }
    }
}