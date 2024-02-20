using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Animation.Animation2D.Interfaces
{
    public interface ISpriteSheetKeyFrame
    {
        public Vector2 Cell { get; set; }
        public TimeSpan Time { get; set; }
    }
}
