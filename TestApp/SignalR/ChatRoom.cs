
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Webkit;


namespace TestApp
{
    [Activity(Label = "ChatRoom")]
    public class ChatRoom : Activity
    {

        public static WebView web_view;
        public string UserName;

        //  Button send;

        ImageButton send;
        EditText writeMessage;
        protected override async void OnCreate(Bundle bundle)
        {

            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.chatRoom);
            web_view = FindViewById<WebView>(Resource.Id.webview);
            web_view.Settings.JavaScriptEnabled = true;
            web_view.LoadUrl("http://chatservices.azurewebsites.net");
            web_view.SetWebViewClient(new HelloWebViewClient());


            web_view.Settings.LoadWithOverviewMode = true;
            web_view.Settings.UseWideViewPort = true;

        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back && ChatRoom.web_view.CanGoBack())
            {
                ChatRoom.web_view.GoBack();
                return true;
            }

            return base.OnKeyDown(keyCode, e);
        }
     




    }

    public class HelloWebViewClient : WebViewClient
    {
        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            view.LoadUrl(url);
            return true;
        }

       



       


    }

}