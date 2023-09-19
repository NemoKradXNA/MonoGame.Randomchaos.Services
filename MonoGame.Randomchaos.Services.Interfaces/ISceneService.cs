
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for scene service. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ISceneService
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current scene. </summary>
        ///
        /// <value> The current scene. </value>
        ///-------------------------------------------------------------------------------------------------

        IScene CurrentScene { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the scenes. </summary>
        ///
        /// <value> The scenes. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<string, IScene> Scenes { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a scene. </summary>
        ///
        /// <param name="scene">    The scene. </param>
        ///-------------------------------------------------------------------------------------------------

        void AddScene(IScene scene);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads a scene. </summary>
        ///
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        void LoadScene(string name);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads a scene. </summary>
        ///
        /// <param name="name">         The name. </param>
        /// <param name="parameters">   A variable-length parameters list containing parameters. </param>
        ///-------------------------------------------------------------------------------------------------

        void LoadScene(string name, params object[] parameters);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current scene state. </summary>
        ///
        /// <value> The current scene state. </value>
        ///-------------------------------------------------------------------------------------------------

        SceneStateEnum CurrentSceneState { get; }
    }
}
