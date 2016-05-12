using System;
using Newtonsoft.Json;

namespace TestApp
{
    public class UserFriends
    {


        //Copy of user
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "sex")]
        public string Sex { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        [JsonProperty(PropertyName = "points")]
        public int Points { get; set; }

        [JsonProperty(PropertyName = "profilePicture")]
        public string ProfilePicture { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public string Lat { get; set; }

        [JsonProperty(PropertyName = "long")]
        public string Lon { get; set; }

        [JsonProperty(PropertyName = "online")]
        public bool Online { get; set; }


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

