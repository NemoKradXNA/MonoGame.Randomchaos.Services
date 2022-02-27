
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Interfaces;

namespace MonoGame.Randomchaos.Services.Input.Models
{
    public class MouseStateManager : GameComponent, IMouseStateManager
    {
        public MouseState State { get; set; }
        public MouseState LastState { get; set; }

        public Point ScreenPoint { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 LastPosition { get; set; }
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
        public Vector2 Velocity { get { return Position - LastPosition; } }
        public Rectangle PositionRect { get { return new Rectangle((int)Position.X, (int)Position.Y, 1, 1); } }

        public MouseStateManager(Game game) : base(game) { }

        public bool LeftClicked { get { return (LeftButtonState == ButtonState.Released && LastState.LeftButton == ButtonState.Pressed); } }
        public bool RightClicked { get { return (RighttButtonState == ButtonState.Released && LastState.RightButton == ButtonState.Pressed); } }
        public bool MiddleClicked { get { return (MiddleButtonState == ButtonState.Released && LastState.MiddleButton == ButtonState.Pressed); } }
        public bool XButton1Clicked { get { return (State.XButton1 == ButtonState.Released && LastState.XButton1 == ButtonState.Pressed); } }
        public bool XButton2Clicked { get { return (State.XButton2 == ButtonState.Released && LastState.XButton2 == ButtonState.Pressed); } }
        public bool InitialLeftButtonDown { get { return LeftButtonState == ButtonState.Pressed && LastState.LeftButton != ButtonState.Pressed; } }
        public bool InitialRightButtonDown { get { return RighttButtonState == ButtonState.Pressed && LastState.RightButton != ButtonState.Pressed; } }
        public bool InitialMiddleButtonDown { get { return MiddleButtonState == ButtonState.Pressed && LastState.MiddleButton != ButtonState.Pressed; } }
        public ButtonState LeftButtonState { get { return State.LeftButton; } }
        public ButtonState RighttButtonState { get { return State.RightButton; } }
        public ButtonState MiddleButtonState { get { return State.MiddleButton; } }
        public bool LeftButtonDown { get { return LeftButtonState == ButtonState.Pressed; } }
        public bool RightButtonDown { get { return RighttButtonState == ButtonState.Pressed; } }
        public bool MiddleButtonDown { get { return MiddleButtonState == ButtonState.Pressed; } }
        public bool XButton1ButtonDown { get { return State.XButton1 == ButtonState.Pressed; } }
        public bool XButton2ButtonDown { get { return State.XButton2 == ButtonState.Pressed; } }
        public int ScrollWheelValue { get { return State.ScrollWheelValue; } }

        private int lastScrollVal = 0;
        public int ScrollWheelDelta { get { return ScrollWheelValue - lastScrollVal; } }
        public Vector2 PositionDelta { get { return LastPosition - Position; } }

        public override void Update(GameTime gameTime)
        {
            State = Mouse.GetState();
            ScreenPoint = new Point(State.X, State.Y);
            Position = new Vector2(State.X, State.Y);
            base.Update(gameTime);
        }

        public void PreUpdate(GameTime gameTime)
        {
            LastState = State;
            lastScrollVal = LastState.ScrollWheelValue;

            LastPosition = Position;
        }
    }
}
