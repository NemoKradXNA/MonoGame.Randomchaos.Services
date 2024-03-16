
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for sprite animator. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ISpriteAnimator
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current animation. </summary>
        ///
        /// <value> The current animation. </value>
        ///-------------------------------------------------------------------------------------------------

        string CurrentAnimation { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sprte sheet asset. </summary>
        ///
        /// <value> The sprte sheet asset. </value>
        ///-------------------------------------------------------------------------------------------------

        string SprteSheetAsset { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sprite sheet texture. </summary>
        ///
        /// <value> The sprite sheet texture. </value>
        ///-------------------------------------------------------------------------------------------------

        Texture2D SpriteSheetTexture { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current cell rectangle. </summary>
        ///
        /// <value> The current cell rectangle. </value>
        ///-------------------------------------------------------------------------------------------------

        Rectangle CurrentCellRect { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the clips. </summary>
        ///
        /// <value> The clips. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<string, ISpriteSheetAnimationClip> Clips { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts an animation. </summary>
        ///
        /// <param name="animation">    The animation. </param>
        ///-------------------------------------------------------------------------------------------------

        void StartAnimation(string animation);
        /// <summary>   Stops an animation. </summary>
        void StopAnimation();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads animation data. </summary>
        ///
        /// <param name="data"> The data. </param>
        ///-------------------------------------------------------------------------------------------------

        void LoadAnimationData(SpriteAnimatorData data);
    }
}
