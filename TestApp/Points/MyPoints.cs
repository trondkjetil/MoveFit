using System;

namespace TestApp.Points
{

    public class MyPoints 
    {
        public MyPoints()
        {
            
        }

        public static int calculatePoints(string type, int distance)
        {
            int sumPoints = 0;

            if (type == "Walking")
            {

                sumPoints += distance / 5;    // (int)Math.Round(0.4) / 2;

            }
           else if (type == "Running")
            {

                sumPoints +=  distance / 5; //* (int)Math.Round(0.4) / 2;

            }
            else if (type == "Hiking")
            {

                sumPoints += distance / 5; //* (int)Math.Round(0.3) / 2;

            }
            else if (type == "Bicycling")
            {
                sumPoints += distance / 5; // * (int)Math.Round(0.2) / 2;

            }
            else if (type == "Skiing")
            {

                sumPoints += distance / 5; //* (int) Math.Round(0.45) / 2;

            }
            else
                sumPoints = 0;

                return sumPoints;
        }

    }
}