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
        ScrollView scroll;

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

             scroll = FindViewById<ScrollView>(Resource.Id.scrollView);

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

                RunOnUiThread(() =>
                {
                    userList = connectedUsers;
                    var test = userName;
                   


                });

            });





            chatHubProxy.On<string, string>("messageReceived", (user, message) =>
            {

                var firstName = user.Substring(0, user.IndexOf(" "));

                RunOnUiThread(() =>
                {
                    TextView txt = new TextView(this);
                    txt.Text = firstName + ": " + message;
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


                    Toast.MakeText(this, firstName.ToString() + ": " + message.ToString(), ToastLength.Long).Show();

                });
            });



            //chatHubProxy.On<string,string, string>("SendPrivateMessage", (userId, userName, message) =>
            //{
            //   var firstName = userName.Substring(0, userName.IndexOf(" "));

            //    RunOnUiThread(() =>
            //    {

              
            //        TextView txt = new TextView(this);
            //        txt.Text = firstName.ToString() + ": " + message.ToString();
            //        txt.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
            //        txt.SetPadding(10, 10, 10, 10);
                 

            //        if (userName == MainStart.userName)
            //        {
            //            txt.SetTextColor(Color.Blue);
            //            txt.SetBackgroundColor(Color.AliceBlue);

            //        }
            //        else
            //        {
            //            txt.SetTextColor(Color.Red);
            //            txt.SetBackgroundColor(Color.PaleVioletRed);
            //        }
                       



            //        var grav = GravityFlags.Right;

            //        if (MainStart.userName == userName)
            //        {
            //            grav = GravityFlags.Right;
            //        }
            //        else
            //            grav = GravityFlags.Left;


            //        txt.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            //        {
            //            TopMargin = 10,
            //            BottomMargin = 10,
            //            LeftMargin = 10,
            //            RightMargin = 10,
            //            Gravity = grav

            //        };

            //        FindViewById<LinearLayout>(Resource.Id.llChatMessages).AddView(txt);

            //    });
            //});

           
            await hubConnection.Start();

            await chatHubProxy.Invoke("Connect", new object[] { MainStart.userName});
          
            Toast.MakeText(this, "You are online!", ToastLength.Short).Show();

      

            send.Click += async (o, e2) =>
            {
                string tmpMessage = "";
                try
                {

                    
                    var message = writeMessage.Text;


                    //sendTouserId =  userList.Find(UserDetail => UserDetail.UserName == toUserName).ConnectionId;
                    //if (sendTouserId == "")
                    //    sendTouserId = "";

                    //userList.Find(UserDetail => UserDetail.UserName == "jens").ConnectionId;
                    // userList.Find(UserDetail => UserDetail.UserName == MainStart.userName).ConnectionId; 



                   
                   messageLayout(message,MainStart.userId);
                    //await causes delay, drop? Still 
                    tmpMessage = message;
                    writeMessage.Text = "";

  //[self.scrollView setContentOffset: bottomOffset animated: YES];
            var send = await Azure.AddMessage(MainStart.userId, tmpMessage, converSationId);
                   
                    //  await chatHubProxy.Invoke("SendPrivateMessage", new object[] { sendTouserId, message });
                    
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
                userName = toUserName + " 101";

            var firstName = userName.Substring(0, userName.IndexOf(" "));
            userName = firstName;

            TextView txt = new TextView(this);
            txt.Text = userName.ToString() + ": " + message.ToString();
            txt.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
            txt.SetPadding(10, 10, 10, 10);
            txt.SetBackgroundColor(Color.AliceBlue);
            txt.SetTextColor(Color.Black);
           

            if (senderID == MainStart.userId)
            {
                txt.SetTextColor(Color.Blue);
              
            }
            else
            {
                txt.SetTextColor(Color.Red);
              
            }

            var grav = GravityFlags.Right;

            if (senderID == MainStart.userId)
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
                Gravity = grav,
                

            };

            FindViewById<LinearLayout>(Resource.Id.llChatMessages).AddView(txt);
            scroll.FullScroll(FocusSearchDirection.Down);
        }


    }

}