using System;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace TestApp
{
    [Activity(Label = "UserProfile", Theme = "@style/Theme2")]
    public class UserProfile : AppCompatActivity
    {

        public static String[] array;
        public static IMenuItem itemProfilePic;
        public static IMenuItem itemGender;
        public static IMenuItem itemAge;
        public static IMenuItem itemExit;
        public EditText aboutMeEdit;

        SupportToolbar toolbar;
        ImageView profilePic2;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.userProfile);
            array = Intent.GetStringArrayExtra("MyData");
            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbarNew);
            SetSupportActionBar(toolbar);

            // ImageView profilePic1 = FindViewById<ImageView>(Resource.Id.profilePic1);
            profilePic2 = FindViewById<ImageView>(Resource.Id.profilePic2);

            TextView userName = FindViewById<TextView>(Resource.Id.userName);
            TextView points = FindViewById<TextView>(Resource.Id.points);
            TextView aboutMe = FindViewById<TextView>(Resource.Id.aboutMe);

            Button editImage = FindViewById<Button>(Resource.Id.editImage);

             aboutMeEdit = FindViewById<EditText>(Resource.Id.aboutMeEdit);


            try
            {


                editImage.Click += (object sender, EventArgs eventArgs) =>
                {

                    var imageIntent = new Intent();
                    imageIntent.SetType("image/*");
                    imageIntent.SetAction(Intent.ActionGetContent);
                    StartActivityForResult(
                     Intent.CreateChooser(imageIntent, "Select photo"), 0);


                };




            if (array.Length == 0)
                Finish();


            Bitmap bmp = IOUtilz.GetImageBitmapFromUrl(array[3]);
            BitmapDrawable icon = new BitmapDrawable(bmp);
        
            SupportActionBar.SetDisplayShowTitleEnabled(false);
             SupportActionBar.SetIcon(icon);
           

                userName.Text = array[0];
                profilePic2.SetImageResource(Resource.Drawable.test);
                points.Text = "Points: " +array[4];
                aboutMeEdit.Text = array[5];

                if (MainStart.userId != array[6])
            {
                aboutMeEdit.Focusable = false;
            }else
                aboutMeEdit.Focusable = true;


            }
            catch (Exception )
            {

             
            }

        }



        // Friendlist app crash, find a partner. update
        // Crash trykker på map
        //Chat crash send test

        // Friendlist app crash, find a partner. update
        // Crash trykker på map
        //Chat crash send test

        // Friendlist app crash, find a partner. update
        // Crash trykker på map
        //Chat crash send test

        // Friendlist app crash, find a partner. update
        // Crash trykker på map
        //Chat crash send test

        // Friendlist app crash, find a partner. update
        // Crash trykker på map
        //Chat crash send test

        // Friendlist app crash, find a partner. update
        // Crash trykker på map
        //Chat crash send test





        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            try
            {



                if ((resultCode == Result.Ok) && (data != null))
                {
                    Android.Net.Uri uri = data.Data;
                    profilePic2.SetImageURI(uri);
                    profilePic2.RefreshDrawableState();
                }

                else if (data.Data == null)
                {
                    profilePic2.SetImageResource(Resource.Drawable.test);
                    Toast.MakeText(this, "Not able to load picture! " + data.Data.ToString(), ToastLength.Long).Show();
                }

            }
            catch (Exception)
            {


            }

        }





        public override bool OnCreateOptionsMenu(IMenu menu)

        {
            MenuInflater.Inflate(Resource.Menu.action_menu_profile, menu);

            itemGender = menu.FindItem(Resource.Id.gender);
            itemAge = menu.FindItem(Resource.Id.age);
            itemProfilePic = menu.FindItem(Resource.Id.profilePicture);
            itemExit = menu.FindItem(Resource.Id.exit);

            itemAge.SetTitle("Age " + array[2]);

            if (array[1] == "Male")
            {
                itemGender.SetIcon(Resource.Drawable.male);
            }
            else
                itemGender.SetIcon(Resource.Drawable.female);


            return base.OnCreateOptionsMenu(menu);
        }



        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {


                case Resource.Id.exit:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }

          
        }


        protected override void OnStart()
        {
            base.OnStart();
        }
        protected async override void OnStop()
        {
            base.OnStop();
            var instance = await Azure.setAboutMeInfo(array[6], aboutMeEdit.Text);
        }
        protected async override void OnDestroy()
        {
            base.OnDestroy();

            var instance = await Azure.setAboutMeInfo(array[6],aboutMeEdit.Text);
        }
       
    }
}