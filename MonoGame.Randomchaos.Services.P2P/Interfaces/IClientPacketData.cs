using MonoGame.Randomchaos.Services.P2P.Enums;
using System;
using System.Net;

namespace MonoGame.Randomchaos.Services.P2P.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for client packet data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IClientPacketData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the identifier. </summary>
        ///
        /// <value> The identifier. </value>
        ///-------------------------------------------------------------------------------------------------

        Guid Id { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the UDP address. </summary>
        ///
        /// <value> The UDP address. </value>
        ///-------------------------------------------------------------------------------------------------

        string UdpAddress { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the ud p port. </summary>
        ///
        /// <value> The ud p port. </value>
        ///-------------------------------------------------------------------------------------------------

        int UdPPort { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the type of the connection. </summary>
        ///
        /// <value> The type of the connection. </value>
        ///-------------------------------------------------------------------------------------------------

        ConnectionTypesEnum ConnectionType { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets information describing the player game. </summary>
        ///
        /// <value> Information describing the player game. </value>
        ///-------------------------------------------------------------------------------------------------

        object? PlayerGameData { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets IP end point. </summary>
        ///
        /// <returns>   The IP end point. </returns>
        ///-------------------------------------------------------------------------------------------------

        IPEndPoint GetIPEndPoint();
    }
}
