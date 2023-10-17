
using SampleMonoGame.Randomchaos.Services.P2P.Enums;
using SampleMonoGame.Randomchaos.Services.P2P.Interfaces;
using System;
using System.Net;

namespace SampleMonoGame.Randomchaos.Services.P2P.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A client packet data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class ClientPacketData : IClientPacketData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the identifier. </summary>
        ///
        /// <value> The identifier. </value>
        ///-------------------------------------------------------------------------------------------------

        public Guid Id { get; set; } = Guid.NewGuid();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the UDP address. </summary>
        ///
        /// <value> The UDP address. </value>
        ///-------------------------------------------------------------------------------------------------

        public string UdpAddress { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the ud p port. </summary>
        ///
        /// <value> The ud p port. </value>
        ///-------------------------------------------------------------------------------------------------

        public int UdPPort { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the type of the connection. </summary>
        ///
        /// <value> The type of the connection. </value>
        ///-------------------------------------------------------------------------------------------------

        public ConnectionTypesEnum ConnectionType { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets information describing the player game. </summary>
        ///
        /// <value> Information describing the player game. </value>
        ///-------------------------------------------------------------------------------------------------

        public object? PlayerGameData { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(UdpAddress))
                return $"{UdpAddress}:{UdPPort}";
            else
                return $"UDP Endpoint Unknown";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets IP end point. </summary>
        ///
        /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
        ///
        /// <returns>   The IP end point. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual IPEndPoint GetIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(UdpAddress), UdPPort);
        }
    }
}
