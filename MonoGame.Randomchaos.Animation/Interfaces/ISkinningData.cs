
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for skinning data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ISkinningData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the animation clips. </summary>
        ///
        /// <value> The animation clips. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<string, IKeyframeAnimationClip> AnimationClips { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the bind pose. </summary>
        ///
        /// <value> The bind pose. </value>
        ///-------------------------------------------------------------------------------------------------

        List<Matrix> BindPose { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the inverse bind pose. </summary>
        ///
        /// <value> The inverse bind pose. </value>
        ///-------------------------------------------------------------------------------------------------

        List<Matrix> InverseBindPose { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the skeleton hierarchy. </summary>
        ///
        /// <value> The skeleton hierarchy. </value>
        ///-------------------------------------------------------------------------------------------------

        List<int> SkeletonHierarchy { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds the clips. </summary>
        ///
        /// <param name="clips">    The clips. </param>
        ///-------------------------------------------------------------------------------------------------

        void AddClips(Dictionary<string, IKeyframeAnimationClip> clips);
    }
}
