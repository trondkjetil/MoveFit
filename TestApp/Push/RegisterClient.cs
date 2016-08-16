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
using System.Net.Http;
using Org.Json;
using Java.Net;
using Android.Util;
using Java.Lang;
using Java.Util;
using Org.Apache.Http.Client.Methods;
using System.Net;
using System.IO;

namespace TestApp.Push
{
    class RegisterClient
    {
        private static readonly string PREFS_NAME = "ANHSettings";
        private static readonly string REGID_SETTING_NAME = "ANHRegistrationId";
        private string Backend_Endpoint;
        ISharedPreferences settings;
        protected HttpClient httpClient;
        private string authorizationHeader;

        public RegisterClient(Context context, string backendEnpoint)
        {
            this.settings = context.GetSharedPreferences(PREFS_NAME, 0);
            httpClient = new HttpClient(); 
            Backend_Endpoint = backendEnpoint + "/api/register";
        }

        public string getAuthorizationHeader()
        {
            return authorizationHeader;
        }
        public void setAuthorizationHeader(string authorizationHeader)
        {
            this.authorizationHeader = authorizationHeader;
        }


        public void register(string handle,  string tag) {// ISet<string> tags)  {
        string registrationId = retrieveRegistrationIdOrRequestNewOne(handle);

        JSONObject deviceInfo = new JSONObject();
        deviceInfo.Put("Platform", "gcm");
        deviceInfo.Put("Handle", handle);
        deviceInfo.Put("Tags", new JSONArray(tag)); //tags.ToList()));

        int statusCode = upsertRegistration(registrationId, deviceInfo);

       
        settings.Edit().Remove(REGID_SETTING_NAME).Commit();
        registrationId = retrieveRegistrationIdOrRequestNewOne(handle);
        statusCode = upsertRegistration(registrationId, deviceInfo);
           
    }


        private int upsertRegistration(string registrationId, JSONObject deviceInfo)
           {



            HttpWebRequest HttpWReq =
            (HttpWebRequest)WebRequest.Create(Backend_Endpoint + "/" + registrationId);
            
            ASCIIEncoding encoding = new ASCIIEncoding();
            string stringData = deviceInfo.ToString(); //place body here
            byte[] data = encoding.GetBytes(stringData);

            HttpWReq.Headers.Add("Authorization", "Basic " + authorizationHeader);
            HttpWReq.Headers.Add("Content - Type", "application / json");
           

          
            string responseText;



            HttpStatusCode statusCode = 0;
            try
            {
                HttpWebResponse myResp = (HttpWebResponse)HttpWReq.GetResponse();
                 statusCode = myResp.StatusCode;

                using (var response = HttpWReq.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                  


                }
            }
            Stream newStream = HttpWReq.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            }
            catch (System.Exception)
            {

                throw;
            }



            return (int)statusCode;
    }

    private string retrieveRegistrationIdOrRequestNewOne(string handle) {

            if (settings.Contains(REGID_SETTING_NAME))
                return settings.GetString(REGID_SETTING_NAME, null);

         

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Backend_Endpoint + "?handle=" + handle);
            request.Headers.Add("Authorization", "Basic " + authorizationHeader);
            

            //ASCIIEncoding encoding = new ASCIIEncoding();
            //string stringData = deviceInfo.ToString(); //place body here
            //byte[] data = encoding.GetBytes(stringData);

          


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                     throw new RuntimeException("Error creating Notification Hubs registrationId");
            }
            string registrationId = response.ToString();
            registrationId = registrationId.Substring(1, registrationId.Length - 1);

            settings.Edit().PutString(REGID_SETTING_NAME, registrationId).Commit();

            return registrationId;


        }




    }
}


































//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using System.Net.Http;
//using Org.Json;
//using Java.Net;
//using Android.Util;
//using Java.Lang;
//using Java.Util;
//using Org.Apache.Http.Client.Methods;
//using System.Net;
//using System.IO;

//namespace TestApp.Push
//{
//    class RegisterClient
//    {
//        private static readonly string PREFS_NAME = "ANHSettings";
//        private static readonly string REGID_SETTING_NAME = "ANHRegistrationId";
//        private string Backend_Endpoint;
//        ISharedPreferences settings;
//        protected HttpClient httpClient;
//        private string authorizationHeader;

//        public RegisterClient(Context context, string backendEnpoint)
//        {
//            this.settings = context.GetSharedPreferences(PREFS_NAME, 0);
//            httpClient = new HttpClient(); //DefaultHttpClient();
//            Backend_Endpoint = backendEnpoint + "/api/register";
//        }

//        public string getAuthorizationHeader()
//        {
//            return authorizationHeader;
//        }
//        public void setAuthorizationHeader(string authorizationHeader)
//        {
//            this.authorizationHeader = authorizationHeader;
//        }


//        public void register(string handle, ISet<string> tags)
//        {
//            string registrationId = retrieveRegistrationIdOrRequestNewOne(handle);

//            JSONObject deviceInfo = new JSONObject();
//            deviceInfo.Put("Platform", "gcm");
//            deviceInfo.Put("Handle", handle);
//            deviceInfo.Put("Tags", new JSONArray(tags.ToList()));

//            int statusCode = upsertRegistration(registrationId, deviceInfo);

//            if (statusCode == (int)HttpStatus.Ok)
//            {
//                return;
//            }
//            else if (statusCode == (int)HttpStatus.Gone)
//            {
//                settings.Edit().Remove(REGID_SETTING_NAME).Commit();
//                registrationId = retrieveRegistrationIdOrRequestNewOne(handle);
//                statusCode = upsertRegistration(registrationId, deviceInfo);
//                if (statusCode != (int)HttpStatus.Ok)
//                {
//                    Log.Debug("RegisterClient", "Error upserting registration: " + statusCode);
//                    throw new RuntimeException("Error upserting registration");
//                }
//            }
//            else
//            {
//                Log.Debug("RegisterClient", "Error upserting registration: " + statusCode);
//                throw new RuntimeException("Error upserting registration");
//            }
//        }


//        private int upsertRegistration(string registrationId, JSONObject deviceInfo)
//        {



//            HttpWebRequest HttpWReq =
//            (HttpWebRequest)WebRequest.Create(Backend_Endpoint + "/" + registrationId);

//            ASCIIEncoding encoding = new ASCIIEncoding();
//            string stringData = deviceInfo.ToString(); //place body here
//            byte[] data = encoding.GetBytes(stringData);

//            HttpWReq.Headers.Add("Authorization", "Basic " + authorizationHeader);
//            HttpWReq.Headers.Add("Content - Type", "application / json");
//            //HttpWReq.Method = "PUT";
//            //HttpWReq.ContentType = ""; //place MIME type here
//            //HttpWReq.ContentLength = data.Length;


//            string responseText;



//            HttpStatusCode statusCode = 0;
//            try
//            {
//                HttpWebResponse myResp = (HttpWebResponse)HttpWReq.GetResponse();
//                statusCode = myResp.StatusCode;

//                using (var response = HttpWReq.GetResponse())
//                {
//                    using (var reader = new StreamReader(response.GetResponseStream()))
//                    {
//                        responseText = reader.ReadToEnd();
//                        Console.WriteLine(responseText);


//                    }
//                }
//                Stream newStream = HttpWReq.GetRequestStream();
//                newStream.Write(data, 0, data.Length);
//                newStream.Close();

//            }
//            catch (System.Exception)
//            {

//                throw;
//            }



//            return (int)statusCode;
//        }

//        private string retrieveRegistrationIdOrRequestNewOne(string handle)
//        {

//            if (settings.Contains(REGID_SETTING_NAME))
//                return settings.GetString(REGID_SETTING_NAME, null);



//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Backend_Endpoint + "?handle=" + handle);
//            request.Headers.Add("Authorization", "Basic " + authorizationHeader);


//            //ASCIIEncoding encoding = new ASCIIEncoding();
//            //string stringData = deviceInfo.ToString(); //place body here
//            //byte[] data = encoding.GetBytes(stringData);




//            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//            if (response.StatusCode != HttpStatusCode.Accepted)
//            {
//                throw new RuntimeException("Error creating Notification Hubs registrationId");
//            }
//            string registrationId = response.ToString();
//            registrationId = registrationId.Substring(1, registrationId.Length - 1);

//            settings.Edit().PutString(REGID_SETTING_NAME, registrationId).Commit();

//            return registrationId;






//        }




//    }
//}