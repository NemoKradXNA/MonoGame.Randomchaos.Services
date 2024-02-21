
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for sprite animator data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ISpriteAnimatorData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sprite sheet asset. </summary>
        ///
        /// <value> The sprite sheet asset. </value>
        ///-------------------------------------------------------------------------------------------------

        string SpriteSheetAsset { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the cell size. </summary>
        ///
        /// <value> The size of the cell. </value>
        ///-------------------------------------------------------------------------------------------------

        Point CellSize { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the cells xy. </summary>
        ///
        /// <value> The cells xy. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector2 CellsXY { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the clips. </summary>
        ///
        /// <value> The clips. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<string, SpritAnimatorDataClip> Clips { get; set; }
    }
}
