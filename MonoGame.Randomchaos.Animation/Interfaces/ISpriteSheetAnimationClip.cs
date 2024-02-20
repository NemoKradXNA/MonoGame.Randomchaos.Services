using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D.Interfaces
{
    public interface ISpriteSheetAnimationClip
    {
        public string Name { get; set; }
        public bool Looped { get; set; }
        public TimeSpan Duration { get; set; }

        public List<ISpriteSheetKeyFrame> Keyframes { get; set; }
    }
}
