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
        public int Count { get; set; }


        //added last
        [JsonProperty(PropertyName = "createdat")]
        public string CreatedAt { get; set; }


        // override object.Equals
        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var inst = (Messages)obj;
          
            return base.Equals(obj) && inst.Id == this.Id && inst.Message == this.Message;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            
            return base.GetHashCode();
        }
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

