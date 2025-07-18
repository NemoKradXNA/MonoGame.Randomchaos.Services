﻿using MonoGame.Randomchaos.Services.P2P.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Randomchaos.Services.P2P.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for communications packet. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ICommsPacket
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the identifier. </summary>
        ///
        /// <value> The identifier. </value>
        ///-------------------------------------------------------------------------------------------------

        Guid Id { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the communications. </summary>
        ///
        /// <value> The communications. </value>
        ///-------------------------------------------------------------------------------------------------

        CommsEnum Comms { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the protocol. </summary>
        ///
        /// <value> The protocol. </value>
        ///-------------------------------------------------------------------------------------------------

        ProtocolTypesEnum Protocol { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the IP address. </summary>
        ///
        /// <value> The IP address. </value>
        ///-------------------------------------------------------------------------------------------------

        string IPAddress { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the port. </summary>
        ///
        /// <value> The port. </value>
        ///-------------------------------------------------------------------------------------------------

        int Port { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the Date/Time of the sent. </summary>
        ///
        /// <value> The sent. </value>
        ///-------------------------------------------------------------------------------------------------

        DateTime Sent { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the data. </summary>
        ///
        /// <value> The data. </value>
        ///-------------------------------------------------------------------------------------------------

        object? Data { get; set; }
    }
}
