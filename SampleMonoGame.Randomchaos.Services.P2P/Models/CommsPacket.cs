
using Newtonsoft.Json;
using SampleMonoGame.Randomchaos.Services.P2P.Enums;
using SampleMonoGame.Randomchaos.Services.P2P.Interfaces;
using System;

namespace SampleMonoGame.Randomchaos.Services.P2P.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   The communications packet. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class CommsPacket : ICommsPacket
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the communications. </summary>
        ///
        /// <value> The communications. </value>
        ///-------------------------------------------------------------------------------------------------

        public CommsEnum Comms { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the protocol. </summary>
        ///
        /// <value> The protocol. </value>
        ///-------------------------------------------------------------------------------------------------

        public ProtocolTypesEnum Protocol { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the IP address. </summary>
        ///
        /// <value> The IP address. </value>
        ///-------------------------------------------------------------------------------------------------

        public string IPAddress { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the port. </summary>
        ///
        /// <value> The port. </value>
        ///-------------------------------------------------------------------------------------------------

        public int Port { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the Date/Time of the sent. </summary>
        ///
        /// <value> The sent. </value>
        ///-------------------------------------------------------------------------------------------------

        public DateTime Sent { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the data. </summary>
        ///
        /// <value> The data. </value>
        ///-------------------------------------------------------------------------------------------------

        public object? Data { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public CommsPacket() { Sent = DateTime.UtcNow; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
        ///
        /// <param name="udpComms">     The UDP communications. </param>
        /// <param name="ipAddress">    The IP address. </param>
        /// <param name="data">         (Optional) The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public CommsPacket(CommsEnum udpComms, string ipAddress, object? data = null) : base()
        {
            Comms = udpComms;
            IPAddress = ipAddress;
            Data = data;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
