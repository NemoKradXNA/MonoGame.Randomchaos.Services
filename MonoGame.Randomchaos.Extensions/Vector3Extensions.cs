
using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Randomchaos.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A vector 3 extensions. </summary>
    ///
    /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class Vector3Extensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Vector3 extension method that angle to. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="from">     from to act on. </param>
        /// <param name="location"> The location. </param>
        ///
        /// <returns>   A Vector3. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Vector3 AngleTo(this Vector3 from, Vector3 location)
        {
            Vector3 angle = new Vector3();
            Vector3 v3 = Vector3.Normalize(location - from);

            angle.X = (float)Math.Asin(v3.Y);
            angle.Y = (float)Math.Atan2((double)-v3.X, (double)-v3.Z);

            return angle;
        }
    }
}
