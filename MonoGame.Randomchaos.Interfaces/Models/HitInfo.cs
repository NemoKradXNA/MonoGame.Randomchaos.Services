using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces.Interfaces;

namespace MonoGame.Randomchaos.Interfaces.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Information about the hit. </summary>
    ///
    /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class HitInfo : IHitInfo
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the contact point. </summary>
        ///
        /// <value> The contact point. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 ContactPoint { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the distance. </summary>
        ///
        /// <value> The distance. </value>
        ///-------------------------------------------------------------------------------------------------

        public float Distance { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the contact object. </summary>
        ///
        /// <value> The contact object. </value>
        ///-------------------------------------------------------------------------------------------------

        public object ContactObject { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public HitInfo() { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="point">    The point. </param>
        /// <param name="distance"> The distance. </param>
        ///-------------------------------------------------------------------------------------------------

        public HitInfo(Vector3 point, float distance)
        {
            ContactPoint = point;
            Distance = distance;
        }
    }
}
