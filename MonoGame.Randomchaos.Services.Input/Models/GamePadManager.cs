
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Input.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Manager for game pads. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class GamePadManager : GameComponent, IGamePadManager
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the handled. </summary>
        ///
        /// <value> True if handled, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool Handled { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state. </summary>
        ///
        /// <value> The state. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<PlayerIndex, GamePadState> State { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state of the last. </summary>
        ///
        /// <value> The last state. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<PlayerIndex, GamePadState> LastState { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
            State[PlayerIndex.One] = GamePad.GetState(PlayerIndex.One);
            State[PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two);
            State[PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three);
            State[PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four);

            base.Update(gameTime);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets state for player. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="index">    Zero-based index of the. </param>
        ///
        /// <returns>   The state for player. </returns>
        ///-------------------------------------------------------------------------------------------------

        public GamePadState GetStateForPlayer(PlayerIndex index)
        {
            return State[index];
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Button press. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="index">    Zero-based index of the. </param>
        /// <param name="button">   The button. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

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
            return retVal && !Handled;
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
            Handled = false;
            LastState[PlayerIndex.One] = State[PlayerIndex.One];
            LastState[PlayerIndex.Two] = State[PlayerIndex.Two];
            LastState[PlayerIndex.Three] = State[PlayerIndex.Three];
            LastState[PlayerIndex.Four] = State[PlayerIndex.Four];
        }
    }
}
