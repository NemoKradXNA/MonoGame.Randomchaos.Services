
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Interfaces.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for scene component colection. </summary>
    ///
    /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ISceneComponentColection
    {
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the components. </summary>
        ///
        /// <value> The components. </value>
        ///-------------------------------------------------------------------------------------------------

        List<IGameComponent> Components { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the UI components. </summary>
        ///
        /// <value> The user interface components. </value>
        ///-------------------------------------------------------------------------------------------------

        List<IGameComponent> UIComponents { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the scene components. </summary>
        ///
        /// <value> The scene components. </value>
        ///-------------------------------------------------------------------------------------------------

        List<IGameComponent> SceneComponents { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds component. </summary>
        ///
        /// <param name="component">    The component to add. </param>
        ///-------------------------------------------------------------------------------------------------

        void Add(IGameComponent component);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the given component. </summary>
        ///
        /// <param name="component">    The component to add. </param>
        ///-------------------------------------------------------------------------------------------------

        void Remove(IGameComponent component);


        /// <summary>   Clears this object to its blank/initial state. </summary>
        void Clear();
    }
}
