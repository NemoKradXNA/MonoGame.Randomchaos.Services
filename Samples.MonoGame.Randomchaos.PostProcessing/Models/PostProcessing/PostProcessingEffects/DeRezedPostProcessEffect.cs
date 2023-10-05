using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.PostProcessing.Models;
using Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcess;

namespace Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcessingEffects
{
    public class DeRezedPostProcessEffect : BasePostProcessingEffect
    {
        public int NumberofTiles { get { return _deRezed.NumberOfTiles; } set { _deRezed.NumberOfTiles = value; } }

        private DeRezPostProcess _deRezed;

        public DeRezedPostProcessEffect(Game game, int numberofTiles) : base(game)
        {
            _deRezed = new DeRezPostProcess(game, numberofTiles);

            AddPostProcess(_deRezed);
        }
    }
}
