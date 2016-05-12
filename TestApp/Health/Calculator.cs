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
    [Activity(Label = "Calculator")]
    public class Calculator : Activity
    {
        public double bmr { get; set;}
        public double bmi { get; set; }
        public int weight { get; set;}
        public int height { get; set; }
        public int age { get; set; }
        public bool gender { get; set; }

        public bool kcals { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.calculator);


            TextView age = FindViewById<TextView>(Resource.Id.textView1);
            TextView height = FindViewById<TextView>(Resource.Id.textView2);
            TextView weight = FindViewById<TextView>(Resource.Id.textView3);
            TextView result = FindViewById<TextView>(Resource.Id.textView3);

            EditText _age = FindViewById<EditText>(Resource.Id.editText1);
            EditText _height = FindViewById<EditText>(Resource.Id.editText2);
            EditText _weight = FindViewById<EditText>(Resource.Id.editText3);

            Button calculate = FindViewById<Button>(Resource.Id.results);

            calculate.Click += (sender, e) => {
                string results = "";

                results = CalcBmi(Convert.ToInt32(_age.Text), Convert.ToInt32(_height.Text) , Convert.ToInt32(_weight.Text) );

                result.Text = results + System.Environment.NewLine + "Calories needed to maintain current weight: " + CalcNeededKcals(Convert.ToInt32(_age.Text), Convert.ToInt32(_height.Text), Convert.ToInt32(_weight.Text)) + System.Environment.NewLine +
               "BMR is: " + CalcBmr(Convert.ToInt32(_age.Text), Convert.ToInt32(_height.Text), Convert.ToInt32(_weight.Text));



            };


        }


        public double CalcNeededKcals(int age, int height, int weight)
        {

            double calories = (66 + (13.7 * weight) + (5 * height) - (6.8 * age)); //finally we calculate the final variable which is the number of calories equal to the rest metabolic rate

        //    double activity = Double.parseDouble(reader.nextLine()); //we are using the Double.parseDouble command to convert the user input into a doble variable (a decimal number)

            //     System.out.println(name + ", the ammount of calories you need to intake in order to maintain your weight is " + calories * activity); //and in the process of displaying it we multiply it by the activity coefficient and display the final result to the user.

            return calories;
        }

        public string CalcBmi(int age, int height, int weight)
        {
            string result = "";          
            string bmiDescription = string.Empty;
            bmi = weight / height * height;

            if (bmi < 16.5)
                
                bmiDescription = "severely underweight";
           
            else if (bmi >= 16.5 && bmi < 18.5)
                
                bmiDescription = "underweight";
           
            else if (bmi >= 18.5 && bmi < 25)
               
                bmiDescription = "normal";
            
            else if (bmi >= 25 && bmi <= 30)
                
                bmiDescription = "overweight";
          
            else if (bmi > 30 && bmi <= 35)
               
                bmiDescription = "obese";
           
            else if (bmi > 35 && bmi <= 40)
                
                bmiDescription = "clinically obese";
          
            else
                bmiDescription = "morbidly obese";

            result = string.Format("Your Body Mass Index (BMI) is: {0}. This would be considered {1}.", bmi, bmiDescription);

            return result;
        }


        public double CalcBmr(int age,int height,int weight)
        {

            //BMR = 10 * weight(kg) + 6.25 * height(cm) - 5 * age(y) + 5         (man)
            //  BMR = 10 * weight(kg) + 6.25 * height(cm) - 5 * age(y) - 161(woman)
            //The calories needed to maintain your weight equal to the BMR value, multiplied by an activity factor. To loss 1 pound, or 0.5kg per week, you will need to shave 500 calories from your daily menu.

            if (gender)
            {
                bmr = 10 * weight + 6.25 * height - 5 * age + 5;
            }
            else
                bmr = 10 * weight + 6.25 * height - 5 * age - 161;

            return bmr;
        }




    }
}