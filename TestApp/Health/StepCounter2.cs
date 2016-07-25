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

namespace TestApp
{

    public interface StepListener
    {
         void onStep();
         void passValue();
    }

    [Activity(Label = "StepCounter2")]
    public class StepCounter2 : Activity, ISensorEventListener
    {
        public static int counter;
        TextView resultView;
        private readonly static String TAG = "StepDetector";
        private float mLimit = 10;
        private float []mLastValues = new float[3 * 2];
        private float [] mScale = new float[2];
        private float mYOffset;

        private float[] mLastDirections = new float[3 * 2];
        private float [][]mLastExtremes = { new float[3 * 2], new float[3 * 2]
    };

        private float[] mLastDiff = new float[3 * 2];
    private int mLastMatch = -1;
     private List<StepListener> mStepListeners = new List<StepListener>();


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.stepCounter);


            resultView = FindViewById<TextView>(Resource.Id.stepCounter);
            resultView.Text = "0";
            counter = 0;

            int h = 480; // TODO: remove this constant
            mYOffset = h * 0.5f;
            mScale[0] = -(h * 0.5f * (1.0f / (SensorManager.StandardGravity * 2)));
            mScale[1] = -(h * 0.5f * (1.0f / (SensorManager.MagneticFieldEarthMax)));
        }






        // public StepDetector()
        //{
        //    int h = 480; // TODO: remove this constant
        //    mYOffset = h * 0.5f;
        //    mScale[0] = -(h * 0.5f * (1.0f / (SensorManager.StandardGravity * 2)));
        //    mScale[1] = -(h * 0.5f * (1.0f / (SensorManager.MagneticFieldEarthMax)));
        //}


        public void setSensitivity(float sensitivity)
{
            float sens = 10;
            sensitivity = sens;
    mLimit = sensitivity; // 1.97  2.96  4.44  6.66  10.00  15.00  22.50  33.75  50.62
}

        public void addStepListener(StepListener sl)
        {
            mStepListeners.Add(sl);
        }

       
        public void OnSensorChanged(SensorEvent e)
{
    Sensor sensor = e.Sensor;
            Toast.MakeText(this, e.ToString(), ToastLength.Short).Show();


            if (sensor.Type == SensorType.Orientation)
            {
            }

            else
            {
                int j = (sensor.Type == SensorType.Accelerometer) ? 1 : 0;
                if (j == 1)
                {
                    float vSum = 0;
                    float v = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        v  = mYOffset + e.Values[i] * mScale[j];
                    vSum += v;
                }
                int k = 0;
                 v = 0;
                 v = vSum / 3;

                float direction = (v > mLastValues[k] ? 1 : (v < mLastValues[k] ? -1 : 0));
                if (direction == -mLastDirections[k])
                {
                    // Direction changed
                    int extType = (direction > 0 ? 0 : 1); // minumum or maximum?
                    mLastExtremes[extType][k] = mLastValues[k];
                    float diff = Math.Abs(mLastExtremes[extType][k] - mLastExtremes[1 - extType][k]);

                    if (diff > mLimit)
                    {

                        bool isAlmostAsLargeAsPrevious = diff > (mLastDiff[k] * 2 / 3);
                        bool isPreviousLargeEnough = mLastDiff[k] > (diff / 3);
                        bool isNotContra = (mLastMatch != 1 - extType);

                        if (isAlmostAsLargeAsPrevious && isPreviousLargeEnough && isNotContra)
                        {

                                //added to show steps....
                                resultView.Text = counter++.ToString();


                            foreach (var stepListener in mStepListeners)
                            {
                                stepListener.onStep();
                                    Toast.MakeText(this, "Step made!", ToastLength.Short).Show();

                                }


                                mLastMatch = extType;
                        }
                        else
                        {
                            mLastMatch = -1;
                        }
                    }
                    mLastDiff[k] = diff;
                }
                mLastDirections[k] = direction;
                mLastValues[k] = v;
            }
        }
    }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
           
        }

        public void Dispose()
        {
          
        }
    }




}