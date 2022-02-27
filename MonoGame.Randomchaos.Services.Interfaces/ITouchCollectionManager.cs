using System.Collections.Generic;

using Microsoft.Xna.Framework.Input.Touch;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface ITouchCollectionManager : IInputStateManager
    {
        TouchCollection State { get; set; }
        TouchCollection LastState { get; set; }
        GestureSample Gesture { get; set; }
        List<GestureType> RequiredGestures { get; set; }
    }
}
