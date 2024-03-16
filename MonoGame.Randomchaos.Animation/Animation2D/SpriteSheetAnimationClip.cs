
using MonoGame.Randomchaos.Animation.Animation2D.Interfaces;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A sprite sheet animation clip. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SpriteSheetAnimationClip : ISpriteSheetAnimationClip
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public SpriteSheetAnimationClip() { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="name">         The name. </param>
        /// <param name="duration">     The duration. </param>
        /// <param name="keyframes">    The keyframes. </param>
        /// <param name="looped">       (Optional) True if looped. </param>
        ///-------------------------------------------------------------------------------------------------

        public SpriteSheetAnimationClip(string name, TimeSpan duration, List<ISpriteSheetKeyFrame> keyframes, bool looped = true)
        {
            Name = name;
            Duration = duration;
            Keyframes = keyframes;
            Looped = looped;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copy constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="clip"> The clip. </param>
        ///-------------------------------------------------------------------------------------------------

        public SpriteSheetAnimationClip(SpriteSheetAnimationClip clip)
        {
            Name = clip.Name;
            Duration = clip.Duration;

            SpriteSheetKeyFrame[] frames = new SpriteSheetKeyFrame[clip.Keyframes.Count];
            clip.Keyframes.CopyTo(frames, 0);

            Keyframes = new List<ISpriteSheetKeyFrame>();
            Keyframes.AddRange(frames);

            Looped = clip.Looped;
        }
    }
}
