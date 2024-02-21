
using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Randomchaos.Animation.Animation2D.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for sprit animator data clip. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ISpritAnimatorDataClip
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the start. </summary>
        ///
        /// <value> The start. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 Start { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the end. </summary>
        ///
        /// <value> The end. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 End { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the duration. </summary>
        ///
        /// <value> The duration. </value>
        ///-------------------------------------------------------------------------------------------------

        public TimeSpan Duration { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the looped. </summary>
        ///
        /// <value> True if looped, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool Looped { get; set; }
    }
}
