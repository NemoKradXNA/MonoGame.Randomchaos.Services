using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Input.Models
{
    public class GamePadManager : GameComponent, IGamePadManager
    {
        public Dictionary<PlayerIndex, GamePadState> State { get; set; }
        public Dictionary<PlayerIndex, GamePadState> LastState { get; set; }

        public GamePadManager(Game game) : base(game)
        {
            State = new Dictionary<PlayerIndex, GamePadState>();
            LastState = new Dictionary<PlayerIndex, GamePadState>();

            State.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One));
            State.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two));
            State.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three));
            State.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four));

            LastState.Add(PlayerIndex.One, new GamePadState());
            LastState.Add(PlayerIndex.Two, new GamePadState());
            LastState.Add(PlayerIndex.Three, new GamePadState());
            LastState.Add(PlayerIndex.Four, new GamePadState());
        }

        public override void Update(GameTime gameTime)
        {
            State[PlayerIndex.One] = GamePad.GetState(PlayerIndex.One);
            State[PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two);
            State[PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three);
            State[PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four);

            base.Update(gameTime);
        }


        public GamePadState GetStateForPlayer(PlayerIndex index)
        {
            return State[index];
        }

        public bool ButtonPress(PlayerIndex index, Buttons button)
        {
            bool retVal = false;
            switch (button)
            {
                case Buttons.A:
                    retVal = State[index].Buttons.A == ButtonState.Released && LastState[index].Buttons.A == ButtonState.Pressed;
                    break;
                case Buttons.B:
                    retVal = State[index].Buttons.B == ButtonState.Released && LastState[index].Buttons.B == ButtonState.Pressed;
                    break;
                case Buttons.X:
                    retVal = State[index].Buttons.X == ButtonState.Released && LastState[index].Buttons.X == ButtonState.Pressed;
                    break;
                case Buttons.Y:
                    retVal = State[index].Buttons.Y == ButtonState.Released && LastState[index].Buttons.Y == ButtonState.Pressed;
                    break;
                case Buttons.Back:
                    retVal = State[index].Buttons.Back == ButtonState.Released && LastState[index].Buttons.Back == ButtonState.Pressed;
                    break;
                case Buttons.BigButton:
                    retVal = State[index].Buttons.BigButton == ButtonState.Released && LastState[index].Buttons.BigButton == ButtonState.Pressed;
                    break;
                case Buttons.DPadDown:
                    retVal = State[index].DPad.Down == ButtonState.Released && LastState[index].DPad.Down == ButtonState.Pressed;
                    break;
                case Buttons.DPadLeft:
                    retVal = State[index].DPad.Left == ButtonState.Released && LastState[index].DPad.Left == ButtonState.Pressed;
                    break;
                case Buttons.DPadRight:
                    retVal = State[index].DPad.Right == ButtonState.Released && LastState[index].DPad.Right == ButtonState.Pressed;
                    break;
                case Buttons.DPadUp:
                    retVal = State[index].DPad.Up == ButtonState.Released && LastState[index].DPad.Up == ButtonState.Pressed;
                    break;
                case Buttons.LeftShoulder:
                    retVal = State[index].Buttons.LeftShoulder == ButtonState.Released && LastState[index].Buttons.LeftShoulder == ButtonState.Pressed;
                    break;
                case Buttons.LeftStick:
                    retVal = State[index].Buttons.LeftStick == ButtonState.Released && LastState[index].Buttons.LeftStick == ButtonState.Pressed;
                    break;
                case Buttons.RightShoulder:
                    retVal = State[index].Buttons.RightShoulder == ButtonState.Released && LastState[index].Buttons.RightShoulder == ButtonState.Pressed;
                    break;
                case Buttons.RightStick:
                    retVal = State[index].Buttons.RightStick == ButtonState.Released && LastState[index].Buttons.RightStick == ButtonState.Pressed;
                    break;
                case Buttons.Start:
                    retVal = State[index].Buttons.Start == ButtonState.Released && LastState[index].Buttons.Start == ButtonState.Pressed;
                    break;
            }
            return retVal;
        }

        public void PreUpdate(GameTime gameTime)
        {
            LastState[PlayerIndex.One] = State[PlayerIndex.One];
            LastState[PlayerIndex.Two] = State[PlayerIndex.Two];
            LastState[PlayerIndex.Three] = State[PlayerIndex.Three];
            LastState[PlayerIndex.Four] = State[PlayerIndex.Four];
        }
    }
}
