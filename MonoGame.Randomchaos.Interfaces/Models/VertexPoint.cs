using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces.Interfaces;

namespace MonoGame.Randomchaos.Interfaces.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A vertex point. </summary>
    ///
    /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class VertexPoint : IVertexPoint
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the position. </summary>
        ///
        /// <value> The position. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 Position { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the normal. </summary>
        ///
        /// <value> The normal. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 Normal { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="position"> The position. </param>
        /// <param name="norm">     The normalize. </param>
        ///-------------------------------------------------------------------------------------------------

        public VertexPoint(Vector3 position, Vector3 norm)
        {
            Position = position;
            Normal = norm;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override string ToString()
        {
            return $"{Position} - {Normal}";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Convert this object into a string representation. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="transform">    The transform. </param>
        ///
        /// <returns>   A string that represents this object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public string ToString(Matrix transform)
        {
            return $"{Vector3.Transform(Position, transform)}";
        }
    }
}
