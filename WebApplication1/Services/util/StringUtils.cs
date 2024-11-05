using System;
using System.Collections.Generic;
using System.Text;

namespace paytm.util
{
    class StringUtils
    {

        private const String CHARACTER_SET = "@#!abcdefghijklmonpqrstuvwxyz#@01234567890123456789#@ABCDEFGHIJKLMNOPQRSTUVWXYZ#@";
        private static Random random = new Random((int)DateTime.Now.Ticks);
        //Dim rnd As New Random
        /**
         * This method is used to generate random string of specified length.
         * 
         * @param length the length of random string that is to be generated.
         * @return the randomly generated String of specified length.
         */
        public static String generateRandomString(int length)
        {
            if (length <= 0)
                return "";

            StringBuilder buffer = new StringBuilder("");
            for (int i = 0; i < length; i++)
            {
                int pos = random.Next(CHARACTER_SET.Length);
                buffer.Append(CHARACTER_SET.Substring(pos, 1));
            }
            //string buffer = "1234";
            return buffer.ToString();
        }


        public static String getStringFromBytes(byte[] byteArr)
        {
            if (Constants.USE_UNICODE_ENCODING){
                return System.Text.Encoding.Unicode.GetString(byteArr);
            }else{
                return System.Text.Encoding.ASCII.GetString(byteArr);
            }
        }

        public static byte[] getBytesFromString(String strInput)
        {
            if (Constants.USE_UNICODE_ENCODING){
                return System.Text.Encoding.Unicode.GetBytes(strInput);
            }else{
                return System.Text.Encoding.ASCII.GetBytes(strInput);
            }
        }

        /*
        public static byte[] getUnicodeBytesFromString(String strInput)
        {
            //return System.Text.Encoding.ASCII.GetBytes(strInput);
            return System.Text.Encoding.Unicode.GetBytes(strInput);
        }
         */ 
    }
}
