using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Randomchaos.Animation.Animation2D.Interfaces
{
    public interface ISpritAnimatorDataClip
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Looped { get; set; }
    }
}
