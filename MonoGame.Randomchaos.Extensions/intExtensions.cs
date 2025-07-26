
namespace MonoGame.Randomchaos.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   An int extensions. </summary>
    ///
    /// <remarks>   Charles Humphrey, 26/07/2025. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class intExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   An int extension method that inverse linearly interpolate. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/07/2025. </remarks>
        ///
        /// <param name="value">    The value to act on. </param>
        /// <param name="a">        An int to process. </param>
        /// <param name="b">        An int to process. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static float InverseLerp(this int value, float a, float b)
        {
            if (a == b)
            {
                return 0f; // Avoid division by zero
            }

            return (float)(value - a) / (b - a);
        }
    }
}
