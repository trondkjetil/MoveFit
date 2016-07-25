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
using Microsoft.AspNet.SignalR.Client;
using Android.Graphics;

namespace TestApp.SignalR
{

   
    class SetUp
    {

      public static  HubConnection hubConnection;
        public static IHubProxy chatHubProxy;

        public static List<MessageDetail> currentMessagesWritten;
        public static List<UserDetail> userList;
        public static string converSationId;

        public static string targetId;
        public static  string sendTouserId;
        public static Activity act;


        SetUp(Activity activity)
        {
            act = activity;

            hubConnection = new HubConnection("http://chatservices.azurewebsites.net/");
            chatHubProxy = hubConnection.CreateHubProxy("ChatHub");
        }






        public static async void sendPrivateMessage(string toUserId, string message)
        {
            await chatHubProxy.Invoke("SendPrivateMessage", new object[] { toUserId, message });

        }
        public static async void connectToHub()
        {
            await chatHubProxy.Invoke("Connect", new object[] { MainStart.userName });

            Toast.MakeText(act, "You are online!", ToastLength.Short).Show();
        }

        public static async void setup()
        {


            chatHubProxy.On<string, string, List<UserDetail>, List<MessageDetail>>("onConnected", (currentUserId, userName, connectedUsers, messageDetails) =>
            {

                act.RunOnUiThread(() =>
                {
                    userList = connectedUsers;
                    var test = userName;



                });

            });





            chatHubProxy.On<string, string>("messageReceived", (user, message) =>
            {

                var firstName = user.Substring(0, user.IndexOf(" "));

                act. RunOnUiThread(() =>
                {
                    

                });
            });



            chatHubProxy.On<string, string, string>("SendPrivateMessage", (userId, userName, message) =>
            {
                var firstName = userName.Substring(0, userName.IndexOf(" "));

                act. RunOnUiThread(() =>
                {


               
                });
            });


            await hubConnection.Start();


        }

    }
}