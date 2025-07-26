
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A float extensions. </summary>
    ///
    /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class floatExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A float extension method that wrap angle. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="radians">  The radians to act on. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static float WrapAngle(this float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Inverse linearly interpolate. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/07/2025. </remarks>
        ///
        /// <param name="a">        A float to process. </param>
        /// <param name="b">        A float to process. </param>
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static float InverseLerp(this float value, float a, float b)
        {
            if (a == b)
            {
                return 0f; // Avoid division by zero
            }

            return (value - a) / (b - a);
        }
    }
}
