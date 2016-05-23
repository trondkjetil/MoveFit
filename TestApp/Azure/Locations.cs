using System;
using Newtonsoft.Json;

namespace TestApp
{
    public class Locations
    {

        public string Id { get; set; }
        [JsonProperty(PropertyName = "locations")]
        public string Locationn { get; set; }

  
    }

    public class LocationWrapper : Java.Lang.Object
    {
        public LocationWrapper(Locations item)
        {
            Locations = item;
        }

        public Locations Locations { get; private set; }
    }
}

