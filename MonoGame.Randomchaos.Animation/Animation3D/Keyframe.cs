
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Randomchaos.Animation.Interfaces;
using System;

namespace MonoGame.Randomchaos.Animation.Animation3D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A keyframe. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Keyframe : IKeyframe
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructs a new keyframe object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="bone">         The bone. </param>
        /// <param name="time">         The time. </param>
        /// <param name="transform">    The transform. </param>
        ///-------------------------------------------------------------------------------------------------

        public Keyframe(int bone, TimeSpan time, Matrix transform)
        {
            Bone = bone;
            Time = time;
            Transform = transform;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Private constructor for use by the XNB deserializer. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        private Keyframe()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the index of the target bone that is animated by this keyframe.
        /// </summary>
        ///
        /// <value> The bone. </value>
        ///-------------------------------------------------------------------------------------------------

        [ContentSerializer]
        public int Bone { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the time offset from the start of the animation to this keyframe.
        /// </summary>
        ///
        /// <value> The time. </value>
        ///-------------------------------------------------------------------------------------------------

        [ContentSerializer]
        public TimeSpan Time { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the bone transform for this keyframe. </summary>
        ///
        /// <value> The transform. </value>
        ///-------------------------------------------------------------------------------------------------

        [ContentSerializer]
        public Matrix Transform { get; private set; }
    }
}
