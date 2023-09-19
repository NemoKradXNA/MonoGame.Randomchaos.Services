
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Coroutine.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System.Collections;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Scene.Services
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A service for accessing scenes information. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SceneService : ServiceBase<ISceneService>, ISceneService
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the coroutine service. </summary>
        ///
        /// <value> The coroutine service. </value>
        ///-------------------------------------------------------------------------------------------------

        ICoroutineService CoroutineService { get { return Game.Services.GetService<ICoroutineService>(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the current scene. </summary>
        ///
        /// <value> The current scene. </value>
        ///-------------------------------------------------------------------------------------------------

        public IScene CurrentScene { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current scene state. </summary>
        ///
        /// <value> The current scene state. </value>
        ///-------------------------------------------------------------------------------------------------

        public SceneStateEnum CurrentSceneState
        {
            get
            {
                if (CurrentScene != null)
                    return CurrentScene.State;

                return SceneStateEnum.Unknown;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the scenes. </summary>
        ///
        /// <value> The scenes. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<string, IScene> Scenes { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public SceneService(Game game) : base(game)
        {
            Scenes = new Dictionary<string, IScene>();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a scene. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="scene">    The scene. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddScene(IScene scene)
        {
            Scenes.Add(scene.Name, scene);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads a scene. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public void LoadScene(string name)
        {
            if (Scenes.ContainsKey(name))
                CoroutineService.StartCoroutine(LoadScene(Scenes[name]));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads a scene. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="name">         The name. </param>
        /// <param name="parameters">   A variable-length parameters list containing parameters. </param>
        ///-------------------------------------------------------------------------------------------------

        public void LoadScene(string name, params object[] parameters)
        {
            if (Scenes.ContainsKey(name))
                CoroutineService.StartCoroutine(LoadScene(Scenes[name], parameters));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads a scene. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="scene">        The scene. </param>
        /// <param name="paramters">    A variable-length parameters list containing paramters. </param>
        ///
        /// <returns>   The scene. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected IEnumerator LoadScene(IScene scene, params object[] paramters)
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
            scene.LoadScene(paramters);
        }
    }
}
