using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    public class AutoGenerateNumbers
    {
        static string numbers = "1234567890";
        static string characters = numbers;
        static string AppNo = string.Empty;
        public static string autoGenNum(int noOfDigit)
        {
            for (int i = 0; i < noOfDigit; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (AppNo.IndexOf(character) != -1);
                AppNo += character;
            }
            return AppNo;
        }
        //public static string FiveDigitNo(int length)
        //{
        //    for (int i = 0; i < length; i++)
        //    {
        //        string character = string.Empty;
        //        do
        //        {
        //            int index = new Random().Next(0, characters.Length);
        //            character = characters.ToCharArray()[index].ToString();
        //        } while (AppNo.IndexOf(character) != -1);
        //        AppNo += character;
        //    }
        //    return AppNo;
        //}
        //public static string sixDigitNo(int length)
        //{
        //    for (int i = 0; i < length; i++)
        //    {
        //        string character = string.Empty;
        //        do
        //        {
        //            int index = new Random().Next(0, characters.Length);
        //            character = characters.ToCharArray()[index].ToString();
        //        } while (AppNo.IndexOf(character) != -1);
        //        AppNo += character;
        //    }
        //    return AppNo;
        //}
        //public static string sevenDigitNo(int length)
        //{
        //    for (int i = 0; i < length; i++)
        //    {
        //        string character = string.Empty;
        //        do
        //        {
        //            int index = new Random().Next(0, characters.Length);
        //            character = characters.ToCharArray()[index].ToString();
        //        } while (AppNo.IndexOf(character) != -1);
        //        AppNo += character;
        //    }
        //    return AppNo;
        //}
        //public static string eightDigitNo(int length)
        //{
        //    for (int i = 0; i < length; i++)
        //    {
        //        string character = string.Empty;
        //        do
        //        {
        //            int index = new Random().Next(0, characters.Length);
        //            character = characters.ToCharArray()[index].ToString();
        //        } while (AppNo.IndexOf(character) != -1);
        //        AppNo += character;
        //    }
        //    return AppNo;
        //}
    }
}
