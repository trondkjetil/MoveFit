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
    public class Email
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Time { get; set; }
        public bool Favorite { get; set; }
    }
}