using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.PostProcessing.Models;
using Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcess;

namespace Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcessingEffects
{
    public class ChromaticAberrationEffect : BasePostProcessingEffect
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the curvature. </summary>
        ///
        /// <value> The curvature. </value>
        ///-------------------------------------------------------------------------------------------------

        public float ScreenCurvature { get { return _chromaticAberration.ScreenCurvature; } set { _chromaticAberration.ScreenCurvature = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the blur. </summary>
        ///
        /// <value> The blur. </value>
        ///-------------------------------------------------------------------------------------------------

        public float Blur { get { return _chromaticAberration.Blur; } set { _chromaticAberration.Blur = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the line density. </summary>
        ///
        /// <value> The line density. </value>
        ///-------------------------------------------------------------------------------------------------

        public float LineDensity { get { return _chromaticAberration.LineDensity; } set { _chromaticAberration.LineDensity = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the scan line opacity. </summary>
        ///
        /// <value> The scan line opacity. </value>
        ///-------------------------------------------------------------------------------------------------

        public float ScanLineOpacity { get { return _chromaticAberration.ScanLineOpacity; } set { _chromaticAberration.ScanLineOpacity = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the noise opacity. </summary>
        ///
        /// <value> The noise opacity. </value>
        ///-------------------------------------------------------------------------------------------------

        public float NoiseOpacity { get { return _chromaticAberration.NoiseOpacity; } set { _chromaticAberration.NoiseOpacity = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the flickering. </summary>
        ///
        /// <value> The flickering. </value>
        ///-------------------------------------------------------------------------------------------------

        public float Flickering { get { return _chromaticAberration.Flickering; } set { _chromaticAberration.Flickering = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the RGB horizontal shift. </summary>
        ///
        /// <value> The RGB horizontal shift. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 RGBHorizontalShift { get { return _chromaticAberration.RGBHorizontalShift; } set { _chromaticAberration.RGBHorizontalShift = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the noise texture asset. </summary>
        ///
        /// <value> The noise texture asset. </value>
        ///-------------------------------------------------------------------------------------------------

        public string NoiseTextureAsset { get { return _chromaticAberration.NoiseTextureAsset; } set { _chromaticAberration.NoiseTextureAsset = value; } }

        /// <summary>   The bleach by pass. </summary>
        private ChromaticAberration _chromaticAberration;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public ChromaticAberrationEffect(Game game) : base(game)
        {
            _chromaticAberration = new ChromaticAberration(game);

            AddPostProcess(_chromaticAberration);
        }
    }
}
