
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Animation.Animation2D.Delegates;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for sprite sheet animation player. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ISpriteSheetAnimationPlayer
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current animation. </summary>
        ///
        /// <value> The current animation. </value>
        ///-------------------------------------------------------------------------------------------------

        public string CurrentAnimation { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the set the animation off belongs to. </summary>
        ///
        /// <value> The animation off set. </value>
        ///-------------------------------------------------------------------------------------------------

        public TimeSpan AnimationOffSet { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object is playing. </summary>
        ///
        /// <value> True if this object is playing, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsPlaying { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the current cell. </summary>
        ///
        /// <value> The current cell. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector2 CurrentCell { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the current keyframe. </summary>
        ///
        /// <value> The current keyframe. </value>
        ///-------------------------------------------------------------------------------------------------

        public int CurrentKeyframe { get; set; }

        /// <summary>   Event queue for all listeners interested in OnAnimationStopped events. </summary>
        public event AnimationStopped OnAnimationStopped;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the clip linearly interpolate value. </summary>
        ///
        /// <value> The clip linearly interpolate value. </value>
        ///-------------------------------------------------------------------------------------------------

        public float ClipLerpValue { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current clip. </summary>
        ///
        /// <value> The current clip. </value>
        ///-------------------------------------------------------------------------------------------------

        ISpriteSheetAnimationClip CurrentClip { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current time. </summary>
        ///
        /// <value> The current time. </value>
        ///-------------------------------------------------------------------------------------------------

        TimeSpan CurrentTime { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the clips. </summary>
        ///
        /// <value> The clips. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<string, ISpriteSheetAnimationClip> Clips { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts a clip. </summary>
        ///
        /// <param name="name">     The name. </param>
        /// <param name="frame">    (Optional) The frame. </param>
        ///-------------------------------------------------------------------------------------------------

        void StartClip(string name, int frame = 0);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts a clip. </summary>
        ///
        /// <param name="clip">     The clip. </param>
        /// <param name="frame">    (Optional) The frame. </param>
        ///-------------------------------------------------------------------------------------------------

        void StartClip(ISpriteSheetAnimationClip clip, int frame = 0);
        /// <summary>   Stops a clip. </summary>
        void StopClip();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given lerp. </summary>
        ///
        /// <param name="time"> The time. </param>
        ///-------------------------------------------------------------------------------------------------

        void Update(TimeSpan time);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given lerp. </summary>
        ///
        /// <param name="lerp"> The linearly interpolate. </param>
        ///-------------------------------------------------------------------------------------------------

        void Update(float lerp);
    }
}
