using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.PostProcessing.Models;
using MonoGame.Randomchaos.Primitives3D.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;
using MonoGame.Randomchaos.UI;
using Samples.MonoGame.Randomchaos.PostProcessing.Models.PostProcessing.PostProcessingEffects;
using System;
using System.Collections.Generic;

namespace Samples.MonoGame.Randomchaos.PostProcessing.Scenes
{
    public class MainMenuScene : SceneFadeBase
    {
        protected UIButton btnExit;
        protected UIButton btnWireFrame;
        protected UIButton btnCulling;
        protected UIButton btnBleach;
        protected UIButton btndeRezed;
        protected UISlider sldDeRezTiles;
        protected UIButton btnChroma;


        protected BleachEffect bleachEffect;
        protected DeRezedPostProcessEffect deRezedEffect;
        protected ChromaticAberrationEffect chromaEffect;

        /// <summary>   The camera. </summary>
        protected ICameraService _camera { get { return Game.Services.GetService<ICameraService>(); } }

        /// <summary>   The triangle. </summary>
        TriangleBasicEffect triangle;
        /// <summary>   The quad. </summary>
        QuadBasicEffect quad;
        /// <summary>   The cube. </summary>
        CubeBasicEffect cube;
        /// <summary>   The sphere. </summary>
        SphereBasicEfect sphere;
        /// <summary>   The capsule. </summary>
        CapsuleBasicEffect capsule;
        /// <summary>   The cylinder. </summary>
        CylinderBasicEffect cylinder;
        /// <summary>   The plane. </summary>
        PlaneBasicEffect plane;

        /// <summary>   True to render wire frame. </summary>
        protected bool _renderWireFrame = false;
        /// <summary>   True to disable, false to enable the culling. </summary>
        protected bool _cullingOff = false;

        /// <summary>   The font. </summary>
        private SpriteFont font;
        private SpriteFont buttonFont;
        /// <summary>   True to exiting. </summary>
        bool exiting;
        /// <summary>   The next scene. </summary>
        protected string NextScene;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public MainMenuScene(Game game, string name) : base(game, name) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            UIComponentTypes.Add(typeof(IUIBase));

            font = Game.Content.Load<SpriteFont>("Fonts/font");
            buttonFont = Game.Content.Load<SpriteFont>("Fonts/ButtonFont");

            postProcess = new PostProcessingComponent(Game) { Enabled = true };

            bleachEffect = new BleachEffect(Game, 1) { Enabled = false };
            postProcess.AddEffect(bleachEffect);

            deRezedEffect = new DeRezedPostProcessEffect(Game, 512) { Enabled = false };
            postProcess.AddEffect(deRezedEffect);

            chromaEffect = new ChromaticAberrationEffect(Game) { Enabled = false, ScreenCurvature = 39f,Blur = .075f, LineDensity = .25f, Flickering = .05f };
            postProcess.AddEffect(chromaEffect);

            Vector2 c = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) * .5f;
            Point btnSize = new Point(256, buttonFont.LineSpacing + 16);

            int menuTop = GraphicsDevice.Viewport.Height / 6;

            Point pos = new Point(8, 8);

            btnWireFrame = CreateButton($"Wire Frame {(_renderWireFrame ? "On" : "Off")}", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);
            Components.Add(btnWireFrame);

            pos += new Point(0, buttonFont.LineSpacing + 16 + 8);
            btnCulling = CreateButton($"Culling {(!_cullingOff ? "On" : "Off")}", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);
            Components.Add(btnCulling);

            pos += new Point(0, buttonFont.LineSpacing + 16 + 8);
            btnBleach = CreateButton($"Bleach Effect {(bleachEffect.Enabled ? "On" : "Off")}", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);
            Components.Add(btnBleach);

            pos += new Point(0, buttonFont.LineSpacing + 16 + 8);
            btndeRezed = CreateButton($"DeRezed Effect {(deRezedEffect.Enabled ? "On" : "Off")}", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);
            Components.Add(btndeRezed);

            sldDeRezTiles = CreateSlider(pos + new Point(btnSize.X + (btnSize.X/4), buttonFont.LineSpacing/2), btnSize, $"Tiles: {deRezedEffect.NumberofTiles}", font, 1);
            Components.Add(sldDeRezTiles);

            pos += new Point(0, buttonFont.LineSpacing + 16 + 8);
            btnChroma = CreateButton($"TV Effect {(chromaEffect.Enabled ? "On" : "Off")}", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);
            Components.Add(btnChroma);

            pos = new Point(GraphicsDevice.Viewport.Width - (btnSize.X + 8), GraphicsDevice.Viewport.Height - (btnSize.Y + 8));
            btnExit = CreateButton("Exit", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);
            //btnExit.TextPositionOffset = new Vector2(0,-.5f);


            Components.Add(btnExit);

            base.Initialize();

            Vector3 ld = new Vector3(1, -1, -1);
            Vector3 amb = Color.Black.ToVector3();

            sphere.SetDirectionalLight(ld, amb);
            cube.SetDirectionalLight(ld, amb);
            triangle.SetDirectionalLight(ld, amb);
            quad.SetDirectionalLight(ld, amb);
            capsule.SetDirectionalLight(ld, amb);
            cylinder.SetDirectionalLight(ld, amb);
            plane.SetDirectionalLight(ld, amb);
        }

        protected UIButton CreateButton(string text, Texture2D bgTeture, Point pos, Point size)
        {
            var btn = new UIButton(Game, pos, size)
            {
                BackgroundTexture = bgTeture,
                Font = buttonFont,
                Text = text,
                HighlightColor = Color.SkyBlue,
                Segments = new Rectangle(8, 8, 8, 8),
                TextColor = Color.DarkSlateGray,
            };

            btn.OnMouseClick += Btn_OnMouseClick;

            return btn;
        }

        protected UISlider CreateSlider(Point position, Point size, string text, SpriteFont font, float startingValue)
        {
            UISlider sldr = new UISlider(Game, position, size,4,new Point(24,24)) 
            {
                Font = font,
                Label = text,
                LabelTint = Color.Black,
                BarTexture = CreateBox(300, 32, new Rectangle(1, 1, 1, 1), new Color(.2f, .2f, .5f, 1f), Color.DodgerBlue),
                Tint = Color.Black,
                SliderTexture = Game.Content.Load<Texture2D>("Textures/UI/circle"),
                SliderColor = Color.DarkSlateGray,
                SliderHoverColor = Color.SkyBlue,
                Value = startingValue,                  
            };

            return sldr;
        }

        public Texture2D CreateBox(int width, int height, Rectangle thickenss, Color bgColor, Color edgeColor, float horizontalFade = 2, float verticalFade = 2)
        {
            Texture2D boxTexture = new Texture2D(Game.GraphicsDevice, width, height);

            Color[] c = new Color[width * height];

            Color color = new Color(0, 0, 0, 0);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x < thickenss.X || x >= width - thickenss.Width || y < thickenss.Height || y >= height - thickenss.Y)
                        color = edgeColor;
                    else
                    {
                        Vector4 col = bgColor.ToVector4();

                        if (horizontalFade > 0)
                            col *= MathF.Min(1, horizontalFade - ((float)x / (width - (thickenss.X + thickenss.Width))));
                        else
                            col *= MathF.Min(1, Math.Abs(horizontalFade) - (1 - ((float)x / (width - (thickenss.X + thickenss.Width)))));

                        if (verticalFade > 0)
                            col *= MathF.Min(1, verticalFade - ((float)y / height));
                        else
                            col *= MathF.Min(1, Math.Abs(verticalFade) - (1 - ((float)y / height)));

                        color = new Color(col);
                    }



                    c[x + y * width] = color;
                }
            }

            boxTexture.SetData(c);

            return boxTexture;
        }

        public override void LoadScene()
        {
            _camera.Transform.Position = new Vector3(0, 0, 10);
            _camera.Transform.Rotation = Quaternion.Identity;

            triangle = new TriangleBasicEffect(Game);
            triangle.Transform.Position = new Vector3(-1f, 0, 0);
            Components.Add(triangle);

            quad = new QuadBasicEffect(Game);
            quad.Transform.Position = new Vector3(1f, 0, 0);
            Components.Add(quad);

            cube = new CubeBasicEffect(Game);
            cube.Texture = Game.Content.Load<Texture2D>("Textures/boxTexture");
            cube.UVMap = new List<Vector2>()
            {
                new Vector2(.25f, .25f),new Vector2(.5f, .25f),new Vector2(.5f, .5f),new Vector2(.25f, .5f), // F
                new Vector2(.5f, .75f),new Vector2(.25f, .75f),new Vector2(.25f, 1),new Vector2(.5f, 1), // bK
                new Vector2(.5f, .5f),new Vector2(.25f, .5f),new Vector2(.25f, .75f),new Vector2(.5f, .75f), // T
                new Vector2(.25f, 0),new Vector2(.5f, 0),new Vector2(.5f, .25f),new Vector2(.25f, .25f), // B
                new Vector2(0, .5f),new Vector2(.25f, .5f),new Vector2(.25f, .75f),new Vector2(0, .75f), // L
                new Vector2(.75f, .5f),new Vector2(.5f, .5f),new Vector2(.5f, .75f),new Vector2(.75f, .75f), // R
            };
            cube.Transform.Position = new Vector3(3, 1, 3);
            Components.Add(cube);

            sphere = new SphereBasicEfect(Game);
            sphere.Transform.Position = new Vector3(3, 1f, -3);
            Components.Add(sphere);

            capsule = new CapsuleBasicEffect(Game);
            capsule.Transform.Position = new Vector3(-3, 1, 3);
            Components.Add(capsule);

            cylinder = new CylinderBasicEffect(Game);
            cylinder.Transform.Position = new Vector3(-3, 1, -3);
            Components.Add(cylinder);

            plane = new PlaneBasicEffect(Game);
            plane.Transform.Position = new Vector3(0, -1, -.5f);
            Components.Add(plane);

            base.LoadScene();
        }

        private void Btn_OnMouseClick(IUIBase sender, IMouseStateManager mouseState)
        {
            if (State == SceneStateEnum.Loaded)
            {
                if (sender == btnExit)
                {
                    exiting = true;
                    State = SceneStateEnum.Unloading;
                    UnloadScene();
                }
                else if (sender == btnWireFrame)
                {
                    _renderWireFrame = !_renderWireFrame;
                    btnWireFrame.Text = $"Wire Frame {(_renderWireFrame ? "On" : "Off")}";
                }
                else if (sender == btnCulling)
                {
                    _cullingOff = !_cullingOff;
                    btnCulling.Text = $"Culling {(!_cullingOff ? "On" : "Off")}";
                }
                else if (sender == btnBleach)
                {
                    bleachEffect.Enabled = !bleachEffect.Enabled;
                    btnBleach.Text = $"Bleach Effect {(bleachEffect.Enabled ? "On" : "Off")}";
                }
                else if (sender == btndeRezed)
                {
                    deRezedEffect.Enabled = !deRezedEffect.Enabled;
                    btndeRezed.Text = $"DeRezed Effect {(deRezedEffect.Enabled ? "On" : "Off")}";
                }
                else if (sender == btnChroma)
                {
                    chromaEffect.Enabled = !chromaEffect.Enabled;
                    btnChroma.Text = $"TV Effect {(chromaEffect.Enabled ? "On" : "Off")}";
                }

            }
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

            if (State == SceneStateEnum.Loaded)
            {
                // Camera controls..
                float speedTran = .1f;
                float speedRot = .01f;

                if (kbManager.KeyDown(Keys.W))
                    _camera.Transform.Translate(Vector3.Forward * speedTran);
                if (kbManager.KeyDown(Keys.S))
                    _camera.Transform.Translate(Vector3.Backward * speedTran);
                if (kbManager.KeyDown(Keys.A))
                    _camera.Transform.Translate(Vector3.Left * speedTran);
                if (kbManager.KeyDown(Keys.D))
                    _camera.Transform.Translate(Vector3.Right * speedTran);

                if (kbManager.KeyDown(Keys.Left))
                    _camera.Transform.Rotate(Vector3.Up, speedRot);
                if (kbManager.KeyDown(Keys.Right))
                    _camera.Transform.Rotate(Vector3.Up, -speedRot);
                if (kbManager.KeyDown(Keys.Up))
                    _camera.Transform.Rotate(Vector3.Right, speedRot);
                if (kbManager.KeyDown(Keys.Down))
                    _camera.Transform.Rotate(Vector3.Right, -speedRot);

                deRezedEffect.NumberofTiles = (int)MathHelper.Lerp(128, 512, sldDeRezTiles.Value);
                sldDeRezTiles.Label = $"Tiles: {deRezedEffect.NumberofTiles}";
            }

            SetRasterizerState();

            base.Update(gameTime);

            if (State == SceneStateEnum.Unloaded && exiting)
                Game.Exit();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_camera.ClearColor);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw(gameTime);

            DrawFader(gameTime);
        }

        public void SetRasterizerState()
        {
            GraphicsDevice.RasterizerState = new RasterizerState() { FillMode = _renderWireFrame ? FillMode.WireFrame : FillMode.Solid, CullMode = _cullingOff ? CullMode.None : CullMode.CullCounterClockwiseFace };
        }
    }
}
