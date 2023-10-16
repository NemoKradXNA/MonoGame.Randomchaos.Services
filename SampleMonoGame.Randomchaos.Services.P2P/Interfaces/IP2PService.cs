using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMonoGame.Randomchaos.Services.P2P.Interfaces
{
    public interface IP2PService
    {
        bool IsServer { get; set; }
        int ListeningPort { get; }
        string ServerIPv4Address { get; }
        string LocalIPv4Address { get; }
        string MachineName { get; }

        int PlayerCount { get; }
        void StartServer(int port);
        void StopServer();

        void ConnectClient(string serverIPv4Address, int port);
        void Disconnect();
    }
}
