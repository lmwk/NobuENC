using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using System.Xml;

namespace NobuENC
{
    class Program
    {
        static void Main(string[] args)
        {
            bool Cont = true;

            while (Cont == true)
            {
                Console.WriteLine("Enter text you'd like to Encrypt or Decrypt!");

                string text = Console.ReadLine().ToString();

                Console.WriteLine("Enter a hash you'd like to use!");

                string hash = Console.ReadLine().ToString();

                Console.WriteLine("Would you like to encrypt or decrypt? Answer: (E or D)");

                var response = Console.ReadLine();

                switch (response)
                {
                    case "E":
                        Encrypt(text, hash);
                        break;

                    case "D":
                        Decrypt(text, hash);
                        break;
                    default:
                        Console.WriteLine("Error: Didnt write appropriate response, Aborting En/Decryption");
                        break;
                }

                Console.WriteLine("Do you wish to Keep encrypting messages? Answer: Yes or No");

                var contine = Console.ReadLine();

                if(contine == "Yes" || contine == "yes")
                {
                    Cont = true;
                }
                else if(contine == "No" || contine == "no")
                {
                    Cont = false;
                }
                else
                {
                    Cont = false;
                }
            }

            
        }

        private static void Encrypt(string text, string hash)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(text);

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripdes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripdes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);

                    string encrypt = Convert.ToBase64String(results, 0, results.Length);

                    Console.WriteLine("{0} is the encrypted text", encrypt);
                }
            }
        }

        private static void Decrypt(string text, string hash)
        {
            byte[] data = Convert.FromBase64String(text);

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripdes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripdes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);

                    string encrypt = UTF8Encoding.UTF8.GetString(results);

                    Console.WriteLine("{0} is the decrypted text", encrypt);
                }
            }
        }
    }
}
