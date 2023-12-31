﻿using System.Text;
using System.Security.Cryptography;


public class Security
{
    /// <summary>
    /// Ma hoa SHA 256, voi UTF8Eoncoding
    /// </summary>
    /// <param name="phrase"></param>
    /// <returns></returns>
    public static string SHA256encrypt(string phrase)
    {
        UTF8Encoding encoder = new UTF8Encoding();
        SHA256Managed sha256hasher = new SHA256Managed();
        byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(phrase));
        return byteArrayToString(hashedDataBytes);
    }

    public static string byteArrayToString(byte[] inputArray)
    {
        StringBuilder output = new StringBuilder("");
        for (int i = 0; i < inputArray.Length; i++)
        {
            output.Append(inputArray[i].ToString("X2"));
        }
        return output.ToString();
    }


}