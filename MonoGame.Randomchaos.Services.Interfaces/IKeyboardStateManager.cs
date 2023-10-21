
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for keyboard state manager. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IKeyboardStateManager : IInputStateManager
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state. </summary>
        ///
        /// <value> The state. </value>
        ///-------------------------------------------------------------------------------------------------

        KeyboardState State { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state of the last. </summary>
        ///
        /// <value> The last state. </value>
        ///-------------------------------------------------------------------------------------------------

        KeyboardState LastState { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the number is locked. </summary>
        ///
        /// <value> True if number lock, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool NumLock { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the capabilities is locked. </summary>
        ///
        /// <value> True if capabilities lock, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool CapsLock { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the shift is down. </summary>
        ///
        /// <value> True if shift is down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool ShiftIsDown { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the control is down. </summary>
        ///
        /// <value> True if control is down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool CtrlIsDown { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Keys pressed. </summary>
        ///
        /// <returns>   A Keys[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        Keys[] KeysPressed();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Keys pressed this frame. </summary>
        ///
        /// <returns>   A List&lt;Keys&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        List<Keys> KeysPressedThisFrame();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Key down. </summary>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        bool KeyDown(Keys key);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Key press. </summary>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        bool KeyPress(Keys key);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Keys to string. </summary>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   A string. </returns>
        ///-------------------------------------------------------------------------------------------------

        string KeysToString(Keys key);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Keys to string. </summary>
        ///
        /// <param name="keys"> The keys. </param>
        ///
        /// <returns>   A string. </returns>
        ///-------------------------------------------------------------------------------------------------

        string KeysToString(List<Keys> keys);
    }
}
