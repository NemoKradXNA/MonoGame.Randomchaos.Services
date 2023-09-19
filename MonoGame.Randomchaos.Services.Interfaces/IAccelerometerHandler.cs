
using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for accelerometer handler. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IAccelerometerHandler : IInputStateManager
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state of the accelerometer. </summary>
        ///
        /// <value> The accelerometer state. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector3 AccelerometerState { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the time stamp. </summary>
        ///
        /// <value> The time stamp. </value>
        ///-------------------------------------------------------------------------------------------------

        DateTimeOffset TimeStamp { get; set; }
    }
}
