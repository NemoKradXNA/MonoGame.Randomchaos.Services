
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Animation.Animation3D;
using MonoGame.Randomchaos.Animation.Interfaces;
using MonoGame.Randomchaos.ContentPipelineExtensions.Utilities;
using System.Collections.Generic;
using System.ComponentModel;

namespace MonoGame.Randomchaos.ContentPipelineExtensions.Processors
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A skinned animation processor. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [ContentProcessor(DisplayName = "MonoGame.Randomchaos - Skinned Animation Processor")]
    public class SkinnedAnimationProcessor : ContentProcessor<NodeContent, ISkinnedData>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   If a skinned mesh, this will be the name of the animation clip. </summary>
        ///
        /// <value> The name of the animation clip. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue("Animation")]
        [Description("If a skinned mesh, this will be the name of the animation clip"), TypeConverter(typeof(BooleanConverter))]
        [Category("Void Engine")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DisplayName("Animation Clip Name")]
        public string AnimationClipName { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Set to true if you want logging on. </summary>
        ///
        /// <value> True if enable logging, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue(false)]
        [Description("Set to true if you want logging on"), TypeConverter(typeof(BooleanConverter))]
        [Category("Void Engine")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DisplayName("Enable Logging")]
        public bool EnableLogging { get; set; } = false;

        [DefaultValue(1)]
        public virtual float Scale { get { return _modelProcessor.Scale; } set { _modelProcessor.Scale = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the rotation z coordinate. </summary>
        ///
        /// <value> The rotation z coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual float RotationZ { get { return _modelProcessor.RotationZ; } set { _modelProcessor.RotationZ = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the rotation y coordinate. </summary>
        ///
        /// <value> The rotation y coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual float RotationY { get { return _modelProcessor.RotationY; } set { _modelProcessor.RotationY = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the rotation x coordinate. </summary>
        ///
        /// <value> The rotation x coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual float RotationX { get { return _modelProcessor.RotationX; } set { _modelProcessor.RotationX = value; } }

        public virtual bool SwapWindingOrder { get { return _modelProcessor.SwapWindingOrder; } set { _modelProcessor.SwapWindingOrder = value; } }

        /// <summary>   The model processor. </summary>
        protected ModelProcessor _modelProcessor;

        public SkinnedAnimationProcessor() : base()
        {
            _modelProcessor = new ModelProcessor();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Process this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/02/2024. </remarks>
        ///
        /// <exception cref="InvalidContentException">
        ///     Thrown when an Invalid Content error condition occurs.
        /// </exception>
        ///
        /// <param name="input">    The input. </param>
        /// <param name="context">  The context. </param>
        ///
        /// <returns>   An ISkinnedData. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override ISkinnedData Process(NodeContent input, ContentProcessorContext context)
        {
            string skinnedAnimationFileName = input.Identity.SourceFilename.Substring(input.Identity.SourceFilename.LastIndexOf("\\") + 1);

            if (EnableLogging)
            {
                Logger.LogName = string.Format("{0}.log", skinnedAnimationFileName.Substring(skinnedAnimationFileName.LastIndexOf("/") + 1));
                Logger.WriteToLog(string.Format("Process started for {0}", skinnedAnimationFileName));
            }

            ModelContent baseModel = _modelProcessor.Process(input, context);

            if (EnableLogging)
                Logger.WriteToLog("Get Animation Data");

            MeshUtilis.ValidateMesh(input, context, null);

            // Find the skeleton.
            BoneContent skeleton = MeshHelper.FindSkeleton(input);

            if (skeleton == null)
                throw new InvalidContentException("Input skeleton not found.");

            // We don't want to have to worry about different parts of the model being
            // in different local coordinate systems, so let's just bake everything.
            MeshUtilis.FlattenTransforms(input, skeleton);

            // Read the bind pose and skeleton hierarchy data.
            IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);

            if (EnableLogging)
                Logger.WriteToLog($"Found {bones.Count} bones..");

            if (bones.Count > SkinnedEffect.MaxBones)
            {
                throw new InvalidContentException(string.Format(
                    "Skeleton has {0} bones, but the maximum supported is {1}.",
                    bones.Count, SkinnedEffect.MaxBones));
            }

            List<Matrix> bindPose = new List<Matrix>();
            List<Matrix> inverseBindPose = new List<Matrix>();
            List<int> skeletonHierarchy = new List<int>();

            foreach (BoneContent bone in bones)
            {
                bindPose.Add(bone.Transform);
                inverseBindPose.Add(Matrix.Invert(bone.AbsoluteTransform));
                skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
            }

            if (EnableLogging)
                Logger.WriteToLog($"Found {skeleton.Animations.Count} animation(s)..");

            // Convert animation data to our runtime format.
            Dictionary<string, IKeyframeAnimationClip> animationClips = animationClips = MeshUtilis.ProcessAnimations(skeleton.Animations, bones, AnimationClipName);

            if (EnableLogging)
            {
                foreach (string key in animationClips.Keys)
                    Logger.WriteToLog($"Animation {key} : {animationClips[key].Duration}");
            }

            ISkinnedData skinningData = new SkinnedData(animationClips, bindPose, inverseBindPose, skeletonHierarchy);

            return skinningData;
        }
    }
}
