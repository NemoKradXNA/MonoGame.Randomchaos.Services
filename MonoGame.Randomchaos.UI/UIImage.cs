
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.UI.BaseClasses;

namespace MonoGame.Randomchaos.UI
{
    public class UIImage : UIBase
    {
        public Texture2D Texture { get; set; }
        public Texture2D Backgeound { get; set; }
        public UIImage(Game game, Point position, Point size) : base(game, position, size)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            Color tint = Tint;

            if (!Enabled)
            {
                tint = GreyScaleColor(Tint);
            }

            // Draw BG
            if (Backgeound != null)
                _spriteBatch.Draw(Backgeound, Rectangle, tint);

            if (Texture != null)
                _spriteBatch.Draw(Texture, Rectangle, tint);

            _spriteBatch.End();
        }
    }
}
