using System;
using Newtonsoft.Json;

namespace TestApp
{
    public class Route
    {

        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "info")]
        public string Info { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public string Distance { get; set; }

        [JsonProperty(PropertyName = "review")]
        public string Review { get; set; }

        [JsonProperty(PropertyName = "trips")]
        public int Trips { get; set; }

        [JsonProperty(PropertyName = "difficulty")]
        public string Difficulty { get; set; }


        [JsonProperty(PropertyName = "routeType")]
        public string RouteType { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string User_id { get; set; }

        //Last edited / added

        [JsonProperty(PropertyName = "createdat")]
        public string CreatedAt { get; set; }


    }

    public class RoutesWrapper : Java.Lang.Object
    {
        public RoutesWrapper(Route item)
        {
            Route = item;
        }

        public Route Route { get; private set; }
    }
}

