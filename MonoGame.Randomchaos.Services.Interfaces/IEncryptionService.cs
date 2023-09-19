
namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for encryption service. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IEncryptionService
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Encrypts. </summary>
        ///
        /// <param name="data"> The data. </param>
        ///
        /// <returns>   A byte[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        byte[] Encrypt(string data);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Decrypts the given data. </summary>
        ///
        /// <param name="data"> The data. </param>
        ///
        /// <returns>   A string. </returns>
        ///-------------------------------------------------------------------------------------------------

        string Decrypt(byte[] data);
    }
}
