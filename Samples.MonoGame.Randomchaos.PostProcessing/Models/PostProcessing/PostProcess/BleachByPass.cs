using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.PostProcessing.Models;

namespace Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcess
{
    public class BleachByPass : BasePostProcess
    {
        public float Opacity { get; set; }


        public BleachByPass(Game game, float opacity) : base(game) { Opacity = opacity; }


        public override void Draw(GameTime gameTime)
        {
            if (effect == null)
            {
                effect = Game.Content.Load<Effect>("Shaders/PostProcessing/BleachByPass");
            }

            effect.Parameters["Opacity"].SetValue(Opacity);

            // Set Params.
            base.Draw(gameTime);
        }
    }
}
