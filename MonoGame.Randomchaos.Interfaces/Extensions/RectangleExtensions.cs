
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Interfaces.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A rectangle extensions. </summary>
    ///
    /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class RectangleExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Rectangle extension method that transform rectangle. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="rect">             The rect to act on. </param>
        /// <param name="transformToUse">   The transform to use. </param>
        ///
        /// <returns>   A Rectangle. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Rectangle TransformRectangle(this Rectangle rect, ITransform transformToUse)
        {
            Vector2 pos = Vector2.Transform(new Vector2(rect.X, rect.Y), transformToUse.World);

            Rectangle newRect = new Rectangle((int)pos.X, (int)pos.Y, rect.Width, rect.Height);

            return newRect;
        }
    }
}
