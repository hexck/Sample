using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Server.Core.Manager
{
    public class EncryptionManager
    {
        public RSACryptoServiceProvider RSA { get; }
        public string PublicKey { get; }
        public string PrivateKey { get; }
       

        public EncryptionManager()
        {
            RSA = new RSACryptoServiceProvider();
            PublicKey = RSA.ToXmlString(false);
            PrivateKey = RSA.ToXmlString(true);
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            byte[] encryptedData;
            encryptedData = RSA.Encrypt(plaintext, true);
            return encryptedData;
        }

        public byte[] Decrypt(byte[] ciphertext) 
        {
            byte[] decryptedData;
            decryptedData = RSA.Decrypt(ciphertext, true);
            return decryptedData;
        }

        public byte[] Decrypt(string ciphertext)
        {
            byte[] decryptedData;
            decryptedData = RSA.Decrypt(Convert.FromBase64String(ciphertext), true);
            return decryptedData;
        }
    }
}
