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
using Android.Net;

namespace TestApp
{


    class ConnectionManagement
    {

        //private static ConnectionManagement current;

        private static readonly ConnectivityManager connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);


        public static ConnectivityManager ConnectivityManager
        {


            get
            {
                //if (connectivityManager == null)
                //{
                //    //connectivityManager
                //}

                return ConnectivityManager;

            }

        }


    }

}




