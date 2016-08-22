using System;
using Android.Graphics;
using System.Net;
using Android.Net;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Android.App;

namespace TestApp
{
    public class IOUtilz
	{

		public static Bitmap profileImage  {set;get;}

		public IOUtilz ()
		{
                   }
        public static bool isOnline(ConnectivityManager mng)
        {

            NetworkInfo activeConnection = mng.ActiveNetworkInfo;
            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

            return isOnline;
        }
        //Simple read and write
        //        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //        string filename = Path.Combine(path, "myfile.txt");

        //            using (var streamWriter = new StreamWriter(filename, true))
        //            {
        //                streamWriter.WriteLine(DateTime.UtcNow);
        //            }

        //            using (var streamReader = new StreamReader(filename))
        //            {
        //                string content = streamReader.ReadToEnd();
        //System.Diagnostics.Debug.WriteLine(content);
        //            }

        //public static Bitmap deCodeByte(byte[] image)
        //{

        //	Bitmap bit = BitmapFactory.DecodeByteArray (image, 0, image.Length);

        //	return bit;
        //}

        //public static byte[] codeByte(Bitmap bitmap){
        //		byte[] bitmapData = null;
        //	using (var stream = new MemoryStream())
        //	{
        //	bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);

        //	}

        //	return bitmapData;
        //}


        //public static void listDir ()
        //{
        //var directories = Directory.EnumerateDirectories("./");
        //foreach (var directory in directories) {
        //	Console.WriteLine(directory);
        //}
        //}



        //void readFile(){
        //	var text = File.ReadAllText("TestData/ReadMe.txt");
        //	Console.WriteLine(text);
        //}

        ////xml serialization

        //void serializeXml(){

        //	using (TextReader reader = new StreamReader("./TestData/test.xml")) {
        //	//	XmlSerializer serializer = new XmlSerializer(typeof(MyObject));
        //		//var xml = (MyObject)serializer.Deserialize(reader);
        //	}

        //}



        //void creatingFilesAndDir(){

        //	var documents =
        //		Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments); 
        //	var filename = System.IO.Path.Combine (documents, "Write.txt");
        //	File.WriteAllText(filename, "Write this text into a file");

        //	 documents =
        //		Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
        //	var directoryname = System.IO.Path.Combine (documents, "NewDirectory");
        //	Directory.CreateDirectory(directoryname);

        //}








        //public static void SaveFile(string directory, Bitmap bitmap)
        //{
        //	var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
        //	var filePath = System.IO.Path.Combine (sdCardPath, "MyFile");
        //	string fileName = directory;

        //	using (var os = new FileStream(filePath, FileMode.CreateNew))
        //	{
        //		bitmap.Compress(Bitmap.CompressFormat.Jpeg, 95, os);
        //	}
        //}



        public string distanceMoved(int distanceMoved)
        {
            string unit = " km";
            double dist = 0;
            var pref = LoadPreferences();
            if (pref[1] == 1)
            {
                unit = " miles";
                dist = (int)ConvertKilometersToMiles(distanceMoved / 1000);
            }
            else
            {
                dist = distanceMoved / 1000;
            }
            return dist.ToString();
        }
        public static double ConvertMilesToKilometers(double miles)
        {
            //
            // Multiply by this constant and return the result.
            //
            return miles * 1.609344;
        }

        public static double ConvertKilometersToMiles(double kilometers)
        {
            //
            // Multiply by this constant.
            //
            return kilometers * 0.621371192;
        }



        public static int[] LoadPreferences()
        {
            int value1 = 0;
            int value2 = 0;
            int value3 = 0;
            var prefs = Application.Context.GetSharedPreferences("preferences", FileCreationMode.Private);

            //if (prefs.Contains("distance"))
            //{
            //    value = prefs.GetInt("distance", 0);
            //}else if (prefs.Contains("unit"))
            //{
            //    value = prefs.GetInt("unit", 0);
            //}else if (prefs.Contains("interval"))
            //{
            //    value = prefs.GetInt("interval", 0);
            //}

            value1 = prefs.GetInt("distance", 0);
            value2 = prefs.GetInt("unit", 0);
            value3 = prefs.GetInt("interval", 0);


            int[] result = new int[3];
            result[0] = value1;
            result[1] = value2;
            result[2] = value3;

            return result;
        }

        public static void SavePreferences(int unit, int distance, int interval)
        {
            var prefs = Application.Context.GetSharedPreferences("preferences", FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutInt("unit",unit );
            editor.PutInt("distance", distance);
            editor.PutInt("interval", interval);
            editor.Commit();
        }

        public static bool IsKitKatWithStepCounter(PackageManager pm)
        {

            // Require at least Android KitKat
            int currentApiVersion = (int)Build.VERSION.SdkInt;
            // Check that the device supports the step counter and detector sensors
            return currentApiVersion >= 19
            && pm.HasSystemFeature(Android.Content.PM.PackageManager.FeatureSensorStepCounter)
            && pm.HasSystemFeature(Android.Content.PM.PackageManager.FeatureSensorStepDetector);

        }

        public static Bitmap scaleDown(Bitmap realImage, float maxImageSize,
			bool filter) {

			float ratio = Math.Min((float) maxImageSize / realImage.GetBitmapInfo ().Width,(float) maxImageSize / realImage.GetBitmapInfo ().Height);
			int width = (int)Math.Round ((float)ratio * realImage.GetBitmapInfo ().Width);
			int height = (int)Math.Round ((float)ratio * realImage.GetBitmapInfo ().Height);

			Bitmap newBitmap = Bitmap.CreateScaledBitmap(realImage, width,
				height, filter);
			return newBitmap;
		}


		public static Bitmap GetImageBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

            try
            {

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}


            }
            catch (Exception)
            {

                
            }

            return getRoundedShape(imageBitmap);
		}

        public static Bitmap getRoundedShape(Bitmap scaleBitmapImage)
        {
            const int targetWidth = 115;
            const int targetHeight = 115;
            Bitmap targetBitmap = Bitmap.CreateBitmap(targetWidth,
                targetHeight, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(targetBitmap);
            Path path = new Path();
            path.AddCircle(((float)targetWidth - 1) / 2,
                ((float)targetHeight - 1) / 2,
                (Math.Min(((float)targetWidth),
                    ((float)targetHeight)) / 2),
               Path.Direction.Ccw);

            canvas.ClipPath(path);
            Bitmap sourceBitmap = scaleBitmapImage;
            canvas.DrawBitmap(sourceBitmap,
                new Rect(0, 0, sourceBitmap.Width,
                    sourceBitmap.Height),
                new Rect(0, 0, targetWidth, targetHeight), null);
            return targetBitmap;
        }

  //      public static Bitmap getProfileImage(string url){
		//	string url_profilePic = url;
		//	//profilePicture = GetImageBitmapFromUrl(url_profilePic);
		//	WebClient web = new WebClient();
		//	web.DownloadDataCompleted += new DownloadDataCompletedEventHandler(web_DownloadDataCompleted);
		//	web.DownloadDataAsync(new Uri(url_profilePic)); 


		//	return profileImage;
		//}

		//static void web_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
		//{
		//	if (e.Error != null)
		//	{
				
		//		Console.WriteLine (e.Result);
		//	}
		//	else
		//	{

		//		Bitmap bm = BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length);
		//		bm = IOUtilz.scaleDown (bm,180,false);
		//		bm = IOUtilz.getRoundedShape (bm);
		//		profileImage = bm;
					
		//	}

		//}

	


	}
}

