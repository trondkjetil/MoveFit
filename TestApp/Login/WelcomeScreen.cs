using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using System.IO;
using Android.Graphics;
using Android.Webkit;
using Android.Views;
using Android.Content.Res;
using Android.Widget;
using Auth0.SDK;
using Android.Util;
using System;
using System.Threading;
using Android.Content.PM;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Diagnostics;
using Android.Net;

namespace TestApp
{

    [Activity(Label = "MoveFit", Icon = "@drawable/tt", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class WelcomeScreen : Activity
    {
        public static Activity instance;
        public WebView webLoadingIcon;
        Auth0Client client = new Auth0Client(
            "none.au.auth0.com",
            "TdalffyKrRuyjwWtd3GJp9VcIHnq5fig");
        ProgressDialog progressDialog;
        public string accessToken;
        Stopwatch wt;

        string[] table = new string[10];

        string name;
        string profilePic;
        string userID;
        string idProvider;

        public bool isOnline()
        {
            ConnectivityManager cm =
                (ConnectivityManager)GetSystemService(Context.ConnectivityService);
            NetworkInfo netInfo = cm.ActiveNetworkInfo;

            return netInfo != null && netInfo.IsConnectedOrConnecting;
        }


        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.welcome);
            instance = this;
            //New database in Azure requires initialisation with ToDoItem setup in order to work
            //  CurrentPlatform.Init ();
            Azure.initAzure();

            wt = new Stopwatch();


            try
            {

                if (isOnline())
                {
                    loginWithWidget();
                }
                else
                {
                    // var layout = FindViewById<RelativeLayout>(Resource.Id.rel);

                    TextView status = FindViewById<TextView>(Resource.Id.status);
                    status.Text = "No internet connection!";

                    Toast.MakeText(this,"No internet connection!", ToastLength.Long).Show();
                   
                    //TextView txt = new TextView(this);
                    //txt.Text = "No internet Connection!";
                    //txt.SetTextSize(ComplexUnitType.Sp, 20);
                    ////txt.SetPadding(10, 10, 10, 10);
                    //txt.Gravity = GravityFlags.CenterHorizontal;
                    //txt.TextAlignment = TextAlignment.Center;
                 
                    //txt.SetBackgroundColor(Color.White);
                    //txt.SetTextColor(Color.Blue);
                  
                    //layout.AddView(txt);


                    User login =  await Azure.getOfflineUser();
                    name = login.UserName;
                    profilePic = login.ProfilePicture;
                    userID = login.Id;

                    startUp();
                }


            }
            catch (Exception a)
            {

                Toast.MakeText(this, a.Message, ToastLength.Long).Show();

            }
        }

        public async void loginWithWidget()
        {

            progressDialog = new ProgressDialog(this);
            progressDialog.SetMessage("loading...");

            try
            {
                var user = await client.LoginAsync(this);
                name = user.Profile["name"].ToString();
                profilePic = user.Profile["picture"].ToString();
                userID = user.Profile["user_id"].ToString();
              	//email = user.Profile["email"].ToString();
                accessToken = user.Auth0AccessToken;
                idProvider = user.Profile["provider"].ToString();


            }
            catch (AggregateException )
            {
                // FindViewById<TextView>(Resource.Id.txtResult).Text = e.Flatten().Message;
              //  Toast.MakeText(this, e.Message + " Login", ToastLength.Long).Show();
            }
            catch (Exception )
            {
                // FindViewById<TextView>(Resource.Id.txtResult).Text = e.Message;
              //  Toast.MakeText(this, e.Message + " Login", ToastLength.Long).Show();

            }
            finally
            {

                //	startMain (); 
               // await startMain();

               wt.Start();
               while(wt.Elapsed.Seconds < 1)
                {
                }
                wt.Stop();
                startUp();
            }
        }
        public override void OnBackPressed()
        {



            base.OnBackPressed();
            Finish();
            Intent intent = new Intent(this, typeof(WelcomeScreen));      
            StartActivity(intent);       

        }
        private async void startUp()
        {
       
            await Task.Delay(1600);
            Bundle b = new Bundle();
            b.PutStringArray("MyData", new[] {
                name,
                profilePic,
                userID,
                idProvider
            });

            Intent myIntent = new Intent(this, typeof(MainStart));
            myIntent.PutExtras(b);
            StartActivity(myIntent);
            Finish();

        }
     

      

    }


}
