
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for game pad manager. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IGamePadManager : IInputStateManager
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state. </summary>
        ///
        /// <value> The state. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<PlayerIndex, GamePadState> State { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state of the last. </summary>
        ///
        /// <value> The last state. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<PlayerIndex, GamePadState> LastState { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets state for player. </summary>
        ///
        /// <param name="index">    Zero-based index of the. </param>
        ///
        /// <returns>   The state for player. </returns>
        ///-------------------------------------------------------------------------------------------------

        GamePadState GetStateForPlayer(PlayerIndex index);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Button press. </summary>
        ///
        /// <param name="index">    Zero-based index of the. </param>
        /// <param name="button">   The button. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        bool ButtonPress(PlayerIndex index, Buttons button);
    }
}
