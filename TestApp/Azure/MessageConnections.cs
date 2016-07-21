using System;
using Newtonsoft.Json;

namespace TestApp
{
    public class MessageConnections
    {


        public string Id { get; set; }

        [JsonProperty(PropertyName = "userlink1")]
        public string UserLink1 { get; set; }

        [JsonProperty(PropertyName = "userlink2")]
        public string UserLink2 { get; set; }

        



    }

    public class MessageConnectionsWrapper : Java.Lang.Object
    {
        public MessageConnectionsWrapper(MessageConnections item)
        {
            MessageConnections = item;
        }

        public MessageConnections MessageConnections { get; private set; }
    }
}

