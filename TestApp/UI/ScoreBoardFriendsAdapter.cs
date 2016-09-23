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
using Android.Graphics;

namespace TestApp
{
    class UserAdapterScoreboard : BaseAdapter<User>
    {
        private Context mContext;
        private int mRowLayout;
        private List<User> users;
        private int [] mAlternatingColors;

        public UserAdapterScoreboard(Context context, int rowLayout, List<User> users)
        {
            mContext = context;
            mRowLayout = rowLayout;
            this.users = users; //009900 
             mAlternatingColors = new int[] { 0xF2F2F2, 0x6567dd };
        }

        public override int Count
        {
            get { return users.Count; }
        }

        public override User this[int position]
        {
            get { return users[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(mRowLayout, parent, false);
            }

            row.SetBackgroundColor(GetColorFromInteger(mAlternatingColors[position % mAlternatingColors.Length]));

            ImageView image = row.FindViewById<ImageView>(Resource.Id.profileImage_score);
            image.SetImageBitmap(IOUtilz.GetImageBitmapFromUrl(users[position].ProfilePicture));

            TextView lastName = row.FindViewById<TextView>(Resource.Id.txtLastName);
            lastName.Text = users[position].UserName;

            TextView age = row.FindViewById<TextView>(Resource.Id.txtAge);
            age.Text = users[position].Age.ToString();

            TextView gender = row.FindViewById<TextView>(Resource.Id.txtGender);
            gender.Text = users[position].Sex;

			TextView score = row.FindViewById<TextView>(Resource.Id.txtScore);
			score.Text = users[position].Points.ToString();

            if ((position % 2) == 1)
            {
                //X colored background, set text white

                //firstName.SetTextColor(Color.White);
                lastName.SetTextColor(Color.White);
                age.SetTextColor(Color.White);
                gender.SetTextColor(Color.White);
				score.SetTextColor (Color.White);
            }

            else
            {
                //White background, set text black

                //firstName.SetTextColor(Color.Black);
                lastName.SetTextColor(Color.Black);
                age.SetTextColor(Color.Black);
                gender.SetTextColor(Color.Black);
				score.SetTextColor (Color.Black);
            }

            return row;
        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }
    }
}