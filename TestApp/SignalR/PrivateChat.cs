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

        HubConnection hubConnection;
        IHubProxy chatHubProxy;


        List<MessageDetail> currentMessagesWritten;
        List<UserDetail> userList;
        string converSationId;

        string targetId;
        static string sendTouserId;
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
            targetId = array[1];


            hubConnection = new HubConnection("http://chatservices.azurewebsites.net/");
            chatHubProxy = hubConnection.CreateHubProxy("ChatHub");

            MessageConnections createNewConversation = null;


            try
            {

               
                createNewConversation = await Azure.getMessageConnectionId(MainStart.userId, targetId.ToString());
                if (createNewConversation == null)
                {
                    createNewConversation = await Azure.AddMessageConnection(MainStart.userId, targetId.ToString());

                }

                converSationId = createNewConversation.Id;

                var messages = await Azure.getMessages(converSationId);

                foreach (var item in messages)
                {
                    messageLayout(item.Message, item.Sender);

                }

            }
            catch (Exception e)
            {
                throw e;

            }






            chatHubProxy.On<string, string, List<UserDetail>, List<MessageDetail>>("onConnected", (currentUserId, userName, connectedUsers, messageDetails) =>
            {

                userList = connectedUsers;
                userList = userList;
                //chatHubProxy.On<string, string, List<UserDetail>, List<MessageDetail>>("onConnected", (id, userName, ConnectedUsers, CurrentMessage) =>
                //{
                RunOnUiThread(() =>
                {
                    userList = connectedUsers;
                    var test = userName;
                    test = test;


                });

            });


            chatHubProxy.On<string,string, string>("SendPrivateMessage", (userId, userName, message) =>
            {
               var firstName = userName.Substring(0, userName.IndexOf(" "));

                RunOnUiThread(() =>
                {

              
                    TextView txt = new TextView(this);
                    txt.Text = firstName.ToString() + ": " + message.ToString();
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


            await chatHubProxy.Invoke("Connect", new object[] { MainStart.userName });
            Toast.MakeText(this, "You are now online!", ToastLength.Short).Show();



            send.Click += async (o, e2) =>
            {

                try
                {

                    
                    var message = writeMessage.Text;
                    sendTouserId = userList.Find(UserDetail => UserDetail.UserName == "jens").ConnectionId;
                  


                      //   var cloudMessage = await Azure.AddMessage(MainStart.userId,message, converSationId);

                      await chatHubProxy.Invoke("SendPrivateMessage", new object[] { sendTouserId, message });
                    writeMessage.Text = "";
                }
                catch (Exception a)
                {

                   
                }

            };



     












    }




        public void messageLayout(string message, string senderID)

        {
            var userName = "";
            if (senderID == MainStart.userId)
            {
                userName = MainStart.userName;
            }
            else
                userName = toUserName;

            var firstName = userName.Substring(0, userName.IndexOf(" "));
            userName = firstName;
            TextView txt = new TextView(this);
            txt.Text = userName.ToString() + ": " + message.ToString();
            txt.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
            txt.SetPadding(10, 10, 10, 10);


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

        }




            // currentMessagesWritten;
            //listOfconnectedUser;

          


          




            //try
            //{



            //    string usersConnected = "";
            //    toUserName = MainStart.userName;
            //    foreach (var item in userList)
            //    {

            //        usersConnected += "-" + item.UserName;
            //        if (item.UserName == toUserName)
            //        {
            //            sendTouserId = item.ConnectionId;

            //            Toast.MakeText(this, item.UserName + " is available! " + "(" + sendTouserId + ")", ToastLength.Long).Show();

            //        }
            //    }


            //    Toast.MakeText(this, "connected users:" + usersConnected, ToastLength.Long).Show();




            //}
            //catch (Exception e)
            //{

            //    throw e;
            //}








        

    }

}