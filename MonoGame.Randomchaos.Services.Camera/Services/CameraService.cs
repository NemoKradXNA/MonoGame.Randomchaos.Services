using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Randomchaos.Services.Camera
{
    public class CameraService : ServiceBase<CameraService>, ICameraService
    {
        public float AspectRatio
        {
            get
            {
                return Game.GraphicsDevice.PresentationParameters.BackBufferWidth / (float)Game.GraphicsDevice.PresentationParameters.BackBufferHeight;
            }
        }


        public ITransform Transform { get; set; }


        protected Matrix view;

        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }

        protected Matrix projection;

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        protected Viewport viewport;
        public Viewport Viewport
        {
            get { return viewport; }
            set { viewport = value; }
        }

        protected BoundingFrustum _Frustum;

        public BoundingFrustum Frustum
        {
            get
            {
                return _Frustum;
            }
        }

        public float FieldOfView { get; set; }

        public float NearClipPlane { get; set; }

        public float FarClipPlane { get; set; }

        public Color ClearColor { get; set; }
        public bool RenderWireFrame { get; set; }

        public CameraService(Game game, float minDepth, float maxDepth) : base(game)
        {
            ClearColor = Color.CornflowerBlue;

            Transform = new Transform(null);

            FieldOfView = MathHelper.PiOver4;
            NearClipPlane = minDepth;
            FarClipPlane = maxDepth;
        }

        public override void Initialize()
        {
            viewport = Game.GraphicsDevice.Viewport;

            viewport.MinDepth = NearClipPlane;
            viewport.MaxDepth = FarClipPlane;

            base.Initialize();
        }

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
