using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface IMouseStateManager : IInputStateManager
    {
        MouseState State { get; set; }
        MouseState LastState { get; set; }

        bool LeftClicked { get; }
        bool RightClicked { get; }
        bool MiddleClicked { get; }
        bool XButton1Clicked { get; }
        bool XButton2Clicked { get; }
        bool InitialLeftButtonDown { get; }
        bool InitialRightButtonDown { get; }
        bool InitialMiddleButtonDown { get; }
        ButtonState LeftButtonState { get; }
        ButtonState RighttButtonState { get; }
        ButtonState MiddleButtonState { get; }
        bool LeftButtonDown { get; }
        bool RightButtonDown { get; }
        bool MiddleButtonDown { get; }
        bool XButton1ButtonDown { get; }
        bool XButton2ButtonDown { get; }
        int ScrollWheelValue { get; }
        int ScrollWheelDelta { get; }
        Vector2 PositionDelta { get; }

        Point ScreenPoint { get; set; }
        Vector2 Position { get; set; }
        Vector2 LastPosition { get; set; }
        Vector2 Direction { get; }
        Vector2 Velocity { get; }
        Rectangle PositionRect { get; }
    }
}
