
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Animation.Animation3D.Delegates;
using MonoGame.Randomchaos.Animation.Interfaces;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation3D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A key frame animation player. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class KeyFrameAnimationPlayer : IKeyframeAnimationPlayer
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the set the animation off belongs to. </summary>
        ///
        /// <value> The animation off set. </value>
        ///-------------------------------------------------------------------------------------------------

        public TimeSpan AnimationOffSet { get; set; }
        /// <summary>   True if is playing, false if not. </summary>
        protected bool _IsPlaying = false;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object is playing. </summary>
        ///
        /// <value> True if this object is playing, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsPlaying { get { return _IsPlaying; } }

        public bool IsPaused { get; set; }

        /// <summary>   Event queue for all listeners interested in animationClipEnd events. </summary>
        public event OnAnimationClipEnd AnimationClipEndEvent;

        #region Fields


        /// <summary>   Information about the currently playing animation clip. </summary>
        protected IKeyframeAnimationClip prevClipValue;
        /// <summary>   The previous time value. </summary>
        TimeSpan prevTimeValue;
        /// <summary>   The previous frame. </summary>
        int prevFrame;
        /// <summary>   The blend speed. </summary>
        float blendSpeed = .001f;
        /// <summary>   The blend position. </summary>
        float blendPosition = 0;

        /// <summary>   The current clip value. </summary>
        protected IKeyframeAnimationClip currentClipValue;
        /// <summary>   The current time value. </summary>
        TimeSpan currentTimeValue;
        /// <summary>   The current keyframe. </summary>
        int currentKeyframe;


        /// <summary>   Current animation transform matrices. </summary>
        Matrix[] boneTransforms;
        /// <summary>   The world transforms. </summary>
        Matrix[] worldTransforms;
        /// <summary>   The skin transforms. </summary>
        Matrix[] skinTransforms;


        /// <summary>   The skinning data value. </summary>
        ISkinnedData _skinningDataValue;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Back link to the bind pose and skeleton hierarchy data. </summary>
        ///
        /// <value> The skinning data value. </value>
        ///-------------------------------------------------------------------------------------------------

        public ISkinnedData SkinningDataValue
        {
            get { return _skinningDataValue; }
            set
            {
                _skinningDataValue = value;

                if (_skinningDataValue != null)
                {
                    boneTransforms = new Matrix[_skinningDataValue.BindPose.Count];
                    worldTransforms = new Matrix[_skinningDataValue.BindPose.Count];
                    skinTransforms = new Matrix[_skinningDataValue.BindPose.Count];
                }
            }
        }


        #endregion

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructs a new animation player. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="skinningData">     (Optional) Information describing the skinning. </param>
        /// <param name="animationOffSet">  (Optional) Set the animation off belongs to. </param>
        ///-------------------------------------------------------------------------------------------------

        public KeyFrameAnimationPlayer(ISkinnedData skinningData = null, TimeSpan animationOffSet = new TimeSpan())
        {
            AnimationOffSet = animationOffSet;
            SkinningDataValue = skinningData;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts decoding the specified animation clip. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="clip">         The clip. </param>
        /// <param name="blendSpeed">   (Optional) The blend speed. </param>
        ///-------------------------------------------------------------------------------------------------

        public void StartClip(IKeyframeAnimationClip clip, float blendSpeed = .001f)
        {
            if (clip != null)
            {
                if (currentClipValue != null)
                {
                    prevClipValue = currentClipValue;
                    prevTimeValue = currentTimeValue;
                    prevFrame = currentKeyframe;
                    this.blendSpeed = blendSpeed;
                    blendPosition = 0;

                    AnimationOffSet = currentTimeValue;
                }

                currentClipValue = clip;
                currentTimeValue = TimeSpan.Zero + AnimationOffSet;
                currentKeyframe = 0;

                if (prevClipValue == null)
                {
                    // Initialize bone transforms to the bind pose.
                    SkinningDataValue.BindPose.CopyTo(boneTransforms, 0);
                }

                _IsPlaying = true;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Stops a clip. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void StopClip()
        {
            if (currentClipValue != null && IsPlaying)
            {
                AnimationOffSet = currentTimeValue;
                _IsPlaying = false;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Advances the current animation position. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="time">                     The time. </param>
        /// <param name="relativeToCurrentTime">    True to relative to current time. </param>
        /// <param name="rootTransform">            The root transform. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Update(TimeSpan time, bool relativeToCurrentTime,
                           Matrix rootTransform)
        {
            if (IsPlaying && !IsPaused)
                UpdateBoneTransforms(time, relativeToCurrentTime);

            UpdateWorldTransforms(rootTransform);
            UpdateSkinTransforms();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Helper used by the Update method to refresh the BoneTransforms data. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        ///
        /// <param name="time">                     The time. </param>
        /// <param name="relativeToCurrentTime">    True to relative to current time. </param>
        ///-------------------------------------------------------------------------------------------------

        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
            if (currentClipValue == null)
                throw new InvalidOperationException(
                            "AnimationPlayer.Update was called before StartClip");

            Matrix[] preBones = new Matrix[boneTransforms.Length];
            Matrix[] curBones = new Matrix[boneTransforms.Length];

            TimeSpan elT = time;

            curBones = GetClipBoneTransform(currentClipValue, elT, ref currentTimeValue, ref currentKeyframe, relativeToCurrentTime);

            if (prevClipValue != null)
            {
                preBones = GetClipBoneTransform(prevClipValue, elT, ref prevTimeValue, ref prevFrame, relativeToCurrentTime);

                for (int b = 0; b < boneTransforms.Length; b++)
                {
                    boneTransforms[b] = Matrix.Lerp(preBones[b], curBones[b], blendPosition);
                    blendPosition = Math.Min(1, blendPosition + blendSpeed);
                }
            }
            else
                curBones.CopyTo(boneTransforms, 0);


        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets clip bone transform. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        ///
        /// <param name="clipValue">                The clip value. </param>
        /// <param name="time">                     The time. </param>
        /// <param name="timeValue">                [in,out] The time value. </param>
        /// <param name="keyFrame">                 [in,out] The key frame. </param>
        /// <param name="relativeToCurrentTime">    True to relative to current time. </param>
        ///
        /// <returns>   An array of matrix. </returns>
        ///-------------------------------------------------------------------------------------------------

        Matrix[] GetClipBoneTransform(IKeyframeAnimationClip clipValue, TimeSpan time, ref TimeSpan timeValue, ref int keyFrame, bool relativeToCurrentTime)
        {
            Matrix[] returnValue = new Matrix[boneTransforms.Length];
            boneTransforms.CopyTo(returnValue, 0);

            // Update the animation position.
            if (relativeToCurrentTime)
            {
                time += timeValue;

                // If we reached the end, loop back to the start.
                while (time >= clipValue.Duration)
                    time -= clipValue.Duration;
            }

            if ((time < TimeSpan.Zero) || (time >= clipValue.Duration))
                throw new ArgumentOutOfRangeException("time");

            // If the position moved backwards, reset the keyframe index.
            if (time < timeValue)
            {
                if (clipValue.Looped)
                    keyFrame = 0;
                else
                {
                    keyFrame = clipValue.Keyframes.Count - 1;

                    if (AnimationClipEndEvent != null && clipValue == currentClipValue)
                        AnimationClipEndEvent(clipValue);
                }
                //skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
            }

            timeValue = time;

            // Read keyframe matrices.
            IList<IKeyframe> keyframes = clipValue.Keyframes;

            while (currentKeyframe < keyframes.Count)
            {
                IKeyframe keyframe = keyframes[keyFrame];

                // Stop when we've read up to the current time position.
                if (keyframe.Time > timeValue)
                    break;

                // Use this key frame.
                returnValue[keyframe.Bone] = keyframe.Transform;

                keyFrame++;
            }

            return returnValue;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Helper used by the Update method to refresh the WorldTransforms data. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="rootTransform">    The root transform. </param>
        ///-------------------------------------------------------------------------------------------------

        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // Root bone.
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // Child bones.
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = SkinningDataValue.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Helper used by the Update method to refresh the SkinTransforms data. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = SkinningDataValue.InverseBindPose[bone] *
                                            worldTransforms[bone];
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current bone transform matrices, relative to their parent bones. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <returns>   An array of matrix. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current bone transform matrices, in absolute format. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <returns>   An array of matrix. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Matrix[] GetWorldTransforms()
        {
            return worldTransforms;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the current bone transform matrices, relative to the skinning bind pose.
        /// </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <returns>   An array of matrix. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Matrix[] GetSkinTransforms()
        {
            return skinTransforms;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the clip currently being decoded. </summary>
        ///
        /// <value> The current clip. </value>
        ///-------------------------------------------------------------------------------------------------

        public IKeyframeAnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the last animation clip. </summary>
        ///
        /// <value> The last animation clip. </value>
        ///-------------------------------------------------------------------------------------------------

        public IKeyframeAnimationClip LastAnimationClip
        {
            get { return prevClipValue; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current play position. </summary>
        ///
        /// <value> The current time. </value>
        ///-------------------------------------------------------------------------------------------------

        public TimeSpan CurrentTime
        {
            get { return currentTimeValue; }
        }
    }
}
