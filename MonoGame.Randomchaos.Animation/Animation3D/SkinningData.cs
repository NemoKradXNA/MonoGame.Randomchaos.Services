
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Randomchaos.Animation.Interfaces;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation3D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    /// Combines all the data needed to render and animate a skinned object. This is typically stored
    /// in the Tag property of the Model being animated.
    /// </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SkinningData : ISkinningData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructs a new skinning data object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="animationClips">       The animation clips. </param>
        /// <param name="bindPose">             The bind pose. </param>
        /// <param name="inverseBindPose">      The inverse bind pose. </param>
        /// <param name="skeletonHierarchy">    The skeleton hierarchy. </param>
        ///-------------------------------------------------------------------------------------------------

        public SkinningData(Dictionary<string, IKeyframeAnimationClip> animationClips,
                            List<Matrix> bindPose, List<Matrix> inverseBindPose,
                            List<int> skeletonHierarchy)
        {
            AnimationClips = animationClips;
            BindPose = bindPose;
            InverseBindPose = inverseBindPose;
            SkeletonHierarchy = skeletonHierarchy;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Private constructor for use by the XNB deserializer. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public SkinningData()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets a collection of animation clips. These are stored by name in a dictionary, so
        /// there could for instance be clips for "Walk", "Run", "JumpReallyHigh", etc.
        /// </summary>
        ///
        /// <value> The animation clips. </value>
        ///-------------------------------------------------------------------------------------------------

        [ContentSerializer]
        public Dictionary<string, IKeyframeAnimationClip> AnimationClips { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Bindpose matrices for each bone in the skeleton, relative to the parent bone.
        /// </summary>
        ///
        /// <value> The bind pose. </value>
        ///-------------------------------------------------------------------------------------------------

        [ContentSerializer]
        public List<Matrix> BindPose { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Vertex to bonespace transforms for each bone in the skeleton. </summary>
        ///
        /// <value> The inverse bind pose. </value>
        ///-------------------------------------------------------------------------------------------------

        [ContentSerializer]
        public List<Matrix> InverseBindPose { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   For each bone in the skeleton, stores the index of the parent bone. </summary>
        ///
        /// <value> The skeleton hierarchy. </value>
        ///-------------------------------------------------------------------------------------------------

        [ContentSerializer]
        public List<int> SkeletonHierarchy { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a clips. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="clips">    The clips. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddClips(Dictionary<string, IKeyframeAnimationClip> clips)
        {
            foreach (string clipName in clips.Keys)
            {
                if (!AnimationClips.ContainsKey(clipName))
                {
                    IKeyframeAnimationClip clip = new KeyFrameAnimationClip(clips[clipName]);
                    AnimationClips.Add(clipName, clip);
                }
            }
        }

    }
}
