using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Animation.Animation2D.Delegates;
using MonoGame.Randomchaos.Animation.Animation2D.Interfaces;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D
{
    public class SpriteSheetAnimationPlayer : ISpriteSheetAnimationPlayer
    {
        public string CurrentAnimation { get { return currentClip.Name; } }
        public TimeSpan AnimationOffSet { get; set; }

        protected bool _IsPlaying = false;
        public bool IsPlaying { get { return _IsPlaying; } }

        public Vector2 CurrentCell { get; set; }

        public int CurrentKeyframe { get; set; }

        public event AnimationStopped OnAnimationStopped;

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

        protected ISpriteSheetAnimationClip currentClip;
        public ISpriteSheetAnimationClip CurrentClip
        {
            get { return currentClip; }
        }


        /// <summary>
        /// Gets the current play position.
        /// </summary>
        protected TimeSpan currentTime;
        public TimeSpan CurrentTime
        {
            get { return currentTime; }
        }
        public Dictionary<string, ISpriteSheetAnimationClip> Clips { get; set; }

        public SpriteSheetAnimationPlayer(Dictionary<string, ISpriteSheetAnimationClip> clips = null, TimeSpan animationOffSet = new TimeSpan())
        {
            AnimationOffSet = animationOffSet;
            Clips = clips;
        }

        public void StartClip(string name, int frame = 0)
        {
            StartClip(Clips[name]);
        }

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

        public void StopClip()
        {
            if (currentClip != null && IsPlaying)
            {
                _IsPlaying = false;

                if (OnAnimationStopped != null)
                    OnAnimationStopped(currentClip);
            }
        }

        public void Update(TimeSpan time)
        {
            if (currentClip != null)
                GetCurrentCell(time);
        }

        public void Update(float lerp)
        {
            if (currentClip != null)
                GetCurrentCell(lerp);
        }

        protected void GetCurrentCell(float lerp)
        {
            CurrentKeyframe = (int)MathHelper.Lerp(0, currentClip.Keyframes.Count - 1, lerp);
            CurrentCell = currentClip.Keyframes[CurrentKeyframe].Cell;
        }

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
