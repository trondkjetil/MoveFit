using System;
using Newtonsoft.Json;

namespace TestApp
{
    public class User
    {


        public string Id { get; set; }

        [JsonProperty(PropertyName = "aboutme")]
        public string AboutMe { get; set; }

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


        [JsonProperty(PropertyName = "activitylevel")]
        public string ActivityLevel { get; set; }

    }

    public class UserWrapper : Java.Lang.Object
    {
        public UserWrapper(User item)
        {
            User = item;
        }

        public User User { get; private set; }
    }
}

