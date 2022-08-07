using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface IScene : IGameComponent
    {
        ISceneService sceneManager { get; }
        string Name { get; set; }
        IScene LastScene { get; set; }

        SceneStateEnum State { get; set; }

        List<IGameComponent> Components { get; set; }

        void LoadScene();
        void LoadScene(params object[] paramters);
        void UnloadScene();
    }
}
