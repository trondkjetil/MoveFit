using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Locations;
using Android.Net;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.WindowsAzure.MobileServices.Query;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Android.Gms.Maps.GoogleMap;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace TestApp
{


    [Activity(Label = "MainMenu", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainStart : AppCompatActivity, ILocationListener
    {
        public readonly string logTag = "MainActivity";
        SupportToolbar mToolbar;
        ActionBarDrawerToggle mDrawerToggle;
        DrawerLayout mDrawerLayout;
        ListView mLeftDrawer;
        ListView mRightDrawer;
        ArrayAdapter mLeftAdapter;
        ArrayAdapter mRightAdapter;
        List<string> mLeftDataSet;
        List<string> mRightDataSet;
        public string[] table;
        public string text;
        public static string facebookUserId;
        public static string userId;
        public ImageView profilePicture;
        public static Bitmap profilePic;
        public string profilePictureUrl;

        TextView latText;
        TextView longText;
        TextView altText;
        TextView speedText;
        TextView bearText;
        TextView accText;

        Intent myIntent;

        public Azure azure;
        public static bool changed;
        public static Location loc;
        public static Location currentLocation;
        public static string userName;
        public static String[] array;
        public List<User> user;
        public static bool chk;
        public ProgressBar loadingImage;


        public static TextView _address;
        public Address oldAddress;
        public static GoogleMap mMap;

        TextView points;
        public static Activity mainActivity;

        User waitingUpload;
        User userInstanceOne;


        public static bool isOnline;

        public static ConnectivityManager connectivityManager;

        public static IMenuItem menItem;

        TextView messages;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.drawerLayout);
            TestIfGooglePlayServicesIsInstalled();
            mainActivity = this;

            changed = false;
            user = null;
            chk = false;

            waitingUpload = null;
            userInstanceOne = null;

            isOnline = false;

            connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

            mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
            // mRightDrawer = FindViewById<ListView>(Resource.Id.right_drawer);

            mRightDrawer = FindViewById<ListView>(Resource.Id.ContactsListView);

            TextView greetings = FindViewById<TextView>(Resource.Id.textView1);
            TextView steps = FindViewById<TextView>(Resource.Id.steps);
            TextView totalDistance = FindViewById<TextView>(Resource.Id.distance);


            ImageView pictureFriend1 = FindViewById<ImageView>(Resource.Id.pic1);
            ImageView pictureFriend2 = FindViewById<ImageView>(Resource.Id.pic2);
            ImageView pictureFriend3 = FindViewById<ImageView>(Resource.Id.pic3);

            TextView pers1 = FindViewById<TextView>(Resource.Id.pers1);
            TextView pers2 = FindViewById<TextView>(Resource.Id.pers2);
            TextView pers3 = FindViewById<TextView>(Resource.Id.pers3);

            messages = FindViewById<TextView>(Resource.Id.messageInfo);

            steps.Text = "Steps: 0";

            if(userInstanceOne != null)
            {
                totalDistance.Text = "Total Distance: ";
            }
            else
            totalDistance.Text = "Total Distance: 0";

            points = FindViewById<TextView>(Resource.Id.points);

        

            array = Intent.GetStringArrayExtra("MyData");
            greetings.Text = "Greetings " + array[0] + "!";
            userName = array[0];
            profilePictureUrl = array[1];
            profilePic = IOUtilz.GetImageBitmapFromUrl(array[1]);
            profilePicture = FindViewById<ImageView>(Resource.Id.profilePicture);
            profilePicture.SetImageBitmap(profilePic);


        //    loadingImage = FindViewById<ProgressBar>(Resource.Id.progressBar);
            // loadingImage.Visibility = ViewStates.Invisible;
            facebookUserId = array[2];
            //Removes the whole progress bar view, showing only the profile picture

            //Fix to remove space from profile pic and loadingbar
            //  ((ViewGroup)loadingImage.Parent).RemoveView(loadingImage);


            mLeftDrawer.Tag = 0;
            //mRightDrawer.Tag = 1;
            mRightDrawer.Tag = 1;

          


            SetSupportActionBar(mToolbar);

            mLeftDataSet = new List<string>();
            //mLeftDataSet.Add("bump");
            //mLeftDataSet.Add("bump");
            //mLeftDataSet.Add("bump");
            //mLeftDataSet.Add("bump");
            //mLeftDataSet.Add("bump");

            if (IOUtilz.IsKitKatWithStepCounter(PackageManager))
            {
                // mLeftDataSet.Add("STEP COUNTER 2");
            }

            mLeftDataSet.Add("Scoreboard");
            mLeftDataSet.Add("Routes");
            mLeftDataSet.Add("Social");
            mLeftDataSet.Add("Calculator");
            mLeftDataSet.Add("Step counter");
            mLeftDataSet.Add("Messages");
            mLeftDataSet.Add("My profile");



            //mLeftDataSet.Add("Scoreboard");
            //mLeftDataSet.Add("Routes");
            //mLeftDataSet.Add("Social");
            //mLeftDataSet.Add("Messages");
            //mLeftDataSet.Add("Step counter");
            //mLeftDataSet.Add("Calculator");
            //mLeftDataSet.Add("My profile");





            mLeftAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mLeftDataSet);
            mLeftDrawer.Adapter = mLeftAdapter;

            mLeftDrawer.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                var item = mLeftAdapter.GetItem(e.Position);
                if (e.Position == 0)
                {
                    myIntent = new Intent(this, typeof(ScoreBoardPersonActivity));
                    StartActivity(myIntent);
                }
                if (e.Position == 1)
                {
                    myIntent = new Intent(this, typeof(RouteOverview));
                    StartActivity(myIntent);
                }
                else if (e.Position == 2)
                {
                    myIntent = new Intent(this, typeof(FriendsOverview));
                    StartActivity(myIntent);
                }
                else if (e.Position == 3)
                {
                    myIntent = new Intent(this, typeof(Calculator));
                    StartActivity(myIntent);
                }


                else if (e.Position == 4)
                {
                    myIntent = new Intent(this, typeof(StepCounter));
                    StartActivity(myIntent);
                }
                else if (e.Position == 5)
                {

                    myIntent = new Intent(this, typeof(FriendsOverview));
                    StartActivity(myIntent);
                }
                else if (e.Position == 6)
                {
                    

                    try
                    {



                        User instance = null;

                        if (user.Count != 0 || userInstanceOne != null)
                        {
                            instance = user.FirstOrDefault();

                            if (instance == null)
                                instance = userInstanceOne;

                        }



                        if (instance != null)
                        {

                            Bundle b = new Bundle();
                            b.PutStringArray("MyData", new String[] {
                        instance.UserName,
                        instance.Sex,
                        instance.Age.ToString(),
                        instance.ProfilePicture,
                        instance.Points.ToString(),
                        instance.AboutMe,
                        instance.Id


                    });

                            Intent myIntent = new Intent(this, typeof(UserProfile));
                            myIntent.PutExtras(b);
                            StartActivity(myIntent);

                        }

                    }
                    catch (Exception)
                    {


                    }

                }


                //if (e.Position == 7)
                //{
                //    myIntent = new Intent(this, typeof(ScoreBoardPersonActivity));
                //    StartActivity(myIntent);
                //}
                //if (e.Position == 8)
                //{
                //    myIntent = new Intent(this, typeof(RouteOverview));
                //    StartActivity(myIntent);
                //}
                //else if (e.Position == 9)
                //{
                //    myIntent = new Intent(this, typeof(FriendsOverview));
                //    StartActivity(myIntent);
                //}
                //else if (e.Position == 10)
                //{
                //    myIntent = new Intent(this, typeof(Chat));
                //    StartActivity(myIntent);
                //}


                //else if (e.Position == 11)
                //{
                //    myIntent = new Intent(this, typeof(StepCounter));
                //    StartActivity(myIntent);
                //}
                //else if (e.Position == 12)
                //{

                //    myIntent = new Intent(this, typeof(Calculator));
                //    StartActivity(myIntent);
                //}
                //else if (e.Position == 13)
                //{


                //    User instance = null;

                //    if (user.Count != 0)
                //    {
                //        instance = user.FirstOrDefault();
                //    }


                //    if (instance != null)
                //    {

                //        Bundle b = new Bundle();
                //        b.PutStringArray("MyData", new String[] {
                //        instance.UserName,
                //        instance.Sex,
                //        instance.Age.ToString(),
                //        instance.ProfilePicture,
                //        instance.Points.ToString(),
                //        instance.AboutMe,
                //        instance.Id


                //    });

                //        Intent myIntent = new Intent(this, typeof(UserProfile));
                //        myIntent.PutExtras(b);
                //        StartActivity(myIntent);

                //    }

                //  }
                //if (e.Position == 5)
                //{
                //    myIntent = new Intent(this, typeof(ScoreBoardActivity));
                //    StartActivity(myIntent);
                //}
                //else if (e.Position == 6)
                //{
                //    myIntent = new Intent(this, typeof(Calculator));
                //    StartActivity(myIntent);
                //}
                //else if (e.Position == 7)
                //{
                //    myIntent = new Intent(this, typeof(Chat));
                //    StartActivity(myIntent);
                //}
                //else if (e.Position == 8)
                //{
                //    myIntent = new Intent(this, typeof(RouteOverview));
                //    StartActivity(myIntent);
                //}
                //else if (e.Position == 9)
                //{
                //    myIntent = new Intent(this, typeof(FriendsOverview));
                //    StartActivity(myIntent);
                //}






            };

            var RightAdapter = new ContactsAdapter(this);
            //   var contactsListView = FindViewById<ListView>(Resource.Id.ContactsListView);
            mRightDrawer.Adapter = RightAdapter;



            mDrawerToggle = new ActionBarDrawerToggle(
                this,                           //Host Activity
                mDrawerLayout,                  //DrawerLayout
                Resource.String.openDrawer,     //Opened Message
                Resource.String.closeDrawer     //Closed Message
            );

            //added
            mDrawerToggle.DrawerIndicatorEnabled = true;


            mDrawerLayout.SetDrawerListener(mDrawerToggle);
           
            //addedds
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            

            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            mDrawerToggle.SyncState();




            if (mToolbar != null)
            {
                SupportActionBar.SetTitle(Resource.String.openDrawer);
                SetSupportActionBar(mToolbar);
            }


            if (savedInstanceState != null)
            {
                if (savedInstanceState.GetString("DrawerState") == "Opened")
                {
                    //SupportActionBar.SetTitle(Resource.String.openDrawer);
                }

                else
                {
                    //SupportActionBar.SetTitle(Resource.String.closeDrawer);
                }
            }

            else
            {
                //This is the first the time the activity is ran

                SupportActionBar.SetTitle(Resource.String.closeDrawer);

            }

            try
            {
                user = await Azure.userRegisteredOnline(userName);

                if (user.Count == 0)
                {

                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    DialogUserInfo newDialog = new DialogUserInfo();
                    newDialog.DialogClosed += OnDialogClosed;
                    newDialog.Show(transaction, "User Info");
                    //waitingUpload = await Azure.AddUser();
                    //Toast.MakeText(this, "User Added!", ToastLength.Short).Show();

                }
                else
                {

                    waitingUpload = user.FirstOrDefault();
                    var setOnline = await Azure.SetUserOnline(userName, true);
                    setPoints();
                    isOnline = true;

                    userInstanceOne = waitingUpload;
                }



            }
            catch (Exception)
            {

            }

            messagePushNotification();
            profilePicture = FindViewById<ImageView>(Resource.Id.profilePicture);
            initPersonTracker();


            if(userInstanceOne != null)
            {
                points.Text = userInstanceOne.Points.ToString();
            }else
             points.Text = "Score: 0";
            
            
            try
            {

                List<User> topUsers = await Azure.getTop3People();

                if (topUsers[0] != null)
                {
                    pers1.Text = topUsers[0].UserName; ;
                    pictureFriend1.SetImageBitmap(IOUtilz.GetImageBitmapFromUrl(topUsers[0].ProfilePicture));
                }
                else


                if (topUsers[1] != null)
                {
                    pers2.Text = topUsers[1].UserName; 
                    pictureFriend2.SetImageBitmap(IOUtilz.GetImageBitmapFromUrl(topUsers[1].ProfilePicture));
                }


                if (topUsers[2] != null)
                {
                    pers3.Text = topUsers[2].UserName;  //"Test friend3 Score: 0";
                    pictureFriend3.SetImageBitmap(IOUtilz.GetImageBitmapFromUrl(topUsers[2].ProfilePicture));
                }


            }
            catch (Exception)
            {



            }



         
        }

        public async void messagePushNotification()
        {
            var hubConnection = new HubConnection("http://chatservices.azurewebsites.net/");
            var chatHubProxy = hubConnection.CreateHubProxy("ChatHub");
            messages.Text = "No new messages";
            //  int  messageCount = 0;

            chatHubProxy.On<string, int, string>("UpdateChatMessage", (message, color, user) =>
            {
                //UpdateChatMessage has been called from server

                
                RunOnUiThread(() =>
                {
                   
                    //TextView txt = new TextView(this);
                    //txt.Text = user + ": " + message;
                    //txt.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
                    //txt.SetPadding(10, 10, 10, 10);

                    if (user != MainStart.userName)
                    {
                        //messages.Text = "New message from:"+ System.Environment.NewLine +
                        //user.ToString();

                    }
                    messages.Text = "New message from:" + System.Environment.NewLine +
                    user.ToString();
                    messageNotification(message);
                });
            });

            await hubConnection.Start();
          
        }


        public void messageNotification(string message)
        {

            Notification.Builder builder;
            Notification notification;
            NotificationManager notificationManager;


            builder = new Notification.Builder(this)
           //   .SetContentIntent(pendingIntent)
              .SetContentTitle("MoveFit")
              .SetContentText("You have a new message!"+System.Environment.NewLine+ message)
              .SetDefaults(NotificationDefaults.Sound)
              // .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
              .SetSmallIcon(Resource.Drawable.tt);

            // Build the notification:
            notification = builder.Build();


            // Get the notification manager:
            notificationManager =
           GetSystemService(Context.NotificationService) as NotificationManager;



            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);


        }


        public async void setPoints()
        {
            List<User> userInstance = await Azure.getUserId(userName);
            if (userInstance.Count != 0)
            {
                userId = userInstance.First().Id;
                User userPoints = await Azure.getMyPoints(userId);
                points.Text = "Score: " + userPoints.Points.ToString();
            }
        }
        public bool Changed
        {
            get { return changed; }
            set
            {
                changed = value;

                if (changed && !chk)
                {

                    updateLocationTimer();
                }

            }
        }

        public void updateLocationTimer()
        {


            //Toast.MakeText(this, "Your location has been updated!", ToastLength.Short).Show();


            var timer = new System.Threading.Timer((e) =>
            {
                var timerTest = Azure.updateUserLocation(userName);

            }, null, 0, Convert.ToInt32(TimeSpan.FromMinutes(1).TotalMilliseconds));

        }


        public void initPersonTracker()
        {

            //PersonTracker service
            App.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
                Log.Debug(logTag, "ServiceConnected Event Raised");
                // notifies us of location changes from the system
                App.Current.LocationService.LocationChanged += HandleLocationChanged;
                //notifies us of user changes to the location provider (ie the user disables or enables GPS)
                App.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
                App.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
                // notifies us of the changing status of a provider (ie GPS no longer available)
                App.Current.LocationService.StatusChanged += HandleStatusChanged;
            };

            //Starts the person activity tracker
            StartService(new Intent(this, typeof(LocationService)));
        }

        public bool TestIfGooglePlayServicesIsInstalled()
        {
            int InstallGooglePlayServicesId = 1000;
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                Log.Info("", "Google Play Services is installed on this device.");
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Log.Error("", "There is a problem with Google Play Services on this device: {0} - {1}", queryResult, errorString);
                Dialog errorDialog = GoogleApiAvailability.Instance.GetErrorDialog(this, queryResult, InstallGooglePlayServicesId);
                ErrorDialogFragment dialogFrag = new ErrorDialogFragment();


                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Google play service missing");
                alert.SetMessage("Google play service missing!");
                alert.SetNeutralButton("Ok", (senderAlert, args) => {

                });

                dialogFrag.Show(FragmentManager, "GooglePlayServicesDialog");
            }
            return false;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {

                case Android.Resource.Id.Home:
                    //The hamburger icon was clicked which means the drawer toggle will handle the event
                    //all we need to do is ensure the right drawer is closed so the don't overlap
                    mDrawerLayout.CloseDrawer(mRightDrawer);
                    mDrawerToggle.OnOptionsItemSelected(item);


                    //adde
                    InvalidateOptionsMenu();
                    return true;

                //case Resource.Id.statusOnline:

                //    if (isOnline)
                //    {
                //        item.SetIcon(Resource.Drawable.greenonline);
                //        isOnline = true;
                //        Toast.MakeText(this, "Showing as Online", ToastLength.Short).Show();
                //    }
                //    else
                //    {
                //        item.SetIcon(Resource.Drawable.redoffline);
                //        isOnline = false;
                //        Toast.MakeText(this, "Showing as Offline", ToastLength.Short).Show();
                //    }


                //    return true;


                case Resource.Id.action_help:
                    if (mDrawerLayout.IsDrawerOpen(mRightDrawer))
                    {
                        //Right Drawer is already open, close it
                        mDrawerLayout.CloseDrawer(mRightDrawer);

                        //adde
                        InvalidateOptionsMenu();
                    }
                    else
                    {
                        //Right Drawer is closed, open it and just in case close left drawer
                        mDrawerLayout.OpenDrawer(mRightDrawer);
                        //  mDrawerLayout.CloseDrawer(mLeftDrawer);

                        //adde
                        InvalidateOptionsMenu();
                    }

                    Android.App.AlertDialog alertMessage = new Android.App.AlertDialog.Builder(this).Create();
                    alertMessage.SetTitle("Info");
                    alertMessage.SetMessage("User: " + facebookUserId + System.Environment.NewLine + System.Environment.NewLine + "About: This is a prototype app in a masters project. Developed...." + System.Environment.NewLine +
                    System.Environment.NewLine +
                    "Instructions: Open right and left menu by sliding left and right with your finger from the sides towrds the middle" + System.Environment.NewLine
                    );
                    alertMessage.Show();
                    return true;


                // Starts movement tracker (indoor)
                case Resource.Id.action_alarm:

                    var waiting = Azure.findNearBy(userName);

                    if (SimpleService.status == false || SimpleService.Status == false)
                    {
                        StartService(new Intent(this, typeof(SimpleService)));
                        Toast.MakeText(this, "Activity alarm activated", ToastLength.Short).Show();
                        SimpleService.status = true;
                        SimpleService.Status = true;
                    }
                    else if (SimpleService.status == true || SimpleService.Status == false)
                    {
                        StopService(new Intent(this, typeof(SimpleService)));
                        Toast.MakeText(this, "Activity alarm off", ToastLength.Short).Show();
                        SimpleService.status = false;
                        SimpleService.Status = false;
                    }

                    //adde
                    InvalidateOptionsMenu();

                    return true;


                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {


            MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            menItem = menu.FindItem(Resource.Id.statusOnline);
            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            if (mDrawerLayout.IsDrawerOpen((int)GravityFlags.Left))
            {
                outState.PutString("DrawerState", "Opened");
            }

            else
            {
                outState.PutString("DrawerState", "Closed");
            }

            base.OnSaveInstanceState(outState);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            try
            {
                mDrawerToggle.SyncState();
            }
            catch (Exception)
            {


            }

        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            try
            {
                mDrawerToggle.OnConfigurationChanged(newConfig);
            }
            catch (Exception)
            {


            }

        }


        protected override void OnStop()
        {
            base.OnStop();
            // Clean up: shut down the service when the Activity is no longer visible.


            //adde
            InvalidateOptionsMenu();

            //Added!
            //  StopService(new Intent(this, typeof(SimpleService)));
            //   StopService(new Intent(this, typeof(LocationService)));
        }
        protected override void OnPause()
        {
            Log.Debug(logTag, "Location app is moving to background");
            base.OnPause();

            //adde
            InvalidateOptionsMenu();
        }

        protected override void OnResume()
        {
            Log.Debug(logTag, "Location app is moving into foreground");
            base.OnPause();
        }



        protected async override void OnDestroy()
        {

            //var a = await Azure.SetUserOnline(userName, false);
            Log.Debug(logTag, "Location app is becoming inactive");
            base.OnDestroy();

            // var b = await Azure.SetUserOnline(userName, false);
            await logOff();

           

        }

        public async Task<List<User>> logOff()
        {
            var user = await Azure.SetUserOnline(userName, false);
            StopService(new Intent(this, typeof(LocationService)));
            return user;
        }

        /// Updates UI with location data
        public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            Location location = e.Location;
            currentLocation = location;



            OnLocationChanged(location);

            Changed = true;
            changed = true;
            if (changed && !chk)
            {
                chk = true;
            }
        }

        public void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
        {
            Log.Debug(logTag, "Location provider disabled event raised");
        }

        public void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
        {
            Log.Debug(logTag, "Location provider enabled event raised");
        }

        public void HandleStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Log.Debug(logTag, "Location status changed, event raised");
        }




        async Task<Address> ReverseGeocodeCurrentLocation()
        {
            Geocoder geocoder = new Geocoder(this);
            IList<Address> addressList =
                await geocoder.GetFromLocationAsync(currentLocation.Latitude, currentLocation.Longitude, 10);

            Address address = addressList.FirstOrDefault();
            return address;
        }

        void DisplayAddress(Address address)
        {
            if (address != null)
            {
                System.Text.StringBuilder deviceAddress = new System.Text.StringBuilder();
                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.AppendLine(address.GetAddressLine(i));
                }
                // Remove the last comma from the end of the address.
                if (_address.Text == deviceAddress.ToString())
                {

                }
                else
                    _address.Text = deviceAddress.ToString();
            }


            else
            {
                _address.Text = "Unable to determine the address";
            }
        }



        public static void setMarker(LatLng myPosition, GoogleMap mMap)
        {

            MarkerOptions markerOpt1;

            try
            {

                mMap.Clear();
                markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(myPosition);
                markerOpt1.SetTitle("My Position");
                markerOpt1.SetSnippet("My Position");
                BitmapDescriptor image = BitmapDescriptorFactory.FromBitmap(MainStart.profilePic); //(Resource.Drawable.test);
                markerOpt1.SetIcon(image); //BitmapDescriptorFactory.DefaultMarker (BitmapDescriptorFactory.HueCyan));
                mMap.AddMarker(markerOpt1);
                //  mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(myPosition, 15));
            }
            catch (Exception es)
            {


            }
        }



        public async void OnLocationChanged(Location location)
        {
            currentLocation = location;
            if (currentLocation == null)
            {
                _address.Text = "Unable to determine your location. Try again in a short while.";
            }
            else
            {


                try
                {

                    if (IOUtilz.isOnline(connectivityManager))
                    {

                        //Toast.MakeText(this, "User is online", ToastLength.Long).Show();

                        Address address = await ReverseGeocodeCurrentLocation();
                        if (oldAddress != address)
                        {
                            oldAddress = address;
                            DisplayAddress(address);

                        }


                    }


                    var currentPos = new LatLng(currentLocation.Latitude, currentLocation.Longitude);

                    setMarker(currentPos, mMap);

                    mMap.MoveCamera(CameraUpdateFactory.ZoomIn());
                    mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(location.Latitude, location.Longitude), 14));





                }
                catch (Exception)
                {


                }







            }
        }

        public void OnProviderDisabled(string provider)
        {

        }
        public void OnProviderEnabled(string provider)
        {

        }
        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {

        }


        public class ContactsAdapter : BaseAdapter, IOnMapReadyCallback, IOnMapClickListener
        {
            List<Contact> _contactList;
            public Activity _activity;


            //public static GoogleMap mMap;
            public static List<User> nearbyUsers;
            public static ImageButton findMoreFriends;

            public ContactsAdapter(Activity activity)
            {
                _activity = activity;
                FillContacts();
            }

            void FillContacts()
            {
                _contactList = new List<Contact>();
                _contactList.Add(new Contact
                {
                    Id = 123,
                    DisplayName = "1"
                });
            }


            public void OnMapReady(GoogleMap googleMap)
            {
                mMap = googleMap;
            }

            class Contact
            {
                public long Id { get; set; }
                public string DisplayName { get; set; }

            }

            public override int Count
            {
                get { return _contactList.Count; }
            }

            public override Java.Lang.Object GetItem(int position)
            {

                return null;
            }

            public override long GetItemId(int position)
            {
                return _contactList[position].Id;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.rightDrawerMenu, parent, false);
                var contactName = view.FindViewById<TextView>(Resource.Id.ContactName);
                contactName.Text = _contactList[position].DisplayName;

                MapFragment mapFrag = (MapFragment)_activity.FragmentManager.FindFragmentById(Resource.Id.map);
                mMap = mapFrag.Map;

                if (mMap != null)
                {
                    mMap.MapType = GoogleMap.MapTypeTerrain;  // The GoogleMap object is ready to go.
                }

                mMap.SetOnMapClickListener(this);

                mMap.UiSettings.ZoomControlsEnabled = true;
                mMap.UiSettings.RotateGesturesEnabled = false;
                mMap.UiSettings.ScrollGesturesEnabled = false;

                contactName.Text = "";

                //TextView altText;
                //TextView speedText;
                TextView bearText;
                // TextView accText;



                //altText = view.FindViewById<TextView>(Resource.Id.alt);
                //speedText = view.FindViewById<TextView>(Resource.Id.speed);
                bearText = view.FindViewById<TextView>(Resource.Id.bear);
                //accText = view.FindViewById<TextView>(Resource.Id.acc);
                _address = view.FindViewById<TextView>(Resource.Id.location_text);

                Switch location = view.FindViewById<Switch>(Resource.Id.switch1);
                location.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e) {
                    if (e.IsChecked == true)
                    {
                        Toast.MakeText(_activity, "Your location tracking has been turned on", ToastLength.Long).Show();

                        _activity.StartService(new Intent(_activity, typeof(LocationService)));

                        var a = Azure.SetUserOnline(userName, true);
                        isOnline = true;

                        menItem.SetIcon(Resource.Drawable.greenonline);
                        //Android.App.AlertDialog alertMessage = new Android.App.AlertDialog.Builder(_activity).Create();
                        //alertMessage.SetTitle("User location tracking");
                        //alertMessage.SetMessage("Your location tracking has been turned on");
                        //alertMessage.Show();

                    }
                    else
                    {
                        Toast.MakeText(_activity, "Tracking stopped!", ToastLength.Long).Show();
                        _activity.StopService(new Intent(_activity, typeof(LocationService)));

                        var b = Azure.SetUserOnline(userName, false);
                        isOnline = false;
                        menItem.SetIcon(Resource.Drawable.redoffline);
                        //Android.App.AlertDialog alertMessage = new Android.App.AlertDialog.Builder(_activity).Create();
                        //alertMessage.SetTitle("User location tracking");
                        //alertMessage.SetMessage("Your location tracking has been turned off");
                        //alertMessage.Show();



                    }
                };

                //  findMoreFriends = view.FindViewById<ImageButton>(Resource.Drawable.lupe);

                //_activity.RunOnUiThread(() =>
                //{
                //    try
                //    {

                //        // markOnMap(nearbyUsers, mMap);
                //        // findMoreFriends.Click += startFriendMap_Click;
                //        //    await AddressInitiate();
                //    }
                //    catch (Exception e)
                //    {

                //    }


                //});



                return view;
            }


            //public async static void markOnMap(List<User> users, GoogleMap mMap)
            //{

            //    try
            //    {

            //        nearbyUsers = await Azure.getImagesOnMap();
            //        foreach (User x in nearbyUsers)
            //        {
            //           // setMarker(new LatLng(Convert.ToDouble(x.Lat), Convert.ToDouble(x.Lon)), IOUtilz.GetImageBitmapFromUrl(x.ProfilePicture), mMap);

            //        }


            //    }
            //    catch (Exception ex)
            //    {

            //    }

            //}

            public void OnMapClick(LatLng point)
            {
                try
                {
                    Intent myIntent = new Intent(_activity, typeof(GoogleMapsPeople));
                    _activity.StartActivity(myIntent);
                }
                catch (Exception)
                {


                }

            }
        }

        public override void OnBackPressed()
        {

            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("Exit app");
            alert.SetMessage("Do you want to exit the application?");
            alert.SetPositiveButton("Yes", (senderAlert, args) => {
                //change value write your own set of instructions
                //you can also create an event for the same in xamarin
                //instead of writing things here

                base.OnBackPressed();
                var b = Azure.SetUserOnline(userName, false);

            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                //perform your own task for this conditional button click

            });
            //run the alert in UI thread to display in the screen
            RunOnUiThread(() => {
                alert.Show();
            });


        }

        async void OnDialogClosed(object sender, DialogUserInfo.DialogEventArgs e)
        {

            String[] returnData;
            returnData = e.ReturnValue.Split(',');
            string gender = returnData[0];
            string activityLevel = returnData[1];
            string ageString = returnData[2];
            int age = Convert.ToInt32(ageString);


          

            try
            {

                waitingUpload = await Azure.AddUser("", userName, gender, age, 0, profilePictureUrl, "0", "0", true, activityLevel);

                userInstanceOne = waitingUpload;
                Toast.MakeText(this, "Welcome! :)", ToastLength.Short).Show();

                points.Text = "Score: 0";


                
            }
            catch (Exception)
            {



            }


        }


    }


}
