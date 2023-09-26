
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces.Models;

namespace MonoGame.Randomchaos.Interfaces.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for triangle. </summary>
    ///
    /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ITriangle
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the point 1. </summary>
        ///
        /// <value> The point 1. </value>
        ///-------------------------------------------------------------------------------------------------

        VertexPoint Point1 { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the point 2. </summary>
        ///
        /// <value> The point 2. </value>
        ///-------------------------------------------------------------------------------------------------

        VertexPoint Point2 { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the point 3. </summary>
        ///
        /// <value> The point 3. </value>
        ///-------------------------------------------------------------------------------------------------

        VertexPoint Point3 { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the center. </summary>
        ///
        /// <value> The center. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector3 Center { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the normal. </summary>
        ///
        /// <value> The normal. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector3 Normal { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the plane. </summary>
        ///
        /// <value> The plane. </value>
        ///-------------------------------------------------------------------------------------------------

        Plane Plane { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Convert this object into a string representation. </summary>
        ///
        /// <param name="transform">    The transform. </param>
        ///
        /// <returns>   A string that represents this object. </returns>
        ///-------------------------------------------------------------------------------------------------

        string ToString(Matrix transform);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Contans point. </summary>
        ///
        /// <param name="point">        The point. </param>
        /// <param name="transform">    (Optional) The transform. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        bool ContansPoint(Vector3 point, Matrix? transform = null);
    }
}
