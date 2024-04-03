

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Interfaces;

namespace MonoGame.Randomchaos.Services.Input.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Manager for mouse states. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class MouseStateManager : GameComponent, IMouseStateManager
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

        public MouseState State { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state of the last. </summary>
        ///
        /// <value> The last state. </value>
        ///-------------------------------------------------------------------------------------------------

        public MouseState LastState { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the screen point. </summary>
        ///
        /// <value> The screen point. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point ScreenPoint { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the position. </summary>
        ///
        /// <value> The position. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 Position { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the last position. </summary>
        ///
        /// <value> The last position. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 LastPosition { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the direction. </summary>
        ///
        /// <value> The direction. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 Direction
        {
            get
            {
                if (Velocity != Vector2.Zero)
                    return Vector2.Normalize(Velocity);
                else
                    return Velocity;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the velocity. </summary>
        ///
        /// <value> The velocity. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 Velocity { get { return Position - LastPosition; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the position rectangle. </summary>
        ///
        /// <value> The position rectangle. </value>
        ///-------------------------------------------------------------------------------------------------

        public Rectangle PositionRect { get { return !Handled ?  new Rectangle((int)Position.X, (int)Position.Y, 1, 1) : Rectangle.Empty; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public MouseStateManager(Game game) : base(game) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the left clicked. </summary>
        ///
        /// <value> True if left clicked, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool LeftClicked { get { return LeftButtonState == ButtonState.Released && LastState.LeftButton == ButtonState.Pressed && !Handled; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the right clicked. </summary>
        ///
        /// <value> True if right clicked, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool RightClicked { get { return RighttButtonState == ButtonState.Released && LastState.RightButton == ButtonState.Pressed && !Handled; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the middle clicked. </summary>
        ///
        /// <value> True if middle clicked, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool MiddleClicked { get { return MiddleButtonState == ButtonState.Released && LastState.MiddleButton == ButtonState.Pressed && !Handled; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the button 1 clicked. </summary>
        ///
        /// <value> True if button 1 clicked, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool XButton1Clicked { get { return State.XButton1 == ButtonState.Released && LastState.XButton1 == ButtonState.Pressed && !Handled; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the button 2 clicked. </summary>
        ///
        /// <value> True if button 2 clicked, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool XButton2Clicked { get { return State.XButton2 == ButtonState.Released && LastState.XButton2 == ButtonState.Pressed && !Handled; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the initial left button down. </summary>
        ///
        /// <value> True if initial left button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool InitialLeftButtonDown { get { return LeftButtonState == ButtonState.Pressed && LastState.LeftButton != ButtonState.Pressed; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the initial right button down. </summary>
        ///
        /// <value> True if initial right button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool InitialRightButtonDown { get { return RighttButtonState == ButtonState.Pressed && LastState.RightButton != ButtonState.Pressed && !Handled; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the initial middle button down. </summary>
        ///
        /// <value> True if initial middle button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool InitialMiddleButtonDown { get { return MiddleButtonState == ButtonState.Pressed && LastState.MiddleButton != ButtonState.Pressed && !Handled; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the state of the left button. </summary>
        ///
        /// <value> The left button state. </value>
        ///-------------------------------------------------------------------------------------------------

        public ButtonState LeftButtonState { get { return !Handled ? State.LeftButton : ButtonState.Released; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the state of the rightt button. </summary>
        ///
        /// <value> The rightt button state. </value>
        ///-------------------------------------------------------------------------------------------------

        public ButtonState RighttButtonState { get { return !Handled ?  State.RightButton : ButtonState.Released; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the state of the middle button. </summary>
        ///
        /// <value> The middle button state. </value>
        ///-------------------------------------------------------------------------------------------------

        public ButtonState MiddleButtonState { get { return !Handled ? State.MiddleButton : ButtonState.Released; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the left button down. </summary>
        ///
        /// <value> True if left button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool LeftButtonDown { get { return LeftButtonState == ButtonState.Pressed && ! Handled; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the right button down. </summary>
        ///
        /// <value> True if right button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool RightButtonDown { get { return RighttButtonState == ButtonState.Pressed && !Handled; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the middle button down. </summary>
        ///
        /// <value> True if middle button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool MiddleButtonDown { get { return MiddleButtonState == ButtonState.Pressed && !Handled; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the button 1 button down. </summary>
        ///
        /// <value> True if button 1 button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool XButton1ButtonDown { get { return State.XButton1 == ButtonState.Pressed && !Handled; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the button 2 button down. </summary>
        ///
        /// <value> True if button 2 button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool XButton2ButtonDown { get { return State.XButton2 == ButtonState.Pressed && !Handled; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the scroll wheel value. </summary>
        ///
        /// <value> The scroll wheel value. </value>
        ///-------------------------------------------------------------------------------------------------

        public int ScrollWheelValue { get { return !Handled ? State.ScrollWheelValue : 0; } }

        /// <summary>   The last scroll value. </summary>
        private int lastScrollVal = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the scroll wheel delta. </summary>
        ///
        /// <value> The scroll wheel delta. </value>
        ///-------------------------------------------------------------------------------------------------

        public int ScrollWheelDelta { get { return ScrollWheelValue - lastScrollVal; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the position delta. </summary>
        ///
        /// <value> The position delta. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 PositionDelta { get { return LastPosition - Position; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
            State = Mouse.GetState();
            ScreenPoint = new Point(State.X, State.Y);
            Position = new Vector2(State.X, State.Y);
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
            Handled = false;
            LastState = State;
            lastScrollVal = LastState.ScrollWheelValue;

            LastPosition = Position;
        }
    }
}
