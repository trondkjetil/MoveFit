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
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

namespace TestApp
{
    [Activity(Label = "Settings", Theme = "@style/Theme2")]
    public class Settings : AppCompatActivity
    {
        SupportToolbar toolbar;
        int unitType;
        int distance;
        int interval;
        int tracker;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            SetContentView(Resource.Layout.settings);



            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbars);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            Button save = FindViewById<Button>(Resource.Id.save);


            TextView dist = FindViewById<TextView>(Resource.Id.textView4);
            RadioGroup radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            RadioButton radioButton = FindViewById<RadioButton>(Resource.Id.radioButton1);
            RadioButton radioButton2 = FindViewById<RadioButton>(Resource.Id.radioButton2);


            RadioGroup timeInterValGroup = FindViewById<RadioGroup>(Resource.Id.rad1);
            RadioButton min45 = FindViewById<RadioButton>(Resource.Id.b1);
            RadioButton min60 = FindViewById<RadioButton>(Resource.Id.b2);
            RadioButton min120 = FindViewById<RadioButton>(Resource.Id.b3);

            if (tracker == 0)
            {
                tracker = 1;
            }


            ImageButton alarm = FindViewById<ImageButton>(Resource.Id.alarmButton);
            Switch location = FindViewById<Switch>(Resource.Id.switch1);

            alarm.Click += (a, e) =>
            {

                if (SimpleService.isRunning == false)
                {
                    StartService(new Intent(this, typeof(SimpleService)));
                    Toast.MakeText(this, "Activity alarm activated", ToastLength.Short).Show();
                }
                else if (SimpleService.isRunning == true)
                {
                   StopService(new Intent(this, typeof(SimpleService)));
                    Toast.MakeText(this, "Activity alarm off", ToastLength.Short).Show();

                }

            };


         

            location.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                if (e.IsChecked == true)
                {
                    Toast.MakeText(this, "Your location tracking has been turned on, you are now visible!", ToastLength.Long).Show();
                    tracker = 1;
                    StartService(new Intent(this, typeof(LocationService)));
                    try
                    {
                        var a = Azure.SetUserOnline(MainStart.userId, true);

                        MainStart.isOnline = true;

                    }
                    catch (Exception)
                    {


                    }

                   MainStart. menItemOnlineIcion.SetIcon(Resource.Drawable.greenonline);
                    MainStart.menItemOnlineText.SetTitle("Online");

                }
                else if (e.IsChecked == false)
                {
                    Toast.MakeText(this, "Tracking stopped, you are now invisible!", ToastLength.Long).Show();
                    StopService(new Intent(this, typeof(LocationService)));

                    tracker = 2;
                    try
                    {
                        var b = Azure.SetUserOnline(MainStart.userId, false);
                        MainStart.isOnline = true;
                    }
                    catch (Exception)
                    {


                    }

                    MainStart.menItemOnlineIcion.SetIcon(Resource.Drawable.redoffline);
                    MainStart.menItemOnlineText.SetTitle("Offline");

                }
                
            };



            SeekBar _seekBar = FindViewById<SeekBar>(Resource.Id.seekBar1);
            _seekBar.Max = 100;
            _seekBar.Progress = 100;



            

            unitType = 0;
            distance = 100;
            interval = 45;


            save.Click += (e, a) =>
             {

              
                 IOUtilz.SavePreferences(unitType, distance, interval, tracker);
                 Toast.MakeText(this, "Settings Saved", ToastLength.Short).Show();

             };

            timeInterValGroup.CheckedChange += (s, e) =>
            {
                
                if (min45.Checked)
                {
                    interval = 45;
                }
                else if (min60.Checked)
                {

                    interval = 60;
                }else if (min120.Checked)
                {
                    interval = 120;
                }
                   

            };
           
            radioGroup.CheckedChange += (s, e) =>
            {

                if (radioButton.Checked)
                {
                    unitType = 0;
                    _seekBar.Max = 100;
                }
                else
                {
                unitType = 1;
                _seekBar.Max = 62;
                }
                  


            };



            _seekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                  
                    distance = e.Progress;

                    if (unitType == 1)
                    {
                        dist.Text = distance + " miles";
                    }
                    else
                        dist.Text = distance + " km";

                }
            };



            var list = IOUtilz.LoadPreferences();



            if (list[3] == 2)
            {
                location.Checked = false;
            }
            else if (list[3] == 1 || list[3] == 0)
            {
                location.Checked = true;
            }


            if (list[2] != 0)
            {
                if (list[2] == 45)
                {

                    //((RadioButton)timeInterValGroup.GetChildAt(0)).Checked = true;
                    min45.Checked = true;
                    //min45.Enabled = true;
                    

                }
                else if (list[2] == 60)
                {
                  //  ((RadioButton)timeInterValGroup.GetChildAt(1)).Checked = true;
                   min60.Checked = true;
                    //min60.Enabled = true;
                    //min60.Selected = true;
                }

                else if (list[2] == 120)
                {
                   // ((RadioButton)timeInterValGroup.GetChildAt(2)).Checked = true;
                    min120.Checked = true;
                    //min120.Enabled = true;
                }
               

                if (list[1] == 0)
                {
                    radioButton.Checked = true;
                    _seekBar.Max = 100;
                }
                else if (list[1] == 1)
                {
                        radioButton2.Checked = true;
                    _seekBar.Max = 62;
                   
                }



                if (unitType == 1)
                {
                    dist.Text = list[0] + " miles";
                }
                else
                    dist.Text = list[0] + " km";


                _seekBar.Progress = list[0];
            }


           

        }









        public override bool OnCreateOptionsMenu(IMenu menu)

        {
            MenuInflater.Inflate(Resource.Menu.action_menu_nav, menu);



            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {

                //case Resource.Id.exit:
                //    OnBackPressed();
                //    return true;

                case Android.Resource.Id.Home:// Resource.Id.back:
                    OnBackPressed();
                    return true;

                //case Resource.Id.home:

                //    //Intent myIntent = new Intent(this, typeof(WelcomeScreen));
                //    //StartActivity(myIntent);

                //    OnBackPressed();


                //    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }


        }







    }
}