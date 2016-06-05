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
using TestApp;

namespace TestApp
{
    public class Azure
    {

        //const string applicationURL = @"https://movefit.azure-mobile.net/";
        //const string applicationKey = @"vaLLzAEGOZguaHOsXqTPkoRsqBYNGP34";

        const string applicationURL = @"https://movefitt.azurewebsites.net";
        //  const string applicationKey = @"vaLLzAEGOZguaHOsXqTPkoRsqBYNGP34";
        const string localDbFilename = "localstore.db";


        //Mobile Service Client reference
        public static MobileServiceClient client { get; set; }

        //Mobile Service sync table used to access data, local storage
        public static IMobileServiceSyncTable<User> userTable { get; set; }
        public static IMobileServiceSyncTable<Locations> locTable { get; set; }
        public static IMobileServiceSyncTable<Route> tableRoute { get; set; }


        //Online storage
        public static IMobileServiceTable<Route> routeTable { get; set; }
        public static IMobileServiceTable<Locations> locationsTable { get; set; }
        public static IMobileServiceTable<User> table { get; set; }
        //  private static IMobileServiceSyncTable<ToDoItem> toDoTable;

        public List<User> userList;
        public List<Route> routeList;
        public List<Locations> locationsList;

        public static async void initAzure()
        {
            CurrentPlatform.Init();
            client = new MobileServiceClient(applicationURL);
            await InitLocalStoreAsync();
            // Get the Mobile Service sync table instance to use

            //Local
            userTable = client.GetSyncTable<User>();
            locTable = client.GetSyncTable<Locations>();
            tableRoute = client.GetSyncTable<Route>();
            //Cloud 
            table = client.GetTable<User>();
            locationsTable = client.GetTable<Locations>();
            routeTable = client.GetTable<Route>();


            //   toDoTable = client.GetSyncTable<ToDoItem>();
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
        public static async Task<List<User>> getUserId(String providedUserName)
        {

            List<User> userList = await table.Where(user => user.UserName == providedUserName).ToListAsync();
            return userList;

        }
        public static async Task<List<Route>> getRoutes()
        {

            List<Route> routeList = await routeTable.Where(Route => Route.Id != null || Route.Id != "").ToListAsync();
            return routeList;

        }


        public static async Task<List<Route>> getLatestRouteId(string userId)
        {

            List<Route> routeList = await routeTable.Where(Route => Route.User_id == userId).OrderByDescending(Route => Route.User_id).ToListAsync();
            routeList = routeList;
            return routeList;

        }


        public static async Task<List<Route>> getMyRoutes(string userId)
        {
            
             List<Route> routeList = await routeTable.Where(Route => Route.User_id == userId).ToListAsync();
            return routeList;

        }


        public static async Task<List<Locations>> getLocationsForRoute(string providedRouteID)
        {

             List<Locations> locationsList = await locationsTable.Where(Locations => Locations.Route_id == providedRouteID).ToListAsync();

            return locationsList;

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

        public static async Task<List<Locations>> updateLocationRouteID(string routeIdOld, string RouteIdNew)
        {

            List<Locations> LocationsList = await locationsTable.Where(Locations => Locations.Route_id == routeIdOld).ToListAsync();
            LocationsList.Find(Locations => Locations.Route_id == routeIdOld).Route_id = RouteIdNew;

            List<Locations> newRouteInstanceUpdated = LocationsList.FindAll(Locations => Locations.Route_id == RouteIdNew);
            //  var inserted 

            foreach (var item in newRouteInstanceUpdated)
            {
                await locationsTable.UpdateAsync(item);
            }
            
            return LocationsList;

        }

        public static async Task<List<User>> userRegisteredOnline(string userName)
        {
            List<User> userList = await table.Where(user => user.UserName == userName).ToListAsync();
            return userList;
        }

        public static async Task<List<User>> SetUserOnline(string userName, bool onlineStatus)
        {
            List<User> userList = await table.Where(User => User.UserName == userName).ToListAsync();
            userList.Find(User => User.UserName == userName).Online = onlineStatus;
            User user = userList.Find(User => User.UserName == userName);

            await table.UpdateAsync(user);
            return userList;
        }



        [Java.Interop.Export()]
        public static async void AddLocation(string location, string routeId)

        {
            var loc = new Locations
            {

                Location = location,
                Route_id = routeId

            };


            try
            {

                //ToDoItem im = new ToDoItem { Text = "Awesome item" };
                //await client.GetTable<ToDoItem>().InsertAsync(im);
                await locTable.InsertAsync(loc); // insert the new item into the local database
                await SyncAsync(); // send changes to the mobile service


            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Error");
            }



        }

        [Java.Interop.Export()]
        public static async Task<List<Route>> AddRoute(string routeName, string routeInfo,string routeDistance, string routeReview,int routeTrips, string routeDifficulty,string routeType, string routeUserId)

        {
            var route = new Route
            {

                Name = routeName,
                Info = routeInfo,
                Distance = routeDistance,
                Review = routeReview,
                Trips = routeTrips,
                Difficulty = routeDifficulty,
                RouteType = routeType,
                User_id = routeUserId
            };


            List<Route> rList = new List<Route>();
            rList.Add(route);
            try
            {

                //ToDoItem im = new ToDoItem { Text = "Awesome item" };
                //await client.GetTable<ToDoItem>().InsertAsync(im);
               await tableRoute.InsertAsync(route); // insert the new item into the local database
             await SyncAsync(); // send changes to the mobile service


            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Error");
            }

            return rList;

        }


        [Java.Interop.Export()]
        public static async void AddUser()
        {
            // Create a new item
            var user = new User
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

                //ToDoItem im = new ToDoItem { Text = "Awesome item" };
                //await client.GetTable<ToDoItem>().InsertAsync(im);
                await userTable.InsertAsync(user); // insert the new item into the local database
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
                await tableRoute.PullAsync("allRoutes", tableRoute.CreateQuery());
                await locTable.PullAsync("allLocations", locTable.CreateQuery());
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
            store.DefineTable<Locations>();
            store.DefineTable<Route>();
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

