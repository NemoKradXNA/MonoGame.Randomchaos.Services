
using Microsoft.Xna.Framework.Content;
using MonoGame.Randomchaos.Animation.Interfaces;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation3D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    /// An animation clip is the runtime equivalent of the
    /// Microsoft.Xna.Framework.Content.Pipeline.Graphics.AnimationContent type. It holds all the
    /// keyframes needed to describe a single animation.
    /// </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class KeyFrameAnimationClip : IKeyframeAnimationClip

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
        /// <summary>   Constructs a new animation clip object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="duration">     The duration. </param>
        /// <param name="keyframes">    The keyframes. </param>
        /// <param name="looped">       (Optional) True if looped. </param>
        ///-------------------------------------------------------------------------------------------------

        public KeyFrameAnimationClip(TimeSpan duration, List<IKeyframe> keyframes, bool looped = true)
        {
            Duration = duration;
            Keyframes = keyframes;
            Looped = looped;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Private constructor for use by the XNB deserializer. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        private KeyFrameAnimationClip()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Private constructor for use by the XNB deserializer. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="clip"> The clip. </param>
        ///-------------------------------------------------------------------------------------------------

        public KeyFrameAnimationClip(IKeyframeAnimationClip clip)
        {
            Name = clip.Name;
            Duration = clip.Duration;

            IKeyframe[] frames = new IKeyframe[clip.Keyframes.Count];
            clip.Keyframes.CopyTo(frames, 0);

            Keyframes = new List<IKeyframe>();
            Keyframes.AddRange(frames);

            Looped = clip.Looped;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the total length of the animation. </summary>
        ///
        /// <value> The duration. </value>
        ///-------------------------------------------------------------------------------------------------

        [ContentSerializer]
        public TimeSpan Duration { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets a combined list containing all the keyframes for all bones, sorted by time.
        /// </summary>
        ///
        /// <value> The keyframes. </value>
        ///-------------------------------------------------------------------------------------------------

        [ContentSerializer]
        public List<IKeyframe> Keyframes { get; private set; }
    }
}
