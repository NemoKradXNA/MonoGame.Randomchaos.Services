
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for sprite sheet animation clip. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ISpriteSheetAnimationClip
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the looped. </summary>
        ///
        /// <value> True if looped, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool Looped { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the duration. </summary>
        ///
        /// <value> The duration. </value>
        ///-------------------------------------------------------------------------------------------------

        public TimeSpan Duration { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the keyframes. </summary>
        ///
        /// <value> The keyframes. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<ISpriteSheetKeyFrame> Keyframes { get; set; }
    }
}
