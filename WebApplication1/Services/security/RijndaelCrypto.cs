using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using paytm.util;

namespace paytm.security
{
    class RijndaelCrypto
    {

        private static Boolean USE_CUSTOM_INIT_VECTOR = true;

        /**
         * This method is used to Encrypt specified text based on the given master key.
         *
         * @param clearText the text that needs to be encrypted.
         * @param masterKey the master key that is to be used for encryption.
         * 
         * @return the encrypted text.
         */
        public static String Encrypt(String clearText, String masterKey)
        {
            // First we need to turn the input string into a byte array. 
            byte[] clearBytes = StringUtils.getBytesFromString(clearText);
            byte[] keyBytes = StringUtils.getBytesFromString(masterKey);

            // Then, we need to turn the password into Key and IV 
            // We are using salt to make it harder to guess our key using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(masterKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            byte[] initVectorBytes;
            if (USE_CUSTOM_INIT_VECTOR){
                initVectorBytes = Constants.CRYPTO_INIT_VECTOR;
            }else{
                initVectorBytes = pdb.GetBytes(16);
            }

            byte[] encryptedData = Encrypt(clearBytes, keyBytes, initVectorBytes);
            return Convert.ToBase64String(encryptedData);
        }

        // Encrypt a byte array into a byte array using a key and an IV 
        private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            // Create a MemoryStream to accept the encrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because
            // the algorithm is operating in its default 
            // mode called CBC (Cipher Block Chaining).
            // The IV is XORed with the first block (8 byte) 
            // of the data before it is encrypted, and then each
            // encrypted block is XORed with the 
            // following block of plaintext.
            // This is done to make encryption more secure. 

            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 
            alg.Key = Key;
            alg.IV = IV;
            

            // Added by KK
            alg.Mode = CipherMode.CBC;
            alg.Padding = PaddingMode.PKCS7;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream and the output will be written
            // in the MemoryStream we have provided. 
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption 
            cs.Write(clearData, 0, clearData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our encryption and
            // there is no more data coming in, 
            // and it is now a good time to apply the padding and
            // finalize the encryption process. 
            cs.Close();

            // Now get the encrypted data from the MemoryStream.
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }

        /**
         * This method is used to decrypt the specified text based on the given master key.
         *
         * @param cipherText the text that needs to be decrypted.
         * @param masterKey the master key that is to be used for decryption.
         * 
         * @return the decrypted text.
         */
        public static String Decrypt(String cipherText, String masterKey)
        {
            // First we need to turn the input string into a byte array. 
            // We presume that Base64 encoding was used 
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            //string hex = BitConverter.ToString(cipherBytes);
            byte[] keyBytes = StringUtils.getBytesFromString(masterKey);

            // Then, we need to turn the password into Key and IV 
            // We are using salt to make it harder to guess our key using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(masterKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            byte[] initVectorBytes;
            if (USE_CUSTOM_INIT_VECTOR){
                initVectorBytes = Constants.CRYPTO_INIT_VECTOR;
            }else{
                initVectorBytes = pdb.GetBytes(16);
            }

            byte[] decryptedData = Decrypt(cipherBytes, keyBytes, initVectorBytes);
            return StringUtils.getStringFromBytes(decryptedData);
        }

        // Decrypt a byte array into a byte array using a key and an IV 
        private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            // Create a MemoryStream that is going to accept the decrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because the algorithm
            // is operating in its default 
            // mode called CBC (Cipher Block Chaining). The IV is XORed with
            // the first block (8 byte) 
            // of the data after it is decrypted, and then each decrypted
            // block is XORed with the previous 
            // cipher block. This is done to make encryption more secure. 
            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 
            alg.Key = Key;
            alg.IV = IV;

            // Added by KK
            alg.Mode = CipherMode.CBC;
            alg.Padding = PaddingMode.PKCS7;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream 
            // and the output will be written in the MemoryStream
            // we have provided. 
            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the decryption 
            cs.Write(cipherData, 0, cipherData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our decryption
            // and there is no more data coming in, 
            // and it is now a good time to remove the padding
            // and finalize the decryption process. 
            cs.Close();

            // Now get the decrypted data from the MemoryStream. 
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }

    } // End of Class
}
