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
using Android.Graphics;
using Microsoft.AspNet.SignalR.Client;

namespace TestApp
{
    [Activity(Label = "PrivateChat")]
    public class PrivateChat : Activity
    {
        public string toUserName;
      
        //  Button send;

        ImageButton send;
        EditText writeMessage;

        string[] array;
        protected override async void OnCreate(Bundle bundle)
        {

            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.chatPrivate);


            //    send = FindViewById<Button>(Resource.Id.btnSend);
            send = FindViewById<ImageButton>(Resource.Id.btnSend);
            send.SetBackgroundColor(Color.Blue);

            writeMessage = FindViewById<EditText>(Resource.Id.txtChat);
            array = Intent.GetStringArrayExtra("MyData");
            toUserName = array[0];

            var myUserName = MainStart.userName;
            
            var hubConnection = new HubConnection("http://chatservices.azurewebsites.net/");
            var chatHubProxy = hubConnection.CreateHubProxy("ChatHub");


         
            //chatHubProxy.On<string, string>("onNewUserConnected", (userId, message) =>
            //{
            //    RunOnUiThread(() =>
            //    {
            //        Toast.MakeText(this, userId + " has connected!", ToastLength.Long).Show();

            //    });

            //});

            chatHubProxy.On<string, string>("onConnected", (userId, message) =>
            {
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, userId + "Is Online", ToastLength.Long).Show();

                });

            });



            chatHubProxy.On<string,string, string>("sendPrivateMessage",  (userId, userName, message) =>
            {
               

               
           
                RunOnUiThread(() =>
                {
                    TextView txt = new TextView(this);
                    txt.Text = userId + ": " + message;
                    txt.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
                    txt.SetPadding(10, 10, 10, 10);

                    if (userId == MainStart.userId)
                    {
                        txt.SetTextColor(Color.Blue);

                    }
                    else
                        txt.SetTextColor(Color.Red);



                    var grav = GravityFlags.Right;

                    if (MainStart.userId == userId)
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


            //************************ MISTAKE CANT SEND


            send.Click += async (o, e2) =>
            {

                try
                {
                    var message = writeMessage.Text;

                    await chatHubProxy.Invoke("SendPrivateMessage", new object[] { toUserName, message });

                    writeMessage.Text = "";
                }
                catch (Exception)
                {


                }

            };





            try
            {
                await chatHubProxy.Invoke("Connect", new object[] { MainStart.userName });
            }
            catch (Exception)
            {


            }












        }
    }

}