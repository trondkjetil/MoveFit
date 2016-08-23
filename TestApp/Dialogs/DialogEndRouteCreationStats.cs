using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.IO;
using Android.Graphics;
using Android.Content;

namespace TestApp
{
   
	 class DialogEndRouteCreationStats : DialogFragment
	{
        public event EventHandler<DialogEventArgs> DialogClosed;
        
        public static string valueReturned;
        public static string givenDifficulty;

     
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState){

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.dialogEndRouteCreationStats, container,false);

            Button startRoute = (Button)view.FindViewById(Resource.Id.startRoute);
            Button share = (Button)view.FindViewById(Resource.Id.share);


            this.Cancelable = false;
            ImageView map = view.FindViewById<ImageView>(Resource.Id.snapShot);
            map.SetImageBitmap(CreateRoute.snapShot);

            TextView speed = view.FindViewById<TextView>(Resource.Id.namePropt);
            speed.Text = "You have used " + CreateRoute.elapsedTime + " to create a route with distance: " + CreateRoute.dist + " meters";
       
            TextView points = view.FindViewById<TextView>(Resource.Id.infoPropt);
            points.Text = "You have earned " + CreateRoute.score+  " points!";

            share.Click += (a, e) =>
            {

                try
                {

               
                Share(CreateRoute.dist.ToString() + "meters", CreateRoute.elapsedTime);

                }
                catch (Exception)
                {

                 
                }
            };
           

            startRoute.Click += (sender, e) =>
            {


                Dismiss();

            };


    

			return view;
		}

        public void Share(string title, string content)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
                return;

            Bitmap b = CreateRoute.snapShot; //BitmapFactory.DecodeResource(Resources, Resource.Drawable.test);

            var tempFilename = "test.png";
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filePath = System.IO.Path.Combine(sdCardPath, tempFilename);
            using (var os = new FileStream(filePath, FileMode.Create))
            {
                b.Compress(Bitmap.CompressFormat.Png, 100, os);
            }
            b.Dispose();

            var imageUri = Android.Net.Uri.Parse($"file://{sdCardPath}/{tempFilename}");
            var sharingIntent = new Intent();
            sharingIntent.SetAction(Intent.ActionSend);
            sharingIntent.SetType("image/*");
            sharingIntent.PutExtra(Intent.ExtraText, content);
            sharingIntent.PutExtra(Intent.ExtraStream, imageUri);
            sharingIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
            StartActivity(Intent.CreateChooser(sharingIntent, title));
        }
        public override void OnDismiss(Android.Content.IDialogInterface dialog)
        {
            valueReturned = ""; 
            base.OnDismiss(dialog);
            if (DialogClosed != null)
            {
             
                DialogClosed(this, new DialogEventArgs { ReturnValue = valueReturned });
            }

        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            if (e.Position == 0)
            {
                givenDifficulty = "Easy";
            }
            else if (e.Position == 1)
            {
                givenDifficulty = "Medium";
            }
            else if (e.Position == 2)
            {
                givenDifficulty = "Hard";
            }
        }
        public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle); //set the title bar to invisible
			base.OnActivityCreated (savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;  // set the animation

		}

   


        public class DialogEventArgs
        {
            //you can put other properties here that may be relevant to check from activity
            //for example: if a cancel button was clicked, other text values, etc.

            public string ReturnValue { get; set; }
          

        }
    }
}


