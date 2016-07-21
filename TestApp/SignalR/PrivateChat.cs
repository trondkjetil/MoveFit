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
using System.Text.RegularExpressions;

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


        static string sendTouserId;
        static string myUserId;
        static string myUserName;
        List<MessageDetail> currentMessagesWritten;
        List<UserDetail> userList;
        protected override async void OnCreate(Bundle bundle)
        {

            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.chatPrivate);

            send = FindViewById<ImageButton>(Resource.Id.btnSend);
            send.SetBackgroundColor(Color.Blue);

            writeMessage = FindViewById<EditText>(Resource.Id.txtChat);
            array = Intent.GetStringArrayExtra("MyData");
            toUserName = array[0];
   
            var hubConnection = new HubConnection("http://chatservices.azurewebsites.net/");
            var chatHubProxy = hubConnection.CreateHubProxy("ChatHub");



            //chatHubProxy.On<string, string>("onNewUserConnected", (userId, message) =>
            //{
            //    RunOnUiThread(() =>
            //    {
            //        Toast.MakeText(this, userId + " has connected!", ToastLength.Long).Show();

            //    });

            //});

            //chatHubProxy.On<string, string,List<UserDetail>,List<MessageDetail>>("onConnected", (id, userName, ConnectedUsers, CurrentMessage) =>
            //{
            //    RunOnUiThread(() =>
            //    { 

            //        myUserId = id;
            //        myUserName = userName;
            //        userList = ConnectedUsers;
            //        // currentMessagesWritten = CurrentMessage;


            //        var usersConnected = "";
            //        foreach (var item in userList)
            //        {
            //            usersConnected += "-" + item.UserName;
            //            if (item.UserName == toUserName)
            //            {
            //                sendTouserId = item.ConnectionId;
            //            }
            //        }


            //        Toast.MakeText(this,"SendTo: " + sendTouserId + " ConnectedUsers: " + usersConnected, ToastLength.Long).Show();

            //    });

            //});

             string usersConnected ="";
            foreach (var item in MainStart.listOfConnectedUsers)
            {
                usersConnected += "-" + item.UserName;
                if (item.UserName == toUserName)
                {
                    Toast.MakeText(this, toUserName+ " is found in the list!", ToastLength.Long).Show();

                }
            }

          


            chatHubProxy.On<string,string, string>("sendPrivateMessage",  (userId, userName, message) =>
            {
                var firstName = userName.Substring(0, userName.IndexOf(" "));

                RunOnUiThread(() =>
                {

              
                    TextView txt = new TextView(this);
                    txt.Text = firstName + ": " + message;
                    txt.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
                    txt.SetPadding(10, 10, 10, 10);

                    

                    if (userName == MainStart.userName)
                    {
                        txt.SetTextColor(Color.Blue);

                    }
                    else
                        txt.SetTextColor(Color.Red);



                    var grav = GravityFlags.Right;

                    if (MainStart.userName == userName)
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
                    var name = toUserName;
                    var message = writeMessage.Text;
                    
                    await chatHubProxy.Invoke("SendPrivateMessage", new object[] { name, message });
                    writeMessage.Text = "";
                }
                catch (Exception a)
                {

                    throw a;
                }

            };





            //try
            //{
            //    await chatHubProxy.Invoke("Connect", new object[] { MainStart.userName });
            //}
            //catch (Exception)
            //{


            //}












        }
    }

}