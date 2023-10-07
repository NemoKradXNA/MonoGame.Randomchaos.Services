using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Randomchaos.Extensions
{
    public static class Texture2DExtension
    {
        public static Texture2D WhitePixel(GraphicsDevice grapicsDevice)
        {
            Texture2D texture = new Texture2D(grapicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });

            return texture;
        }
    }
}
