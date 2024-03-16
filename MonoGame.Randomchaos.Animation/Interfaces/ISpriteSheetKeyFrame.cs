
using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Animation.Animation2D.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for sprite sheet key frame. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ISpriteSheetKeyFrame
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the cell. </summary>
        ///
        /// <value> The cell. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 Cell { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the time. </summary>
        ///
        /// <value> The time. </value>
        ///-------------------------------------------------------------------------------------------------

        public TimeSpan Time { get; set; }
    }
}
