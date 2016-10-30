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
        public static double bmr;
       // public static double bmi;
        public static double weight;
        public static double height;
        public static int age;
        public static bool gender;
        public static bool kcals;
        public static double activityLevel;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.calculator);



            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinneren);
            spinner.ItemSelected += spinner_ItemSelected;
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.activity_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            TextView ageView = FindViewById<TextView>(Resource.Id.textView1);
            TextView heightView = FindViewById<TextView>(Resource.Id.textView2);
            TextView weightView = FindViewById<TextView>(Resource.Id.textView3);
            TextView resultView = FindViewById<TextView>(Resource.Id.textView4);

            RadioGroup radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            RadioButton radioButton = FindViewById<RadioButton>(radioGroup.CheckedRadioButtonId);
            RadioButton radioButton2 = FindViewById<RadioButton>(radioGroup.CheckedRadioButtonId);


            EditText _age = FindViewById<EditText>(Resource.Id.editText1);  
            EditText _height = FindViewById<EditText>(Resource.Id.editText2);
            EditText _weight = FindViewById<EditText>(Resource.Id.editText3);


            gender = true;

          

                radioGroup.CheckedChange += (s, e) =>
                {

                    if (radioButton.Checked)
                    {
                        gender = true;

                    }
                    else
                        gender = false;

                };




                _age.TextChanged += (sender, e) =>
                {
                    try
                    {
                        age = Convert.ToInt32(_age.Text.ToString());
                    }
                    catch(Exception)
                    {
                        displayErrorMessage();

                    }
                   

                };
                _height.TextChanged += (sender, e) =>
                {
                    try
                    {
                        height = Convert.ToDouble(_height.Text.ToString());
                    }
                    catch (Exception)
                    {

                        displayErrorMessage();
                    }
                  
                    
                };
                _weight.TextChanged += (sender, e) =>
                {
                    try
                    {
                        weight = Convert.ToDouble(_weight.Text.ToString());
                    }
                    catch (Exception)
                    {
                        displayErrorMessage();

                    }
                    
                   
                };


          

            Button calculate = FindViewById<Button>(Resource.Id.results);

            calculate.Click += (sender, e) => {
                string results = "";
                if (gender)
                {

                    results = "Female: ";
                   
                }
                else
                {
                    results = "Male: ";
                    

                }
             //   double avg = CalcNeededKcals(age, height, weight) + CalcBmr(age, height, weight, gender) / 2;

                double avg = CalcBmr(age, height, weight, gender);
                avg = (int) Math.Round(avg);

                results = CalcBmi(height,weight);

                resultView.Text = results + System.Environment.NewLine + System.Environment.NewLine + "Calories needed to maintain current weight: ca " + avg; 
            };


        }


       

        public string CalcBmi(double height, double weight)
        {

            string res = "";
            string bmiDescription = "";
            double bmi = 0;

            string testen = height.ToString();



         
            try
            {


                //   testen = testen.Substring(0, 1) + "." + testen.Substring(1, 2);
                //if (testen.Contains("."))
                //{
                //    testen = testen.Replace(".", "");
                //    bmi = weight / (Convert.ToDouble(testen) * Convert.ToDouble(testen));
               // bmi = weight / (height * height);
                //}else
                //{

                testen = testen.Substring(0, 1) + "." + testen.Substring(1, 2);
                bmi = weight / (Convert.ToDouble(testen) * Convert.ToDouble(testen));

          


               // }




            }
            catch (Exception)
            {

            }

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



            bmi = Convert.ToInt32(bmi);

           

            res = string.Format("Your Body Mass Index (BMI) is: {0}. This would be considered {1}.", bmi, bmiDescription);

            return res;
        }

        public double CalcNeededKcals(double age, double height, double weight)
        {

            double calories = (66 + (13.7 * weight) + (5 * height) - (6.8 * age)); //finally we calculate the final variable which is the number of calories equal to the rest metabolic rate

            //    double activity = Double.parseDouble(reader.nextLine()); //we are using the Double.parseDouble command to convert the user input into a doble variable (a decimal number)

            //     System.out.println(name + ", the ammount of calories you need to intake in order to maintain your weight is " + calories * activity); //and in the process of displaying it we multiply it by the activity coefficient and display the final result to the user.

            return calories * activityLevel;
        }
        public double CalcBmr(double age,double height,double weight, bool gender)
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

            return bmr * activityLevel;
        }


        public void displayErrorMessage()
        {


            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Invalid input");
            alert.SetMessage("You can only write numbers!");
            alert.SetNeutralButton("Ok", (senderAlert, args) => {
                //Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
            });

            alert.Create().Show();
           
           // Dialog dialog = alert.Create().Show();
            //dialog.Show();
        }




        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            if (e.Position == 0)
            {
                activityLevel = 1.2;
            }
            else if (e.Position == 1)
            {
                activityLevel = 1.375;


            }
            else if (e.Position == 2)
            {

                activityLevel = 1.55;
            }
            else if (e.Position == 3)
            {
                activityLevel = 1.725;

            }
            else if (e.Position == 4)
            {

                activityLevel = 1.9;
            }
           
           
        }





        public override void OnBackPressed()
        {

            base.OnBackPressed();
            Finish();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();


        }




    }
}