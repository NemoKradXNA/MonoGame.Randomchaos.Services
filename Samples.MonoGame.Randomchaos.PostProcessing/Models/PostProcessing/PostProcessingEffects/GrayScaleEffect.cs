using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.PostProcessing.Models;
using Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcess;

namespace Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcessingEffects
{
    public class GrayScaleEffect : BasePostProcessingEffect
    {
        GrayScale greyScale;

        public GrayScaleEffect(Game game) : base(game)
        {
            greyScale = new GrayScale(game);

            AddPostProcess(greyScale);
        }
    }
}
