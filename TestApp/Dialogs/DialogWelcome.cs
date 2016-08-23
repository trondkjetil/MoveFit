using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using static Android.Widget.NumberPicker;
using Android.Graphics;

namespace TestApp
{
	 class DialogWelcome : DialogFragment
	{
        public event EventHandler<DialogEventArgs> DialogClosed;
      
    
      
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState){

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.dialogWelcome, container,false);

            this.Cancelable = false;


			Button okButton = (Button) view.FindViewById(Resource.Id.startRoute);

            TextView introText = view.FindViewById<TextView>(Resource.Id.intro);
            introText.Text = "Hello " + MainStart.userName + "! Welcome to MoveFit"
                + System.Environment.NewLine
                + System.Environment.NewLine
                + "MoveFit is all about making friends, and being active at the same time!"
                + System.Environment.NewLine
                + "Hopefully you will be able to meet new people, and go on a hike or a walk together. "
                + System.Environment.NewLine
                + "The app has point system rewarding you for being active. This allows for competition between you and your friends."
                + System.Environment.NewLine
                + "You get points by creating routes, using other peoples routes, or by being active through out the day!"
                 + System.Environment.NewLine
                + "Have fun being healthy!"
                ;



            introText.SetTypeface(Typeface.SansSerif, TypefaceStyle.Italic);
            introText.TextSize = 18;

      
            okButton.Click += (sender, e) => Dismiss();


			return view;
		}


        public override void OnDismiss(Android.Content.IDialogInterface dialog)
        {

            base.OnDismiss(dialog);
            if (DialogClosed != null)
            {

                DialogClosed(this, new DialogEventArgs { ReturnValue = "0" });
            }

        }



        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //set the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;  // set the animation



        }



        public class DialogEventArgs
        {

            public string ReturnValue { get; set; }


        }
    }


   


    }



