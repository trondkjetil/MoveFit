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

            Button save = FindViewById<Button>(Resource.Id.save);


            //RadioGroup radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            //RadioButton radioButton = FindViewById<RadioButton>(radioGroup.CheckedRadioButtonId);
            //RadioButton radioButton2 = FindViewById<RadioButton>(radioGroup.CheckedRadioButtonId);


            //RadioGroup timeInterValGroup = FindViewById<RadioGroup>(Resource.Id.rad1);
            //RadioButton min45 = FindViewById<RadioButton>(timeInterValGroup.CheckedRadioButtonId);
            //RadioButton min60 = FindViewById<RadioButton>(timeInterValGroup.CheckedRadioButtonId);
            //RadioButton min120 = FindViewById<RadioButton>(timeInterValGroup.CheckedRadioButtonId);


            RadioGroup radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            RadioButton radioButton = FindViewById<RadioButton>(Resource.Id.radioButton1);
            RadioButton radioButton2 = FindViewById<RadioButton>(Resource.Id.radioButton2);


            RadioGroup timeInterValGroup = FindViewById<RadioGroup>(Resource.Id.rad1);
            RadioButton min45 = FindViewById<RadioButton>(Resource.Id.b1);
            RadioButton min60 = FindViewById<RadioButton>(Resource.Id.b2);
            RadioButton min120 = FindViewById<RadioButton>(Resource.Id.b3);

            SeekBar _seekBar = FindViewById<SeekBar>(Resource.Id.seekBar1);
            _seekBar.Max = 80;
            _seekBar.Progress = 0;



        

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
                   
                }
                else
                    unitType = 1;


            };



            _seekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                  
                    distance = e.Progress;
                    

                }
            };



            var list = IOUtilz.LoadPreferences();
            list = list;
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
                 //   radioButton.Enabled = true;
                }
                else if (list[1] == 1)
                {
                        radioButton2.Checked = true;
                //    radioButton2.Enabled = true;
                }
                    

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