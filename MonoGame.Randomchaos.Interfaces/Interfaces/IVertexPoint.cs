
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Interfaces.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for vertex point. </summary>
    ///
    /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IVertexPoint
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the position. </summary>
        ///
        /// <value> The position. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector3 Position { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the normal. </summary>
        ///
        /// <value> The normal. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector3 Normal { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Convert this object into a string representation. </summary>
        ///
        /// <param name="transform">    The transform. </param>
        ///
        /// <returns>   A string that represents this object. </returns>
        ///-------------------------------------------------------------------------------------------------

        string ToString(Matrix transform);
    }
}
