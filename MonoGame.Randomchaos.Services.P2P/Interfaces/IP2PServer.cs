
using MonoGame.Randomchaos.Services.P2P.Delegates;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.P2P.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for peer 2 peer server. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IP2PServer
    {
        /// <summary>   Event queue for all listeners interested in OnServerStart events. </summary>
        event ServerStartEvent OnServerStart;
        /// <summary>   Event queue for all listeners interested in OnServerStop events. </summary>
        event ServerStopEvent OnServerStop;
        /// <summary>   Event queue for all listeners interested in OnConnectionAttempt events. </summary>
        event ConnectionAttemptEvent OnConnectionAttempt;
        /// <summary>   Event queue for all listeners interested in OnNewClientAdded events. </summary>
        event ConnectionAttemptEvent OnNewClientAdded;
        /// <summary>   Event queue for all listeners interested in OnConnectionDropped events. </summary>
        event ConnectionDroppedEvent OnConnectionDropped;
        /// <summary>   Event queue for all listeners interested in OnTcpDataReceived events. </summary>
        event DataReceivedEvent OnTcpDataReceived;
        /// <summary>   Event queue for all listeners interested in OnUdpDataReceived events. </summary>
        event DataReceivedEvent OnUdpDataReceived;
        /// <summary>   Event queue for all listeners interested in OnError events. </summary>
        event ErrorEvent OnError;
        /// <summary>   Event queue for all listeners interested in OnClientCommsError events. </summary>
        event ClientCommsError OnClientCommsError;
        /// <summary>   Event queue for all listeners interested in OnLog events. </summary>
        event LogEvent OnLog;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets information describing the player. </summary>
        ///
        /// <value> Information describing the player. </value>
        ///-------------------------------------------------------------------------------------------------

        IPlayerData PlayerData { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the accepting connections. </summary>
        ///
        /// <value> True if accepting connections, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool AcceptingConnections { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the local IPv4 address. </summary>
        ///
        /// <value> The local IPv4 address. </value>
        ///-------------------------------------------------------------------------------------------------

        string LocalIPv4Address { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the external IP 4v address. </summary>
        ///
        /// <value> The external IP 4v address. </value>
        ///-------------------------------------------------------------------------------------------------

        string ExternalIP4vAddress { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the machine. </summary>
        ///
        /// <value> The name of the machine. </value>
        ///-------------------------------------------------------------------------------------------------

        string MachineName { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the port. </summary>
        ///
        /// <value> The port. </value>
        ///-------------------------------------------------------------------------------------------------

        int Port { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object is running. </summary>
        ///
        /// <value> True if this object is running, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool IsRunning { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the clients. </summary>
        ///
        /// <value> The clients. </value>
        ///-------------------------------------------------------------------------------------------------

        List<IClientData> Clients { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the clear inactive connection timeout. </summary>
        ///
        /// <value> The clear inactive connection timeout. </value>
        ///-------------------------------------------------------------------------------------------------

        TimeSpan ClearInactiveConnectionTimeout { get; set; }

        /// <summary>   Starts this object. </summary>
        void Start();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to. </summary>
        ///
        /// <param name="clients">  The clients. </param>
        /// <param name="data">     The data. </param>
        ///
        /// <returns>   An Exception. </returns>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<IClientData, Exception> SendDataTo(List<IClientData> clients, ICommsPacket data);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to all. </summary>
        ///
        /// <param name="data"> The data. </param>
        ///
        /// <returns>   A Dictionary&lt;IClientData,Exception&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<IClientData, Exception> SendDataToAll(ICommsPacket data);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Denies client. </summary>
        ///
        /// <param name="client">   The client. </param>
        ///-------------------------------------------------------------------------------------------------

        void DenyClient(IClientData client);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Disconnects the client described by client. </summary>
        ///
        /// <param name="client">   The client. </param>
        ///-------------------------------------------------------------------------------------------------

        void DisconnectClient(IClientData client);
        /// <summary>   Stops this object. </summary>
        void Stop();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to. </summary>
        ///
        /// <param name="client">   The client. </param>
        /// <param name="data">     The data. </param>
        ///
        /// <returns>   An Exception. </returns>
        ///-------------------------------------------------------------------------------------------------

        Exception SendDataTo(IClientData client, ICommsPacket data);
    }
}
