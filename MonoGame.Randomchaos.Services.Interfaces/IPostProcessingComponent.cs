
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Interfaces.PostProcessing;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for post processing component. </summary>
    ///
    /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IPostProcessingComponent : IGameComponent
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the camera. </summary>
        ///
        /// <value> The camera. </value>
        ///-------------------------------------------------------------------------------------------------

        ICameraService Camera { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the post processing effects. </summary>
        ///
        /// <value> The post processing effects. </value>
        ///-------------------------------------------------------------------------------------------------

        List<IBasePostProcessingEffect> PostProcessingEffects { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is enabled. </summary>
        ///
        /// <value> True if enabled, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool Enabled { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   
        /// Gets or sets the render target screen scale. The screen render target is divided by this value.
        /// </summary>
        ///
        /// <value> The render target screen scale. </value>
        ///-------------------------------------------------------------------------------------------------

        Point RenderTargetScreenScale { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the render target depth format. </summary>
        ///
        /// <value> The render target depth format. </value>
        ///-------------------------------------------------------------------------------------------------

        DepthFormat RenderTargetDepthFormat { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the number of render target multi samples. </summary>
        ///
        /// <value> The number of render target multi samples. </value>
        ///-------------------------------------------------------------------------------------------------

        int RenderTargetMultiSampleCount { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the render target clear. </summary>
        ///
        /// <value> The color of the render target clear. </value>
        ///-------------------------------------------------------------------------------------------------

        Color RenderTargetClearColor { get; set; }


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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an effect. </summary>
        ///
        /// <param name="ppEfect">  The efect. </param>
        ///-------------------------------------------------------------------------------------------------

        void AddEffect(IBasePostProcessingEffect ppEfect);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the final render texture. </summary>
        ///
        /// <value> The final render texture. </value>
        ///-------------------------------------------------------------------------------------------------

        RenderTarget2D FinalRenderTexture { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts post process. </summary>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        void StartPostProcess(GameTime gameTime);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ends post process. </summary>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        void EndPostProcess(GameTime gameTime, bool drawFinal = false);

        /// <summary>   Draw final render texture. </summary>
        void DrawFinalRenderTexture(params IPostProcessingComponent[] postProcessors);
    }
}
