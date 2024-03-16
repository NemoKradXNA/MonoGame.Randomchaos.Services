
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Animation.Animation2D.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A sprite animator data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SpriteAnimatorData : ISpriteAnimatorData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sprite sheet asset. </summary>
        ///
        /// <value> The sprite sheet asset. </value>
        ///-------------------------------------------------------------------------------------------------

        public string SpriteSheetAsset { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the cell size. </summary>
        ///
        /// <value> The size of the cell. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point CellSize { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the cells xy. </summary>
        ///
        /// <value> The cells xy. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 CellsXY { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the clips. </summary>
        ///
        /// <value> The clips. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<string, SpritAnimatorDataClip> Clips { get; set; }
    }
}
