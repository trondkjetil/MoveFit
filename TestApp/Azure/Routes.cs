using System;
using Newtonsoft.Json;

namespace TestApp
{
    public class Routes
    {



        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "info")]
        public string Info { get; set; }

        [JsonProperty(PropertyName = "review")]
        public string Review { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public string Lat { get; set; }

        [JsonProperty(PropertyName = "long")]
        public string Lon { get; set; }

       


    }

    public class RoutesWrapper : Java.Lang.Object
    {
        public RoutesWrapper(Routes item)
        {
            Routes = item;
        }

        public Routes Routes { get; private set; }
    }
}

