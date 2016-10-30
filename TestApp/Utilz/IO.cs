using System;
using Android.Graphics;
using System.Net;
using Android.Net;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Android.App;
using Android.Widget;

namespace TestApp
{
    public class IOUtilz
	{

		public IOUtilz ()
		{
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
            int value4 = 0;

            var prefs = Application.Context.GetSharedPreferences("preferences", FileCreationMode.Private);
            value1 = prefs.GetInt("distance", 0);
            value2 = prefs.GetInt("unit", 0);
            value3 = prefs.GetInt("interval", 0);
            value4 = prefs.GetInt("tracker", 0);

            int[] result = new int[4];
            result[0] = value1;
            result[1] = value2;
            result[2] = value3;
            result[3] = value4;

            return result;
        }
     
        public static void SavePreferences(int unit, int distance, int interval, int state)
        {
            var prefs = Application.Context.GetSharedPreferences("preferences", FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutInt("unit",unit );
            editor.PutInt("distance", distance);
            editor.PutInt("interval", interval);
            editor.PutInt("tracker", state);
            editor.Commit();
        }

      

        public static Bitmap scaleDown(Bitmap realImage, float maxImageSize,
			bool filter) {
            Bitmap newBitmap = null;

            float ratio = Math.Min((float) maxImageSize / realImage.GetBitmapInfo ().Width,(float) maxImageSize / realImage.GetBitmapInfo ().Height);
			int width = (int)Math.Round ((float)ratio * realImage.GetBitmapInfo ().Width);
			int height = (int)Math.Round ((float)ratio * realImage.GetBitmapInfo ().Height);

			 newBitmap = Bitmap.CreateScaledBitmap(realImage, width,
				height, filter);
			return newBitmap;
		}
        public static Bitmap DownloadImageUrl(string url)
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

            return scaleDown(imageBitmap,80,false); 
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
            Bitmap targetBitmap = null;
            const int targetWidth = 115;
            const int targetHeight = 115;
             targetBitmap = Bitmap.CreateBitmap(targetWidth,
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

	


	}
}

