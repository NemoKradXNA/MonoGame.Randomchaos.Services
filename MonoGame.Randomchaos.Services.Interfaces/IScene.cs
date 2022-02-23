using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface IScene : IGameComponent
    {
        ISceneManager sceneManager { get; }
        string Name { get; set; }
        IScene LastScene { get; set; }

        SceneStateEnum State { get; set; }

        List<IGameComponent> Components { get; set; }

        void LoadScene();
        void UnloadScene();
    }
}
