using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Coroutine.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Randomchaos.Services.Scene.Services
{
    public class SceneService : ServiceBase<ISceneService>, ISceneService
    {
        ICoroutineService CoroutineService { get { return Game.Services.GetService<ICoroutineService>(); } }

        public IScene CurrentScene { get; set; }
        public SceneStateEnum CurrentSceneState
        {
            get
            {
                if (CurrentScene != null)
                    return CurrentScene.State;

                return SceneStateEnum.Unknown;
            }
        }


        public Dictionary<string, IScene> Scenes { get; set; }

        protected Game Game { get; set; }

        public SceneService(Game game) : base(game)
        {
            Game = game;
            Scenes = new Dictionary<string, IScene>();

            Game.Services.AddService(typeof(ISceneService), this);
        }

        public void AddScene(IScene scene)
        {
            Scenes.Add(scene.Name, scene);
        }

        public void LoadScene(string name)
        {
            if (Scenes.ContainsKey(name))
                CoroutineService.StartCoroutine(LoadScene(Scenes[name]));
        }


        protected IEnumerator LoadScene(IScene scene)
        {
            if (CurrentScene != null)
            {
                CurrentScene.State = SceneStateEnum.Unloading;
                CurrentScene.UnloadScene();

                while (CurrentScene.State != SceneStateEnum.Unloaded)
                    yield return new WaitForEndOfFrame(Game);
            }

            CurrentScene = scene;
            CurrentScene.State = SceneStateEnum.Loading;
            scene.LoadScene();
        }
    }
}
