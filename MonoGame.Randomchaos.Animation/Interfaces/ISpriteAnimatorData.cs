using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D.Interfaces
{
    public interface ISpriteAnimatorData
    {
        string Name { get; set; }
        string SpriteSheetAsset { get; set; }
        Point CellSize { get; set; }
        Vector2 CellsXY { get; set; }

        Dictionary<string, SpritAnimatorDataClip> Clips { get; set; }
    }
}
