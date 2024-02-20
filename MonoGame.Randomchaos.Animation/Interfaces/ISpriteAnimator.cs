using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D.Interfaces
{
    public interface ISpriteAnimator
    {
        string Name { get; set; }
        string CurrentAnimation { get; }
        string SprteSheetAsset { get; set; }
        Texture2D SpriteSheetTexture { get; set; }
        Rectangle CurrentCellRect { get; }
        Dictionary<string, ISpriteSheetAnimationClip> Clips { get; set; }

        void StartAnimation(string animation);
        void StopAnimation();
        void LoadAnimationData(SpriteAnimatorData data);
    }
}
