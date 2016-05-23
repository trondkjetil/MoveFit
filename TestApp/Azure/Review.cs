using System;
using Newtonsoft.Json;

namespace TestApp
{
    public class Review
    {

        public string Id { get; set; }
        [JsonProperty(PropertyName = "rating")]
        public string Rating { get; set; }

      

 
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

