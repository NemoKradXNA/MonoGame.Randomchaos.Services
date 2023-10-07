using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.PostProcessing.Models;

namespace Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcess
{
    internal class GrayScale : BasePostProcess
    {
        public GrayScale(Game game) : base(game) { }


        public override void Draw(GameTime gameTime)
        {
            if (effect == null)
            {
                effect = Game.Content.Load<Effect>("Shaders/PostProcessing/GrayScale");
            }


            // Set Params.
            base.Draw(gameTime);
        }
    }
}
