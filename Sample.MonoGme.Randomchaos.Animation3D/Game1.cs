using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Animation.Interfaces;
using MonoGame.Randomchaos.Primitives3D.Models;
using MonoGame.Randomchaos.Services.Camera;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using Newtonsoft.Json;
using Sample.MonoGme.Randomchaos.Animation3D.Models;
using System;

namespace Sample.MonoGme.Randomchaos.Animation3D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        IInputStateService _inputService { get { return (IInputStateService)Services.GetService<IInputStateService>(); } }
        IKeyboardStateManager _kbInput { get { return _inputService.KeyboardManager; } }
        ICameraService _camera { get { return (ICameraService)Services.GetService<ICameraService>(); } }

        PlaneBasicEffect plane;
        SkinnedModel skinnedModel;

        float camTranslationSpeed = 3f;
        float camRotationSpeed = .5f;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            new InputHandlerService(this, new KeyboardStateManager(this), new MouseStateManager(this));

            new CameraService(this, .1f, 20000);

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _camera.Transform.Position = new Vector3(0, 1, 7);

            plane = new PlaneBasicEffect(this);
            plane.Transform.Position = new Vector3(0, 0, 0);
            Components.Add(plane);

            skinnedModel = new SkinnedModel(this, "Models/Female Tough Walk");
            skinnedModel.Transform.Position = new Vector3(3, 0, 0);
            Components.Add(skinnedModel);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("Fonts/font");

            // TODO: use this.Content to load your game content here

            plane.Texture = Content.Load<Texture2D>("Textures/MG");

            Color AmbientLightColor = Color.Gray;
            Vector3 light0Dir = new Vector3(-1, -1, -.5f);
            Color light0DiffuseColor = Color.White;
            Color light0SpecualrColor = Color.DarkGray;
            byte light0SpecularPower = 255;

            skinnedModel.MeshEffects = new System.Collections.Generic.Dictionary<string, SkinnedEffect>()
            {
                {"Beta_Surface",
                new SkinnedEffect(GraphicsDevice)
                {
                    AmbientLightColor = AmbientLightColor.ToVector3(),
                    DiffuseColor = new Vector3(1,.5f,0),
                    SpecularColor = Color.Gold.ToVector3(),
                    SpecularPower = 255,
                    PreferPerPixelLighting = true,
                    
                } },
                {"Beta_Joints",
                new SkinnedEffect(GraphicsDevice)
                {
                    AmbientLightColor = AmbientLightColor.ToVector3(),
                    DiffuseColor = new Vector3(.2f,.2f,.2f),
                    SpecularColor = Color.Silver.ToVector3(),
                    SpecularPower = 255,
                    PreferPerPixelLighting = true,
                    
                } }
            };

            skinnedModel.MeshEffects["Beta_Surface"].DirectionalLight0.Direction = light0Dir;
            skinnedModel.MeshEffects["Beta_Surface"].DirectionalLight0.DiffuseColor = light0DiffuseColor.ToVector3();
            skinnedModel.MeshEffects["Beta_Surface"].DirectionalLight0.SpecularColor = light0SpecualrColor.ToVector3();

            skinnedModel.MeshEffects["Beta_Joints"].DirectionalLight0.Direction = light0Dir;
            skinnedModel.MeshEffects["Beta_Joints"].DirectionalLight0.DiffuseColor = light0DiffuseColor.ToVector3();
            skinnedModel.MeshEffects["Beta_Joints"].DirectionalLight0.SpecularColor = light0SpecualrColor.ToVector3();

            // Load other animations and stack them in animator.
            // Need to be able to change this so we can just load in a stack of animations.
            var animationData = skinnedModel._modelData.SkinningData.AnimationClips;

            ISkinnedData anim = Content.Load<ISkinnedData>("Models/Idle");
            skinnedModel.SkinningData.AddClips(anim.AnimationClips);

            anim = Content.Load<ISkinnedData>("Models/Running");
            skinnedModel.SkinningData.AddClips(anim.AnimationClips);

            var json = JsonConvert.SerializeObject(skinnedModel._modelData.SkinningData.AnimationClips);

            skinnedModel.StartAnimation("Idle0");
        }

        protected override void Update(GameTime gameTime)
        {
            if (_kbInput.KeyPress(Keys.Escape))
                Exit();

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!skinnedModel.AnimationPlayer.IsPaused && skinnedModel.AnimationPlayer.IsPlaying && skinnedModel.AnimationPlayer.CurrentClip.Name == "Walking0")
            {
                skinnedModel.Transform.Translate(Vector3.Backward * .02f);
                skinnedModel.Transform.Rotate(Vector3.Down, (float)(Math.Sin(gameTime.ElapsedGameTime.TotalSeconds * .125f) * Math.PI));
            }
            else if (!skinnedModel.AnimationPlayer.IsPaused && skinnedModel.AnimationPlayer.IsPlaying && skinnedModel.AnimationPlayer.CurrentClip.Name == "Running0")
            {
                skinnedModel.Transform.Translate(Vector3.Backward * .05f);
                skinnedModel.Transform.Rotate(Vector3.Down, (float)(Math.Sin(gameTime.ElapsedGameTime.TotalSeconds * .3125f) * Math.PI));
            }

            // TODO: Add your update logic here
            if (_kbInput.KeyDown(Keys.W))
            {
                _camera.Transform.Translate(Vector3.Forward * camTranslationSpeed * t);
            }
            if (_kbInput.KeyDown(Keys.S))
            {
                _camera.Transform.Translate(Vector3.Backward * camTranslationSpeed * t);
            }
            if (_kbInput.KeyDown(Keys.A))
            {
                _camera.Transform.Translate(Vector3.Left * camTranslationSpeed * t);
            }
            if (_kbInput.KeyDown(Keys.D))
            {
                _camera.Transform.Translate(Vector3.Right * camTranslationSpeed * t);
            }

            if (_kbInput.KeyDown(Keys.Up))
            {
                _camera.Transform.Rotate(Vector3.Right, camRotationSpeed * t);
            }
            if (_kbInput.KeyDown(Keys.Down))
            {
                _camera.Transform.Rotate(Vector3.Left, camRotationSpeed * t);
            }
            if (_kbInput.KeyDown(Keys.Left))
            {
                _camera.Transform.Rotate(Vector3.Up, camRotationSpeed * t);
            }
            if (_kbInput.KeyDown(Keys.Right))
            {
                _camera.Transform.Rotate(Vector3.Down, camRotationSpeed * t);
            }

            if (_kbInput.KeyPress(Keys.F1))
            {
                switch (skinnedModel.AnimationPlayer.CurrentClip.Name)
                {
                    case "Idle0":
                        skinnedModel.StartAnimation("Walking0");
                        break;
                    case "Walking0":
                        if (skinnedModel.AnimationPlayer.LastAnimationClip != null && skinnedModel.AnimationPlayer.LastAnimationClip.Name == "Idle0")
                            skinnedModel.StartAnimation("Running0");
                        else if (skinnedModel.AnimationPlayer.LastAnimationClip != null && skinnedModel.AnimationPlayer.LastAnimationClip.Name == "Running0")
                            skinnedModel.StartAnimation("Idle0");
                        break;
                    case "Running0":
                        skinnedModel.StartAnimation("Walking0");
                        break;

                }
            }


            if (_kbInput.KeyPress(Keys.P)) 
            {
                skinnedModel.AnimationPlayer.IsPaused = !skinnedModel.AnimationPlayer.IsPaused;
            }

                _inputService.PreUpdate(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            RasterizerState rasterizerState = GraphicsDevice.RasterizerState;
            BlendState blendState = GraphicsDevice.BlendState;
            DepthStencilState depthStencilState = GraphicsDevice.DepthStencilState;

            _spriteBatch.Begin();

            Vector2 s = Vector2.One * -1;
            Vector2 p = new Vector2(8, 8);
            _spriteBatch.DrawString(_font, "Esc - Exit", p, Color.Black);
            _spriteBatch.DrawString(_font, "Esc - Exit", p + s, Color.Gold);

            p.Y += _font.LineSpacing;
            _spriteBatch.DrawString(_font, "F1 - Move to next Animation", p, Color.Black);
            _spriteBatch.DrawString(_font, "F1 - Move to next Animation", p + s, Color.Gold);

            p.Y += _font.LineSpacing;
            _spriteBatch.DrawString(_font, "P - Pause animation", p, Color.Black);
            _spriteBatch.DrawString(_font, "P - Pause animation", p + s, Color.Gold);

            string animNam = skinnedModel.AnimationPlayer.CurrentClip.Name;

            float r = skinnedModel.AABoundingSpheres[0].Radius * 1.5f;
            p = _camera.WorldPositionToScreenPosition(skinnedModel.Transform.Position + (Vector3.Up * r)) + new Vector2(16 + _font.MeasureString(animNam).X / -2, -_font.LineSpacing);
            _spriteBatch.DrawString(_font, animNam, p, Color.Black);
            _spriteBatch.DrawString(_font, animNam, p + s, Color.Red);

            _spriteBatch.End();

            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = blendState;
            GraphicsDevice.DepthStencilState = depthStencilState;
        }
    }
}
