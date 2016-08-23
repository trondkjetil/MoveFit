using System;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using System.IO;
using Android.Media;
using Java.IO;
using Java.Nio;
using System.Collections.Generic;
using System.Drawing;
using Android.Provider;
using Android.Database;
using System.IO.Compression;

namespace TestApp
{
    
    [Activity(Label = "UserProfile", Theme = "@style/Theme2")]
    public class UserProfile : AppCompatActivity
    {
        Java.IO.File imgFile;
        public static String[] array;
        public static IMenuItem itemProfilePic;
        public static IMenuItem itemHome;
        public static IMenuItem itemExit;
        public EditText aboutMeEdit;

        SupportToolbar toolbar;
        ImageView profilePic2;
        BitmapDrawable icon;
        Bitmap bmp;
        List<UserImage> instance;

        public static readonly int PickImageId = 1000;





        public static Bitmap getImageProper(string fileName, int width, int height)
        {

            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };

            // First we get the the dimensions of the file on disk
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            // Next we calculate the ratio that we need to resize the image by


            if (outHeight > height || outWidth > width)
                {
                    inSampleSize = outWidth > outHeight
                        ? outHeight / height
                            : outWidth / width;
                }


            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;



            // Now we will load the image and have BitmapFactory resize it for us.
            BitmapFactory.DecodeFile(fileName, options);
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

              
           

            // Images are being saved in landscape, so rotate them back to portrait if they were taken in portrait
                Matrix mtx = new Matrix();
                ExifInterface exif = new ExifInterface(fileName);
                string orientation = exif.GetAttribute(ExifInterface.TagOrientation);

                switch (orientation)
                {
                    case "6": // portrait
                        mtx.PreRotate(90);
                        resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
                        mtx.Dispose();
                        mtx = null;
                        break;
                    case "1": // landscape
                        break;
                    default:
                        mtx.PreRotate(90);
                        resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
                        mtx.Dispose();
                        mtx = null;
                        break;
                }

            
           
           
            return resizedBitmap;
        }







        protected override async void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.userProfile);
            array = Intent.GetStringArrayExtra("MyData");

           

            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbarNew);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            profilePic2 = FindViewById<ImageView>(Resource.Id.profilePic2);
            profilePic2.SetImageResource(Resource.Drawable.tt);

            TextView userName = FindViewById<TextView>(Resource.Id.userName);
            TextView points = FindViewById<TextView>(Resource.Id.points);
            TextView aboutMe = FindViewById<TextView>(Resource.Id.aboutMe);
            TextView age = FindViewById<TextView>(Resource.Id.age);

            ImageView gender = FindViewById<ImageView>(Resource.Id.gender);


            aboutMeEdit = FindViewById<EditText>(Resource.Id.aboutMeEdit);

            Button editImage = FindViewById<Button>(Resource.Id.editImage);



            try
            {

                instance = await Azure.getUserProfileImage(array[6]);

                if (array[6] != MainStart.userId)
                {

                    editImage.Visibility = ViewStates.Invisible;
                }




                editImage.Click += (object sender, EventArgs eventArgs) =>
                {

                    var imageIntent = new Intent();
                    imageIntent.SetType("image/*");
                    imageIntent.SetAction(Intent.ActionGetContent);
                    StartActivityForResult(
                    Intent.CreateChooser(imageIntent, "Select photo"), PickImageId);


                };

              

                if (array.Length == 0)
                    Finish();


                 bmp = IOUtilz.GetImageBitmapFromUrl(array[3]);
                 icon = new BitmapDrawable(bmp);

                SupportActionBar.SetDisplayShowTitleEnabled(false);
                SupportActionBar.SetIcon(icon);
               

                userName.Text = array[0];
               


                if (array[1] == "Male")
                {
                    gender.SetImageResource(Resource.Drawable.male);
                }
                else
                    gender.SetImageResource(Resource.Drawable.female);


                age.Text =  "Age: " +array[2];

                points.Text = "Points: " + array[4];

                if(array[5] != "")
                {
                    aboutMeEdit.Text = array[5];
                }
                else
                {
                    aboutMeEdit.Hint = "Write something about your self here..";
                }
               

                if (MainStart.userId != array[6])
                {
                    aboutMeEdit.Focusable = false;
                }
                else
                    aboutMeEdit.Focusable = true;

                if(instance.Count > 0 && instance[0].Image[0] != 0)
                {

                //   var deCompressed = Decompress(instance[0].Image);

                   var imageBitmap = BitmapFactory.DecodeByteArray(instance[0].Image, 0, instance[0].Image.Length);
                    profilePic2.SetImageBitmap(imageBitmap);
                }

            
            }
            catch (Exception e)
            {

                Toast.MakeText(this, e.Message , ToastLength.Long).Show();

            }

        }



       
        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            try
            {


                //if ((resultCode == Result.Ok) && (data != null))
                //{
                if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
                {

                   // Android.Net.Uri uri = data.Data;
             string path = GetPathToImage(data.Data);

                    // Uri ju = new Uri(uri.ToString());
                    //var test =   getRotation(ju);




                    //switch (test)
                    //{

                    //    case (int)Android.Media.Orientation.Rotate90:
                    //        test = test;
                    //        break;
                    //    case (int)Android.Media.Orientation.Rotate180:
                    //        test = test;
                    //        break;
                    //    case (int)Android.Media.Orientation.Rotate270:
                    //        test = test;
                    //        break;

                    //    default:
                    //        break;
                    //}


                

                    imgFile = null;
                   // imgFile = new Java.IO.File(GetFilePath(uri)); //GetPathToImage(uri));
                    imgFile = new Java.IO.File(path);


                    // BitmapFactory.Options options = new BitmapFactory.Options();
                    // options.InPreferredConfig = Bitmap.Config.Argb8888;
                    // Bitmap bmap = BitmapFactory.DecodeFile(imgFile.Path, options);


                    // ExifInterface exif = new ExifInterface(imgFile.Name);

                    //// ExifInterface exif = new ExifInterface(imgFile.Path);

                    // var orienta = exif.GetAttributeInt(ExifInterface.TagOrientation, 1);

                    // var orientation = exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Android.Media.Orientation.Normal);

                    // Matrix matrix = new Matrix();
                    // //matrix.PostRotate(90);
                    // orienta = orienta;
                    // switch (orientation)
                    // {

                    //     case (int)Android.Media.Orientation.Rotate90:
                    //         matrix.PostRotate(90);
                    //         break;
                    //     case (int)Android.Media.Orientation.Rotate180:
                    //         matrix.PostRotate(180);
                    //         break;
                    //     case (int)Android.Media.Orientation.Rotate270:
                    //         matrix.PostRotate(270);
                    //         break;

                    //     default:
                    //         break;
                    // }

                    // var bitmapScalled = Bitmap.CreateScaledBitmap(bmap, 550, 350, true);
                    // //  var rotatedBitmap = Bitmap.CreateBitmap(bitmapScalled, 0, 0, bmap.Width, bmap.Height, matrix, true);
                    // var rotatedBitmap = Bitmap.CreateBitmap(bitmapScalled, 0, 0, bitmapScalled.Width, bitmapScalled.Height, matrix, true);

                    // // bitmapScalled.Recycle();
                    // bmap.Recycle();


                    // profilePic2.SetImageBitmap(rotatedBitmap);

                    Bitmap scaledBitmap = remade(imgFile.Path, 550, 350);
                    profilePic2.SetImageBitmap(scaledBitmap);
                    profilePic2.RefreshDrawableState();


                    byte[] bitmapData = null;

                    using (var stream = new MemoryStream())
                    {

                        if (imgFile.Path.ToLower().EndsWith("png"))
                        {
                            //rotatedBitmap.Compress(Bitmap.CompressFormat.Png, 50, stream);
                            scaledBitmap.Compress(Bitmap.CompressFormat.Png, 50, stream);
                            bitmapData = stream.ToArray();

                        }

                        else
                        {
                            // rotatedBitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);
                            scaledBitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);

                            bitmapData = stream.ToArray();

                        }



                    }

                    if (instance.Count > 0 )
                    {
                       

                        var remove = await Azure.removeProfileImage();
                        var insert = await Azure.AddUserImage(bitmapData);

                    }
                    else
                    {
                        var insertBasicImage = await Azure.AddUserImage(bitmapData);
                    }

                }
                   
                else if (data.Data == null)
                {
                   
                    Toast.MakeText(this, "Not able to load picture! " + data.Data.ToString(), ToastLength.Long).Show();
                }

            }
            catch (Exception a)
            {

            //    profilePic2.SetImageBitmap(remade(imgFile.Path, 550, 350));
               Toast.MakeText(this, "Please choose an image from the image Gallery!", ToastLength.Long).Show();

            }

        }


     

        private static int getRotation(Uri jj)
        {

            
            int rotation = 0;
            ContentResolver content = Application.Context.ContentResolver; // ContentResolver; //context.ContentResolver;


            var mediaCursor = content.Query(MediaStore.Images.Media.ExternalContentUri,
                    new String[] { "orientation", "date_added" }, null, null, "date_added desc");

            if (mediaCursor != null && mediaCursor.Count != 0)
            {
                while (mediaCursor.MoveToNext())
                {
                    rotation = mediaCursor.GetInt(0);
                    break;
                }
            }
            mediaCursor.Close();
         
            return rotation;
        }


     

        static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
        public static Bitmap remade( string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                    ? outHeight / height
                        : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            // Images are being saved in landscape, so rotate them back to portrait if they were taken in portrait
            Matrix mtx = new Matrix();
            ExifInterface exif = new ExifInterface(fileName);
            string orientation = exif.GetAttribute(ExifInterface.TagOrientation);

            switch (orientation)
            {
                case "6": // portrait
                    mtx.PreRotate(90);
                    resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
                    mtx.Dispose();
                    mtx = null;
                    break;
                case "1": // landscape
                    break;
                default:
                    mtx.PreRotate(90);
                    resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
                    mtx.Dispose();
                    mtx = null;
                    break;
            }



            return resizedBitmap;
        }
        static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }

  

        private string GetFilePath(Android.Net.Uri uri)
        {
            string[] proj = { MediaStore.Images.ImageColumns.Data };
            //Deprecated
            //var cursor = ManagedQuery(uri, proj, null, null, null);
            var cursor = ContentResolver.Query(uri, proj, null, null, null);
            var colIndex = cursor.GetColumnIndex(MediaStore.Images.ImageColumns.Data);
            cursor.MoveToFirst();
            return cursor.GetString(colIndex);
        }

        private string GetPathToImage(Android.Net.Uri uri)
        {
            string path = null;

            try
            {

           
            // The projection contains the columns we want to return in our query.
            string[] projection = new[] { MediaStore.Audio.Media.InterfaceConsts.Data };
            using (ICursor cursor = ContentResolver.Query(uri, projection, null, null, null))

               // ManagedQuery(
            {
                if (cursor != null)
                {
                    int columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Audio.Media.InterfaceConsts.Data);
                    cursor.MoveToFirst();
                    path = cursor.GetString(columnIndex);
                }
            }

            }
            catch (Exception)
            {

               
            }


            return path;
        }


        public static byte[] toByte(Bitmap bitmap)
        {
            ByteBuffer byteBuffer = ByteBuffer.Allocate(bitmap.ByteCount);
            bitmap.CopyPixelsToBuffer(byteBuffer);
            byte[] bytes = byteBuffer.ToArray<byte>();

            return bytes;

        }



        public override bool OnCreateOptionsMenu(IMenu menu)

        {
            MenuInflater.Inflate(Resource.Menu.action_menu_profile, menu);

            //itemGender = menu.FindItem(Resource.Id.gender);
            //itemAge = menu.FindItem(Resource.Id.age);
            itemProfilePic = menu.FindItem(Resource.Id.profilePicture);
          //  itemExit = menu.FindItem(Resource.Id.exit);
            itemHome = menu.FindItem(Resource.Id.home);

        

            return base.OnCreateOptionsMenu(menu);
        }



        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.home:
                    Finish();
                    return true;

                //case Resource.Id.back:
                //    Finish();
                //    return true;
                case Android.Resource.Id.Home:// Resource.Id.back:
                    OnBackPressed();
                    return true;

                case Resource.Id.bmi:
                   Intent myIntent = new Intent(this, typeof(Calculator));
                    StartActivity(myIntent);

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }


        }


       

        protected override void OnStart()
        {
            base.OnStart();
        }
        protected async override void OnStop()
        {
            base.OnStop();
            var instance = await Azure.setAboutMeInfo(array[6], aboutMeEdit.Text);
        }
        protected async override void OnDestroy()
        {
            base.OnDestroy();

            var instance = await Azure.setAboutMeInfo(array[6], aboutMeEdit.Text);
        }

    }
}