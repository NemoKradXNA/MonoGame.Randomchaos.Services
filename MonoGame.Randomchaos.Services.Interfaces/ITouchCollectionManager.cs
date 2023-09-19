
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for touch collection manager. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ITouchCollectionManager : IInputStateManager
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state. </summary>
        ///
        /// <value> The state. </value>
        ///-------------------------------------------------------------------------------------------------

        TouchCollection State { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state of the last. </summary>
        ///
        /// <value> The last state. </value>
        ///-------------------------------------------------------------------------------------------------

        TouchCollection LastState { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the gesture. </summary>
        ///
        /// <value> The gesture. </value>
        ///-------------------------------------------------------------------------------------------------

        GestureSample Gesture { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the required gestures. </summary>
        ///
        /// <value> The required gestures. </value>
        ///-------------------------------------------------------------------------------------------------

        List<GestureType> RequiredGestures { get; set; }
    }
}
