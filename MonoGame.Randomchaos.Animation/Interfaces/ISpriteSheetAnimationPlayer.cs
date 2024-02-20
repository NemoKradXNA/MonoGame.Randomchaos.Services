using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Animation.Animation2D.Delegates;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D.Interfaces
{
    public interface ISpriteSheetAnimationPlayer
    {
        public string CurrentAnimation { get; }
        public TimeSpan AnimationOffSet { get; set; }
        public bool IsPlaying { get; }
        Vector2 CurrentCell { get; set; }
        public int CurrentKeyframe { get; set; }

        public event AnimationStopped OnAnimationStopped;
        public float ClipLerpValue { get; }

        ISpriteSheetAnimationClip CurrentClip { get; }
        TimeSpan CurrentTime { get; }
        Dictionary<string, ISpriteSheetAnimationClip> Clips { get; set; }

        void StartClip(string name, int frame = 0);
        void StartClip(ISpriteSheetAnimationClip clip, int frame = 0);
        void StopClip();

        void Update(TimeSpan time);
        void Update(float lerp);
    }
}
