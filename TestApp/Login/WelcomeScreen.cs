using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Felipecsl.GifImageViewLibrary;
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

namespace TestApp
{

    [Activity(Label = "MoveFit", Icon = "@drawable/tt", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class WelcomeScreen : Activity
    {

        public WebView webLoadingIcon;
        Auth0Client client = new Auth0Client(
            "none.au.auth0.com",
            "TdalffyKrRuyjwWtd3GJp9VcIHnq5fig");
        ProgressDialog progressDialog;
        public string accessToken;

        string[] table = new string[10];

        string name;
        string profilePic;
        string userID;




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.welcome);

            //New database in Azure requires initialisation with ToDoItem setup in order to work
            //  CurrentPlatform.Init ();
            Azure.initAzure();



            try
            {

       

            loginWithWidget();


            }
            catch (Exception)
            {

               
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
                //	email = user.Profile["email"].ToString();
                accessToken = user.Auth0AccessToken;


            }
            catch (AggregateException e)
            {
                FindViewById<TextView>(Resource.Id.txtResult).Text = e.Flatten().Message;
            }
            catch (Exception e)
            {
                FindViewById<TextView>(Resource.Id.txtResult).Text = e.Message;
            }
            finally
            {

                //	startMain (); 
                await startMain();
            }
        }


        async Task startMain()
        {
            //Intent myIntent = new Intent (this, typeof(FaceBookFriendsActivity));
            //	myIntent.PutExtra ("greeting", "Hello from the Second Activity!");
            //SetResult (Result.Ok, myIntent);

            await Task.Delay(50);//3000);
            Bundle b = new Bundle();
            b.PutStringArray("MyData", new[] {
                name,
                profilePic,
                userID
            });

            Intent myIntent = new Intent(this, typeof(MainStart));
            myIntent.PutExtras(b);
            StartActivity(myIntent);
            Finish();
        }




    }


}
