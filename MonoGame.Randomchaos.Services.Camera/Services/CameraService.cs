
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Services.Interfaces;

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
                if(aspectRatio == -1)
                    aspectRatio =  Game.GraphicsDevice.PresentationParameters.BackBufferWidth / (float)Game.GraphicsDevice.PresentationParameters.BackBufferHeight;

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
    }
}
