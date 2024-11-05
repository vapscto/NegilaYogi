using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace paytm.util
{
    class SecurityUtils
    {

        //private const String VALUE_SEPARATOR_TOKEN = "|";

        /**
         * This method is used to create CheckSum string corresponding to specified parameters.
         * 
         * @param parameters the parameter dictionary corresponding to which CheckSum string to be generated.
         * @return the CheckSum string corresponding to specified parameters.
         */
        public static String createCheckSumString(Dictionary<String, String> parameters)
        {
            if (parameters == null) return "";

            MessageConsole.WriteLine(); MessageConsole.WriteLine("Input Dict::::"); printDictionary(parameters);
            SortedDictionary<String, String> sorted = new SortedDictionary<string, string>(parameters,StringComparer.Ordinal);
            MessageConsole.WriteLine(); MessageConsole.WriteLine("Sorted Dict::::"); printSortedDictionary(sorted);

            StringBuilder checkSumString = new StringBuilder("");
            foreach (KeyValuePair<string, String> pair in sorted)
            {
                //Console.WriteLine("{0}, {1}", pair.Key, pair.Value);

                String value = pair.Value;
                if (value == null || value.Trim().Equals("NULL"))
                {
                    value = "";
                }
                checkSumString.Append(value).Append(Constants.VALUE_SEPARATOR_TOKEN);
            }

            //Console.WriteLine("ChechSumString:: " + checkSumString.ToString());
            return checkSumString.ToString();
        }

        /**
         * This method is used to get Hashed String corresponding to specified input String.
         * It is internally using SHA256Managed.
         * 
         * @param inputValue the input String corresponding to which Hashed String to be generated.
         * @return the generated Hash String.
         */
        public static String getHashedString(String inputValue)
        {
            // First get Byte[] corresponding to input value
            //byte[] inputBytes = System.Text.Encoding.Unicode.GetBytes(inputValue);
            byte[] inputBytes = StringUtils.getBytesFromString(inputValue);

            SHA256Managed sha = new SHA256Managed();

            byte[] checksum = sha.ComputeHash(inputBytes);

            // BitConverted.ToString gives:
            // A string of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in value; for example, "7F-2C-4A-00"
            String hashString = BitConverter.ToString(checksum);
            hashString = hashString.Replace("-", "").ToLower();
            
            return hashString; // Convert.ToBase64String(checksum);
        }

        // Helper Methods
        private static void printDictionary(Dictionary<String, String> dict)
        {
            if (dict == null) return;

            foreach (KeyValuePair<string, String> pair in dict)
            {
                MessageConsole.WriteLine("{0}, {1}", pair.Key, pair.Value);
            }
        }
        private static void printSortedDictionary(SortedDictionary<String, String> dict)
        {
            if (dict == null) return;

            foreach (KeyValuePair<string, String> pair in dict)
            {
                MessageConsole.WriteLine("{0}, {1}", pair.Key, pair.Value);
            }
        }

    }
}
