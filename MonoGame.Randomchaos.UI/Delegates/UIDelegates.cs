
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;

namespace MonoGame.Randomchaos.UI.Delegates
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Mouse event. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///
    /// <param name="sender">       The sender. </param>
    /// <param name="mouseState">   State of the mouse. </param>
    ///-------------------------------------------------------------------------------------------------

    public delegate void UIMouseEvent(IUIBase sender, IMouseStateManager mouseState);
}
