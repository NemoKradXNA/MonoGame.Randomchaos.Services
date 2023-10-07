
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;
using MonoGame.Randomchaos.UI;
using System.Text;
using Test.Scenes.MenuScenes;

namespace Test.Scenes
{
    public class MainMenuScene : BaseMenuScene
    {
        protected UIButton btnExit;
        protected UIButton btnOptions;
        protected UIButton btnGame;

        /// <summary>   True to exiting. </summary>
        bool exiting;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public MainMenuScene(Game game, string name, string audioAsset) : base(game, name, audioAsset)
        {

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/font");

            int buttonTop = 128;
            Point buttonSize = new Point(267, 40);

            Point pos = new Point((GraphicsDevice.Viewport.Width / 2) - buttonSize.X / 2, buttonTop);

            btnOptions = CreateButton("Options", pos, buttonSize);

            pos.Y += buttonSize.Y + 24;
            btnGame = CreateButton("Play Game", pos, buttonSize);

            pos.Y += buttonSize.Y + 24;
            btnExit = CreateButton("Exit", pos, buttonSize);

            Components.Add(btnOptions);
            Components.Add(btnGame);
            Components.Add(btnExit);

            base.Initialize();
        }



        protected override void BtnExit_OnMouseClick(IUIBase sender, IMouseStateManager mouseState)
        {
            if (State == SceneStateEnum.Loaded)
            {
                if (sender == btnExit)
                {
                    exiting = true;
                    State = SceneStateEnum.Unloading;
                    UnloadScene();
                }
                else if (sender == btnOptions)
                {
                    FadeAudioIn = FadeAudioOut = false;
                    sceneManager.LoadScene("optionsScene");
                }
                else if (sender == btnGame)
                {
                    FadeAudioIn = FadeAudioOut = true;
                    sceneManager.LoadScene("gameScene");
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
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap);

            _spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/UI/Background1"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.Gold);


            _spriteBatch.End();

            base.Draw(gameTime);

            DrawFader(gameTime);
        }
    }
}
