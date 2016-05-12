using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace TestApp
{
	 class DialogStartRoute : DialogFragment
	{

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState){

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.dialogStartRoute, container,false);
		

			Button dismiss = (Button) view.FindViewById(Resource.Id.cancel);
			Button startRoute = (Button) view.FindViewById(Resource.Id.startRoute);
		
			dismiss.Click += (sender, e) => Dismiss();
			startRoute.Click += (sender, e) => Dismiss();


			return view;
		}




		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle); //set the title bar to invisible
			base.OnActivityCreated (savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;  // set the animation


		
		}

	}
}


