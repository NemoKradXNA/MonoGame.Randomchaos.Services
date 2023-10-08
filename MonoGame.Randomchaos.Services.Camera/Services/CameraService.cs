
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Interfaces.Interfaces;
using MonoGame.Randomchaos.Interfaces.Models;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using System;

namespace MonoGame.Randomchaos.Services.Camera
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A service for accessing cameras information. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class CameraService : ServiceBase<ICameraService>, ICameraService
    {
        /// <summary>   The aspect ratio. </summary>
        protected float aspectRatio = -1;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the aspect ratio. </summary>
        ///
        /// <value> The aspect ratio. </value>
        ///-------------------------------------------------------------------------------------------------

        public float AspectRatio
        {
            get
            {
                if (aspectRatio == -1)
                    aspectRatio = Game.GraphicsDevice.PresentationParameters.BackBufferWidth / (float)Game.GraphicsDevice.PresentationParameters.BackBufferHeight;

                return aspectRatio;
            }

            set
            {
                aspectRatio = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the transform. </summary>
        ///
        /// <value> The transform. </value>
        ///-------------------------------------------------------------------------------------------------

        public ITransform Transform { get; set; }


        /// <summary>   The view. </summary>
        protected Matrix view;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the view. </summary>
        ///
        /// <value> The view. </value>
        ///-------------------------------------------------------------------------------------------------

        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }

        /// <summary>   The projection. </summary>
        protected Matrix projection;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the projection. </summary>
        ///
        /// <value> The projection. </value>
        ///-------------------------------------------------------------------------------------------------

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        /// <summary>   The viewport. </summary>
        protected Viewport viewport;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the viewport. </summary>
        ///
        /// <value> The viewport. </value>
        ///-------------------------------------------------------------------------------------------------

        public Viewport Viewport
        {
            get { return viewport; }
            set { viewport = value; }
        }

        /// <summary>   The frustum. </summary>
        protected BoundingFrustum _Frustum;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the frustum. </summary>
        ///
        /// <value> The frustum. </value>
        ///-------------------------------------------------------------------------------------------------

        public BoundingFrustum Frustum
        {
            get
            {
                return _Frustum;
            }
            set
            {
                _Frustum = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the field of view. </summary>
        ///
        /// <value> The field of view. </value>
        ///-------------------------------------------------------------------------------------------------

        public float FieldOfView { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the near clip plane. </summary>
        ///
        /// <value> The near clip plane. </value>
        ///-------------------------------------------------------------------------------------------------

        public float NearClipPlane { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the far clip plane. </summary>
        ///
        /// <value> The far clip plane. </value>
        ///-------------------------------------------------------------------------------------------------

        public float FarClipPlane { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the clear. </summary>
        ///
        /// <value> The color of the clear. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color ClearColor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets a value indicating whether the wire frame should be rendered.
        /// </summary>
        ///
        /// <value> True if render wire frame, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool RenderWireFrame { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">     The game. </param>
        /// <param name="minDepth"> The minimum depth. </param>
        /// <param name="maxDepth"> The maximum depth. </param>
        ///-------------------------------------------------------------------------------------------------

        public CameraService(Game game, float minDepth, float maxDepth) : base(game)
        {
            ClearColor = Color.CornflowerBlue;

            Transform = new Transform(null);

            FieldOfView = MathHelper.PiOver4;
            NearClipPlane = minDepth;
            FarClipPlane = maxDepth;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            viewport = Game.GraphicsDevice.Viewport;

            viewport.MinDepth = NearClipPlane;
            viewport.MaxDepth = FarClipPlane;

            base.Initialize();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            viewport.MinDepth = NearClipPlane;
            viewport.MaxDepth = FarClipPlane;

            projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Viewport.MinDepth, Viewport.MaxDepth);

            _Frustum = new BoundingFrustum(Matrix.CreateTranslation(Transform.Position) * View * Projection);

            view = Matrix.Invert(Transform.World);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ray from camera. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        ///
        /// <returns>   A Ray. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Ray RayFromCamera(Point screenPixel)
        {
            Viewport viewPort = new Viewport(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight);

            viewPort.MinDepth = Viewport.MinDepth;
            viewPort.MaxDepth = Viewport.MaxDepth;

            Vector3 nearSource = viewPort.Unproject(new Vector3(screenPixel.X, screenPixel.Y, Viewport.MinDepth), Projection, View, Matrix.Identity);
            Vector3 farSource = viewPort.Unproject(new Vector3(screenPixel.X, screenPixel.Y, Viewport.MaxDepth), Projection, View, Matrix.Identity);
            Vector3 direction = farSource - nearSource;

            direction.Normalize();

            return new Ray(nearSource, direction);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ray picking. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        /// <param name="volume">       The volume. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        public float RayPicking(Point screenPixel, BoundingBox volume)
        {
            float? retVal = float.MaxValue;

            Ray ray = RayFromCamera(screenPixel);

            ray.Intersects(ref volume, out retVal);

            if (retVal != null)
                return retVal.Value;
            else
                return float.MaxValue;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ray picking. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        /// <param name="volume">       The volume. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        public float RayPicking(Point screenPixel, BoundingSphere volume)
        {
            float? retVal = float.MaxValue;

            Ray ray = RayFromCamera(screenPixel);

            ray.Intersects(ref volume, out retVal);

            if (retVal != null)
                return retVal.Value;
            else
                return float.MaxValue;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ray picking. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        /// <param name="volume">       The volume. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        public float RayPicking(Point screenPixel, Plane volume)
        {
            float? retVal = float.MaxValue;

            Ray ray = RayFromCamera(screenPixel);

            ray.Intersects(ref volume, out retVal);

            if (retVal != null)
                return retVal.Value;
            else
                return float.MaxValue;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ray picking. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        /// <param name="volume">       The volume. </param>
        /// <param name="hitInfo">      [out] Information describing the hit. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        public float RayPicking(Point screenPixel, BoundingBox volume, out IHitInfo hitInfo)
        {
            float? retVal = float.MaxValue;

            Ray ray = RayFromCamera(screenPixel);

            ray.Intersects(ref volume, out retVal);

            if (retVal != null)
            {
                Vector3 p = ray.Position + (ray.Direction * retVal.Value);
                hitInfo = new HitInfo(p, retVal.Value);
                return retVal.Value;
            }
            else
            {
                hitInfo = new HitInfo();
                return float.MaxValue;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ray picking. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        /// <param name="volume">       The volume. </param>
        /// <param name="hitInfo">      [out] Information describing the hit. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        public float RayPicking(Point screenPixel, BoundingSphere volume, out IHitInfo hitInfo)
        {
            float? retVal = float.MaxValue;

            Ray ray = RayFromCamera(screenPixel);

            ray.Intersects(ref volume, out retVal);

            if (retVal != null)
            {
                Vector3 p = ray.Position + (ray.Direction * retVal.Value);
                hitInfo = new HitInfo(p, retVal.Value);
                return retVal.Value;
            }
            else
            {
                hitInfo = new HitInfo();
                return float.MaxValue;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   World position to screen position. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="worldPosition">    The world position. </param>
        ///
        /// <returns>   A Vector2. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 WorldPositionToScreenPosition(Vector3 worldPosition)
        {
            Matrix ViewProjectionMatrix = View * Projection;

            Vector4 result4 = Vector4.Transform(worldPosition, ViewProjectionMatrix);

            if (result4.W <= 0)
                return new Vector2(Viewport.Width, 0);

            Vector3 result = new Vector3(result4.X / result4.W, result4.Y / result4.W, result4.Z / result4.W);

            Vector2 retVal = new Vector2((int)Math.Round(+result.X * (Viewport.Width / 2)) + (Viewport.Width / 2), (int)Math.Round(-result.Y * (Viewport.Height / 2)) + (Viewport.Height / 2));
            return retVal;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   World position to screen text coordinates. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="worldPosition">    The world position. </param>
        ///
        /// <returns>   A Vector2. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 WorldPositionToScreenTextCoords(Vector3 worldPosition)
        {
            Vector2 retVal;

            retVal = WorldPositionToScreenPosition(worldPosition);

            retVal.X /= Viewport.Width;
            retVal.Y /= Viewport.Height;

            return retVal;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Screen pixel to view pixel. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="screenPixel">  The screen pixel. </param>
        ///
        /// <returns>   A Vector3. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 ScreenPixelToViewPixel(Point screenPixel)
        {
            Vector3 nearSource = Viewport.Unproject(new Vector3(screenPixel.X, screenPixel.Y, Viewport.MinDepth), Projection, View, Matrix.Identity);

            return nearSource;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets camera screen position. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="screenPoint">  The screen point. </param>
        ///
        /// <returns>   The camera screen position. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 GetCameraScreenPosition(Point screenPoint)
        {
            Vector3 worldPos = ScreenPixelToViewPixel(screenPoint);
            Vector2 retVal = WorldPositionToScreenPosition(worldPos);

            return retVal;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets screen pixel size. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <returns>   The screen pixel size. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 GetScreenPixelSize()
        {
            return new Vector2(1f / (float)Game.GraphicsDevice.Viewport.Width,
                                     1f / (float)Game.GraphicsDevice.Viewport.Height);
        }
    }
}
