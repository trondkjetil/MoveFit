using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace TestApp
{
	 class DialogEndRoute : DialogFragment
	{
        public event EventHandler<DialogEventArgs> DialogClosed;

        public string rate;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState){

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.dialogEndRouteRating, container,false);

       
          //  Button dismiss = (Button) view.FindViewById(Resource.Id.cancel1);
			Button startRoute = (Button) view.FindViewById(Resource.Id.startRoute1);
            TextView rating = view.FindViewById<TextView>(Resource.Id.ratingPropt);
           
            TextView finish = view.FindViewById<TextView>(Resource.Id.finish);
            finish.Text = "Congratulations!" + System.Environment.NewLine + "You have finished the route!";

            TextView speed = view.FindViewById<TextView>(Resource.Id.avgSpeed);
            speed.Text = "Avg speed: " + StartRoute.avgSpeed + " km/h";
            

            RatingBar ratingbar = view. FindViewById<RatingBar>(Resource.Id.ratingbarEndRoute);
            ratingbar.Visibility = ViewStates.Visible;

            ratingbar.RatingBarChange += (o, e) =>
            {
                rate = ratingbar.Rating.ToString();
            };


            //dismiss.Click += (sender, e) => Dismiss();
			startRoute.Click += (sender, e) => Dismiss();


			return view;
		}


        public override void OnDismiss(Android.Content.IDialogInterface dialog)
        {
            String data = "";
            base.OnDismiss(dialog);
            if (DialogClosed != null)
            {
                data = rate;
                DialogClosed(this, new DialogEventArgs { ReturnValue = rate });
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


