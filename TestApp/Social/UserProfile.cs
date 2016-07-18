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

        public static String[] array;
        public static IMenuItem itemProfilePic;
        public static IMenuItem itemGender;
        public static IMenuItem itemAge;
        public static IMenuItem itemExit;
        public EditText aboutMeEdit;

        SupportToolbar toolbar;
        ImageView profilePic2;
        BitmapDrawable icon;
        Bitmap bmp;
        List<UserImage> instance;


        protected override async void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.userProfile);
            array = Intent.GetStringArrayExtra("MyData");

           

            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbarNew);
            SetSupportActionBar(toolbar);

     
            profilePic2 = FindViewById<ImageView>(Resource.Id.profilePic2);
            profilePic2.SetImageResource(Resource.Drawable.tt);

            TextView userName = FindViewById<TextView>(Resource.Id.userName);
            TextView points = FindViewById<TextView>(Resource.Id.points);
            TextView aboutMe = FindViewById<TextView>(Resource.Id.aboutMe);

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
                    Intent.CreateChooser(imageIntent, "Select photo"), 0);


                };

              



                if (array.Length == 0)
                    Finish();


                 bmp = IOUtilz.GetImageBitmapFromUrl(array[3]);
                 icon = new BitmapDrawable(bmp);

                SupportActionBar.SetDisplayShowTitleEnabled(false);
                SupportActionBar.SetIcon(icon);


                userName.Text = array[0];

                points.Text = "Points: " + array[4];

                if(array[5] != "")
                {
                    aboutMeEdit.Text = array[5];
                }
                else
                {
                    aboutMeEdit.Text = "Write something about you self..";
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


                if ((resultCode == Result.Ok) && (data != null))
                {
                    Android.Net.Uri uri = data.Data;
                    //profilePic2.SetImageURI(uri);
                    //profilePic2.RefreshDrawableState();
                    //profilePic2.BuildDrawingCache();
                    //Bitmap bmap = profilePic2.GetDrawingCache(true);


                    Java.IO.File imgFile = new Java.IO.File(GetPathToImage(uri));


                    //if (imgFile.Length() < 15000000)
                    //{
                    //Toast.MakeText(this, "FILE IS MORE THAN 15MB!" + imgFile.Name, ToastLength.Short).Show();
                    //}

                   

                        BitmapFactory.Options options = new BitmapFactory.Options();
                        options.InPreferredConfig = Bitmap.Config.Argb8888;
                        Bitmap bmap = BitmapFactory.DecodeFile(imgFile.Path, options);
                 
           
                   
                    var bitmapScalled = Bitmap.CreateScaledBitmap(bmap, 400, 350, true);
                    bmap.Recycle();
                    profilePic2.SetImageBitmap(bitmapScalled);
                    profilePic2.RefreshDrawableState();



                    ///byte[] tested = System.IO.File.ReadAllBytes(imgFile.Path);

                    //using (var streamReader = new StreamReader(imgFile.Path))
                    //{
                    //    var bytes = default(byte[]);
                    //    using (var memstream = new MemoryStream())
                    //    {
                    //        streamReader.BaseStream.CopyTo(memstream);
                    //        bytes = memstream.ToArray();


                    //    }

                    //}



                        byte[] bitmapData;
                    // bitmapData = Compress(toByte(bitmapScalled));


                    using (var stream = new MemoryStream())
                    {

                        if (imgFile.Path.ToLower().EndsWith("png"))
                            bitmapScalled.Compress(Bitmap.CompressFormat.Png, 100, stream);
                        else
                            bitmapScalled.Compress(Bitmap.CompressFormat.Jpeg, 95, stream);


                       // bitmapScalled.Compress(Bitmap.CompressFormat.Png, 100, ms);
                       

                        bitmapData = stream.ToArray();


                    }


                    if (instance.Count > 0 && instance[0].Image[0] != 0)
                    {
                        await Azure.setProfileImage(MainStart.userId, bitmapData);
                    }else
                    {
                        var insertBasicImage = await Azure.AddUserImage(MainStart.userId, bitmapData);
                    }
                  
                

                    //if (instance[0].Id == null || instance[0].Id == "")
                    //    {
                    //        var insertBasicImage = await Azure.AddUserImage(MainStart.userId, bitmapData);

                    //    }
                    //    else
                    //        await Azure.setProfileImage(MainStart.userId, bitmapData);




                        //       profilePic2.SetImageBitmap(toBitmap(bitmapData));


                    }


                    // var test =  Azure.setProfileImage(MainStart.userId, toByte(bitmapScalled));
                

                else if (data.Data == null)
                {
                   
                    Toast.MakeText(this, "Not able to load picture! " + data.Data.ToString(), ToastLength.Long).Show();
                }

            }
            catch (Exception a)
            {

                Toast.MakeText(this, "Problem occured :( " +  a.Message, ToastLength.Long).Show();

            }

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


        private string GetPathToImage(Android.Net.Uri uri)
        {
            string path = null;
            // The projection contains the columns we want to return in our query.
            string[] projection = new[] { MediaStore.Audio.Media.InterfaceConsts.Data };
            using (ICursor cursor =  ManagedQuery(uri, projection, null, null,null))
            {
                if (cursor != null)
                {
                    int columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Audio.Media.InterfaceConsts.Data);
                    cursor.MoveToFirst();
                    path = cursor.GetString(columnIndex);
                }
            }
            return path;
        }



        //public Bitmap toBitmap(byte[] avatarBytes)
        //{
        //    Bitmap imageBitmap = null;
        //    //var avatarImageView = profilePic2;
        //    if (profilePic2 != null)
        //    {
        //        imageBitmap = BitmapFactory.DecodeByteArray(avatarBytes, 0, avatarBytes.Length);
        //     //  avatarImageView.SetImageBitmap(imageBitmap);
        //    }

        //    return imageBitmap;
        //}


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

            itemGender = menu.FindItem(Resource.Id.gender);
            itemAge = menu.FindItem(Resource.Id.age);
            itemProfilePic = menu.FindItem(Resource.Id.profilePicture);
            itemExit = menu.FindItem(Resource.Id.exit);

            try
            {



                itemAge.SetTitle("Age " + array[2]);

                if (array[1] == "Male")
                {
                    itemGender.SetIcon(Resource.Drawable.male);
                }
                else
                    itemGender.SetIcon(Resource.Drawable.female);

            }
            catch (Exception)
            {


            }

            return base.OnCreateOptionsMenu(menu);
        }



        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {


                case Resource.Id.exit:
                    Finish();
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