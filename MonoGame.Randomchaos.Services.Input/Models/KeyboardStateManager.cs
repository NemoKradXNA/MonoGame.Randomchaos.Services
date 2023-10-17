
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Headers;
using static System.Formats.Asn1.AsnWriter;
using System.Xml;

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

        public string KeysToString(Keys keys)
        {
            string retVal = string.Empty;
            switch (keys)
            {
                case Keys.None:
                case Keys.Back:
                case Keys.Tab:
                case Keys.Enter:
                case Keys.CapsLock:
                case Keys.Escape:
                case Keys.Space:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.End:
                case Keys.Home:
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                case Keys.Select:
                case Keys.Print:
                case Keys.Execute:
                case Keys.PrintScreen:
                case Keys.Insert:
                case Keys.Delete:
                case Keys.Help:
                    break;
                case Keys.D0:
                    retVal = "0";
                    break;
                case Keys.D1:
                    retVal = "1";
                    break;
                case Keys.D2:
                    retVal = "2";
                    break;
                case Keys.D3:
                    retVal = "3";
                    break;
                case Keys.D4:
                    retVal = "4";
                    break;
                case Keys.D5:
                    retVal = "5";
                    break;
                case Keys.D6:
                    retVal = "6";
                    break;
                case Keys.D7:
                    retVal = "7";
                    break;
                case Keys.D8:
                    retVal = "8";
                    break;
                case Keys.D9:
                    retVal = "9";
                    break;
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.G:
                case Keys.H:
                case Keys.I:
                case Keys.J:
                case Keys.K:
                case Keys.L:
                case Keys.M:
                case Keys.N:
                case Keys.O:
                case Keys.P:
                case Keys.Q:
                case Keys.R:
                case Keys.S:
                case Keys.T:
                case Keys.U:
                case Keys.V:
                case Keys.W:
                case Keys.X:
                case Keys.Y:
                case Keys.Z:
                    retVal = keys.ToString();
                    break;
                case Keys.LeftWindows:
                case Keys.RightWindows:
                case Keys.Apps:
                case Keys.Sleep:
                    break;
                case Keys.NumPad0:
                    retVal = "0";
                    break;
                case Keys.NumPad1:
                    retVal = "1";
                    break;
                case Keys.NumPad2:
                    retVal = "2";
                    break;
                case Keys.NumPad3:
                    retVal = "3";
                    break;
                case Keys.NumPad4:
                    retVal = "4";
                    break;
                case Keys.NumPad5:
                    retVal = "5";
                    break;
                case Keys.NumPad6:
                    retVal = "6";
                    break;
                case Keys.NumPad7:
                    retVal = "7";
                    break;
                case Keys.NumPad8:
                    retVal = "8";
                    break;
                case Keys.NumPad9:
                    retVal = "9";
                    break;
                case Keys.Multiply:
                    retVal = "*";
                    break;
                case Keys.Add:
                    retVal = "+";
                    break;
                case Keys.Separator:
                    retVal = "_";
                    break;
                case Keys.Subtract:
                    retVal = "-";
                    break;
                case Keys.Decimal:
                    retVal = ".";
                    break;
                case Keys.Divide:
                    retVal = "/";
                    break;
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.F13:
                case Keys.F14:
                case Keys.F15:
                case Keys.F16:
                case Keys.F17:
                case Keys.F18:
                case Keys.F19:
                case Keys.F20:
                case Keys.F21:
                case Keys.F22:
                case Keys.F23:
                case Keys.F24:
                case Keys.NumLock:
                case Keys.Scroll:
                case Keys.LeftShift:
                case Keys.RightShift:
                case Keys.LeftControl:
                case Keys.RightControl:
                case Keys.LeftAlt:
                case Keys.RightAlt:
                case Keys.BrowserBack:
                case Keys.BrowserForward:
                case Keys.BrowserRefresh:
                case Keys.BrowserStop:
                case Keys.BrowserSearch:
                case Keys.BrowserFavorites:
                case Keys.BrowserHome:
                case Keys.VolumeMute:
                case Keys.VolumeDown:
                case Keys.VolumeUp:
                case Keys.MediaNextTrack:
                case Keys.MediaPreviousTrack:
                case Keys.MediaStop:
                case Keys.MediaPlayPause:
                case Keys.LaunchMail:
                case Keys.SelectMedia:
                case Keys.LaunchApplication1:
                case Keys.LaunchApplication2:
                    break;
                case Keys.OemSemicolon:
                    retVal = ";";
                    break;
                case Keys.OemPlus:
                    retVal = "+";
                    break;
                case Keys.OemComma:
                    retVal = ",";
                    break;
                case Keys.OemMinus:
                    retVal = "-";
                    break;
                case Keys.OemPeriod:
                    retVal = ".";
                    break;
                case Keys.OemQuestion:
                    retVal = "?";
                    break;
                case Keys.OemTilde:
                    retVal = "~";
                    break;
                case Keys.OemOpenBrackets:
                    retVal = ")";
                    break;
                case Keys.OemPipe:
                    retVal = "|";
                    break;
                case Keys.OemCloseBrackets:
                    retVal = ")";
                    break;
                case Keys.OemQuotes:
                    retVal = "\"";
                    break;
                case Keys.Oem8:
                    retVal = "*";
                    break;
                case Keys.OemBackslash:
                    retVal = "\\";
                    break;
                case Keys.ProcessKey:
                case Keys.Attn:
                case Keys.Crsel:
                case Keys.Exsel:
                case Keys.EraseEof:
                case Keys.Play:
                case Keys.Zoom:
                case Keys.Pa1:
                case Keys.OemClear:
                case Keys.ChatPadGreen:
                case Keys.ChatPadOrange:
                case Keys.Pause:
                case Keys.ImeConvert:
                case Keys.ImeNoConvert:
                case Keys.Kana:
                case Keys.Kanji:
                case Keys.OemAuto:
                case Keys.OemCopy:
                case Keys.OemEnlW:
                    break;
            }

            return retVal;
        }

        public string KeysToString(List<Keys> keys)
        {
            string retVal = string.Empty;

            foreach (Keys key in keys)
            {
                retVal += KeysToString(key);
            }

            return retVal;
        }
    }
}
