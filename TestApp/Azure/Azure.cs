using System;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Android.App;
using System.IO;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Android.Views;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using Android.Widget;
using Newtonsoft.Json.Linq;
using TestApp;
using Android.Locations;
using Android.OS;

namespace TestApp
{
    public class Azure //, IMobileServiceLocalStore
    {

      //  const string applicationURL = @"https://movefitt.azurewebsites.net";

        const string applicationURL = @"https://moveit.azurewebsites.net";
        const string localDbFilename = "localstore1.db";


        //Mobile Service Client reference
        public static MobileServiceClient client { get; set; }

        //Mobile Service sync table used to access data, local storage
        public static IMobileServiceSyncTable<User> userTable { get; set; }
        public static IMobileServiceSyncTable<Locations> locTable { get; set; }
        public static IMobileServiceSyncTable<Route> tableRoute { get; set; }
        public static IMobileServiceSyncTable<Review> tableReview { get; set; }
        public static IMobileServiceSyncTable<UserFriends> tableUserFriends { get; set; }
        public static IMobileServiceSyncTable<UserImage> tableImage { get; set; }
        public static IMobileServiceSyncTable<MessageConnections> tableMessageConnection { get; set; }
        public static IMobileServiceSyncTable<Messages> tableUserMessages { get; set; }

        //Online storage
        public static IMobileServiceTable<UserImage> imageTable { get; set; }
        public static IMobileServiceTable<Route> routeTable { get; set; }
        public static IMobileServiceTable<Locations> locationsTable { get; set; }
        public static IMobileServiceTable<User> table { get; set; }
        public static IMobileServiceTable<Review> reviewTable { get; set; }
        public static IMobileServiceTable<UserFriends> userFriendsTable { get; set; }


        public static IMobileServiceTable<MessageConnections> messageConnection { get; set; }
        public static IMobileServiceTable<Messages> userMessages { get; set; }

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
            tableReview = client.GetSyncTable<Review>();
            tableUserFriends = client.GetSyncTable<UserFriends>();
            tableImage = client.GetSyncTable<UserImage>();
            tableMessageConnection = client.GetSyncTable<MessageConnections>();
            tableUserMessages = client.GetSyncTable<Messages>();

         //Cloud 


            table = client.GetTable<User>();
            locationsTable = client.GetTable<Locations>();
            routeTable = client.GetTable<Route>();
            reviewTable = client.GetTable<Review>();
            userFriendsTable = client.GetTable<UserFriends>();
            imageTable = client.GetTable<UserImage>();
            messageConnection = client.GetTable<MessageConnections>();
            userMessages = client.GetTable<Messages>();

            //Deletes all items in current table
            //await userTable.PurgeAsync();
            //await locTable.PurgeAsync();
            //await tableRoute.PurgeAsync();
            //await tableReview.PurgeAsync();
            //await tableUserFriends.PurgeAsync();


            //List<User> l = userTable.ToListAsync().Result;
            //foreach (User u in l)
            //{
            //    userTable.DeleteAsync(u);
            //}
        }

           public static async Task<List<User>> findNearBy(string userName)
        {

            var list = await getUserId(userName);
            User me = list.FirstOrDefault();

           
            //  List<User> userList;

            //  var r = calc.Distance(me, p, DistanceType.Kilometers);
            //   Convert.ToDouble(p.Lat), Convert.ToDouble(p.Lon), userLat, userLong


            //try {


            // var que = table.Where(p => calc.Distance(me, p, DistanceType.Kilometers) <= 35);

            //   IMobileServiceTableQuery<User> query = table.CreateQuery().Select(p => calc.Distance(me, p, DistanceType.Kilometers) <= 35) as IMobileServiceTableQuery<User>;

            // IMobileServiceTableQuery<User> query = table.CreateQuery().Query = "";

            //IMobileServiceTableQuery<User> query = (from q in table.CreateQuery()
            //                                        where calc.Distance(me, q, DistanceType.Kilometers) <= 35
            //                                        select q);

            //IMobileServiceTableQuery<User> query = (from q in table.CreateQuery()
            //                                        where (3960 * Math.Acos(Math.Cos(toRadian(11.9443326)) *
            //                                               Math.Cos(toRadian( Convert.ToDouble(q.Lat)) ) * Math.Cos(toRadian(Convert.ToDouble(q.Lon)) - toRadian(108.4343983)) +
            //                                               Math.Sin(toRadian(11.9443326)) * Math.Sin(toRadian(Convert.ToDouble(q.Lat))))) < 100
            //                                            select q);


            //IMobileServiceTableQuery<User> query = (from q in table.CreateQuery()
            //                                        where q.Id != null
            //                                        select q);


            //getDis(Convert.ToDouble(q.Lat), Convert.ToDouble(q.Lon), 11.9443326, 108.4343983) < 100
            //   List<User> userList = await query.ToListAsync();

            List<User> userList = await table.Where(User => User.Id != null).ToListAsync();
            //  userList.FindAll(Route => Route.Id == routeId);

            //List<User> list2 = await table.Where(p => calc.Distance(me, p, DistanceType.Kilometers) <= 35).ToListAsync();

            //list2 = list2;

            string names = "";
            float[] result = new float[1];
            foreach (var item in userList)
            {
                Location.DistanceBetween(Convert.ToDouble(item.Lat), Convert.ToDouble(item.Lon), 11.9443326, 108.4343983, result);
                if (result[0] < 100)
                {
                    names += " " + item.UserName;
                }
            }
            if (names == "")
                names = "None";
               // userList = userList;

            //string names = "";
            //foreach (var item in userList)
            //{
            //    names += " " + item.UserName;
            //    Log.Debug("SQL", item.UserName + "******************************");
            //    System.Diagnostics.Debug.WriteLine(item.UserName + "********************************************************");
            //}

         AlertDialog alertMessage = new AlertDialog.Builder(MainStart.mainActivity).Create();
            alertMessage.SetTitle("Info");
            alertMessage.SetMessage(names);
            alertMessage.Show();

            //}catch(Exception e)
            //{
            //    throw e;
            //}

            //var members = (from member in db.Stops_edited_smalls
            //               where Math.Abs(Convert.ToDouble(member.Latitude) - curLatitude) < 0.05
            //               && Math.Abs(Convert.ToDouble(member.Longitude) - curLongitude) < 0.05
            //               select new { member, DistanceFromAddress = Math.Sqrt(Math.Pow(Convert.ToDouble(member.Latitude) - curLatitude, 2) + Math.Pow(Convert.ToDouble(member.Longitude) - curLongitude, 2)) * 62.1371192 }).Take(25);


            return null;

        }

        public static double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }
        public static  double getDis(double a, double b, double c, double d)
        {

            float[] result = new float[1];
            Location.DistanceBetween(123211, 21312, 3213, 32131, result);

            return result[0];
        }


        public static async Task<List<Route>> giveRouteRating(string routeId, string rating)
        {

            List<Route> routeList = await routeTable.Where(Route => Route.Id == routeId).ToListAsync();
            routeList.Find(Route => Route.Id == routeId).Review = rating;
            Route newRouteInstanceUpdated = routeList.Find(Route => Route.Id == routeId);

            await routeTable.UpdateAsync(newRouteInstanceUpdated);

            return routeList;

        }

        public static async Task<List<Review>> getRouteReview(string routeID)
        {
            List<Review> reviewList = await reviewTable.Where(Review => Review.RouteId == routeID).ToListAsync();
            return reviewList;


        }
        public static async Task<User> getMyPoints(string userId)
        {

            List<User> pointlist = await table.Where(user => user.Id == userId).ToListAsync();
            return pointlist.First();

        }

        public static async Task<List<User>> getImagesOnMap()
        {
            // Get the items that weren't marked as completed and add them in the adapter
            List<User> userList = await table.Where(user => user.Online == true).ToListAsync();
            return userList;

        }
        public static async Task<List<UserImage>> setProfileImage(string userId, byte[] profileImage)
        {
            
             List<UserImage> userList = await imageTable.Where(UserImage => UserImage.Id == MainStart.userId ).ToListAsync();
            
             userList.Find(UserImage => UserImage.Userid == userId).Image = profileImage;
             UserImage user = userList.Find(UserImage => UserImage.Userid == userId);

            await imageTable.UpdateAsync(user);
         
            return userList;

        }


        public static async Task<List<UserImage>> getUserProfileImage(string userIdGiven)
        {
            List<UserImage> listOfImages = await imageTable.Where(UserImage => UserImage.Userid == userIdGiven).ToListAsync();
            return listOfImages;
        }


        public static async Task<List<User>> getPeople()
        {
            //
            List<User> userList = await table.Where(user => user.Id != null && user.Deleted == false && user.Id != MainStart.userId).ToListAsync();  //
                                                                                                                       //  List<User> userList = await table.Where(user => user.Id != null && user.Deleted == false).ToListAsync();
            return userList;

        }

        public static async Task<List<User>> getTop3People()
        {

            List<User> userList = await table.Where(user => user.Id != null && user.Deleted == false && user.Id != MainStart.userId).OrderBy(User => User.UserName).Take(3).ToListAsync();
            return userList;

        }
        public static async Task<List<Route>> getTop3Routes()
        {

            List<Route> userList = await routeTable.Where(Route => Route.Id != null).OrderBy(Route => Route.Review).Take(3).ToListAsync();
            return userList;

        }

        public static async Task<List<User>> getUserId(String providedUserName)
        {

            List<User> userList = await table.Where(user => user.UserName == providedUserName).ToListAsync();
            return userList;

        }

        //public static async Task<List<UserFriends>> removeUser(String providedUserName)
        //{

        //    List<UserFriends> userList = await userFriendsTable.Where(user => user.UserName == providedUserName).ToListAsync();
        //     await  table.DeleteAsync(userList.FirstOrDefault());
           

        //    return userList;

        //}
        public static async Task<List<UserFriends>> deleteFriend(string myUser, string providedUserName)
        {
            
            List<UserFriends> friendShips = await userFriendsTable.Where(user => user.UserLink1 == myUser && user.UserLink2 == providedUserName || user.UserLink2 == myUser && user.UserLink1 == providedUserName).ToListAsync();
            await userFriendsTable.DeleteAsync(friendShips.FirstOrDefault());

            return friendShips;

        }
        public static async Task<List<Route>> getRoutes()
        {

            List<Route> routeList = await routeTable.Where(Route => Route.Id != null).ToListAsync();
            return routeList;

        }


        public static async Task<List<Route>> getLatestRouteId(string userId)
        {

            List<Route> routeList = await routeTable.Where(Route => Route.User_id == userId).OrderByDescending(Route => Route.User_id).ToListAsync();

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




        public static async Task<List<User>> userRegisteredOnline(string userName)
        {
            List<User> userList = await table.Where(user => user.UserName == userName).ToListAsync();
            return userList;
        }

        public static async Task<List<User>> SetUserOnline(string userID, bool onlineStatus)
        {
            List<User> userList = await table.Where(User => User.Id == userID).ToListAsync();
            userList.Find(User => User.Id == userID).Online = onlineStatus;
            User user = userList.Find(User => User.Id == userID);

            await table.UpdateAsync(user);
            return userList;
        }


        public static async Task<User> addToMyPoints(string userId, int points)
        {
            List<User> userlist = await table.Where(User => User.Id == userId).ToListAsync();

            userlist.Find(User => User.Id == userId).Points = points;
            User user = userlist.Find(User => User.Id == userId);

            await table.UpdateAsync(user);
            return user;

        }

        public static async Task<Route> increaseTripCount(string routeID)
        {
            List<Route> routeList = await routeTable.Where(Route => Route.Id == routeID).ToListAsync();

            routeList.Find(Route => Route.Id == routeID).Trips = routeList.Find(Route => Route.Id == routeID).Trips + 1;
            Route route = routeList.Find(Route => Route.Id == routeID);

            await routeTable.UpdateAsync(route);
            return route;

        }


        public static async Task<User> setAboutMeInfo(string userId,string info)
        {
            List<User> userList = await table.Where(User => User.Id == userId).ToListAsync();

            userList.Find(User => User.Id == userId).AboutMe = info;
            User userInstance = userList.FirstOrDefault();

            await table.UpdateAsync(userInstance);
            return userInstance;

        }





        public static async Task<List<UserFriends>> setFriendAcceptance(string myId,string userId, bool accept)
        {

            List<UserFriends> userFriendList = await userFriendsTable.Where(UserFriends => UserFriends.FriendRequest == true && UserFriends.UserLink1 == userId && UserFriends.UserLink2 == myId).ToListAsync();

            UserFriends acceptRequest = userFriendList.FirstOrDefault();
            acceptRequest.IsAccepted = accept;


            /*Find(UserFriends => UserFriends.FriendRequest == true && UserFriends.UserLink2 == myId).IsAccepted = accept;
*/


            //  UserFriends acceptRequest = userFriendList.Find(UserFriends => UserFriends.FriendRequest == true && UserFriends.UserLink2 == myId);


            await userFriendsTable.UpdateAsync(acceptRequest);
            return userFriendList;
        }

        public static async Task<List<User>> getFriendRequests(string myUserId)
        {

            List<UserFriends> userFriendList = await userFriendsTable.Where(UserFriends => UserFriends.FriendRequest == true && UserFriends.IsAccepted == false && UserFriends.UserLink2 == myUserId).ToListAsync();


            List<User> userList = await table.Where(user => user.Id != null && user.Id != myUserId).ToListAsync();

            List<User> userProfiles = new List<User>();

            for (int i = 0; i < userFriendList.Count; i++)
            {

                for (int x = 0; x < userList.Count; x++)
                {

                    if (userFriendList[i].UserLink1 == userList[x].Id)
                    {
                        userProfiles.Add(userList[x]);
                    }
                }

            }
         
          
            return userProfiles;

        }

        public static async Task<List<User>> getUsersFriends(string userId)
        {

            List<UserFriends> userFriendList = await userFriendsTable.Where(UserFriends => UserFriends.FriendRequest == true && UserFriends.IsAccepted == true && UserFriends.IsDeleted == false && ( UserFriends.UserLink1 == userId || UserFriends.UserLink2 == userId)).ToListAsync();

            List<User> userList = await table.Where(user => user.Id != userId).ToListAsync();

            List<User> userProfiles = new List<User>();

            for (int i = 0; i < userFriendList.Count; i++)
            {

                for (int x = 0; x < userList.Count; x++)
                {

                    if (userFriendList[i].UserLink2 == userList[x].Id || userFriendList[i].UserLink1 == userList[x].Id)
                    {
                        userProfiles.Add(userList[x]);
                    }
                }

            }



            //var commonUsers = userFriendList.Select((a => a.UserLink2)).Intersect(userList.Select(b => b.Id));

            //List<User> userProfiles = await table.Where(user => commonUsers.Contains(user.Id)).ToListAsync();





            //List<UserFriends> userFriendList = await userFriendsTable.Where(UserFriends => UserFriends.IsAccepted == true && UserFriends.UserLink1 == userId).ToListAsync();

            //List<User> userList = await table.Where(user => user.Id != null && user.Id != userId).ToListAsync();

            //var commonUsers = userFriendList.Select(a => a.UserLink1).Intersect(userList.Select(b => b.Id));

            //List<User> userProfiles = await table.Where(user => commonUsers.Contains(user.Id)).ToListAsync();


            return userProfiles;
        }



        [Java.Interop.Export()]
        public static async Task AddFriendShip(string userId1, string userId2)

        {
            var friendship = new UserFriends
            {
                FriendRequest = true,
                UserLink1 = userId1,
                UserLink2 = userId2,
                IsAccepted = false

            };


            try
            {

               
                await userFriendsTable.InsertAsync(friendship); // insert the new item into the local database
                await SyncAsync(); // send changes to the mobile service

            }
            catch (Exception)
            {
              
            }

           
        }


        [Java.Interop.Export()]
        public static async void AddReview(string routeId, int rate, string userId)

        {
            var review = new Review
            {

                Rating = rate,
                RouteId = routeId,
                UserId = userId

            };


            try
            {

                //ToDoItem im = new ToDoItem { Text = "Awesome item" };
                //await client.GetTable<ToDoItem>().InsertAsync(im);
                await reviewTable.InsertAsync(review); // insert the new item into the local database
                await SyncAsync(); // send changes to the mobile service


            }
            catch (Exception e)
            {
                throw e;
            }



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
                throw e;
            }

        }

        [Java.Interop.Export()]
        public static async Task<List<Route>> AddRoute(string routeName, string routeInfo, string routeDistance, string routeReview, int routeTrips, string routeDifficulty, string routeType, string routeUserId, string time)

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
                User_id = routeUserId,
                Time = time

            };


            List<Route> rList = new List<Route>();
            rList.Add(route);
            try
            {

                await tableRoute.InsertAsync(route); // insert the new item into the local database
                await SyncAsync(); // send changes to the mobile service


            }
            catch (Exception e)
            {
                throw e;
            }

            return rList;

        }


        [Java.Interop.Export()]
        public static async Task<User> AddUser(string aboutme,string userName, string gender,int age,int points,string profileimage,string lat, string lon, bool online, string activityLevel)
        {
            // Create a new item
            var user = new User
            {
                AboutMe = aboutme,
                UserName = userName ,//MainStart.userName,
                Sex = gender,
                Age = age,
                Points = points,
                ProfilePicture =  profileimage, //MainStart.array[1],
                Lat = "0",//MainStart.currentLocation.Latitude.ToString(),
                Lon = "0", //MainStart.currentLocation.Longitude.ToString()
                Online = online,
                ActivityLevel = activityLevel
            };

            try
            {

                
               // await client.GetTable<User>().InsertAsync(user);
                await userTable.InsertAsync(user); // insert the new item into the local database
                await SyncAsync(); // send changes to the mobile service


            }
            catch (Exception e)
            {
                throw e;
            }

            return user;

        }


        [Java.Interop.Export()]
        public static async Task<UserImage> AddUserImage(string userId, byte[] img)
        {
            // Create a new item
            var imageToInsert = new UserImage
            {
                Userid = userId,
                Image = img

            };

            try
            {

                
                // await client.GetTable<User>().InsertAsync(user);
                await tableImage.InsertAsync(imageToInsert); // insert the new item into the local database
                await SyncAsync(); // send changes to the mobile service
              

            }
            catch (Exception e)
            {
                throw e;
            }

            return imageToInsert;

        }

        [Java.Interop.Export()]
        public static async Task<MessageConnections> AddMessageConnection(string userId1, string userId2)
        {
            // Create a new item
            var messageConnection = new MessageConnections
            {
               UserLink1 = userId1,
               UserLink2 = userId2
             

            };

            try
            {


                // await client.GetTable<User>().InsertAsync(user);
                await tableMessageConnection.InsertAsync(messageConnection); // insert the new item into the local database
                await SyncAsync(); // send changes to the mobile service


            }
            catch (Exception e)
            {
                throw e;
            }

            return messageConnection;

        }

        [Java.Interop.Export()]
        public static async Task<Messages> AddMessage(string userId, string msg, string conversatioonId)
        {
            // Create a new item
            var message = new Messages
            {
                Sender = userId,
                Message = msg,
                Conversation = conversatioonId



            };

            try
            {


                // await client.GetTable<User>().InsertAsync(user);
                await tableUserMessages.InsertAsync(message); // insert the new item into the local database
                await SyncAsync(); // send changes to the mobile service


            }
            catch (Exception e)
            {
                throw e;
            }

            return message;

        }
        public static async Task SyncAsync()
        {
            try
            {
                await client.SyncContext.PushAsync();

                await userTable.PullAsync("allUsers", userTable.CreateQuery()); // query ID is used for incremental sync
                await tableRoute.PullAsync("allRoutes", tableRoute.CreateQuery());
                await locTable.PullAsync("allLocations", locTable.CreateQuery());
                await tableReview.PullAsync("allReviews", tableReview.CreateQuery());
                await tableUserFriends.PullAsync("allUserFriends", tableUserFriends.CreateQuery());
                await tableImage.PullAsync("allUserImages", tableImage.CreateQuery());

                await tableMessageConnection.PullAsync("allMessageConnections", tableMessageConnection.CreateQuery());
                await tableUserMessages.PullAsync("allUserMessages", tableUserMessages.CreateQuery());

            }




            catch (Java.Net.MalformedURLException a)
            {
                throw a;
            }
            catch (Exception e)
            {
                throw e;
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
            store.DefineTable<Review>();
            store.DefineTable<UserFriends>();
            store.DefineTable<UserImage>();
            store.DefineTable<MessageConnections>();
            store.DefineTable<Messages>();
            // Uses the default conflict handler, which fails on conflict
            // To use a different conflict handler, pass a parameter to InitializeAsync. For more details, see http://go.microsoft.com/fwlink/?LinkId=521416
            await client.SyncContext.InitializeAsync(store);
        }















        // Local stuff


        //public Task InitializeAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<global::Newtonsoft.Json.Linq.JToken> ReadAsync(MobileServiceTableQueryDescription query)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task UpsertAsync(string tableName, IEnumerable<global::Newtonsoft.Json.Linq.JObject> items, bool ignoreMissingColumns)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task DeleteAsync(MobileServiceTableQueryDescription query)
        //{

        //    throw new NotImplementedException();
        //}

        //public Task DeleteAsync(string tableName, IEnumerable<string> ids)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<global::Newtonsoft.Json.Linq.JObject> LookupAsync(string tableName, string id)
        //{
        //    throw new NotImplementedException();
        //}

    }
}

