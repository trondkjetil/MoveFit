using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using static Android.Widget.NumberPicker;
using Android.Graphics;

namespace TestApp
{
	 class DialogUserInfo : DialogFragment,IOnValueChangeListener
	{
        public event EventHandler<DialogEventArgs> DialogClosed;
      
        
        public static string valueReturned;

        public static int age;
        public static string activityLevel;
        public static string gender;
      
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState){

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.dialogUserInfo, container,false);

            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinnerUserInfo);
            spinner.ItemSelected += spinner_ItemSelected;
            var adapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.activity_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;


			Button okButton = (Button) view.FindViewById(Resource.Id.startRoute);

            RadioButton radio_red = view.FindViewById<RadioButton>(Resource.Id.radioButton1);
            RadioButton radio_blue = view.FindViewById<RadioButton>(Resource.Id.radioButton2);

            radio_red.Click += RadioButtonClick;
            radio_blue.Click += RadioButtonClick;

            TextView introText = view.FindViewById<TextView>(Resource.Id.intro);
            introText.Text = "Hello!" + System.Environment.NewLine + "Please tell a bit about yourself";
        
          //  TextView age = view.FindViewById<TextView>(Resource.Id.age);
            TextView activityLevel = view.FindViewById<TextView>(Resource.Id.actlevel);

            introText.SetTypeface(Typeface.SansSerif, TypefaceStyle.Italic);
            introText.TextSize = 18;

            activityLevel.SetTypeface(Typeface.SansSerif, TypefaceStyle.Italic);
            activityLevel.TextSize = 15;


            gender = "Male";

            NumberPicker np = (NumberPicker)view.FindViewById(Resource.Id.numberPicker);
            np.MaxValue = 5; // restricted number to minimum value i.e 1
            np.MaxValue = 99;// restricked number to maximum value i.e. 31
            np.WrapSelectorWheel = true;  /*setWrapSelectorWheel(true);*/
           

            np.SetOnValueChangedListener(this);

            okButton.Click += (sender, e) => Dismiss();


			return view;
		}


        public override void OnDismiss(Android.Content.IDialogInterface dialog)
        {
            valueReturned = ""; 
            base.OnDismiss(dialog);
            if (DialogClosed != null)
            {
                valueReturned = gender+ "," + activityLevel + ","+ age.ToString();
                DialogClosed(this, new DialogEventArgs { ReturnValue = valueReturned });
            }

        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

          

            if (e.Position == 0)
            {
                activityLevel = "Sedentary";
            }
            else if (e.Position == 1)
            {
                activityLevel = "Lightly active";


            }
            else if (e.Position == 2)
            {

                activityLevel = "Moderately active";
            }
            else if (e.Position == 3)
            {
                activityLevel = "Very active";

            }
            else if (e.Position == 4)
            {

                activityLevel = "Extra active";
            }


        }



        public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle); //set the title bar to invisible
			base.OnActivityCreated (savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;  // set the animation

		}


        void IOnValueChangeListener.OnValueChange(NumberPicker picker, int oldVal, int newVal)
        {
           age = newVal;
        }
        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            gender = rb.Text;     
           
        }
        public class DialogEventArgs
        {
            
            public string ReturnValue { get; set; }
          

        }
    }
}


