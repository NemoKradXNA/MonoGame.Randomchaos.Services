
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for noise service. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface INoiseService
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Noises the given coordinate. </summary>
        ///
        /// <param name="x">    The x coordinate. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        float Noise(float x);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Noises the given coordinate. </summary>
        ///
        /// <param name="x">    The x coordinate. </param>
        /// <param name="y">    The y coordinate. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        float Noise(float x, float y);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Noises the given coordinate. </summary>
        ///
        /// <param name="coord">    The coordinate. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        float Noise(Vector2 coord);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Noises the given coordinate. </summary>
        ///
        /// <param name="x">    The x coordinate. </param>
        /// <param name="y">    The y coordinate. </param>
        /// <param name="z">    The z coordinate. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        float Noise(float x, float y, float z);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Noises the given coordinate. </summary>
        ///
        /// <param name="coord">    The coordinate. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        float Noise(Vector3 coord);
    }
}
