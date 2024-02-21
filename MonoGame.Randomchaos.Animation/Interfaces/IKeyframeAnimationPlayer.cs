
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Animation.Animation3D.Delegates;
using System;

namespace MonoGame.Randomchaos.Animation.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for keyframe animation player. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IKeyframeAnimationPlayer
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the set the animation off belongs to. </summary>
        ///
        /// <value> The animation off set. </value>
        ///-------------------------------------------------------------------------------------------------

        TimeSpan AnimationOffSet { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the skinning data value. </summary>
        ///
        /// <value> The skinning data value. </value>
        ///-------------------------------------------------------------------------------------------------

        ISkinningData SkinningDataValue { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current clip. </summary>
        ///
        /// <value> The current clip. </value>
        ///-------------------------------------------------------------------------------------------------

        IKeyframeAnimationClip CurrentClip { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the last animation clip. </summary>
        ///
        /// <value> The last animation clip. </value>
        ///-------------------------------------------------------------------------------------------------

        IKeyframeAnimationClip LastAnimationClip { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current time. </summary>
        ///
        /// <value> The current time. </value>
        ///-------------------------------------------------------------------------------------------------

        TimeSpan CurrentTime { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object is playing. </summary>
        ///
        /// <value> True if this object is playing, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool IsPlaying { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts a clip. </summary>
        ///
        /// <param name="clip">         The clip. </param>
        /// <param name="blendSpeed">   (Optional) The blend speed. </param>
        ///-------------------------------------------------------------------------------------------------

        void StartClip(IKeyframeAnimationClip clip, float blendSpeed = .001f);
        /// <summary>   Stops a clip. </summary>
        void StopClip();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates this object. </summary>
        ///
        /// <param name="time">                     The time. </param>
        /// <param name="relativeToCurrentTime">    True to relative to current time. </param>
        /// <param name="rootTransform">            The root transform. </param>
        ///-------------------------------------------------------------------------------------------------

        void Update(TimeSpan time, bool relativeToCurrentTime, Matrix rootTransform);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the bone transforms. </summary>
        ///
        /// <param name="time">                     The time. </param>
        /// <param name="relativeToCurrentTime">    True to relative to current time. </param>
        ///-------------------------------------------------------------------------------------------------

        void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the world transforms described by rootTransform. </summary>
        ///
        /// <param name="rootTransform">    The root transform. </param>
        ///-------------------------------------------------------------------------------------------------

        void UpdateWorldTransforms(Matrix rootTransform);
        /// <summary>   Updates the skin transforms. </summary>
        void UpdateSkinTransforms();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets bone transforms. </summary>
        ///
        /// <returns>   An array of matrix. </returns>
        ///-------------------------------------------------------------------------------------------------

        Matrix[] GetBoneTransforms();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets world transforms. </summary>
        ///
        /// <returns>   An array of matrix. </returns>
        ///-------------------------------------------------------------------------------------------------

        Matrix[] GetWorldTransforms();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets skin transforms. </summary>
        ///
        /// <returns>   An array of matrix. </returns>
        ///-------------------------------------------------------------------------------------------------

        Matrix[] GetSkinTransforms();

        /// <summary>   Event queue for all listeners interested in animationClipEnd events. </summary>
        event OnAnimationClipEnd AnimationClipEndEvent;
    }
}
