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
    class RouteAdapterScoreboard : BaseAdapter<Route>
    {
        private Context mContext;
        private int mRowLayout;
        private List<Route> routes;
        private int[] mAlternatingColors;

        public RouteAdapterScoreboard(Context context, int rowLayout, List<Route> routes)
        {
            mContext = context;
            mRowLayout = rowLayout;
            this.routes = routes; //009900 
            mAlternatingColors = new int[] { 0xF2F2F2, 0x6567dd };
        }

        public override int Count
        {
            get { return routes.Count; }
        }

        public override Route this[int position]
        {
            get { return routes[position]; }
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


            //TextView firstName = row.FindViewById<TextView>(Resource.Id.txtFirstName);
            //firstName.Text = users[position].UserName;
            ImageView image = row.FindViewById<ImageView>(Resource.Id.profileImage_score);
            image.SetImageResource(Resource.Drawable.maps);

            TextView lastName = row.FindViewById<TextView>(Resource.Id.routeName);
            lastName.Text = routes[position].Name;

            TextView age = row.FindViewById<TextView>(Resource.Id.review);
            age.Text = routes[position].Review.ToString();

            TextView gender = row.FindViewById<TextView>(Resource.Id.distance);
            gender.Text = routes[position].Distance;

            TextView score = row.FindViewById<TextView>(Resource.Id.routeType);
            score.Text = routes[position].RouteType.ToString();

            if ((position % 2) == 1)
            {
                //X colored background, set text white

                //firstName.SetTextColor(Color.White);
                lastName.SetTextColor(Color.White);
                age.SetTextColor(Color.White);
                gender.SetTextColor(Color.White);
                score.SetTextColor(Color.White);
            }

            else
            {
                //White background, set text black

                //firstName.SetTextColor(Color.Black);
                lastName.SetTextColor(Color.Black);
                age.SetTextColor(Color.Black);
                gender.SetTextColor(Color.Black);
                score.SetTextColor(Color.Black);
            }

            return row;
        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }
    }
}