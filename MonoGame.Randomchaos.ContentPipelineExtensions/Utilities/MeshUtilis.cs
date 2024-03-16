
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using MonoGame.Randomchaos.Animation.Animation3D;
using MonoGame.Randomchaos.Animation.Interfaces;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.ContentPipelineExtensions.Utilities
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A mesh utilis. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class MeshUtilis
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Comparison function for sorting keyframes into ascending time order. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="a">    An IKeyframe to process. </param>
        /// <param name="b">    An IKeyframe to process. </param>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static int CompareKeyframeTimes(IKeyframe a, IKeyframe b)
        {
            return a.Time.CompareTo(b.Time);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts an intermediate format content pipeline AnimationContentDictionary object to our
        /// runtime AnimationClip format.
        /// </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <exception cref="InvalidContentException">
        ///     Thrown when an Invalid Content error condition occurs.
        /// </exception>
        ///
        /// <param name="animations">   The animations. </param>
        /// <param name="bones">        The bones. </param>
        /// <param name="name">         (Optional) The name. </param>
        ///
        /// <returns>   A Dictionary&lt;string,IKeyframeAnimationClip&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Dictionary<string, IKeyframeAnimationClip> ProcessAnimations(AnimationContentDictionary animations, IList<BoneContent> bones, string name = null)
        {
            // Build up a table mapping bone names to indices.
            Dictionary<string, int> boneMap = new Dictionary<string, int>();

            for (int i = 0; i < bones.Count; i++)
            {
                string boneName = bones[i].Name;

                if (!string.IsNullOrEmpty(boneName))
                    boneMap.Add(boneName, i);
            }

            // Convert each animation in turn.
            Dictionary<string, IKeyframeAnimationClip> animationClips = new Dictionary<string, IKeyframeAnimationClip>();

            int a = 0;
            foreach (KeyValuePair<string, AnimationContent> animation in animations)
            {
                IKeyframeAnimationClip processed = ProcessAnimation(animation.Value, boneMap);

                if (name == null)
                    name = animation.Key;
                else
                    name += $"{a++}";

                processed.Name = name;

                animationClips.Add(name, processed);
            }

            if (animationClips.Count == 0)
            {
                throw new InvalidContentException("Input file does not contain any animations.");
            }

            return animationClips;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts an intermediate format content pipeline AnimationContent object to our runtime
        /// AnimationClip format.
        /// </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <exception cref="InvalidContentException">
        ///     Thrown when an Invalid Content error condition occurs.
        /// </exception>
        ///
        /// <param name="animation">    The animation. </param>
        /// <param name="boneMap">      The bone map. </param>
        ///
        /// <returns>   An IKeyframeAnimationClip. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static IKeyframeAnimationClip ProcessAnimation(AnimationContent animation, Dictionary<string, int> boneMap)
        {
            List<IKeyframe> keyframes = new List<IKeyframe>();

            // For each input animation channel.
            foreach (KeyValuePair<string, AnimationChannel> channel in animation.Channels)
            {
                // Look up what bone this channel is controlling.
                int boneIndex;

                if (!boneMap.TryGetValue(channel.Key, out boneIndex))
                {
                    throw new InvalidContentException(string.Format(
                        "Found animation for bone '{0}', " +
                        "which is not part of the skeleton.", channel.Key));
                }

                // Convert the keyframe data.
                foreach (AnimationKeyframe keyframe in channel.Value)
                {
                    keyframes.Add(new Keyframe(boneIndex, keyframe.Time, keyframe.Transform));
                }
            }

            // Sort the merged keyframes by time.
            keyframes.Sort(CompareKeyframeTimes);

            if (keyframes.Count == 0)
                throw new InvalidContentException("Animation has no keyframes.");

            if (animation.Duration <= TimeSpan.Zero)
                throw new InvalidContentException("Animation has a zero duration.");

            return new KeyFrameAnimationClip(animation.Duration, keyframes);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Bakes unwanted transforms into the model geometry, so everything ends up in the same
        /// coordinate system.
        /// </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="node">     The node. </param>
        /// <param name="skeleton"> The skeleton. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void FlattenTransforms(NodeContent node, BoneContent skeleton)
        {
            foreach (NodeContent child in node.Children)
            {
                // Don't process the skeleton, because that is special.
                if (child == skeleton)
                    continue;

                // Bake the local transform into the actual geometry.
                MeshHelper.TransformScene(child, child.Transform);

                // Having baked it, we can now set the local
                // coordinate system back to identity.
                child.Transform = Matrix.Identity;

                // Recurse.
                FlattenTransforms(child, skeleton);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Checks whether a mesh contains skininng information. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="mesh"> The mesh. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static bool MeshHasSkinning(MeshContent mesh)
        {
            foreach (GeometryContent geometry in mesh.Geometry)
            {
                if (!geometry.Vertices.Channels.Contains(VertexChannelNames.Weights()))
                    return false;
            }

            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Makes sure this mesh contains the kind of data we know how to animate. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="node">             The node. </param>
        /// <param name="context">          The context. </param>
        /// <param name="parentBoneName">   Name of the parent bone. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void ValidateMesh(NodeContent node, ContentProcessorContext context, string parentBoneName)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                // Validate the mesh.
                if (parentBoneName != null)
                {
                    context.Logger.LogWarning(null, null,
                        "Mesh {0} is a child of bone {1}. SkinnedModelProcessor " +
                        "does not correctly handle meshes that are children of bones.",
                        mesh.Name, parentBoneName);
                }

                if (!MeshHasSkinning(mesh))
                {
                    context.Logger.LogWarning(null, null,
                        "Mesh {0} has no skinning information, so it has been deleted.",
                        mesh.Name);

                    mesh.Parent.Children.Remove(mesh);
                    return;
                }
            }
            else if (node is BoneContent)
            {
                // If this is a bone, remember that we are now looking inside it.
                parentBoneName = node.Name;
            }

            // Recurse (iterating over a copy of the child collection,
            // because validating children may delete some of them).
            foreach (NodeContent child in new List<NodeContent>(node.Children))
                ValidateMesh(child, context, parentBoneName);
        }
    }
}
