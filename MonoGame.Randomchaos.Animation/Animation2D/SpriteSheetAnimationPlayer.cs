
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Animation.Animation2D.Delegates;
using MonoGame.Randomchaos.Animation.Animation2D.Interfaces;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A sprite sheet animation player. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SpriteSheetAnimationPlayer : ISpriteSheetAnimationPlayer
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current animation. </summary>
        ///
        /// <value> The current animation. </value>
        ///-------------------------------------------------------------------------------------------------

        public string CurrentAnimation { get { return currentClip.Name; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the set the animation off belongs to. </summary>
        ///
        /// <value> The animation off set. </value>
        ///-------------------------------------------------------------------------------------------------

        public TimeSpan AnimationOffSet { get; set; }

        /// <summary>   True if is playing, false if not. </summary>
        protected bool _IsPlaying = false;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object is playing. </summary>
        ///
        /// <value> True if this object is playing, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsPlaying { get { return _IsPlaying; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the current cell. </summary>
        ///
        /// <value> The current cell. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 CurrentCell { get; set; }

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

        public float ClipLerpValue
        {
            get
            {
                if (currentClip != null)
                    return (float)CurrentKeyframe / currentClip.Keyframes.Count;
                else
                    return 0;
            }
        }

        /// <summary>   The current clip. </summary>
        protected ISpriteSheetAnimationClip currentClip;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current clip. </summary>
        ///
        /// <value> The current clip. </value>
        ///-------------------------------------------------------------------------------------------------

        public ISpriteSheetAnimationClip CurrentClip
        {
            get { return currentClip; }
        }


        /// <summary>   Gets the current play position. </summary>
        protected TimeSpan currentTime;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current time. </summary>
        ///
        /// <value> The current time. </value>
        ///-------------------------------------------------------------------------------------------------

        public TimeSpan CurrentTime
        {
            get { return currentTime; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the clips. </summary>
        ///
        /// <value> The clips. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<string, ISpriteSheetAnimationClip> Clips { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="clips">            (Optional) The clips. </param>
        /// <param name="animationOffSet">  (Optional) Set the animation off belongs to. </param>
        ///-------------------------------------------------------------------------------------------------

        public SpriteSheetAnimationPlayer(Dictionary<string, ISpriteSheetAnimationClip> clips = null, TimeSpan animationOffSet = new TimeSpan())
        {
            AnimationOffSet = animationOffSet;
            Clips = clips;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts a clip. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="name">     The name. </param>
        /// <param name="frame">    (Optional) The frame. </param>
        ///-------------------------------------------------------------------------------------------------

        public void StartClip(string name, int frame = 0)
        {
            StartClip(Clips[name]);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts a clip. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="clip">     The clip. </param>
        /// <param name="frame">    (Optional) The frame. </param>
        ///-------------------------------------------------------------------------------------------------

        public void StartClip(ISpriteSheetAnimationClip clip, int frame = 0)
        {
            if (clip != null && clip != currentClip)
            {
                currentTime = TimeSpan.Zero + AnimationOffSet;
                CurrentKeyframe = frame;

                currentClip = clip;

                _IsPlaying = true;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Stops a clip. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void StopClip()
        {
            if (currentClip != null && IsPlaying)
            {
                _IsPlaying = false;

                if (OnAnimationStopped != null)
                    OnAnimationStopped(currentClip);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given lerp. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="time"> The time. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Update(TimeSpan time)
        {
            if (currentClip != null)
                GetCurrentCell(time);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given lerp. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="lerp"> The linearly interpolate. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Update(float lerp)
        {
            if (currentClip != null)
                GetCurrentCell(lerp);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets current cell. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="lerp"> The linearly interpolate. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void GetCurrentCell(float lerp)
        {
            CurrentKeyframe = (int)MathHelper.Lerp(0, currentClip.Keyframes.Count - 1, lerp);
            CurrentCell = currentClip.Keyframes[CurrentKeyframe].Cell;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets current cell. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        ///
        /// <param name="time"> The time. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void GetCurrentCell(TimeSpan time)
        {
            time += currentTime;

            // If we reached the end, loop back to the start.
            while (time >= currentClip.Duration)
                time -= currentClip.Duration;

            if ((time < TimeSpan.Zero) || (time >= currentClip.Duration))
            {
                throw new ArgumentOutOfRangeException("time");
            }

            if (time < currentTime)
            {
                if (currentClip.Looped)
                    CurrentKeyframe = 0;
                else
                {
                    CurrentKeyframe = currentClip.Keyframes.Count - 1;
                    StopClip();
                }
            }

            currentTime = time;

            // Read key frame matrices.
            IList<ISpriteSheetKeyFrame> keyframes = currentClip.Keyframes;

            while (CurrentKeyframe < keyframes.Count)
            {
                ISpriteSheetKeyFrame keyframe = keyframes[CurrentKeyframe];

                // Stop when we've read up to the current time position.
                if (keyframe.Time > currentTime)
                    break;

                // Use this key frame.
                CurrentCell = keyframe.Cell;

                CurrentKeyframe++;
            }
        }

    }
}
