using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.P2P.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for player data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 18/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IPlayerData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the session. </summary>
        ///
        /// <value> The session. </value>
        ///-------------------------------------------------------------------------------------------------

        ISessionData Session { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the properties. </summary>
        ///
        /// <value> The properties. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<string, object?> Properties { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets a property. </summary>
        ///
        /// <param name="name">     The name. </param>
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        void SetProperty(string name, object value);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the property described by name. </summary>
        ///
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        void RemoveProperty(string name);
    }
}
