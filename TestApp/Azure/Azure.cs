using System;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Android.App;
using System.IO;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Android.Views;
using System.Collections.Generic;
using Android.Util;
using Android.Widget;
using Newtonsoft.Json.Linq;

namespace TestApp
{
    public class Azure
    {

        const string applicationURL = @"https://movefit.azure-mobile.net/";
        const string applicationKey = @"vaLLzAEGOZguaHOsXqTPkoRsqBYNGP34";
        const string localDbFilename = "localstore1.db";


        //Mobile Service Client reference
        public static MobileServiceClient client { get; set; }

        //Mobile Service sync table used to access data, local storage
        public static IMobileServiceSyncTable<User> userTable { get; set; }

        //Online storage
        public static IMobileServiceTable<User> table { get; set; }

        public List<User> userList;

        public Azure()
        {
        }



        public static async void initAzure()
        {
            CurrentPlatform.Init();
            client = new MobileServiceClient(applicationURL, applicationKey);
            await InitLocalStoreAsync();
            // Get the Mobile Service sync table instance to use

            userTable = client.GetSyncTable<User>();
            table = client.GetTable<User>();
            // Create an adapter to bind the items with the view

            //			//Deletes all items in current table
            //			userTable.PurgeAsync();
            //			List<User> l = userTable.ToListAsync ().Result;
            //			foreach (User u in l) {
            //				userTable.DeleteAsync (u);
            //			}
        }


        public static async Task<List<User>> getImagesOnMap()
        {
            // Get the items that weren't marked as completed and add them in the adapter
            List<User> userList = await table.Where(user => user.Online == true).ToListAsync();
            return userList;

        }

        public static async Task<List<User>> getPeople()
        {
            
            List<User> userList = await table.Where(user => user.Id != null || user.Id != "").ToListAsync();
            return userList;

        }


        public static async Task<List<User>> updateUserLocation(string userName)
        {

            List<User> userList = await table.Where(User => User.UserName == userName).ToListAsync();
            // await table.UpdateAsync(userList.Find(User => User.UserName == userName ));

            //JObject jo = new JObject();
            //jo.Add("Id", userList.Find(User => User.UserName == userName).Id);
            //jo.Add("Text", "Hello World");
            //jo.Add("Lon", "5000");

            userList.Find(User => User.UserName == userName).Lat = MainStart.currentLocation.Latitude.ToString();
            userList.Find(User => User.UserName == userName).Lon = MainStart.currentLocation.Longitude.ToString();
            User user = userList.Find(User => User.UserName == userName);
          //  var inserted 
                await table.UpdateAsync(user); 
            return userList;

        }

        public static async Task<List<User>> userRegisteredOnline(string userName)
        {
           List<User> userList = await table.Where(user => user.UserName == userName).ToListAsync();
            return userList;
        }

        public static async Task<List<User>> SetUserOnline(string userName,bool onlineStatus)
        {
            List<User> userList = await table.Where(User => User.UserName == userName).ToListAsync();
            userList.Find(User => User.UserName == userName).Online = onlineStatus;
            User user = userList.Find(User => User.UserName == userName);
           
            await table.UpdateAsync(user);
            return userList;
        }


        [Java.Interop.Export()]
        public static async void AddItem()
        {
            // Create a new item
            var item = new User
            {
                Text = "test",
                Complete = false,
                UserName = MainStart.userName,
                Sex = "Male",
                Age = 27,
                Points = 100,
                ProfilePicture = MainStart.array[1],
                Lat = "0",//MainStart.currentLocation.Latitude.ToString(),
                Lon = "0", //MainStart.currentLocation.Longitude.ToString()
                Online = true
            };

            try
            {
                await userTable.InsertAsync(item); // insert the new item into the local database
                await SyncAsync(); // send changes to the mobile service
              

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



            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Error");
            }
        }




        public static async Task SyncAsync()
        {
            try
            {
                await client.SyncContext.PushAsync();
                await userTable.PullAsync("allUsers", userTable.CreateQuery()); // query ID is used for incremental sync
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

        public static async Task InitLocalStoreAsync()
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



        public static void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        public static void CreateAndShowDialog(string message, string title)
        {
            Console.WriteLine(message + " " + title);
        }



    }
}

