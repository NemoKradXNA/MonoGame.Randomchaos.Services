
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;
using MonoGame.Randomchaos.UI;

namespace Sample.MonoGame.Randomchaos.Primitives3D.Scenes
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A main menu scene. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class MainMenuScene : SceneFadeBase
    {
        protected UIButton btnPrimitivesScene;
        protected UIButton btnVoxelScene;
        protected UIButton btnExit;

        /// <summary>   The font. </summary>
        private SpriteFont font;
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


            Vector2 c = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) * .5f;
            Point btnSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 12);

            int menuTop = GraphicsDevice.Viewport.Height / 4;

            Point pos = new Point(0, menuTop) + (new Point((int)c.X,0)) - new Point(btnSize.X / 2, btnSize.Y / 2);
            btnPrimitivesScene = CreateButton(string.Empty, Game.Content.Load<Texture2D>("Textures/UI/Primitives3dButton"), pos, btnSize);

            pos += new Point(0, btnSize.Y + 32);
            btnVoxelScene = CreateButton(string.Empty, Game.Content.Load<Texture2D>("Textures/UI/VoxelsButton"), pos, btnSize);

            pos += new Point(0, btnSize.Y + 32);
            btnExit = CreateButton(string.Empty, Game.Content.Load<Texture2D>("Textures/UI/ExitButton"), pos, btnSize);

            Components.Add(btnPrimitivesScene);
            Components.Add(btnVoxelScene);
            Components.Add(btnExit);

            base.Initialize();
        }

        protected UIButton CreateButton(string text, Texture2D bgTeture, Point pos, Point size)
        {
            var btn = new UIButton(Game, pos, size)
            {
                BackgroundTexture = bgTeture,
                Font = font,
                Text = text,
                HighlightColor = Color.SkyBlue,
                Segments = new Rectangle(8, 8, 8, 8)
            };

            btn.OnMouseClick += Btn_OnMouseClick;

            return btn;
        }

        private void Btn_OnMouseClick(IUIBase sender, IMouseStateManager mouseState)
        {
            if (sender == btnPrimitivesScene)
            {
                sceneManager.LoadScene("primitives3DScene");
            }
            else if(sender == btnVoxelScene)
            {
                sceneManager.LoadScene("voxelScene");
            }
            else if (sender == btnExit)
            {
                exiting = true;
                State = SceneStateEnum.Unloading;
                UnloadScene();
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
