using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Interfaces.Interfaces
{
    public interface IHitInfo
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the contact point. </summary>
        ///
        /// <value> The contact point. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector3 ContactPoint { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the distance. </summary>
        ///
        /// <value> The distance. </value>
        ///-------------------------------------------------------------------------------------------------

        float Distance { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the contact object. </summary>
        ///
        /// <value> The contact object. </value>
        ///-------------------------------------------------------------------------------------------------

        object ContactObject { get; set; }
    }
}
