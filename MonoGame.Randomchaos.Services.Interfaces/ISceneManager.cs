using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface ISceneManager
    {
        IScene CurrentScene { get; }
        Dictionary<string, IScene> Scenes { get; set; }
        void AddScene(IScene scene);
        void LoadScene(string name);

        SceneStateEnum CurrentSceneState { get; }
    }
}
