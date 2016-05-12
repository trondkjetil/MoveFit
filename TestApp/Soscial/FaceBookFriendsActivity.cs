
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

using System.Threading.Tasks;
using Android.Util;

namespace TestApp
{
	[Activity (Label = "Socialize")]			
	public class FaceBookFriendsActivity : Activity
	{
		
		// Get your own App ID at developers.facebook.com/apps
		const string FacebookAppId = "975161479232108";
		// You must get this token authorizing by either using Facebook App or a WebView.
		// Please review included samples.


		private const string ExtendedPermissions = "user_about_me,read_stream,publish_stream";
		string userToken;
//		public FacebookClient fb;
		static readonly string TAG = "X:" + typeof (FaceBookFriendsActivity).Name;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView (Resource.Layout.faceBookLayout);

			userToken = Intent.GetStringExtra ("MyData") ?? "Data not available";
		
			//Button bt = FindViewById<Button> (Resource.Id.Facebook);
			//Button messenger = FindViewById<Button> (Resource.Id.messenger);
			//Button dialogBox = FindViewById<Button> (Resource.Id.sendMail);
   //         Button contacts = FindViewById<Button>(Resource.Id.contacts);
          
            
            ImageButton bt = FindViewById<ImageButton>(Resource.Id.faceShare);
            ImageButton messenger = FindViewById<ImageButton>(Resource.Id.faceMessenger);
            ImageButton dialogBox = FindViewById<ImageButton>(Resource.Id.mailSend);
            ImageButton contacts = FindViewById<ImageButton>(Resource.Id.contacts);


            contacts.Click += delegate {
               Intent myIntent = new Intent(this, typeof(ResultBoardActivity));
                StartActivity(myIntent);
            };



            dialogBox.Click += delegate {
				FragmentTransaction transaction = FragmentManager.BeginTransaction();
				Dialog newDialog = new Dialog();
				newDialog.Show(transaction,"Startdialog");

			};


			bt.Click += (sender, e) => {

				//GetMyInfo ();
				Toast.MakeText(this, "Not Implemented yet!",ToastLength.Long).Show();
			};

			messenger.Click += (sender, e) => {

				Intent sendIntent = new Intent();
				sendIntent.SetAction(Intent.ActionSend);
				sendIntent
					.PutExtra(Intent.ExtraText,
						"This is some test text that will be sent!");
				sendIntent.SetType("text/plain");
				sendIntent.SetPackage("com.facebook.orca");

				StartActivity(sendIntent);

			};

	
		
		}


//		public void PostToMyWall ()
//		{
//			 fb = new FacebookClient (userToken);
//			string myMessage = "Hello from Xamarin";
//
//			fb.PostTaskAsync ("me/feed", new { message = myMessage }).ContinueWith (t => {
//				if (!t.IsFaulted) {
//					string message = "Great, your message has been posted to yours wall!";
//					Console.WriteLine (message);
//				}
//			});
////		}
//
//		public void GetMyInfo ()
//		{
//			// This uses Facebook Graph API
//			// See https://developers.facebook.com/docs/reference/api/ for more information.
//
//
//			Toast.MakeText(this, userToken, ToastLength.Long).Show();
//			 fb = new FacebookClient (userToken);
//
//				 fb.GetTaskAsync ("me").ContinueWith (t => {
//				if (!t.IsFaulted) {
//					var result = (IDictionary<string, object>)t.Result;
//					string myDetails = string.Format ("Your name is: {0} {1} and your Facebook profile Url is: {3}", 
//				    (string)result["first_name"], (string)result["last_name"], (string)result["link"]);
//
//					Console.WriteLine (myDetails);
//					Log.Debug(TAG, "My info:{0}",myDetails );
//			
//
//				}
//			});
//		}

//		public void PrintFriendsNames ()
//		{
//			// This uses Facebook Query Language
//			// See https://developers.facebook.com/docs/technical-guides/fql/ for more information.
//			var query = string.Format("SELECT uid,name,pic_square FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0}) ORDER BY name ASC", "me()");
//			FacebookClient fb = new FacebookClient (userToken);
//
//			fb.GetTaskAsync ("fql", new { q = query }).ContinueWith (t => {
//				if (!t.IsFaulted) {
//					var result = (IDictionary<string, object>)t.Result;
//					var data = (IList<object>)result["data"];
//					var count = data.Count;
//					var message = string.Format ("You have {0} friends", count);
//					Console.WriteLine (message);
//
//					foreach (IDictionary<string, object> friend in data)
//						Console.WriteLine ((string) friend["name"]);
//				}
//			});
//		}


		/*
		void HandleGraphApiSample (object sender, EventArgs e)
		{


				fb.GetTaskAsync ("me").ContinueWith (t => {
					if (!t.IsFaulted) {

					var result = (Result)(IDictionary<string, object>);

						string data = "Name: " + (string)result["name"] + "\n" + 
							"First Name: " + (string)result["first_name"] + "\n" +
							"Last Name: " + (string)result["last_name"] + "\n" +
							"Profile Url: " + (string)result["link"];


						RunOnUiThread (() => {
						AlertDialog ("Your Info", data, false, res => {
							});

							AlertDialog.Builder builder = new AlertDialog.Builder(this);
							builder.SetTitle("Get/Replace Facebook App ID");
							//	builder.SetIcon(Android.Resource.Drawable.Icon);
							builder.SetMessage(data);	

							builder.Show();
						});
					}
				});
			
			
		}


*/



	}
}

