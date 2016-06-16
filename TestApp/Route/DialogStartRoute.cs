using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace TestApp
{
	 class DialogStartRoute : DialogFragment
	{
        public event EventHandler<DialogEventArgs> DialogClosed;
        public EditText routeName;
        public EditText routeInfoData;
        
        public static string valueReturned;
        public static string givenDifficulty;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState){

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.dialogStartRoute, container,false);

            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinnerRouteDifficulty);
            spinner.ItemSelected += spinner_ItemSelected;
            var adapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.activity_routeDifficulty, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;


            //Button dismiss = (Button) view.FindViewById(Resource.Id.cancel);
			Button startRoute = (Button) view.FindViewById(Resource.Id.startRoute);


            TextView route = view.FindViewById<TextView>(Resource.Id.namePropt);
            routeName  = view.FindViewById<EditText>(Resource.Id.nameOfroute);

            TextView routeInfo = view.FindViewById<TextView>(Resource.Id.infoPropt);
            routeInfoData = view.FindViewById<EditText>(Resource.Id.routeInfo);


            TextView routeDifficulty = view.FindViewById<TextView>(Resource.Id.difficultyPrompt);
         
            //dismiss.Click += (sender, e) => Dismiss();
			startRoute.Click += (sender, e) => Dismiss();


			return view;
		}


        public override void OnDismiss(Android.Content.IDialogInterface dialog)
        {
            valueReturned = ""; 
            base.OnDismiss(dialog);
            if (DialogClosed != null)
            {
                valueReturned = routeName.Text + "," + routeInfoData.Text + "," + givenDifficulty;
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


