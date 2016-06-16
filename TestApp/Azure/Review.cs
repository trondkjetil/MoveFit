using System;
using Newtonsoft.Json;

namespace TestApp
{
    public class Review
    {

        public string Id { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public int Rating { get; set; }

        [JsonProperty(PropertyName = "routeid")]
        public string RouteId { get; set; }

        [JsonProperty(PropertyName = "userid")]
        public string UserId { get; set; }




    }

    public class ReviewWrapper : Java.Lang.Object
    {
        public ReviewWrapper(Review item)
        {
            Review = item;
        }

        public Review Review { get; private set; }
    }
}

