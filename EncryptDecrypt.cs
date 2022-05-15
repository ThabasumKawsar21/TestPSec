

using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Microsoft.VisualBasic;

/// <summary>
/// Encrypt Decrypt class 
/// </summary>
public class EncryptDecrypt
{
    /// <summary>
    /// encrypt function
    /// </summary>
    /// <param name="toencrypt">to encrypt value</param>
    /// <param name="key">key value</param>
    /// <param name="useHashing">use hashing value</param>
    /// <returns>encrypted value</returns>
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
        {
        }

        return null;
    }

    /// <summary>
    /// Decrypt function
    /// </summary>
    /// <param name="todecrypt">to decrypt value</param>
    /// <param name="key">key value</param>
    /// <param name="useHashing">use hashing value</param>
    /// <returns>decrypted value</returns>
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
        {
        }

        return null;
    } 
}
