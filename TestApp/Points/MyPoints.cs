using System;

namespace TestApp.Points
{

    public class MyPoints 
    {
        public MyPoints()
        {
            
        }

        public static int calculatePoints(string type, double distance)
        {
            double sumPoints = 0;

            if (type == "Walking")
            {

                sumPoints = (int)Math.Round(distance / 5);    // (int)Math.Round(0.4) / 2;

            }
           else if (type == "Running")
            {

                sumPoints = (int)Math.Round(distance / 4);   //* (int)Math.Round(0.4) / 2;

            }
            else if (type == "Hiking")
            {

                sumPoints = (int)Math.Round(distance / 4);  //* (int)Math.Round(0.3) / 2;

            }
            else if (type == "Bicycling")
            {
                sumPoints = (int)Math.Round(distance / 6);   // * (int)Math.Round(0.2) / 2;

            }
            else if (type == "Skiing")
            {

                sumPoints = (int)Math.Round(distance / 5);   //* (int) Math.Round(0.45) / 2;

            }
            else
                sumPoints = 0;


            int result = Convert.ToInt32(sumPoints);

            return result;
        }

    }
}