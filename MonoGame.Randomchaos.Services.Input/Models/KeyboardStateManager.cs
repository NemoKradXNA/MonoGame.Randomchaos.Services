
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Randomchaos.Services.Input.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Manager for keyboard states. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class KeyboardStateManager : GameComponent, IKeyboardStateManager
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state. </summary>
        ///
        /// <value> The state. </value>
        ///-------------------------------------------------------------------------------------------------

        public KeyboardState State { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state of the last. </summary>
        ///
        /// <value> The last state. </value>
        ///-------------------------------------------------------------------------------------------------

        public KeyboardState LastState { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public KeyboardStateManager(Game game) : base(game) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the number is locked. </summary>
        ///
        /// <value> True if number lock, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool NumLock { get { return State.NumLock; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the capabilities is locked. </summary>
        ///
        /// <value> True if capabilities lock, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool CapsLock { get { return State.CapsLock; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the shift is down. </summary>
        ///
        /// <value> True if shift is down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool ShiftIsDown { get { return State.IsKeyDown(Keys.LeftShift) || State.IsKeyDown(Keys.RightShift); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the control is down. </summary>
        ///
        /// <value> True if control is down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool CtrlIsDown { get { return State.IsKeyDown(Keys.LeftControl) || State.IsKeyDown(Keys.RightControl); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Keys pressed. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <returns>   A Keys[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Keys[] KeysPressed()
        {
            return State.GetPressedKeys();
        }

        /// <summary>   The maximum frames. </summary>
        protected int maxFrames = 10;
        /// <summary>   The frame exceptions. </summary>
        protected Dictionary<Keys, int> frameExceptions = new Dictionary<Keys, int>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Keys pressed this frame. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <returns>   A List&lt;Keys&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public List<Keys> KeysPressedThisFrame()
        {
            List<Keys> keysNow = KeysPressed().ToList();
            List<Keys> keysLast = LastState.GetPressedKeys().ToList();

            List<Keys> retVal = new List<Keys>();

            foreach (Keys key in keysNow)
            {
                if (!keysLast.Contains(key))
                {
                    retVal.Add(key);
                    if (frameExceptions.ContainsKey(key))
                        frameExceptions.Remove(key);
                }
                else
                {
                    if (!frameExceptions.ContainsKey(key))
                        frameExceptions.Add(key, 0);
                    else
                        frameExceptions[key]++;

                    if (frameExceptions[key] >= maxFrames)
                    {
                        // Added it
                        retVal.Add(key);
                        frameExceptions.Remove(key);
                    }
                }
            }

            return retVal;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Key down. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool KeyDown(Keys key)
        {
            return State.IsKeyDown(key);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Key press. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool KeyPress(Keys key)
        {
            return (State.IsKeyUp(key) && LastState.IsKeyDown(key));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
            State = Keyboard.GetState();

            base.Update(gameTime);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pre update. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public void PreUpdate(GameTime gameTime)
        {
            LastState = State;
        }
    }
}
