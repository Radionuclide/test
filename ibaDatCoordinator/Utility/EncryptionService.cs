using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace iba.Utility
{

    internal class EncryptionService : iba.Plugins.IEncryptionService
    {
        public EncryptionService(byte[] xorKey, byte[] aesKey)
        {
            this.xorKey = xorKey;
            this.aesKey = aesKey;
        }

        byte[] xorKey;
        byte[] aesKey;

        const int marker = 0x34AB12CD;
        static readonly byte[] fixedIV = new byte[] {
            0xC0, 0xDD, 0xAC, 0x72, 0xE1, 0xE9, 0xF3, 0x6a,
            0x6A, 0xD9, 0xD0, 0xD9, 0x13, 0x4B, 0xC0, 0x9C
        };

        public static System.Threading.ThreadLocal<bool> UseFixedIV = new System.Threading.ThreadLocal<bool>();

        public string Encrypt(string msg)
        {
            if (String.IsNullOrEmpty(msg))
                return msg;

            try
            {
                System.Diagnostics.Debug.Assert(aesKey != null);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = aesKey; // Set the secret key for encryption
                    if (UseFixedIV.Value)
                        aes.IV = fixedIV;
                    else
                        aes.GenerateIV(); // Initialize the Init vector of AES with a random number 

                    ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);
                    using (var memoryStream = new MemoryStream())
                    {
                        memoryStream.Write(aes.IV, 0, aes.IV.Length); // prepend the encrypted data with the IV for later decryption. 
                                                                      // The IV itself doesn't need to be secret.

                        //Encode text into UTF8
                        byte[] textBytes = Encoding.UTF8.GetBytes(msg);

                        // Write the encrypted message
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                        using (BinaryWriter writer = new BinaryWriter(cryptoStream))
                        {
                            writer.Write(marker);
                            writer.Write(textBytes.Length);
                            writer.Write(textBytes);
                            writer.Flush();
                            cryptoStream.FlushFinalBlock();

                            //Convert to base64
                            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                        }
                    }
                }
            }
            catch (Exception)
            {
                Debug.Assert(false);
                return msg;
            }
        }

        public string Decrypt(string msg)
        {
            if (String.IsNullOrEmpty(msg))
                return msg;

            try
            {
                if (msg.StartsWith("0x"))
                {
                    //Old/compatible way

                    msg = msg.Substring(2);
                    byte[] b = new byte[msg.Length / 2];
                    for (int i = 0; i < b.Length; i++)
                    {
                        b[i] = Byte.Parse(msg.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                        b[i] = (byte)(b[i] ^ xorKey[i % xorKey.Length]);
                    }
                    System.Text.Encoding enc = new System.Text.UTF8Encoding();
                    return enc.GetString(b);
                }
                else
                {
                    //New way
                    System.Diagnostics.Debug.Assert(aesKey != null);

                    byte[] input = Convert.FromBase64String(msg);

                    using (Aes aes = Aes.Create())
                    using (MemoryStream memoryStream = new MemoryStream(input))
                    {
                        System.Diagnostics.Debug.Assert(aesKey.Length == aes.KeySize / 8);

                        byte[] iv = new byte[aes.IV.Length];
                        memoryStream.Read(iv, 0, iv.Length); // Read the IV from the data

                        aes.Key = aesKey; // Set the secret key for decryption
                        aes.IV = iv; // Set the IV

                        ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
                        {
                            //Read all encrypted data at once because otherwise we run into trouble with incomplete encryption blocks
                            byte[] decryptedData = new byte[memoryStream.Length - memoryStream.Position];
                            cryptoStream.Read(decryptedData, 0, decryptedData.Length);

                            int textLength = BitConverter.ToInt32(decryptedData, 4);

                            //Check marker and length
                            if ((BitConverter.ToInt32(decryptedData, 0) != marker) ||
                                (textLength > decryptedData.Length - 8) ||
                                (textLength < 0))
                                return msg;

                            //Get text
                            return Encoding.UTF8.GetString(decryptedData, 8, textLength);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //If there was some error then return an empty string instead of the encrypted one
                return "";
            }
        }
    }

}
