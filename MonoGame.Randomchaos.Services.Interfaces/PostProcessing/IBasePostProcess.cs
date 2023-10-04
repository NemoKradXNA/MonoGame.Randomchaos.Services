
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Interfaces;

namespace MonoGame.Randomchaos.Services.Interfaces.PostProcessing
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for base post process. </summary>
    ///
    /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IBasePostProcess
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the game. </summary>
        ///
        /// <value> The game. </value>
        ///-------------------------------------------------------------------------------------------------

        Game Game { get; set; }

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
        /// <summary>   Gets or sets the sampler. </summary>
        ///
        /// <value> The sampler. </value>
        ///-------------------------------------------------------------------------------------------------

        SamplerState Sampler { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the new scene. </summary>
        ///
        /// <value> The new scene. </value>
        ///-------------------------------------------------------------------------------------------------

        RenderTarget2D NewScene { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the buffer for depth data. </summary>
        ///
        /// <value> A buffer for depth data. </value>
        ///-------------------------------------------------------------------------------------------------

        Texture2D DepthBuffer { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object use quad. </summary>
        ///
        /// <value> True if use quad, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool UseQuad { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Current Post Processing BackBuffer. </summary>
        ///
        /// <value> A buffer for back data. </value>
        ///-------------------------------------------------------------------------------------------------

        Texture2D BackBuffer { get; set; }

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
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        void Draw(GameTime gameTime);
    }
}
