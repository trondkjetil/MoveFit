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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            SetContentView(Resource.Layout.settings);



            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbars);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);


           
           
            RadioGroup radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            RadioButton radioButton = FindViewById<RadioButton>(radioGroup.CheckedRadioButtonId);
            RadioButton radioButton2 = FindViewById<RadioButton>(radioGroup.CheckedRadioButtonId);
            Button save = FindViewById<Button>(Resource.Id.save);

            RadioGroup timeInterValGroup = FindViewById<RadioGroup>(Resource.Id.rad1);
            RadioButton min45 = FindViewById<RadioButton>(timeInterValGroup.CheckedRadioButtonId);
            RadioButton min60 = FindViewById<RadioButton>(timeInterValGroup.CheckedRadioButtonId);
            RadioButton min120 = FindViewById<RadioButton>(timeInterValGroup.CheckedRadioButtonId);
            SeekBar _seekBar = FindViewById<SeekBar>(Resource.Id.seekBar1);
            _seekBar.Max = 80;
            _seekBar.Progress = 20;


            var list = IOUtilz.LoadPreferences();
            if(list[2] != 0)
            {
                if (list[2] == 45)
                {
                    min45.Checked = true;
                }
                else if (list[2] == 60)
                {
                    min60.Checked = true;
                }

                else
                    min120.Checked = true;




                if (list[1] == 0)
                {
                    radioButton.Checked = true;
                }
                else
                    radioButton2.Checked = true;

                _seekBar.Progress = list[0];
            }
           

            save.Click += (e, a) =>
             {
                 IOUtilz.SavePreferences(unitType, distance, interval);
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
                }else
                    interval = 120;

            };

            radioGroup.CheckedChange += (s, e) =>
            {

                if (radioButton.Checked)
                {
                    unitType = 0;

                }
                else
                    unitType = 1;


            };



            _seekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                  
                    distance = e.Progress;
                    Toast.MakeText(this, e.Progress, ToastLength.Short).Show();
                }
            };



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

                case Resource.Id.back:
                    OnBackPressed();
                    return true;

                case Resource.Id.home:

                    //Intent myIntent = new Intent(this, typeof(WelcomeScreen));
                    //StartActivity(myIntent);

                    OnBackPressed();


                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }


        }







    }
}