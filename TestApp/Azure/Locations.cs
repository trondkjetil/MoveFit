using System;
using Newtonsoft.Json;
namespace TestApp
{
    public class Locations
    {

        public string Id { get; set; }

        //[JsonProperty(PropertyName = "location")]
        //public string Location { get; set; }


      
     //   public  Geography Coordinates { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public double Lat { get; set; }

        [JsonProperty(PropertyName = "long")]
        public double Lon { get; set; }

        [JsonProperty(PropertyName = "route_id")]
        public string Route_id { get; set; }


    }

    public class LocationsWrapper : Java.Lang.Object
    {
        public LocationsWrapper(Locations item)
        {
            Locations = item;
        }

        public Locations Locations { get; private set; }
    }
}

