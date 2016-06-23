using System;
using Android.Graphics;
using System.IO;
using System.Xml.Serialization;
using System.Net;
using Android.Widget;
using Android.Net;
using System.Threading.Tasks;
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

        /*
		 * 
		 * 
		 * This code enumerates the subdirectories in the current directory (specified by the "./" parameter), which is the location of your application executable. 

		*/


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

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return getRoundedShape(imageBitmap);
		}





        public static Bitmap getRoundedShape(Bitmap scaleBitmapImage)
        {
            const int targetWidth = 95;
            const int targetHeight = 95;
            Bitmap targetBitmap = Bitmap.CreateBitmap(targetWidth,
                targetHeight, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(targetBitmap);
            Android.Graphics.Path path = new Android.Graphics.Path();
            path.AddCircle(((float)targetWidth - 1) / 2,
                ((float)targetHeight - 1) / 2,
                (Math.Min(((float)targetWidth),
                    ((float)targetHeight)) / 2),
                Android.Graphics.Path.Direction.Ccw);

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

		static void web_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				
				Console.WriteLine (e.Result);
			}
			else
			{

				Bitmap bm = BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length);
				bm = IOUtilz.scaleDown (bm,180,false);
				bm = IOUtilz.getRoundedShape (bm);
				profileImage = bm;
					
			}

		}

	


	}
}

