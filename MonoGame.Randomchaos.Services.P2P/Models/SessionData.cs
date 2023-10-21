using MonoGame.Randomchaos.Services.P2P.Interfaces;

namespace MonoGame.Randomchaos.Services.P2P.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A session data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 18/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SessionData : ISessionData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the token. </summary>
        ///
        /// <value> The token. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Token { get; set; }
    }
}
