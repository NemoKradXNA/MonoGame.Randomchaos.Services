using System;
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Animation.Animation2D.Interfaces;

namespace MonoGame.Randomchaos.Animation.Animation2D
{
    public class SpritAnimatorDataClip : ISpritAnimatorDataClip
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Looped { get; set; }
    }
}
