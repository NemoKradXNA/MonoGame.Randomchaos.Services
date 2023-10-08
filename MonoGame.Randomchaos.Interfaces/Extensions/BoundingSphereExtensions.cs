
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Interfaces.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A bounding sphere extensions. </summary>
    ///
    /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class BoundingSphereExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A BoundingSphere extension method that transformed bounding sphere. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="sphereToTransform">    The sphereToTransform to act on. </param>
        /// <param name="transformToUse">       The transform to use. </param>
        ///
        /// <returns>   A BoundingSphere. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static BoundingSphere TransformedBoundingSphere(this BoundingSphere sphereToTransform, ITransform transformToUse)
        {
            Vector3 c = Vector3.Transform(sphereToTransform.Center, transformToUse.World);

            return new BoundingSphere(c, sphereToTransform.Radius);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A BoundingSphere extension method that transformed bounding sphere a. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="sphereToTRansfork">    The sphereToTRansfork to act on. </param>
        /// <param name="transformToUse">       The transform to use. </param>
        ///
        /// <returns>   A BoundingSphere. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static BoundingSphere TransformedBoundingSphereAA(this BoundingSphere sphereToTRansfork, ITransform transformToUse)
        {
            Vector3 c = sphereToTRansfork.Center + transformToUse.Position;

            return new BoundingSphere(c, sphereToTRansfork.Radius);
        }
    }
}
