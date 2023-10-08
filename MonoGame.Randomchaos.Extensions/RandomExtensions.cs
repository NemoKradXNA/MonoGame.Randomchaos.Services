
using System;

namespace MonoGame.Randomchaos.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A random extensions. </summary>
    ///
    /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class RandomExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Random extension method that next float. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="rnd">  The rnd to act on. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static float NextFloat(this Random rnd)
        {
            return (float)rnd.NextDouble();
        }
    }
}
