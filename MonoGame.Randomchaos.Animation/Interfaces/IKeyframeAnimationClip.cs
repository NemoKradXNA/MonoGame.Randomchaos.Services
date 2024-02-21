
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for keyframe animation clip. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IKeyframeAnimationClip
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the looped. </summary>
        ///
        /// <value> True if looped, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool Looped { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the duration. </summary>
        ///
        /// <value> The duration. </value>
        ///-------------------------------------------------------------------------------------------------

        TimeSpan Duration { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the keyframes. </summary>
        ///
        /// <value> The keyframes. </value>
        ///-------------------------------------------------------------------------------------------------

        List<IKeyframe> Keyframes { get; }
    }
}
