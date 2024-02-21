
using MonoGame.Randomchaos.Animation.Animation2D.Interfaces;
using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Animation.Animation2D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A sprite sheet key frame. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SpriteSheetKeyFrame : ISpriteSheetKeyFrame
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public SpriteSheetKeyFrame() { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="cell"> The cell. </param>
        /// <param name="time"> The time. </param>
        ///-------------------------------------------------------------------------------------------------

        public SpriteSheetKeyFrame(Vector2 cell, TimeSpan time)
        {
            Cell = cell;
            Time = time;
        }
    }
}
