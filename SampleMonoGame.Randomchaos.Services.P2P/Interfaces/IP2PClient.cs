using SampleMonoGame.Randomchaos.Services.P2P.Delegates;
using SampleMonoGame.Randomchaos.Services.P2P.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace SampleMonoGame.Randomchaos.Services.P2P.Interfaces
{
    public interface IP2PClient
    {
        event DataReceivedEvent OnTcpDataReceived;
        event DataReceivedEvent OnUdpDataReceived;
        event ConnectionDroppedEvent OnConnectionDropped;
        event ConnectionDroppedEvent OnClientConnectionDropped;
        event ConnectionAttemptEvent OnNewClientAdded;
        event LogEvent OnLog;

        PlayerData PlayerData { get; set; }

        Guid? Id { get; }
        string IPv4Address { get; }
        List<IClientPacketData> Clients { get; }
        int Port { get; }
        string ServerIPv4Address { get; }

        void Connect();
        void Disconnect(bool informServer = true);
        void SendDataTo(ICommsPacket data, IPEndPoint endpoint = null);
        void BroadcastTo(ICommsPacket data, List<IClientPacketData> clients);
    }
}
