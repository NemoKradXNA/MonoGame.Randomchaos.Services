using MonoGame.Randomchaos.Animation.Animation2D.Interfaces;
using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Animation.Animation2D
{
    public class SpriteSheetKeyFrame : ISpriteSheetKeyFrame
    {
        public Vector2 Cell { get; set; }
        public TimeSpan Time { get; set; }

        public SpriteSheetKeyFrame() { }
        public SpriteSheetKeyFrame(Vector2 cell, TimeSpan time)
        {
            Cell = cell;
            Time = time;
        }
    }
}
