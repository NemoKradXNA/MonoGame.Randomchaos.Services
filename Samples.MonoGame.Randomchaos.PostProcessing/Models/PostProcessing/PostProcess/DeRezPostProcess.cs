using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.PostProcessing.Models;


namespace Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcess
{
    public class DeRezPostProcess : BasePostProcess
    {
        public int NumberOfTiles { get; set; } = 512;

        public DeRezPostProcess(Game game, int numberofTiles) : base(game)
        {
            NumberOfTiles = numberofTiles;
        }

        public override void Draw(GameTime gameTime)
        {
            if (effect == null)
                effect = Game.Content.Load<Effect>("Shaders/PostProcessing/DeRezed");

            // Set Params.
            effect.Parameters["numberOfTiles"].SetValue(NumberOfTiles);

            base.Draw(gameTime);

        }
    }
}
