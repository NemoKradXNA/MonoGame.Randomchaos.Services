
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces;

namespace MonoGame.Randomchaos.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A bounding box extensions. </summary>
    ///
    /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class BoundingBoxExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A BoundingBox extension method that transformed. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="boxToTRansfork">   The boxToTRansfork to act on. </param>
        /// <param name="transformToUse">   The transform to use. </param>
        ///
        /// <returns>   A BoundingBox. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static BoundingBox Transformed(this BoundingBox boxToTRansfork, ITransform transformToUse)
        {
            Vector3 min = Vector3.Transform(boxToTRansfork.Min, transformToUse.World);
            Vector3 max = Vector3.Transform(boxToTRansfork.Max, transformToUse.World);

            return new BoundingBox(min, max);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A BoundingBox extension method that transformed a. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="boxToTRansfork">   The boxToTRansfork to act on. </param>
        /// <param name="transformToUse">   The transform to use. </param>
        ///
        /// <returns>   A BoundingBox. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static BoundingBox TransformedAA(this BoundingBox boxToTRansfork, ITransform transformToUse)
        {
            Matrix AAWorld = Matrix.CreateScale(transformToUse.Scale) * Matrix.CreateTranslation(transformToUse.Position);

            Vector3 min = Vector3.Transform(boxToTRansfork.Min, AAWorld);
            Vector3 max = Vector3.Transform(boxToTRansfork.Max, AAWorld);

            return new BoundingBox(min, max);
        }
    }
}
