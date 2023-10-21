
using MonoGame.Randomchaos.Services.P2P.Delegates;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.P2P.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for peer 2 peer service. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IP2PService
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
        /// <summary>   Gets the clients. </summary>
        ///
        /// <value> The clients. </value>
        ///-------------------------------------------------------------------------------------------------

        List<IClientPacketData> Clients { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the session. </summary>
        ///
        /// <value> The session. </value>
        ///-------------------------------------------------------------------------------------------------

        ISessionData Session { get; set; }

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
        /// <summary>   Gets the identifier of the client. </summary>
        ///
        /// <value> The identifier of the client. </value>
        ///-------------------------------------------------------------------------------------------------

        Guid ClientId { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is server. </summary>
        ///
        /// <value> True if this object is server, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool IsServer { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the listening port. </summary>
        ///
        /// <value> The listening port. </value>
        ///-------------------------------------------------------------------------------------------------

        int ListeningPort { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the server IPv4 address. </summary>
        ///
        /// <value> The server IPv4 address. </value>
        ///-------------------------------------------------------------------------------------------------

        string ServerIPv4Address { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the local IPv4 address. </summary>
        ///
        /// <value> The local IPv4 address. </value>
        ///-------------------------------------------------------------------------------------------------

        string LocalIPv4Address { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the external IPv4 address. </summary>
        ///
        /// <value> The external IPv4 address. </value>
        ///-------------------------------------------------------------------------------------------------

        string ExternalIPv4Address { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the machine. </summary>
        ///
        /// <value> The name of the machine. </value>
        ///-------------------------------------------------------------------------------------------------

        string MachineName { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the number of players. </summary>
        ///
        /// <value> The number of players. </value>
        ///-------------------------------------------------------------------------------------------------

        int PlayerCount { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts a server. </summary>
        ///
        /// <param name="port">                 The port. </param>
        /// <param name="externalIPv4Address">  (Optional) The external IPv4 address. </param>
        /// <param name="sessionName">          (Optional) Name of the session. </param>
        /// <param name="sessionToken">         (Optional) The session token. </param>
        /// <param name="name">                 (Optional) The name. </param>
        ///-------------------------------------------------------------------------------------------------

        void StartServer(int port, string externalIPv4Address = null, string sessionName = null, string sessionToken = null, string name = "Server");
        /// <summary>   Stops a server. </summary>
        void StopServer();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Boot client. </summary>
        ///
        /// <param name="id">   The identifier. </param>
        /// <param name="msg">  (Optional) The message. </param>
        ///-------------------------------------------------------------------------------------------------

        void BootClient(Guid id, string msg = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Connects a client. </summary>
        ///
        /// <param name="serverIPv4Address">    The server IPv4 address. </param>
        /// <param name="port">                 The port. </param>
        /// <param name="clientIPv4Address">    The client IPv4 address. </param>
        /// <param name="clientPort">           The client port. </param>
        /// <param name="sessionName">          (Optional) Name of the session. </param>
        /// <param name="sessionToken">         (Optional) The session token. </param>
        /// <param name="name">                 (Optional) The name. </param>
        ///-------------------------------------------------------------------------------------------------

        void ConnectClient(string serverIPv4Address, int port, string clientIPv4Address, int clientPort, string sessionName = null, string sessionToken = null, string name = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Disconnects the given informServer. </summary>
        ///
        /// <param name="informServer"> (Optional) The inform server to disconnect. </param>
        ///-------------------------------------------------------------------------------------------------

        void Disconnect(bool informServer = true);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to. </summary>
        ///
        /// <param name="id">   The identifier. </param>
        /// <param name="data"> The data. </param>
        ///
        /// <returns>   A Dictionary&lt;Guid,Exception&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        Exception SendDataTo(Guid id, object? data);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to. </summary>
        ///
        /// <param name="ids">  The identifiers. </param>
        /// <param name="data"> The data. </param>
        ///
        /// <returns>   A Dictionary&lt;Guid,Exception&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<Guid, Exception> SendDataTo(List<Guid> ids, object? data);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Broadcasts the given data. </summary>
        ///
        /// <param name="data"> The data. </param>
        ///
        /// <returns>   A Dictionary&lt;Guid,Exception&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<Guid, Exception> Broadcast(object? data);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets client by identifier. </summary>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   The client by identifier. </returns>
        ///-------------------------------------------------------------------------------------------------

        IClientPacketData GetClientById(Guid id);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets random port number. </summary>
        ///
        /// <param name="exclude">  A variable-length parameters list containing exclude. </param>
        ///
        /// <returns>   The random port number. </returns>
        ///-------------------------------------------------------------------------------------------------

        int GetRandomPortNumber(params int[] exclude);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets clients player game data. </summary>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        ///
        /// <returns>   The clients player game data. </returns>
        ///-------------------------------------------------------------------------------------------------

        List<T> GetClientsPlayerGameData<T>();
    }
}
