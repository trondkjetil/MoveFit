using System;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.IO;

namespace TestApp
{
    [Activity(Label = "@string/app_name",
               Theme = "@style/AppTheme")]
    public class ToDoActivity : Activity
    {
        //Mobile Service Client reference
        private MobileServiceClient client;

        //Mobile Service sync table used to access data
        private IMobileServiceSyncTable<User> userTable;

        //Adapter to map the items list to the view
        private UserAdapter adapter;

        //EditText containing the "New ToDo" text
        private EditText textNewToDo;

        const string applicationURL = @"https://movefit.azure-mobile.net/";
        const string applicationKey = @"vaLLzAEGOZguaHOsXqTPkoRsqBYNGP34";
        const string localDbFilename = "localstore1.db";

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Activity_To_Do);

            CurrentPlatform.Init();
            // Create the Mobile Service Client instance, using the provided
            // Mobile Service URL and key
            client = new MobileServiceClient(applicationURL, applicationKey);
            await InitLocalStoreAsync();
            // Get the Mobile Service sync table instance to use
            userTable = client.GetSyncTable<User>();
            textNewToDo = FindViewById<EditText>(Resource.Id.textNewToDo);
            // Create an adapter to bind the items with the view
            adapter = new UserAdapter(this, Resource.Layout.Row_List_To_Do);
            var listViewToDo = FindViewById<ListView>(Resource.Id.listViewToDo);
            listViewToDo.Adapter = adapter;
            // Load the items from the Mobile Service
            OnRefreshItemsSelected();
        }

        private async Task InitLocalStoreAsync()
        {
            // new code to initialize the SQLite store
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), localDbFilename);
            File.Delete(path);
            if (!File.Exists(path))
            {
         
                File.Create(path).Dispose();
            }

            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<User>();

            // Uses the default conflict handler, which fails on conflict
            // To use a different conflict handler, pass a parameter to InitializeAsync. For more details, see http://go.microsoft.com/fwlink/?LinkId=521416
            await client.SyncContext.InitializeAsync(store);
        }

        //Initializes the activity menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.activity_main, menu);
            return true;
        }

        //Select an option from the menu
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_refresh)
            {
                item.SetEnabled(false);

                OnRefreshItemsSelected();

                item.SetEnabled(true);
            }
            return true;
        }

        private async Task SyncAsync()
        {
            try
            {
                await client.SyncContext.PushAsync();
                await userTable.PullAsync("allTodoItems", userTable.CreateQuery()); // query ID is used for incremental sync
            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog(new Exception("There was an error creating the Mobile Service. Verify the URL"), "Error");
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Error");
            }
        }

        // Called when the refresh menu option is selected
        private async void OnRefreshItemsSelected()
        {
            await SyncAsync(); // get changes from the mobile service
            await RefreshItemsFromTableAsync(); // refresh view using local database
        }

        //Refresh the list with the items in the local database
        private async Task RefreshItemsFromTableAsync()
        {
            try
            {
                // Get the items that weren't marked as completed and add them in the adapter
                var list = await userTable.Where(item => item.Complete == false).ToListAsync();

                adapter.Clear();

                foreach (User current in list)
                    adapter.Add(current);

            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Error");
            }
        }

        public async Task CheckItem(User item)
        {
            if (client == null)
            {
                return;
            }

            // Set the item as completed and update it in the table
            item.Complete = true;
            try
            {
                await userTable.UpdateAsync(item); // update the new item in the local database
                await SyncAsync(); // send changes to the mobile service

                if (item.Complete)
                    adapter.Remove(item);

            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Error");
            }
        }

        [Java.Interop.Export()]
        public async void AddItem(View view)
        {
            if (client == null || string.IsNullOrWhiteSpace(textNewToDo.Text))
            {
                return;
            }

            // Create a new item
            var item = new User
            {
                Text = textNewToDo.Text,
                Complete = false,
                UserName = "Trond Tufte",
                Sex = "Male",
                Age = 27,
                Points = 100,
                ProfilePicture = "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-xtp1/v/t1.0-1/p50x50/10416996_10153771292926346_4989196998867714535_n.jpg?oh=f11bfe614cb72d234c29b0b632fa83a1&oe=57B3DAD3&__gda__=1471520543_00f991a2704c9f1b3e4994e56e32eb0f",
                Lat = "100",
                Lon = "399"

            };

            try
            {
                await userTable.InsertAsync(item); // insert the new item into the local database
                await SyncAsync(); // send changes to the mobile service

                if (!item.Complete)
                {
                    adapter.Add(item);
                }
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Error");
            }

            textNewToDo.Text = "";
        }




        public void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        public void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}


