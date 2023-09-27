using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;
using MonoGame.Randomchaos.UI;
using System.Threading;

namespace Samples.MonoGame.Randomchaos.Physics.Scenes
{
    public class MainMenuScene : SceneFadeBase
    {
        protected UIButton btnBasicBalistics2D;
        protected UIButton btnBasicBalistics3D;
        protected UIButton btnBasicParticles2D;
        protected UIButton btnBasicForces3D;
        protected UIButton btnExit;

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
            font = Game.Content.Load<SpriteFont>("Fonts/font");
            buttonFont = Game.Content.Load<SpriteFont>("Fonts/ButtonFont");


            Vector2 c = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) * .5f;
            Point btnSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 8);

            int menuTop = GraphicsDevice.Viewport.Height / 6;

            Point pos = new Point(0, menuTop) + (new Point((int)c.X, 0)) - new Point(btnSize.X / 2, btnSize.Y / 2);
            btnBasicBalistics2D = CreateButton("Basic Ballistics 2D", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            pos += new Point(0, btnSize.Y + 32);
            btnBasicBalistics3D = CreateButton("Basic Ballistics 3D", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            pos += new Point(0, btnSize.Y + 32);
            btnBasicParticles2D = CreateButton("Basic Particles 2D", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            pos += new Point(0, btnSize.Y + 32);
            btnBasicForces3D = CreateButton("Basic Forces 3D", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            pos += new Point(0, btnSize.Y + 32);
            btnExit = CreateButton("Exit", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            Components.Add(btnBasicBalistics2D);
            Components.Add(btnBasicBalistics3D);
            Components.Add(btnBasicParticles2D);
            Components.Add(btnBasicForces3D);
            Components.Add(btnExit);

            base.Initialize();
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

        private void Btn_OnMouseClick(IUIBase sender, IMouseStateManager mouseState)
        {
            if (State == SceneStateEnum.Loaded)
            {
                if (sender == btnBasicBalistics2D)
                {
                    sceneManager.LoadScene("basicBallistics2DScene");
                }
                else if (sender == btnBasicBalistics3D)
                {
                    sceneManager.LoadScene("basicBallistics3DScene");
                }
                else if (sender == btnBasicParticles2D)
                {
                    sceneManager.LoadScene("basicParticles2DScene");
                }
                else if (sender == btnBasicForces3D)
                {
                    sceneManager.LoadScene("basicForces3DScene");
                }
                else if (sender == btnExit)
                {
                    exiting = true;
                    State = SceneStateEnum.Unloading;
                    UnloadScene();
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

            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            DrawFader(gameTime);
        }
    }
}
