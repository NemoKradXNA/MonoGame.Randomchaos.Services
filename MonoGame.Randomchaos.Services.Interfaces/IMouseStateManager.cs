
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for mouse state manager. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IMouseStateManager : IInputStateManager
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state. </summary>
        ///
        /// <value> The state. </value>
        ///-------------------------------------------------------------------------------------------------

        MouseState State { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state of the last. </summary>
        ///
        /// <value> The last state. </value>
        ///-------------------------------------------------------------------------------------------------

        MouseState LastState { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the left clicked. </summary>
        ///
        /// <value> True if left clicked, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool LeftClicked { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the right clicked. </summary>
        ///
        /// <value> True if right clicked, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool RightClicked { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the middle clicked. </summary>
        ///
        /// <value> True if middle clicked, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool MiddleClicked { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the button 1 clicked. </summary>
        ///
        /// <value> True if button 1 clicked, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool XButton1Clicked { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the button 2 clicked. </summary>
        ///
        /// <value> True if button 2 clicked, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool XButton2Clicked { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the initial left button down. </summary>
        ///
        /// <value> True if initial left button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool InitialLeftButtonDown { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the initial right button down. </summary>
        ///
        /// <value> True if initial right button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool InitialRightButtonDown { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the initial middle button down. </summary>
        ///
        /// <value> True if initial middle button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool InitialMiddleButtonDown { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the state of the left button. </summary>
        ///
        /// <value> The left button state. </value>
        ///-------------------------------------------------------------------------------------------------

        ButtonState LeftButtonState { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the state of the rightt button. </summary>
        ///
        /// <value> The rightt button state. </value>
        ///-------------------------------------------------------------------------------------------------

        ButtonState RighttButtonState { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the state of the middle button. </summary>
        ///
        /// <value> The middle button state. </value>
        ///-------------------------------------------------------------------------------------------------

        ButtonState MiddleButtonState { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the left button down. </summary>
        ///
        /// <value> True if left button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool LeftButtonDown { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the right button down. </summary>
        ///
        /// <value> True if right button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool RightButtonDown { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the middle button down. </summary>
        ///
        /// <value> True if middle button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool MiddleButtonDown { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the button 1 button down. </summary>
        ///
        /// <value> True if button 1 button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool XButton1ButtonDown { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the button 2 button down. </summary>
        ///
        /// <value> True if button 2 button down, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool XButton2ButtonDown { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the scroll wheel value. </summary>
        ///
        /// <value> The scroll wheel value. </value>
        ///-------------------------------------------------------------------------------------------------

        int ScrollWheelValue { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the scroll wheel delta. </summary>
        ///
        /// <value> The scroll wheel delta. </value>
        ///-------------------------------------------------------------------------------------------------

        int ScrollWheelDelta { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the position delta. </summary>
        ///
        /// <value> The position delta. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector2 PositionDelta { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the screen point. </summary>
        ///
        /// <value> The screen point. </value>
        ///-------------------------------------------------------------------------------------------------

        Point ScreenPoint { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the position. </summary>
        ///
        /// <value> The position. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector2 Position { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the last position. </summary>
        ///
        /// <value> The last position. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector2 LastPosition { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the direction. </summary>
        ///
        /// <value> The direction. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector2 Direction { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the velocity. </summary>
        ///
        /// <value> The velocity. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector2 Velocity { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the position rectangle. </summary>
        ///
        /// <value> The position rectangle. </value>
        ///-------------------------------------------------------------------------------------------------

        Rectangle PositionRect { get; }
    }
}
