using System;
using Newtonsoft.Json;

namespace TestApp
{
    public class UserFriends
    {


        public string Id { get; set; }

        [JsonProperty(PropertyName = "userLink1")]
        public string UserLink1 { get; set; }

        [JsonProperty(PropertyName = "userLink2")]
        public string UserLink2 { get; set; }

        [JsonProperty(PropertyName = "friendrequest")]
        public bool FriendRequest { get; set; }

        [JsonProperty(PropertyName = "isaccepted")]
        public bool IsAccepted { get; set; }

        [JsonProperty(PropertyName = "isdeleted")]
        public bool IsDeleted { get; set; }



    }

    public class UserFriendsWrapper : Java.Lang.Object
    {
        public UserFriendsWrapper(UserFriends item)
        {
            UserFriends = item;
        }

        public UserFriends UserFriends { get; private set; }
    }
}

