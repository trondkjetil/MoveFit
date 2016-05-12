using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace TestApp
{
	 class Dialog : DialogFragment
	{

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState){

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.dialogPopup, container,false);
			Intent email;

			Button sendEmail = (Button) view.FindViewById(Resource.Id.btnDialogEmail);
			EditText subject = (EditText) view.FindViewById(Resource.Id.subject);
			EditText targetEmail = (EditText) view.FindViewById(Resource.Id.targetsEmail);
			EditText message = (EditText)  view.FindViewById(Resource.Id.targetsEmail);

			sendEmail.Click += (sender, e) => {

				email = new Intent (Android.Content.Intent.ActionSend);

				email.PutExtra (Android.Content.Intent.ExtraEmail,
					new string[]{targetEmail.Text} );

				email.PutExtra (Android.Content.Intent.ExtraCc,
					new string[]{""} );

				email.PutExtra (Android.Content.Intent.ExtraSubject, subject.Text);

				email.PutExtra (Android.Content.Intent.ExtraText, message.Text);

				email.SetType ("message/rfc822");

				StartActivity (email);


			};
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


