
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Interfaces.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Physics;
using System.Runtime;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for camera service. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ICameraService
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the transform. </summary>
        ///
        /// <value> The transform. </value>
        ///-------------------------------------------------------------------------------------------------

        ITransform Transform { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the aspect ratio. </summary>
        ///
        /// <value> The aspect ratio. </value>
        ///-------------------------------------------------------------------------------------------------

        float AspectRatio { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the frustum. </summary>
        ///
        /// <value> The frustum. </value>
        ///-------------------------------------------------------------------------------------------------

        BoundingFrustum Frustum { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the viewport. </summary>
        ///
        /// <value> The viewport. </value>
        ///-------------------------------------------------------------------------------------------------

        Viewport Viewport { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the view. </summary>
        ///
        /// <value> The view. </value>
        ///-------------------------------------------------------------------------------------------------

        Matrix View { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the projection. </summary>
        ///
        /// <value> The projection. </value>
        ///-------------------------------------------------------------------------------------------------

        Matrix Projection { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the field of view. </summary>
        ///
        /// <value> The field of view. </value>
        ///-------------------------------------------------------------------------------------------------

        float FieldOfView { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the near clip plane. </summary>
        ///
        /// <value> The near clip plane. </value>
        ///-------------------------------------------------------------------------------------------------

        float NearClipPlane { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the far clip plane. </summary>
        ///
        /// <value> The far clip plane. </value>
        ///-------------------------------------------------------------------------------------------------

        float FarClipPlane { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the clear. </summary>
        ///
        /// <value> The color of the clear. </value>
        ///-------------------------------------------------------------------------------------------------

        Color ClearColor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets a value indicating whether the wire frame should be rendered.
        /// </summary>
        ///
        /// <value> True if render wire frame, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool RenderWireFrame { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ray from camera. </summary>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        ///
        /// <returns>   A Ray. </returns>
        ///-------------------------------------------------------------------------------------------------

        Ray RayFromCamera(Point screenPixel);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ray picking. </summary>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        /// <param name="volume">       The volume. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        float RayPicking(Point screenPixel, BoundingBox volume);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ray picking. </summary>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        /// <param name="volume">       The volume. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        float RayPicking(Point screenPixel, BoundingSphere volume);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ray picking. </summary>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        /// <param name="volume">       The volume. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        float RayPicking(Point screenPixel, Plane volume);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ray picking. </summary>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        /// <param name="volume">       The volume. </param>
        /// <param name="hitInfo">      [out] Information describing the hit. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        float RayPicking(Point screenPixel, BoundingBox volume, out IHitInfo hitInfo);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ray picking. </summary>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        /// <param name="volume">       The volume. </param>
        /// <param name="hitInfo">      [out] Information describing the hit. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        float RayPicking(Point screenPixel, BoundingSphere volume, out IHitInfo hitInfo);
    }
}
