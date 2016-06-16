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

namespace TestApp
{

    public enum DistanceType { Miles, Kilometers };
    class HarvesineCalc
    {
        /// <summary>  
        /// Returns the distance in miles or kilometers of any two  
        /// latitude / longitude points.  
        /// </summary>  
        /// <param name=”pos1″></param>  
        /// <param name=”pos2″></param>  
        /// <param name=”type”></param>  
        /// <returns></returns>  
        public double Distance(User pos1, User pos2, DistanceType type)
        {
            double R = (type == DistanceType.Miles) ? 3960 : 6371;
            double dLat = this.toRadian(Convert.ToDouble(pos2.Lon) - Convert.ToDouble(pos2.Lat));
            double dLon = this.toRadian(Convert.ToDouble(pos2.Lon) - Convert.ToDouble(pos1.Lon));
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(this.toRadian(Convert.ToDouble(pos1.Lat))) * Math.Cos(this.toRadian(Convert.ToDouble(pos2.Lat))
            * Math.Sin(dLon / 2) * Math.Sin(dLon / 2));
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;

            return d;
        }
       
        private double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }


    }
}