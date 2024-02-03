#nullable enable

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace pwm.Core.Crypto
{
    public class BasicCryptographyLogic : ICryptographyLogic
    {
        private readonly HashAlgorithm _hashProvider = new SHA256CryptoServiceProvider();

        public string ToHash(string data)
        {
            return string.Join(string.Empty, _hashProvider.ComputeHash(Encoding.UTF8.GetBytes(data)).Select(x => x.ToString("x2")));
        }

        public string EnCryptography(string textData, string password)
        {
            using var algorithm = CreateAlgorithm();
            var iv = algorithm.IV;
            var key = GenerateKey(password);

            using var encryptor = algorithm.CreateEncryptor(key, iv);
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt, CryptographyLogicConfig.TEXT_ENCODING))
            {
                swEncrypt.Write(textData);
            }

            return Convert.ToBase64String(iv) + CryptographyLogicConfig.IV_SEPARATOR + Convert.ToBase64String(msEncrypt.ToArray());
        }

        public string DeCryptography(string base64Data, string password)
        {
            var separatorIndex = base64Data.IndexOf(CryptographyLogicConfig.IV_SEPARATOR);
            if (separatorIndex == -1)
            {
                throw new InvalidDataException(base64Data);
            }

            var iv = Convert.FromBase64String(base64Data.Substring(0, separatorIndex));
            var key = GenerateKey(password);
            var encryptedData = Convert.FromBase64String(base64Data.Substring(separatorIndex + 1));

            using var algorithm = CreateAlgorithm();
            using var descriptor = algorithm.CreateDecryptor(key, iv);
            using var msDecrypt = new MemoryStream(encryptedData);
            using var csDecrypt = new CryptoStream(msDecrypt, descriptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt, CryptographyLogicConfig.TEXT_ENCODING);

            return srDecrypt.ReadToEnd();
        }

        private SymmetricAlgorithm CreateAlgorithm()
        {
            var algorithm = Aes.Create();
            algorithm.Mode = CipherMode.CBC;
            algorithm.Padding = PaddingMode.PKCS7;
            algorithm.KeySize = CryptographyLogicConfig.KEY_SIZE;
            algorithm.BlockSize = CryptographyLogicConfig.BLOCK_SIZE;

            return algorithm;
        }

        public byte[] GenerateKey(string baseValue)
        {
            var deriveBytes = new Rfc2898DeriveBytes(baseValue, CryptographyLogicConfig.TEXT_ENCODING.GetBytes(CryptographyLogicConfig.SOLT))
            {
                IterationCount = 1000
            };
            return deriveBytes.GetBytes(CryptographyLogicConfig.KEY_SIZE / 8);
        }
    }
}
