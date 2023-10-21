using MonoGame.Randomchaos.Services.P2P.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.P2P.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A player data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 18/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class PlayerData : IPlayerData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the session. </summary>
        ///
        /// <value> The session. </value>
        ///-------------------------------------------------------------------------------------------------

        public ISessionData Session { get; set; } = new SessionData();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the properties. </summary>
        ///
        /// <value> The properties. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<string, object?> Properties { get; set; } = new Dictionary<string, object?>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets a property. </summary>
        ///
        /// <remarks>   Charles Humphrey, 18/10/2023. </remarks>
        ///
        /// <param name="name">     The name. </param>
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void SetProperty(string name, object value)
        {
            if (!Properties.ContainsKey(name))
            {
                Properties.Add(name, value);
            }
            else
            {
                Properties[name] = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the property described by name. </summary>
        ///
        /// <remarks>   Charles Humphrey, 18/10/2023. </remarks>
        ///
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public void RemoveProperty(string name)
        {
            if (Properties.ContainsKey(name))
            {
                Properties.Remove(name);
            }
        }
    }
}
