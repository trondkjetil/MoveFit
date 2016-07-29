using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Auth0.SDK;
using Newtonsoft.Json.Linq;
using Android.Util;
using Xamarin.Auth;
using System.Threading;

namespace TestApp
{
    [Activity(Label = "Auth0Client")]
    public class AuthActivity : Activity
    {

        //

        Auth0Client client = new Auth0Client(
            "none.au.auth0.com",
            "TdalffyKrRuyjwWtd3GJp9VcIHnq5fig");
        ProgressDialog progressDialog;
        public string accessToken;

        string[] table = new string[10];

        string name;
        string profilePic;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.auth);

            progressDialog = new ProgressDialog(this);
            progressDialog.SetMessage("loading...");


            var loginWithWidget = FindViewById<Button>(Resource.Id.loginWithWidget);
            loginWithWidget.Click += async (s, a) =>
            {
                // This will show all connections enabled in Auth0, and let the user choose the identity provider
                try
                {
                    var user = await client.LoginAsync(this);


                    ShowResult(user);

                    name = user.Profile["name"].ToString();
                    profilePic = user.Profile["picture"].ToString();

                    Log.Debug("AuthActivity: ", "PROFILEPICTURE *******************************: {0}", profilePic);
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
                    //					Intent myIntent = new Intent (this, typeof(MainActivity));

                    //					myIntent.PutExtra ("MyData", table); 
                    //					StartActivity(myIntent);
                    //					table[0] = "test";
                    //					table[1] = accessToken;
                    Bundle b = new Bundle();
                    b.PutStringArray("MyData", new String[] { name, profilePic });
                    Intent intent = new Intent(Application.Context, typeof(MainStart));
                    intent.PutExtras(b);
                    StartActivity(intent);

                }
            };

            var loginWithWidgetAndRefreshToken = FindViewById<Button>(Resource.Id.loginWithWidgetAndRefreshToken);

            loginWithWidgetAndRefreshToken.Click += async (s, a) =>
            {
                // This will show all connections enabled in Auth0, and let the user choose the identity provider
                try
                {
                    var user = await client.LoginAsync(this, withRefreshToken: true);
                    ShowResult(user);
                }
                catch (AggregateException e)
                {
                    FindViewById<TextView>(Resource.Id.txtResult).Text = e.Flatten().Message;
                }
                catch (Exception e)
                {
                    FindViewById<TextView>(Resource.Id.txtResult).Text = e.Message;
                }
            };

            var loginWithConnection = FindViewById<Button>(Resource.Id.loginWithConnection);
            loginWithConnection.Click += async (s, a) =>
            {
                // This uses a specific connection: google-oauth2
                try
                {
                    var user = await client.LoginAsync(this, "google-oauth2"); // current context and connection name
                    ShowResult(user);
                }
                catch (AggregateException e)
                {
                    FindViewById<TextView>(Resource.Id.txtResult).Text = e.Flatten().Message;
                }
                catch (Exception e)
                {
                    FindViewById<TextView>(Resource.Id.txtResult).Text = e.Message;
                }
            };

            var loginWithUserPassword = FindViewById<Button>(Resource.Id.loginWithUserPassword);

            loginWithUserPassword.Click += async (s, a) =>
            {
                progressDialog.Show();

                var userName = FindViewById<EditText>(Resource.Id.txtUserName).Text;
                var password = FindViewById<EditText>(Resource.Id.txtUserPassword).Text;
                // This uses a specific connection (named sql-azure-database in Auth0 dashboard) which supports username/password authentication
                try
                {
                    var user = await client.LoginAsync("sql-azure-database", userName, password);
                    ShowResult(user);
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
                    if (progressDialog.IsShowing)
                    {
                        progressDialog.Hide();
                    }
                }
            };

            var refreshWithIdToken = FindViewById<Button>(Resource.Id.refreshWithIdToken);
            refreshWithIdToken.Click += async (s, a) =>
            {
                progressDialog.Show();

                try
                {
                    await client.RenewIdToken();
                    ShowResult(client.CurrentUser);
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
                    if (progressDialog.IsShowing)
                    {
                        this.progressDialog.Hide();
                    }
                }
            };

            var refreshWithRefreshToken = this.FindViewById<Button>(Resource.Id.refreshWithRefreshToken);
            refreshWithRefreshToken.Click += async (s, a) =>
            {
                this.progressDialog.Show();

                try
                {
                    await this.client.RefreshToken();
                    this.ShowResult(this.client.CurrentUser);
                }
                catch (AggregateException e)
                {
                    this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Flatten().Message;
                }
                catch (Exception e)
                {
                    this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Message;
                }
                finally
                {
                    if (this.progressDialog.IsShowing)
                    {
                        this.progressDialog.Hide();
                    }
                }
            };
        }

        public void ShowResult(Auth0User user)
        {
            var id = user.IdToken;
            var profile = user.Profile.ToString();



            var refreshToken = string.IsNullOrEmpty(user.RefreshToken)
                ? "Not requested. Use withRefreshToken: true when calling LoginAsync."
                : user.RefreshToken;

            var truncatedId = id.Remove(0, 20);
            truncatedId = truncatedId.Insert(0, "...");

            FindViewById<TextView>(Resource.Id.txtResult).Text = string.Format(
                "Id: {0}\r\n\r\nProfile: {1}\r\n\r\nRefresh Token:\r\n{2}",
                truncatedId,
                profile,
                refreshToken);


            Thread.Sleep(2000);

            var myEmail = user.Profile["name"].ToString() + "  " + user.Profile["gender"].ToString();
            Log.Debug("AuthActivity: ", "Name *******************************: {0}", myEmail);

        }







    }



}
