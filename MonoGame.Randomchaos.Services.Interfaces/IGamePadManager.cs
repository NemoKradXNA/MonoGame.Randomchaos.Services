using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface IGamePadManager : IInputStateManager
    {
        Dictionary<PlayerIndex, GamePadState> State { get; set; }
        Dictionary<PlayerIndex, GamePadState> LastState { get; set; }

        GamePadState GetStateForPlayer(PlayerIndex index);
        bool ButtonPress(PlayerIndex index, Buttons button);
    }
}
