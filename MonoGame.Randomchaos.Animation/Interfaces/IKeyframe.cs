﻿
using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Randomchaos.Animation.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for keyframe. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IKeyframe
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the bone. </summary>
        ///
        /// <value> The bone. </value>
        ///-------------------------------------------------------------------------------------------------

        int Bone { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the time. </summary>
        ///
        /// <value> The time. </value>
        ///-------------------------------------------------------------------------------------------------

        TimeSpan Time { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the transform. </summary>
        ///
        /// <value> The transform. </value>
        ///-------------------------------------------------------------------------------------------------

        Matrix Transform { get; }
    }
}
