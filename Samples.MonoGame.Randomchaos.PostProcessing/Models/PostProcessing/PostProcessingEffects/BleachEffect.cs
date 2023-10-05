using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.PostProcessing.Models;
using Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcess;

namespace Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcessingEffects
{
    public class BleachEffect : BasePostProcessingEffect
    {
        BleachByPass bleachByPass;


        public float Opacity { get { return bleachByPass.Opacity; } set { bleachByPass.Opacity = value; } }

        public BleachEffect(Game game, float opacity) : base(game)
        {
            bleachByPass = new BleachByPass(game, opacity);

            AddPostProcess(bleachByPass);
        }
    }
}
