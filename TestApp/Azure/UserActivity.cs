 using System;
using Newtonsoft.Json;

namespace TestApp
{
    public class UserActivity
    {


        public string Id { get; set; }

        [JsonProperty(PropertyName = "respons")]
        public bool Respons { get; set; }

        [JsonProperty(PropertyName = "userid")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

       

      

    }

    public class UserActivityWrapper : Java.Lang.Object
    {
        public UserActivityWrapper(UserActivity item)
        {
            UserActivity = item;
        }

        public UserActivity UserActivity { get; private set; }
    }
}

