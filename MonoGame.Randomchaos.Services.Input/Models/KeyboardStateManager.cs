using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Randomchaos.Services.Input.Models
{
    public class KeyboardStateManager : GameComponent, IKeyboardStateManager
    {
        public KeyboardState State { get; set; }
        public KeyboardState LastState { get; set; }

        public KeyboardStateManager(Game game) : base(game) { }

        public bool NumLock { get { return State.NumLock; } }
        public bool CapsLock { get { return State.CapsLock; } }

        public bool ShiftIsDown { get { return State.IsKeyDown(Keys.LeftShift) || State.IsKeyDown(Keys.RightShift); } }

        public bool CtrlIsDown { get { return State.IsKeyDown(Keys.LeftControl) || State.IsKeyDown(Keys.RightControl); } }

        public Keys[] KeysPressed()
        {
            return State.GetPressedKeys();
        }

        protected int maxFrames = 10;
        protected Dictionary<Keys, int> frameExceptions = new Dictionary<Keys, int>();

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

        public bool KeyDown(Keys key)
        {
            return State.IsKeyDown(key);
        }
        public bool KeyPress(Keys key)
        {
            return (State.IsKeyUp(key) && LastState.IsKeyDown(key));
        }

        public override void Update(GameTime gameTime)
        {
            State = Keyboard.GetState();

            base.Update(gameTime);
        }

        public void PreUpdate(GameTime gameTime)
        {
            LastState = State;
        }
    }
}
