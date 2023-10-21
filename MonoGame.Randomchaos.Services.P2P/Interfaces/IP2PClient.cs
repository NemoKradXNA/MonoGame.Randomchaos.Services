
using MonoGame.Randomchaos.Services.P2P.Delegates;
using System;
using System.Collections.Generic;
using System.Net;

namespace MonoGame.Randomchaos.Services.P2P.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for peer 2 peer client. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IP2PClient
    {
        /// <summary>   Event queue for all listeners interested in OnTcpDataReceived events. </summary>
        event DataReceivedEvent OnTcpDataReceived;
        /// <summary>   Event queue for all listeners interested in OnUdpDataReceived events. </summary>
        event DataReceivedEvent OnUdpDataReceived;
        /// <summary>   Event queue for all listeners interested in OnConnectionDropped events. </summary>
        event ConnectionDroppedEvent OnConnectionDropped;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Event queue for all listeners interested in OnClientConnectionDropped events.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------

        event ConnectionDroppedEvent OnClientConnectionDropped;
        /// <summary>   Event queue for all listeners interested in OnNewClientAdded events. </summary>
        event ConnectionAttemptEvent OnNewClientAdded;
        /// <summary>   Event queue for all listeners interested in OnLog events. </summary>
        event LogEvent OnLog;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets information describing the player. </summary>
        ///
        /// <value> Information describing the player. </value>
        ///-------------------------------------------------------------------------------------------------

        IPlayerData PlayerData { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the identifier. </summary>
        ///
        /// <value> The identifier. </value>
        ///-------------------------------------------------------------------------------------------------

        Guid? Id { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the IPv4 address. </summary>
        ///
        /// <value> The IPv4 address. </value>
        ///-------------------------------------------------------------------------------------------------

        string IPv4Address { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the clients. </summary>
        ///
        /// <value> The clients. </value>
        ///-------------------------------------------------------------------------------------------------

        List<IClientPacketData> Clients { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the port. </summary>
        ///
        /// <value> The port. </value>
        ///-------------------------------------------------------------------------------------------------

        int Port { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the server IPv4 address. </summary>
        ///
        /// <value> The server IPv4 address. </value>
        ///-------------------------------------------------------------------------------------------------

        string ServerIPv4Address { get; }

        /// <summary>   Connects this object. </summary>
        void Connect();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Disconnects the given informServer. </summary>
        ///
        /// <param name="informServer"> (Optional) The inform server to disconnect. </param>
        ///-------------------------------------------------------------------------------------------------

        void Disconnect(bool informServer = true);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to. </summary>
        ///
        /// <param name="data">     The data. </param>
        /// <param name="endpoint"> (Optional) The endpoint. </param>
        ///-------------------------------------------------------------------------------------------------

        void SendDataTo(ICommsPacket data, IPEndPoint endpoint = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Broadcasts to. </summary>
        ///
        /// <param name="data">     The data. </param>
        /// <param name="clients">  The clients. </param>
        ///-------------------------------------------------------------------------------------------------

        void BroadcastTo(ICommsPacket data, List<IClientPacketData> clients);
    }
}
