using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class generateOTP
    {

        static string numbers = "1234567890";
        string characters = numbers;
        string otp = "";
        string otpf = "";
        string otpm = "";
        public string getOTP()
        {
            for (int i = 0; i < 6; i++)
            {
                string character = "";
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
        public string getFourDigitOTP()
        {
            for (int i = 0; i < 4; i++)
            {
                string character = "";
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }

        public string getFourDigitOTPFather()
        {
            for (int i = 0; i < 4; i++)
            {
                string character = "";
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otpf.IndexOf(character) != -1);
                otpf += character;
            }
            return otpf;
        }

        public string getFourDigitOTPMother()
        {
            for (int i = 0; i < 4; i++)
            {
                string character = "";
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otpm.IndexOf(character) != -1);
                otpm += character;
            }
            return otpm;
        }


        public string GeneratePassword()
        {
            string PasswordLength = "4";

            string NewPassword = "";
            string allowedChars = "";

            allowedChars = "1,2,3,4,5,6,7,8,9,0";

            char[] sep = { ',' };

            string[] arr = allowedChars.Split(sep);


            string IDString = "";

            string temp = "";

            Random rand = new Random();

            for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
            {

                temp = arr[rand.Next(0, arr.Length)];

                IDString += temp;

                NewPassword = IDString;
            }
             otpf = NewPassword;
            return otpf;
        }

        public string GeneratePassword_Mother()
        {
            string PasswordLength_mother = "4";

            string NewPassword_mother = "";
            string allowedChars_mother = "";

            allowedChars_mother = "1,2,3,4,5,6,7,8,9,0";

            char[] sep = { ',' };

            string[] arr_mother = allowedChars_mother.Split(sep);


            string IDString_mother = "";

            string temp_mother = "";

            Random rand_mother = new Random();

            for (int i = 0; i < Convert.ToInt32(PasswordLength_mother); i++)
            {

                temp_mother = arr_mother[rand_mother.Next(0, arr_mother.Length)];

                IDString_mother += temp_mother;

                NewPassword_mother = IDString_mother;
            }
             otpm = NewPassword_mother;
            return otpm;
        }
    }
}
