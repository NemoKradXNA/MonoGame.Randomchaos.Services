using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface ISceneService
    {
        IScene CurrentScene { get; }
        Dictionary<string, IScene> Scenes { get; set; }
        void AddScene(IScene scene);
        void LoadScene(string name);

        void LoadScene(string name, params object[] parameters);

        SceneStateEnum CurrentSceneState { get; }
    }
}
