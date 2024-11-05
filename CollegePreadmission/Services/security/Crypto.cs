using System;
using System.Collections.Generic;
using System.Text;

namespace paytm.security
{
    /**
     * This PaytmCrypto Class provides Encryption and Decryption API.
     * 
     */
    class Crypto    // Remove public modifier later on
    {

        /**
         * This Encrypt API is used to encrypt specified text based on the given master key.
         *
         * @param clearText the text that needs to be encrypted.
         * @param masterKey the master key that is to be used for encryption.
         * 
         * @return the encrypted text.
         */
        public static String Encrypt(String clearText, String masterKey)
        {
            return RijndaelCrypto.Encrypt(clearText, masterKey);
        }


        /**
         * This Decrypt API is used to decrypt the specified text based on the given master key.
         *
         * @param cipherText the text that needs to be decrypted.
         * @param masterKey the master key that is to be used for decryption.
         * 
         * @return the decrypted text.
         */
        public static String Decrypt(String cipherText, String masterKey)
        {
            return RijndaelCrypto.Decrypt(cipherText, masterKey);
        }

    } // End of Class
}
