
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
    }
}
