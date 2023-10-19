
using Microsoft.Xna.Framework;
using System;
using System.Runtime.CompilerServices;

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a new Vector3 from the given string. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/10/2023. </remarks>
        ///
        /// <param name="vector3">  The third vector. </param>
        ///
        /// <returns>   A Vector3. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Vector3 FromString(this Vector3 v3, string vector3)
        {
            string[] xyz = vector3.Split(",");

            return new Vector3(float.Parse(xyz[0]),float.Parse(xyz[1]), float.Parse(xyz[2]));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Vector3 extension method that  from vector 2. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/10/2023. </remarks>
        ///
        /// <param name="v3">   The v3 to act on. </param>
        /// <param name="v2">   The second value. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void FromVector2(this Vector3 v3, Vector2 v2)
        {
            v3 = new Vector3(v2.X, v2.Y, v3.Z);
        }
    }
}
