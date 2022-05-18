

namespace VMSDev
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;
    using Microsoft.VisualBasic;

    /// <summary>
    /// Encryption and Decryption 
    /// </summary>
    public class EncryptDecrypt
    {
        /// <summary>
        /// The Encrypt method
        /// </summary>
        /// <param name="toencrypt">The toEncrypt parameter</param>
        /// <param name="key">The key parameter</param>
        /// <param name="useHashing">The useHashing parameter</param>
        /// <returns>The string type object</returns>        
        public string Encrypt(string toencrypt, string key, bool useHashing)
        {
            try
            {
                byte[] keyArray = null;
                byte[] toencryptArray = UTF8Encoding.UTF8.GetBytes(toencrypt);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                }
                else
                {
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                }

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform ctransform = tdes.CreateEncryptor();
                byte[] resultArray = ctransform.TransformFinalBlock(toencryptArray, 0, toencryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception)
            {
                throw;
            }
            ////return null;
        }

        /// <summary>
        /// The Decrypt method
        /// </summary>
        /// <param name="todecrypt">The toDecrypt parameter</param>
        /// <param name="key">The key parameter</param>
        /// <param name="useHashing">The useHashing parameter</param>
        /// <returns>The string type object</returns>        
        public string Decrypt(string todecrypt, string key, bool useHashing)
        {
            try
            {
                byte[] keyArray = null;
                byte[] toencryptArray = Convert.FromBase64String(todecrypt);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                }
                else
                {
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                }

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform ctransform = tdes.CreateDecryptor();
                byte[] resultArray = ctransform.TransformFinalBlock(toencryptArray, 0, toencryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception)
            {
                throw;
            }

#pragma warning disable CS0162 // Unreachable code detected
            return null;
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}
