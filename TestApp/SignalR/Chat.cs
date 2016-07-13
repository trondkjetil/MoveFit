using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.AspNet.SignalR.Client;
using Android.Graphics;

namespace TestApp
{
    [Activity(Label = "Messaging")]
    public class Chat : Activity
    {
        public string UserName;
        public int BackgroundColor;
      //  Button send;

        ImageButton send;
        EditText writeMessage;
        protected override async void OnCreate(Bundle bundle)
        {

            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.chat);



            //    send = FindViewById<Button>(Resource.Id.btnSend);
            send = FindViewById<ImageButton>(Resource.Id.btnSend);
            send.SetBackgroundColor(Color.Green);

             writeMessage = FindViewById<EditText>(Resource.Id.txtChat);

             UserName = MainStart.userName;
             BackgroundColor = 0;

                 //var hubConnection = new HubConnection("http://movefitt.azurewebsites.net/");
                 var hubConnection = new HubConnection("http://chatservices.azurewebsites.net/");
                 var chatHubProxy = hubConnection.CreateHubProxy("ChatHub");


            chatHubProxy.On<string, int, string>("UpdateChatMessage", (message, color, user) =>
            {
                //UpdateChatMessage has been called from server

                RunOnUiThread(() =>
                {
                    TextView txt = new TextView(this);
                    txt.Text = user + ": " + message;
                    txt.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
                    txt.SetPadding(10, 10, 10, 10);

                    if (user == MainStart.userName)
                    {
                        txt.SetTextColor(Color.Blue);

                    }
                    else
                        txt.SetTextColor(Color.Red);

                    

                    var grav = GravityFlags.Right;

                    if (MainStart.userName == user)
                    {
                        grav = GravityFlags.Right;
                    }
                    else
                        grav = GravityFlags.Left;


                txt.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
                {
                    TopMargin = 10,
                    BottomMargin = 10,
                    LeftMargin = 10,
                    RightMargin = 10,
                    Gravity = grav

                };


                    FindViewById<LinearLayout>(Resource.Id.llChatMessages).AddView(txt);

                });
            });


            await hubConnection.Start();

            send.Click += async (o, e2) =>
            {

                try
                {
                    var message = writeMessage.Text;

                    await chatHubProxy.Invoke("SendMessage", new object[] { message, BackgroundColor, UserName });

                    writeMessage.Text = "";
                }
                catch (Exception)
                {


                }

            };


        }


    }
}

