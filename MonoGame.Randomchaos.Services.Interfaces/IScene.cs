
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System;
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

        ISceneComponentColection Components { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   
        /// Gets or sets a list of types of the UI components. This is used to distinguish between UI and
        /// regular scene elements. UI elements will be rendered after scene objects and have their own
        /// render loop.
        /// </summary>
        ///
        /// <value> A list of types of the components. </value>
        ///-------------------------------------------------------------------------------------------------

        List<Type> UIComponentTypes { get; set; }

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
