
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using System.Threading.Tasks;
//using System.Threading;
//using Java.Lang;
//using Android.Support.V4.Widget;
//using Android.Graphics;
//using Android.Content.Res;
//using System.ComponentModel;


//namespace TestApp
//{
//	[Activity (Label = "LoginActivity")]			
//	public class LoginActivity : Activity
//	{

//		private const string AppId = "975161479232108";
//		//add permissions here!
//		private const string ExtendedPermissions = "user_about_me,publish_actions"; // publish_stream";
//		public static string userName;
//		public string accessToken;
//		ProgressBar mProgress;
//		private Handler mHandler = new Handler();



//		async Task FooAsync(Intent webAuth)
//		{
//			await Task.Run(() => 
//				{
//					StartActivityForResult(webAuth,0);

//				});

//			await Task.Delay (4000);
////			Intent myIntent = new Intent (this, typeof(MainActivity));
////			myIntent.PutExtra ("MyData", accessToken); 
////			StartActivity(myIntent);

//		}


//		public void runThis(){
////				
////					var webAuth = new Intent (this, typeof (FBWebViewAuthActivity));
////					webAuth.PutExtra ("AppId", AppId);
////					webAuth.PutExtra ("ExtendedPermissions", ExtendedPermissions);
////					FooAsync( webAuth);

//		}

//		protected override void OnCreate (Bundle savedInstanceState)
//		{
//			base.OnCreate (savedInstanceState);
//			SetContentView (Resource.Layout.progressBar);

//			mProgress = FindViewById<ProgressBar>(Resource.Id.progressBar);
//			mProgress.ProgressDrawable.SetColorFilter(Color.Red, PorterDuff.Mode.SrcIn);
		

//			BackgroundWorker worker = new BackgroundWorker();

//			worker.DoWork += worker_DoWork;
//			worker.RunWorkerCompleted += worker_RunWorkerCompleted;
//			worker.RunWorkerAsync();
		
		
//		}


////
////		void mSwipeRefreshLayout_Refresh(object sender, EventArgs e)
////		{
////			BackgroundWorker worker = new BackgroundWorker();
////			worker.DoWork += worker_DoWork;
////			worker.RunWorkerCompleted += worker_RunWorkerCompleted;
////			worker.RunWorkerAsync();
////		}

//		void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
//		{
//			//RunOnUiThread(() => { mSwipeRefreshLayout.Refreshing = false; });

//			RunOnUiThread(() => {
//			Intent myIntent = new Intent (this, typeof(MainStart));
//			myIntent.PutExtra ("MyData", accessToken); 
//			StartActivity(myIntent);


////				var progressDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
////				new System.Threading.Thread(new ThreadStart(delegate
////					{
////						//LOAD METHOD TO GET ACCOUNT INFO
////						RunOnUiThread(() => Toast.MakeText(this, "Toast within progress dialog.", ToastLength.Long).Show());
////						//HIDE PROGRESS DIALOG
////						RunOnUiThread(() => progressDialog.Hide());
////					})).Start();

//			});
//		}

//		 void   worker_DoWork(object sender, DoWorkEventArgs e)
//		{

//			 runThis ();
//			//Will run on separate thread
//			System.Threading.Thread.Sleep(3000);
//		}

//	}


//}

