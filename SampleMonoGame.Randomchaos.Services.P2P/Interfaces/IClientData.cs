
using System;
using System.Net;
using System.Net.Sockets;

namespace SampleMonoGame.Randomchaos.Services.P2P.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for client data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IClientData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the Date/Time of the last activity. </summary>
        ///
        /// <value> The last activity. </value>
        ///-------------------------------------------------------------------------------------------------

        DateTime LastActivity { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets information describing the packet. </summary>
        ///
        /// <value> Information describing the packet. </value>
        ///-------------------------------------------------------------------------------------------------

        IClientPacketData PacketData { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the UDP end point. </summary>
        ///
        /// <value> The UDP end point. </value>
        ///-------------------------------------------------------------------------------------------------

        IPEndPoint UdpEndPoint { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the client. </summary>
        ///
        /// <value> The client. </value>
        ///-------------------------------------------------------------------------------------------------

        TcpClient Client { get; set; }
        /// <summary>   Closes this object. </summary>
        void Close();
    }
}
