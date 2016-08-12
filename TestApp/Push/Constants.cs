using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TestApp
{
    public static class Constants
    {
        public const string SenderID = "49312402366"; // Google API Project Number  
        public const string ListenConnectionString = "Endpoint=sb://movefit.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=8a8w9lHYg/+dfBoSWzqSSkVFTit9M2BJaRYFjsW2BLc=";
        public const string NotificationHubName = "hub"; //"<hub name>";
    }
}