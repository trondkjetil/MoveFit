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
using Java.Util;
using System.Timers;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Graphics.Drawables;

namespace TestApp
{
    [Activity(Label = "PrivateChat", Theme = "@style/Theme2")]
    public class PrivateChat : AppCompatActivity
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
        List<Messages> messages;
        List<Messages> PreviousMessages;
        LinearLayout layout;
        SupportToolbar toolbar;


        public static IMenuItem itemProfilePic;
        public static IMenuItem itemHome;
        public static IMenuItem itemExit;
        BitmapDrawable icon;

        static System.Timers.Timer timer; // From System.Timers
        static List<DateTime> _l; // Stores timer results
        public  List<DateTime> DateList // Gets the results
        {
            get
            {
                if (_l == null) // Lazily initialize the timer
                {
                    Start(); // Start the timer
                }
                return _l; // Return the list of dates
            }
        }
         void Start()
        {
            _l = new List<DateTime>(); // Allocate the list
            timer = new System.Timers.Timer(2000); // Set up the timer for 3 seconds        
            timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            timer.Enabled = true; // Enable it
        }


        async void  _timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            timer.Enabled = false;
            try
            {

            var NewMessages = await Azure.getMessages(converSationId);
         
        
            RunOnUiThread( () =>
            {
                try
                {


                    bool notAnyNewMessages = false;

                    if(PreviousMessages.Count == 0)
                    {
                        notAnyNewMessages = false;
                    }else

                    notAnyNewMessages = PreviousMessages.LastOrDefault().Id.Equals(NewMessages.LastOrDefault().Id) || PreviousMessages.LastOrDefault().Id == NewMessages.LastOrDefault().Id;


                    if (NewMessages.Count != 0 && !notAnyNewMessages)
                {
                
                    layout.RemoveAllViews();
                    foreach (var message in NewMessages)
                    {
                        messageLayout(message.Message, message.Sender);

                    }

                }


                    if(PreviousMessages.Count != 0)
                    PreviousMessages.Clear();

                    PreviousMessages = NewMessages;


                    timer.Enabled = true;
                }
                catch (Exception a)
                {
                    throw a;
                   
                }

            });
            _l.Add(DateTime.Now); // Add date on each timer event

               
            }
            catch (Exception ea)
            {

                throw ea;
            }

          
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (timer != null)
            {

                timer.Elapsed -= new ElapsedEventHandler(_timer_Elapsed);
                timer.Enabled = false;
            }

        }
        protected override void OnStop()
        {
            base.OnStop();
            if (timer != null)
                timer.Elapsed -= new ElapsedEventHandler(_timer_Elapsed);
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (timer != null)
            timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
        }

        protected override async void OnCreate(Bundle bundle)
        {
        
            base.OnCreate(bundle);

       
            SetContentView(Resource.Layout.chatPrivate);
            array = Intent.GetStringArrayExtra("MyData");
            layout = FindViewById<LinearLayout>(Resource.Id.llChatMessages);
            send = FindViewById<ImageButton>(Resource.Id.btnSend);
           // send.SetBackgroundColor(Color.Blue);
            send.SetBackgroundColor(Color.White);
            send.Enabled = false;

            scroll = FindViewById<ScrollView>(Resource.Id.scrollView);

            writeMessage = FindViewById<EditText>(Resource.Id.txtChat);



            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var bmp = IOUtilz.GetImageBitmapFromUrl(array[2]);
            icon = new BitmapDrawable(bmp);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetIcon(icon);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            
            toUserName = array[0];
            targetId = array[1];
            PreviousMessages = new List<Messages>();


            //hubConnection = new HubConnection("http://chatservices.azurewebsites.net/");
            //chatHubProxy = hubConnection.CreateHubProxy("ChatHub");

            MessageConnections createNewConversation = null;



            try
            {
                createNewConversation = await Azure.getMessageConnectionId(MainStart.userId, targetId.ToString());
                if (createNewConversation == null)
                {
                    createNewConversation = await Azure.AddMessageConnection(MainStart.userId, targetId.ToString());

                }

                converSationId = createNewConversation.Id;
                messages = await Azure.getMessages(converSationId);

                foreach (var item in messages)
                {

                    messageLayout(item.Message, item.Sender);

                }

                PreviousMessages = messages;


                var list = DateList;

              
                send.Enabled = true;
                send.SetBackgroundColor(Color.Blue);

            }
            catch (Exception e)
            {
                throw e;

            }

           
            //chatHubProxy.On<string, string, List<UserDetail>, List<MessageDetail>>("onConnected", (currentUserId, userName, connectedUsers, messageDetails) =>
            //{

            //    RunOnUiThread(() =>
            //    {
            //        userList = connectedUsers;
            //        var test = userName;



            //    });

            //});





            //chatHubProxy.On<string, string>("messageReceived", (user, message) =>
            //{

            //    var firstName = user.Substring(0, user.IndexOf(" "));

            //    RunOnUiThread(() =>
            //    {
            //        TextView txt = new TextView(this);
            //        txt.Text = firstName + ": " + message;
            //        txt.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
            //        txt.SetPadding(10, 10, 10, 10);

            //        if (user == MainStart.userName)
            //        {
            //            txt.SetTextColor(Color.Blue);

            //        }
            //        else
            //            txt.SetTextColor(Color.Red);


            //        var grav = GravityFlags.Right;

            //        if (MainStart.userName == user)
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

            //        //  FindViewById<LinearLayout>(Resource.Id.llChatMessages).AddView(txt);
            //        layout.AddView(txt);


            //        Toast.MakeText(this, firstName.ToString() + ": " + message.ToString(), ToastLength.Long).Show();

            //    });
            //});



       //     ******************************* Not used lately

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


            //await hubConnection.Start();

            //await chatHubProxy.Invoke("Connect", new object[] { MainStart.userName });

            //Toast.MakeText(this, "You are online!", ToastLength.Short).Show();



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




                    messageLayout(message, MainStart.userId);
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
                userName = toUserName;

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

            scroll.FullScroll(FocusSearchDirection.Down);
            layout.AddView(txt);
            scroll.FullScroll(FocusSearchDirection.Down);
           
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
        }
        public async void testMethod()
        {

            try
            {


                var NewMessages = await Azure.getMessages(converSationId);
                List<Messages> newMessagesToWrite = new List<Messages>();
                foreach (var newMsg in NewMessages)
                {

                    foreach (var prevMsg in PreviousMessages)
                    {

                        if (newMsg.Id != prevMsg.Id)
                        {
                            newMessagesToWrite.Add(newMsg);
                        }

                    }

                }

                RunOnUiThread(() =>
                {
                    //    Toast.MakeText(this, "Updating chat list..", ToastLength.Long).Show();
                    if (newMessagesToWrite.Count != 0)
                    {


                        foreach (var oldMessage in newMessagesToWrite)
                        {


                            messageLayout(oldMessage.Message, oldMessage.Sender);

                            //if (item.Id != oldMessage.Id)
                            //{
                            //    messageLayout(item.Message, item.Sender);
                            //}

                        }
                    }

                    PreviousMessages = newMessagesToWrite;
                



                });
             


            }
            catch (Exception)
            {

                throw;
            }
        }






        public override bool OnCreateOptionsMenu(IMenu menu)

        {
            MenuInflater.Inflate(Resource.Menu.action_menu_profile_chat, menu);

            //itemGender = menu.FindItem(Resource.Id.gender);
            //itemAge = menu.FindItem(Resource.Id.age);
            itemProfilePic = menu.FindItem(Resource.Id.profilePicture);
            //itemExit = menu.FindItem(Resource.Id.exit);
            itemHome = menu.FindItem(Resource.Id.home);



            return base.OnCreateOptionsMenu(menu);
        }



        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.home:
                    Finish();
                    return true;

                //case Resource.Id.back:
                //    Finish();
                //    return true;
                case Android.Resource.Id.Home:// Resource.Id.back:
                    OnBackPressed();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }


        }








    }

}



