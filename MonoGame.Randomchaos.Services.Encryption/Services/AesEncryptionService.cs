
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using System.IO;
using System.Security.Cryptography;

namespace MonoGame.Randomchaos.Services.Encryption
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A service for accessing aes encryptions information. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class AesEncryptionService : ServiceBase<AesEncryptionService>, IEncryptionService
    {
        /// <summary>   (Immutable) the key. </summary>
        protected readonly byte[] _key;
        /// <summary>   (Immutable) the iv. </summary>
        protected readonly byte[] _iv;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the game. </summary>
        ///
        /// <value> The game. </value>
        ///-------------------------------------------------------------------------------------------------

        protected Game Game { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        /// <param name="key">  The key. </param>
        /// <param name="iv">   The iv. </param>
        ///-------------------------------------------------------------------------------------------------

        public AesEncryptionService(Game game, byte[] key, byte[] iv) : base(game)
        {
            Game = game;

            _key = key;
            _iv = iv;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Decrypts the given data. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="data"> The data. </param>
        ///
        /// <returns>   A string. </returns>
        ///-------------------------------------------------------------------------------------------------

        public string Decrypt(byte[] data)
        {
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(data))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Encrypts. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="data"> The data. </param>
        ///
        /// <returns>   A byte[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        public byte[] Encrypt(string data)
        {
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(data);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
    }
}