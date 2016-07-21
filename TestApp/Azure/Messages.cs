using System;
using Newtonsoft.Json;

namespace TestApp
{
    public class Messages
    {


        public string Id { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "sender")]
        public string Sender { get; set; }


        [JsonProperty(PropertyName = "conversation")]
        public string Conversation { get; set; }

        [JsonProperty(PropertyName = "count")]
        public string Count { get; set; }



    }

    public class MessagesWrapper : Java.Lang.Object
    {
        public MessagesWrapper(Messages item)
        {
            Messages = item;
        }

        public Messages Messages { get; private set; }
    }
}

