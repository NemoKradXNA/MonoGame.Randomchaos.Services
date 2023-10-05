
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.PostProcessing.Models;

namespace Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcess
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A chromatic aberration. </summary>
    ///
    /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class ChromaticAberration : BasePostProcess
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the curvature. </summary>
        ///
        /// <value> The curvature. </value>
        ///-------------------------------------------------------------------------------------------------

        public float ScreenCurvature { get; set; } = 0f;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the blur. </summary>
        ///
        /// <value> The blur. </value>
        ///-------------------------------------------------------------------------------------------------

        public float Blur { get; set; } = .05f;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the line density. </summary>
        ///
        /// <value> The line density. </value>
        ///-------------------------------------------------------------------------------------------------

        public float LineDensity { get; set; } = .9f;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the scan line opacity. </summary>
        ///
        /// <value> The scan line opacity. </value>
        ///-------------------------------------------------------------------------------------------------

        public float ScanLineOpacity { get; set; } = .0125f;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the noise opacity. </summary>
        ///
        /// <value> The noise opacity. </value>
        ///-------------------------------------------------------------------------------------------------

        public float NoiseOpacity { get; set; } = .0125f;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the flickering. </summary>
        ///
        /// <value> The flickering. </value>
        ///-------------------------------------------------------------------------------------------------

        public float Flickering { get; set; } = 0.0125f;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the RGB horizontal shift. </summary>
        ///
        /// <value> The RGB horizontal shift. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 RGBHorizontalShift { get; set; } = new Vector3(.3f, -0.0f, -.3f);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the noise texture asset. </summary>
        ///
        /// <value> The noise texture asset. </value>
        ///-------------------------------------------------------------------------------------------------

        public string NoiseTextureAsset { get; set; } = "Textures/noise";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public ChromaticAberration(Game game) : base(game) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Draw(GameTime gameTime)
        {
            if (effect == null)
            {
                effect = Game.Content.Load<Effect>("Shaders/PostProcessing/ChromaticAberration");
            }

            // Set Params.
            effect.Parameters["CURVATURE"].SetValue(ScreenCurvature);//3.9
            effect.Parameters["BLUR"].SetValue(Blur);//0.021;
            effect.Parameters["density"].SetValue(LineDensity);
            effect.Parameters["opacityScanline"].SetValue(ScanLineOpacity);
            effect.Parameters["opacityNoise"].SetValue(NoiseOpacity);
            effect.Parameters["flickering"].SetValue(Flickering);
            effect.Parameters["shift"].SetValue(RGBHorizontalShift);
            effect.Parameters["iTime"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds * .0125f);
            effect.Parameters["iResolution"].SetValue(new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height));

            effect.Parameters["NoiseTex"].SetValue(Game.Content.Load<Texture2D>(NoiseTextureAsset));

            base.Draw(gameTime);
        }

    }
}
