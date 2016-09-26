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
using Android.Gms.Maps.Model;

namespace TestApp
{
    public enum DistanceUnit { Miles, Kilometers };
    public class Azure //, IMobileServiceLocalStore
    {

      //  const string applicationURL = @"https://movefitt.azurewebsites.net";

      //  const string applicationURL = @"https://moveit.azurewebsites.net";

        const string applicationURL = @"http://movefit.azurewebsites.net";
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
        public static IMobileServiceSyncTable<UserActivity> tableUserActivity { get; set; }



        //Online storage
        public static IMobileServiceTable<UserActivity> userActivityTable { get; set; }
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
            tableUserActivity = client.GetSyncTable<UserActivity>();

            //Cloud 

            table = client.GetTable<User>();
            locationsTable = client.GetTable<Locations>();
            routeTable = client.GetTable<Route>();
            reviewTable = client.GetTable<Review>();
            userFriendsTable = client.GetTable<UserFriends>();
            imageTable = client.GetTable<UserImage>();
            messageConnection = client.GetTable<MessageConnections>();
            userMessages = client.GetTable<Messages>();
            userActivityTable = client.GetTable<UserActivity>();

            //Deletes all items in current table
            //await userTable.PurgeAsync();
            //await locTable.PurgeAsync();
            //await tableRoute.PurgeAsync();
            //await tableReview.PurgeAsync();
            //await tableUserFriends.PurgeAsync();


           
        }
     
        public static async Task<User> getOfflineUser()
        {

            User me = null;
            try
            {


                await SyncAsync();
               

                List<User> ta = await userTable.Where(user => user.Id != null).ToListAsync();
                ta = ta;


                List<User> taa = await table.Where(user => user.Id != null).ToListAsync();
                taa = taa;


                List<Route> route = await routeTable.Where(user => user.Id != null).ToListAsync();
                taa = taa;



            }
            catch (Exception)
            {


            }

            return me;
        }

        public static double ToRadians(double val)
        {
            return (Math.PI / 180) * val;
        }
      

        public static double HaversineDistance(LatLng pos1, LatLng pos2, DistanceUnit unit)
        {

           
            double R = (unit == DistanceUnit.Miles) ? 3960 : 6371;
            var lat = ToRadians((pos2.Latitude - pos1.Latitude));
            var lng = ToRadians((pos2.Longitude - pos1.Longitude));
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                          Math.Cos(ToRadians(pos1.Latitude)) * Math.Cos(ToRadians(pos2.Latitude)) *
                          Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return R * h2;
        }




        public static async Task<List<Route>> updateRouteBestTimeUser(string routeId, string time, string user)
        {

            List<Route> routeList = await routeTable.Where(Route => Route.Id == routeId).ToListAsync();
            routeList.Find(Route => Route.Id == routeId).Time = time + " by " + user;
            Route newRouteInstanceUpdated = routeList.FirstOrDefault();
            await routeTable.UpdateAsync(newRouteInstanceUpdated);

            return routeList;

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


        public static async Task<List<UserImage>> setProfileImage(string userId, byte[] profileImage)
        {
            
             List<UserImage> userList = await imageTable.Where(UserImage => UserImage.Id == MainStart.userId ).ToListAsync();
            
             userList.Find(UserImage => UserImage.Userid == userId).Image = profileImage;
             UserImage userImg = userList.Find(UserImage => UserImage.Userid == userId);

            await imageTable.UpdateAsync(userImg);
         
            return userList;

        }
        public static async Task<List<UserImage>> removeProfileImage()
        {
            List<UserImage> userList = await imageTable.Where(UserImage => UserImage.Userid == MainStart.userId).ToListAsync();

           // userList.Find(UserImage => UserImage.Userid == userId).Image = profileImage;
            UserImage userImg = userList.Find(UserImage => UserImage.Userid == MainStart.userId);
            await imageTable.DeleteAsync(userImg);
           
            return userList;
        }



        public static async Task<List<UserImage>> getUserProfileImage(string userIdGiven)
        {
            List<UserImage> listOfImages = await imageTable.Where(UserImage => UserImage.Userid == userIdGiven).ToListAsync();
            return listOfImages;
        }

        public static async Task<MessageConnections> getMessageConnectionId(string userOne, string userTwo)
        {
            
            List<MessageConnections> messageCon = await messageConnection.Where(MessageConnections => (MessageConnections.UserLink1 == userOne && MessageConnections.UserLink2 == userTwo ) || MessageConnections.UserLink2 == userOne && MessageConnections.UserLink1 == userTwo).ToListAsync();  //
            MessageConnections instance = messageCon.FirstOrDefault();                                                                                                                                //  List<User> userList = await table.Where(user => user.Id != null && user.Deleted == false).ToListAsync();
            return instance;

        }


      
        public static async Task<List<Messages>> getMessages(string conversationId)
        {
           
            List<Messages> messages = await userMessages.Where(Messages => Messages.Conversation == conversationId ).OrderBy(Messages => Messages.CreatedAt).ToListAsync();  //                                                                                                                        //  List<User> userList = await table.Where(user => user.Id != null && user.Deleted == false).ToListAsync();
            return messages;

        }


        public static async Task<bool> setMessageRead(string messageId)
        {

            List<Messages> messages = await userMessages.Where(Messages => Messages.Id == messageId).ToListAsync();  //                                                                                                                        //  List<User> userList = await table.Wher
            var instance = messages.FirstOrDefault();
            instance.Read = true;
            await userMessages.UpdateAsync(instance);

            return true;

        }


        public static async Task<List<User>> getPeople()
        {
            //
            List<User> userList = await table.Where(user => user.Id != null && user.Deleted == false ).ToListAsync();  //
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


        public static async Task<User> getUser(String id)
        {

            List<User> userList = await table.Where(user => user.Id == id).ToListAsync();
            return userList.FirstOrDefault();

        }


        public static async Task<List<User>> getUserByAuthId(String authId)
        {
            List<User> userList = await table.Where(user => user.Id == authId).ToListAsync();
            return userList;
        }

      

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
        public static async Task<List<Route>> getRouteById(string routeId)
        {

            List<Route> routeList = await routeTable.Where(Route => Route.Id == routeId).ToListAsync();
            //Route instance = routeList.FirstOrDefault();
            return routeList;

        }


        public static async Task<List<User>> nearbyPeople()
        {

          
            User user = null;
           
                user = MainStart.userInstanceOne;

                if (user == null || user.Id == "")
                {
                    user = MainStart.waitingUpload;
                }

            var pref = IOUtilz.LoadPreferences();
            double distance = pref[0];
            if(distance == 0)
            {
                distance = 100;
            }
            if (pref[1] == 1)
            {
                distance = IOUtilz.ConvertMilesToKilometers(pref[0]);
            }

            //List<User> nearByPeople = await table.Where(p => p.Lon - user.Lon < 1000 && (p.Lon - user.Lon) > -1000 && (p.Lat - user.Lat) < 1000 && (p.Lat - user.Lat) > -1000  && (p.Id != MainStart.userId )).ToListAsync(); // - user.Lon  < .5 && (  - lon) > -.5 && (Latitude - lat) < .5 && (Latitude - lat) > -.5

            List<User> potentialUsers = await table.Where(p => p.Lon - user.Lon < 1 && (p.Lon - user.Lon) > -1 && (p.Lat - user.Lat) < 1 && (p.Lat - user.Lat) > -1).ToListAsync(); // - user.Lon  < .5 && (  - lon) > -.5 && (Latitude - lat) < .5 && (Latitude - lat) > -.5

            potentialUsers = potentialUsers.FindAll(User => User.Id != MainStart.userId);

            float[] result = new float[1];
            List<User> verifiedUsers = new List<User>();

            foreach (var item in potentialUsers)
            {
                result[0] = 0;
                Location.DistanceBetween(user.Lat, user.Lon, item.Lat, item.Lon, result);
                if ((result[0] / 1000) <= distance)
                {
                    verifiedUsers.Add(item);


                }


            }

            List<User> usersToShow = null;
            usersToShow = new List<User>();

            var list = await getAllUserRelations(MainStart.userId);

            foreach (var potentialUser in verifiedUsers)
            {
                foreach (var userRelation in list)
                {
                    if(userRelation.UserLink1 == potentialUser.Id || userRelation.UserLink2 == potentialUser.Id)
                    {
                        
                        try
                        {
                            if(!usersToShow.Contains(potentialUser))
                            usersToShow.Add(potentialUser);
                        }
                        catch (Exception)
                        {

                            
                        }
                      
                        break;
                       // ids.Add(potentialUser.Id);
                    }
                 
                }
               

            }
           
         
            //  List<User> show = await getUsersFriends(MainStart.userId);
            //List<User> nearbyUsers = nearByPeople.Except(show).ToList();
            verifiedUsers = verifiedUsers.Except(usersToShow).ToList();

            //   List<User> nearbyUsers = verifiedUsers.Except(show).ToList();

            return verifiedUsers;

        }


        public static async Task<List<User>> nearbyPeopleScoreboard()
        {


            User user = null;

            user = MainStart.userInstanceOne;

            if (user == null || user.Id == "")
            {
                user = MainStart.waitingUpload;
            }

            var pref = IOUtilz.LoadPreferences();
            double distance = pref[0];
            if (distance == 0)
            {
                distance = 100;
            }
            if (pref[1] == 1)
            {
                distance = IOUtilz.ConvertMilesToKilometers(pref[0]);
            }

            List<User> potentialUsers = await table.Where(p => p.Lon - user.Lon < 1 && (p.Lon - user.Lon) > -1 && (p.Lat - user.Lat) < 1 && (p.Lat - user.Lat) > -1).ToListAsync(); // - user.Lon  < .5 && (  - lon) > -.5 && (Latitude - lat) < .5 && (Latitude - lat) > -.5
      
            float[] result = new float[1];
            List<User> verifiedUsers = new List<User>();

            foreach (var item in potentialUsers)
            {
                result[0] = 0;
                Location.DistanceBetween(user.Lat, user.Lon, item.Lat, item.Lon, result);
                if ((result[0] / 1000) <= distance)
                {
                    verifiedUsers.Add(item);


                }

            }
            
        
            return verifiedUsers;

        }
        public static async Task<List<Route>> nearbyRoutes()
        {

            //float[] res = new float[1];
            //Location.DistanceBetween(user.Lat, user.Lon, route.Lat, route.Lon, res);

            //  var dist = res[0];
            User user = null;
           
                 user = MainStart.userInstanceOne;

                if(user == null || user.Id == "")
                {
                    user = MainStart.waitingUpload;
                }
                var pref = IOUtilz.LoadPreferences();
                double distance = pref[0];
            if (distance == 0)
            {
                distance = 100;
            }
            if (pref[1] == 1)
                {
                    distance = IOUtilz.ConvertMilesToKilometers(pref[0]);
                }
      

            //List<Route> b = await routeTable.Where(p => p.Lon - user.Lon < 3 && (p.Lon - user.Lon) > -3 && (p.Lat - user.Lat) < 3 && (p.Lat - user.Lat) > -3).ToListAsync(); // - user.Lon  < .5 && (  - lon) > -.5 && (Latitude - lat) < .5 && (Latitude - lat) > -.5
            //b = b;

            //List<Route> c = await routeTable.Where(p => p.Lon - user.Lon < 2 && (p.Lon - user.Lon) > -2 && (p.Lat - user.Lat) < 2 && (p.Lat - user.Lat) > -2).ToListAsync(); // - user.Lon  < .5 && (  - lon) > -.5 && (Latitude - lat) < .5 && (Latitude - lat) > -.5
            //c = c;
            List<Route> potentialLocations = null;
            try
            {
                potentialLocations = await routeTable.Where(p => p.Lon - user.Lon < 1 && (p.Lon - user.Lon) > -1 && (p.Lat - user.Lat) < 1 && (p.Lat - user.Lat) > -1).ToListAsync(); // - user.Lon  < .5 && (  - lon) > -.5 && (Latitude - lat) < .5 && (Latitude - lat) > -.5

            }
            catch (Exception)
            {


            }

            float[] result = new float[1];
                List<Route> verifiedRoutes = new List<Route>();

                foreach (var item in potentialLocations)
                {
                    result[0] = 0;
                    Location.DistanceBetween(user.Lat, user.Lon, item.Lat, item.Lon, result);
                    if ( (result[0] / 1000) <= distance)
                    {
                        verifiedRoutes.Add(item);


                      }

                }


       //     List<Route> show = await getMyRoutes(MainStart.userId);
     
        //    List<Route> nearbyRoutes = verifiedRoutes.Except(show).ToList();


            return verifiedRoutes;
            
        }


        public static async Task<List<Route>> getLatestRouteId(string userId)
        {

            List<Route> routeList = await routeTable.Where(Route => Route.User_id == userId).OrderByDescending(Route => Route.CreatedAt).Take(1).ToListAsync();
          
            return routeList;

        }


        public static async Task<List<Route>> getMyRoutes(string userId)
        {

            List<Route> routeList = await routeTable.Where(Route => Route.User_id == MainStart.userId).ToListAsync();
            return routeList;

        }

        public static async Task<List<Route>> deleteRoute(string routeId)
        {
            List<Route> routeList = await routeTable.Where(Route => Route.User_id == MainStart.userId && Route.Id == routeId).ToListAsync();

            // userList.Find(UserImage => UserImage.Userid == userId).Image = profileImage;
            Route route = routeList.FirstOrDefault();
            await routeTable.DeleteAsync(route);

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

            userList.Find(User => User.UserName == userName).Lat = MainStart.currentLocation.Latitude;
            userList.Find(User => User.UserName == userName).Lon = MainStart.currentLocation.Longitude;
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
            User inst = userList.FirstOrDefault();
            inst.Online = onlineStatus;

            await table.UpdateAsync(inst);
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

        public static async Task<User> addToMyDistance(string userId, double  distance)
        {
            List<User> userlist = await table.Where(User => User.Id == userId).ToListAsync();

            userlist.Find(User => User.Id == userId).DistanceMoved = distance;
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

        public static async Task<List<UserFriends>> getAllUserRelations(string userId)
        {
            List<UserFriends> userFriendList = await userFriendsTable.Where(UserFriends => UserFriends.IsDeleted == false && (UserFriends.UserLink1 == userId || UserFriends.UserLink2 == userId)).ToListAsync();

            
        //    List<User> userList = await table.Where(user => user.Id != userId).ToListAsync();
            
            return userFriendList;
        }

        //public static async Task<List<User>> getGivenUsers(List<UserFriends> lis)
        //{
        //    //  List<User> userList = await table.Where(users => list.Any(t1 => t1.UserLink1 == users.Id || t1.UserLink2 == users.Id)).ToListAsync();

        //    List<User> userList = await table.Select(users => users.Id).ToListAsync();

          
        //    return null;
            
        //}
       
        public static async Task<List<User>> getUsersFriends(string userId)
        {

            List<User> users = new List<User>();
            List<UserFriends> userFriendList = await userFriendsTable.Where(UserFriends => UserFriends.FriendRequest == true && UserFriends.IsAccepted == true && UserFriends.Deleted == false && ( UserFriends.UserLink1 == userId || UserFriends.UserLink2 == userId)).ToListAsync();
         
            User inst = null;
            foreach (var item in userFriendList)
            {
               inst = await getUser(item.UserLink1);
                users.Add(inst);
                inst = await getUser(item.UserLink2);
                users.Add(inst);
            }
             users = users.FindAll(user => user.Id != MainStart.userId);
            //List<User> userList = await table.Where(user => user.Id != userId).ToListAsync();

            //userList = userList.FindAll(user => user.Id != MainStart.userId);

            //List<User> userProfiles = new List<User>();

            //for (int i = 0; i < userFriendList.Count; i++)
            //{
            //    for (int x = 0; x < userList.Count; x++)
            //    {

            //        if (userFriendList[i].UserLink2 == userList[x].Id || userFriendList[i].UserLink1 == userList[x].Id)
            //        {
            //            userProfiles.Add(userList[x]);
            //        }
            //    }

            //}

            return users;
        }



        [Java.Interop.Export()]
        public static async Task AddFriendShip(string userId1, string userId2)

        {
            var friendship = new UserFriends
            {
                FriendRequest = true,
                UserLink1 = userId1,
                UserLink2 = userId2,
                IsAccepted = false,
                Deleted = false

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
        public static async void AddLocation(double lat,double lon, string routeId)

        {
            var loc = new Locations
            {

              //  Location = location,
              Lat = lat,
              Lon =lon,
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
        public static async Task<List<Route>> AddRoute(string routeName, string routeInfo, string routeDistance, string routeReview, int routeTrips, string routeDifficulty, string routeType, string routeUserId, string time, double lat,double lon)

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
                Time = time,
                Lat = lat,
                Lon = lon

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
        public static async Task<List<UserActivity>> AddActivityAlert(bool res)

        {
            var activity = new UserActivity
            {

                Respons = res,
                UserId = MainStart.userId,
                Count = 0
              
              

            };


            List<UserActivity> rList = new List<UserActivity>();
            rList.Add(activity);
            try
            {

                await tableUserActivity.InsertAsync(activity); // insert the new item into the local database
                await SyncAsync(); // send changes to the mobile service


            }
            catch (Exception e)
            {
                throw e;
            }

            return rList;

        }

        [Java.Interop.Export()]
        public static async Task<User> AddUser(string userId,string aboutme,string userName, string gender,int age,int points,string profileimage,string lat, string lon, bool online, string activityLevel,double distanceMoved)
        {
            // Create a new item
            var user = new User
            {
                Id = userId,
                AboutMe = aboutme,
                UserName = userName ,//MainStart.userName,
                Sex = gender,
                Age = age,
                Points = points,
                ProfilePicture =  profileimage, //MainStart.array[1],
                Lat = 0,//MainStart.currentLocation.Latitude.ToString(),
                Lon = 0, //MainStart.currentLocation.Longitude.ToString()
                Online = online,
                ActivityLevel = activityLevel,
                DistanceMoved = distanceMoved
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
        public static async Task<UserImage> AddUserImage(byte[] img)
        {
            // Create a new item
            var imageToInsert = new UserImage
            {
                Userid = MainStart.userId,
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
                Conversation = conversatioonId,
                Read = false
                


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
                await tableUserMessages.PullAsync("allUserActivities", tableUserActivity.CreateQuery());

            }




            catch (Java.Net.MalformedURLException )
            {
            
            }
         catch (MobileServicePushFailedException)
                {

                
                       }
            catch (Exception )
            {
              
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
            store.DefineTable<UserActivity>();
            // Uses the default conflict handler, which fails on conflict
            // To use a different conflict handler, pass a parameter to InitializeAsync. For more details, see http://go.microsoft.com/fwlink/?LinkId=521416
            await client.SyncContext.InitializeAsync(store);
        }



    }
}

