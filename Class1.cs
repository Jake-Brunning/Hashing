using System;
using System.Collections.Generic;
namespace hashing
{
    public class Hash
    {
        public string salt = "eqwru20~dc#x!l"; //salt value, defualuted to this. Honestly this probably shouldn't be here
        //as your meant to tuse a different salt for each hash. However, I put it here anyway as it might be easier for users
        //as they can define a new hash, calculate the salt themselfs, then hash text.

        public string Text; //the text to hash.Keeping this here for the same reason above    

        private int OutputLength; //OutputLength. Private as it shouldn't be changed
        public List<string> Indexes = new(); //the indexes which have been generated

        //Good argument could be make that only OutputLength and indexes should be here as they remain cconstant per each hash.

        public Hash(string salt, int OutputLength)
        {
            this.salt = salt;
            this.OutputLength = OutputLength;
        }
        public Hash(string salt, string Text, int OutputLength)
        {
            this.salt = salt;
            this.Text = Text;
            this.OutputLength = OutputLength;
        }
        public Hash(int OutputLength)
        {
            this.OutputLength = OutputLength;
        }
        
        
        private string generateSalt(string text)
        {
            return "hello world";
        }
        public string HashText(string inputtedText = "", string saltText = "")
        {

            //just some babyproofing
            //------------------------------
            if(Text == null)
            {
                Exception exception = new Exception("No text to hash. Change text to a value");
                return "";
            }
            if(inputtedText != "")
            {
                Text = inputtedText;
            }
            if(salt == null)
            {
                Exception exception = new Exception("No salt value. Change salt to a value");
                return "";
            }
            if(saltText != "")
            {
                salt = generateSalt(saltText);
            }
            if(Text.Length < 4)
            {
                Exception exception = new Exception("Text to hash is too small. Make it bigger");
                return "";
            }
            //------------------------------
            //Okay hash time
            //This about to be amazing

            int firstCharacter = (int)Text[0];
            int lastCharacter = (int)Text[Text.Length - 1];
            int secondToLastCharacter = (int)Text[Text.Length - 2];

            int firstLastAdded = firstCharacter + lastCharacter;
            int toThePower = Math.Abs(firstLastAdded - secondToLastCharacter);

            toThePower %= 5; //so it cant be a larger number than the program can handle
            toThePower++; //so it can never be zero
            Text += salt; //add salt now as salt value is always the same
            int HowMuchToDivideBy = Text[0] % Text[1];
            HowMuchToDivideBy++;
            for (int i = 2; i < Text.Length; i++) //loop of every unicode number modded cause randomness
            {
                HowMuchToDivideBy = HowMuchToDivideBy % (int)Text[i];

            }
            HowMuchToDivideBy++; //so it cant be zero

            char[] x = Text.ToCharArray(); //characters interested in
            UInt64[] unicodeValues = new UInt64[x.Length]; //storing unicode values which are going to have maths performed on them
            UInt64 temp = 0;

            for (int i = 0; i < Text.Length; i++)
            {

                temp = Convert.ToUInt64(Math.Pow(Convert.ToDouble((int)x[i]), toThePower));//raise the unicode value to the power calculated earlier
                unicodeValues[i] = temp; //gets unicode number for character
                try
                {
                    //times it by the unicode value next / 3 cause randomness
                    unicodeValues[i] = Convert.ToUInt64(unicodeValues[i] * Convert.ToUInt64((int)x[i + 1] / 3));
                }
                catch //in case last unicode value
                {
                    unicodeValues[i] = Convert.ToUInt64(unicodeValues[i] * Convert.ToUInt64((int)x[0] / 3));
                }

                unicodeValues[i] /= Convert.ToUInt64(HowMuchToDivideBy);//divide it by random division earlier       
            }

            UInt64 swaps = 10000000000000000;
            int y = 0; //calling it y not i. going to handle indexes
            UInt64 holder = 0; //temp holder to swap values
            int SumOfDigits = 0; //sum of digits of unicode values
            string digits; //string version of unicode values
            for (int count = 0; count < 1000000; count++)
            {
                holder = unicodeValues[y];
                unicodeValues[y] = unicodeValues[swaps % Convert.ToUInt64(unicodeValues.Length)];
                unicodeValues[swaps % Convert.ToUInt64(unicodeValues.Length)] = holder;

                y++;
                count++;
                digits = ConvertIntArrayToString(unicodeValues);
                for (int z = 0; z < swaps.ToString().Length; z++) //adding up all digits in unicode values
                {
                    char digit = digits[z];
                    SumOfDigits += int.Parse(digit.ToString());
                }
                swaps -= Convert.ToUInt64(SumOfDigits);
                SumOfDigits = 0;


                if (y > unicodeValues.Length - 1)
                {
                    y = 0;
                }
            }



            UInt64 TotalSum = 0;
            string Characters = ""; //all the characters which have been generated
            for(int g = 0; g < unicodeValues.Length; g++)
            {
                Characters += Convert.ToChar(unicodeValues[g] % 300 + 20);
                TotalSum += unicodeValues[g]; //total sum of the unicode values
            }
            UInt64 TotalSumOG = TotalSum;
            UInt64 SomethingRandom = (swaps % TotalSum) + 1;       
            
            for(int i = 0; i < unicodeValues.Length * 2; i++) //Making new characters from random data
            {
                TotalSum -= SomethingRandom;
                TotalSum -= Convert.ToUInt64(HowMuchToDivideBy);
                if (TotalSum < 0) //Just in case total sum somehow falls below zero;
                {
                    TotalSum *= Convert.ToUInt64(-1);
                    if(TotalSum == 0)
                    {
                        TotalSum = TotalSumOG;
                    }
                }
                Characters += Convert.ToChar(TotalSum % 300 + 20);
            }

            //picking out characters for output
            string Output = "";
            UInt64 ValuesToTake = SomethingRandom % Convert.ToUInt64(OutputLength);
            UInt64 Count = SomethingRandom;
            while(Output.Length < OutputLength)
            {
               if(ValuesToTake > Convert.ToUInt64(Output.Length))
                {
                    ValuesToTake = 0;
                }
                Output += Characters[Convert.ToInt32(ValuesToTake)];
                ValuesToTake++;
            }        
        }
        private string ConvertIntArrayToString(UInt64[] array)
        {
            string output = "";

            for (int i = 0; i < array.Length; i++)
            {
                output += array[i].ToString();
            }
            return output;
        }

        private Boolean CheckIfIndexExists(string index)
        {
            if (Indexes.Contains(index))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class Exceptions : Exception
    {
        public Exceptions(string message) : base(message)
        {

        }
    }
}
