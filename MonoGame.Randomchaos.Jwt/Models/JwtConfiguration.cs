
namespace MonoGame.Randomchaos.Jwt.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A jwt configuration. </summary>
    ///
    /// <remarks>   Charles Humphrey, 14/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class JwtConfiguration
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the key. </summary>
        ///
        /// <value> The key. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Key { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the issuer. </summary>
        ///
        /// <value> The issuer. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Issuer { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the audience. </summary>
        ///
        /// <value> The audience. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Audience { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 10/10/2023. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="issuer">   The issuer. </param>
        /// <param name="audience"> The audience. </param>
        ///-------------------------------------------------------------------------------------------------

        public JwtConfiguration(string key, string issuer, string audience)
        {
            Key = key;
            Issuer = issuer;
            Audience = audience;
        }
    }
}
