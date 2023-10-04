
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces.PostProcessing
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for base post processing effect. </summary>
    ///
    /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IBasePostProcessingEffect
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the game. </summary>
        ///
        /// <value> The game. </value>
        ///-------------------------------------------------------------------------------------------------

        Game Game { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the post processes. </summary>
        ///
        /// <value> The post processes. </value>
        ///-------------------------------------------------------------------------------------------------

        List<IBasePostProcess> PostProcesses { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the camera. </summary>
        ///
        /// <value> The camera. </value>
        ///-------------------------------------------------------------------------------------------------

        ICameraService Camera { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is enabled. </summary>
        ///
        /// <value> True if enabled, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool Enabled { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the last scene. </summary>
        ///
        /// <value> The last scene. </value>
        ///-------------------------------------------------------------------------------------------------

        RenderTarget2D LastScene { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the original scene. </summary>
        ///
        /// <value> The original scene. </value>
        ///-------------------------------------------------------------------------------------------------

        Texture2D OriginalScene { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        void Update(GameTime gameTime);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws. </summary>
        ///
        /// <param name="gameTime">     The game time. </param>
        /// <param name="currentScene"> The current scene. </param>
        /// <param name="depthBuffer">  Buffer for depth data. </param>
        ///-------------------------------------------------------------------------------------------------

        void Draw(GameTime gameTime, RenderTarget2D currentScene, RenderTarget2D depthBuffer);
    }
}
