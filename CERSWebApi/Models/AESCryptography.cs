using System;
using System.Security.Cryptography;
using System.Text;

namespace CERSWebApi.Models
{
    public class AESCryptography
    {
       public static string key = "CERS&NicHP@23@ece";
        public static String EncryptAES(String plainText)
        {
            try
            {
#if DEBUG
                {
                     return plainText;                   
                }
#else
                {
               
                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(Encrypt(plainBytes, getRijndaelManaged(key)));
                }
#endif

            }
            catch (Exception)
            {
#if DEBUG
                {
return plainText;
                }
                #else
                {
return  "";
                }
#endif
            }
           
        }

        public static String DecryptAES(String encryptedText)
        {
            try
            {
               
                var encryptedBytes = Convert.FromBase64String(encryptedText);
                return Encoding.UTF8.GetString(Decrypt(encryptedBytes, getRijndaelManaged(key)));
            }
            catch (Exception)
            {
#if DEBUG
                {
                    return encryptedText;
                }
#else
                {
return  "";
                }
#endif
            }

        }

        private static RijndaelManaged getRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }

        private static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor()
                .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        private static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor()
                .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }
    }
}