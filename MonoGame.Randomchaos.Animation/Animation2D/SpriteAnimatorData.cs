using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Animation.Animation2D.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D
{
    public class SpriteAnimatorData : ISpriteAnimatorData
    {
        public string Name { get; set; }
        public string SpriteSheetAsset { get; set; }
        public Point CellSize { get; set; }
        public Vector2 CellsXY { get; set; }

        public Dictionary<string, SpritAnimatorDataClip> Clips { get; set; }
    }
}
