
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for scene. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IScene : IGameComponent
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the manager for scene. </summary>
        ///
        /// <value> The scene manager. </value>
        ///-------------------------------------------------------------------------------------------------

        ISceneService sceneManager { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the last scene. </summary>
        ///
        /// <value> The last scene. </value>
        ///-------------------------------------------------------------------------------------------------

        IScene LastScene { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state. </summary>
        ///
        /// <value> The state. </value>
        ///-------------------------------------------------------------------------------------------------

        SceneStateEnum State { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the components. </summary>
        ///
        /// <value> The components. </value>
        ///-------------------------------------------------------------------------------------------------

        List<IGameComponent> Components { get; set; }

        /// <summary>   Loads a scene. </summary>
        void LoadScene();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads a scene. </summary>
        ///
        /// <param name="paramters">    A variable-length parameters list containing paramters. </param>
        ///-------------------------------------------------------------------------------------------------

        void LoadScene(params object[] paramters);
        /// <summary>   Unload scene. </summary>
        void UnloadScene();
    }
}
