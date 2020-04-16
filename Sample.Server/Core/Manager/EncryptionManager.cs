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
        public RSACryptoServiceProvider RSA { get; private set; }
        public string PublicKey { get; private set; }

        public string _privateKey { get; set; }
       

        // https://www.c-sharpcorner.com/forums/i-want-to-generate-rsa-key-pair-in-c-sharp-i-am-able-to-get-xml
        public EncryptionManager()
        {
            RSA = new RSACryptoServiceProvider();
            PublicKey = RSA.ToXmlString(false);
            _privateKey = RSA.ToXmlString(true);
        }

        // https://stackoverflow.com/questions/34613479/rsacryptoserviceprovider-encrypt-and-decrypt-using-own-public-and-private-key
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
