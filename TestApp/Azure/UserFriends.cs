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

