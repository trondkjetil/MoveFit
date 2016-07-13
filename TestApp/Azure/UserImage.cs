 using System;
using Newtonsoft.Json;

namespace TestApp
{
    public class UserImage
    {


        public string Id { get; set; }

        [JsonProperty(PropertyName = "userid")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "image")]
        public byte [] Image { get; set; }


    }

    public class UserImageWrapper : Java.Lang.Object
    {
        public UserImageWrapper(UserImage item)
        {
            UserImage = item;
        }

        public UserImage UserImage { get; private set; }
    }
}

