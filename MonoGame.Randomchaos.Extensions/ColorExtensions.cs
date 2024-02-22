using Microsoft.Xna.Framework;


///-------------------------------------------------------------------------------------------------
// namespace: MonoGame.Randomchaos.Extensions
//
// summary:	.
///-------------------------------------------------------------------------------------------------

namespace MonoGame.Randomchaos.Extensions
{
    public static class ColorExtensions
    {
        public static Color Multiply(this Color otherColor)
        {
            return new Color(Color.Thistle.ToVector4() * otherColor.ToVector4());
        }
    }
}
