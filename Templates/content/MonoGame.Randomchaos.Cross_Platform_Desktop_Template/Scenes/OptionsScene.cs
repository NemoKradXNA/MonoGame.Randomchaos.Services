using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Cross_Platform_Desktop_Template.Scenes.MenuScenes;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;
using MonoGame.Randomchaos.UI;

namespace MonoGame.Randomchaos.Cross_Platform_Desktop_Template.Scenes
{
    public class OptionsScene : BaseMenuScene
    {
        protected UIButton btnBack;

        protected UISlider sldtMasterVolume;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public OptionsScene(Game game, string name) : base(game, name, null) 
        {
            FadeAudioOut = false;
            FadeAudioIn = false;
            MusicMaxVolume = .5f;
        }

        protected override void BtnExit_OnMouseClick(IUIBase sender, IMouseStateManager mouseState)
        {
            if (State == SceneStateEnum.Loaded)
            {
                if (sender == btnBack)
                {
                    sceneManager.LoadScene("mainMenu");
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/font");

            int buttonTop = GraphicsDevice.Viewport.Height - 72;
            Point buttonSize = new Point(128, 40);

            Point pos = new Point(GraphicsDevice.Viewport.Width - (buttonSize.X + 32) , buttonTop);

            btnBack = CreateButton("Back", pos, buttonSize);

            Components.Add(btnBack);

            pos = new Point(128, GraphicsDevice.Viewport.Height / 4);
            sldtMasterVolume = CreateSlider($"Master Volume: {((int)(audioManager.MasterVolume * 100f))}%", pos, new Point(24, 24), audioManager.MasterVolume);
            Components.Add(sldtMasterVolume);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            audioManager.MasterVolume = sldtMasterVolume.Value;

            sldtMasterVolume.Label = $"Master Volume: {((int)(audioManager.MasterVolume*100f))}%";
            base.Update(gameTime);
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

            _spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/UI/Background1"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.LimeGreen);

            string str = "This is the options screen. press the back button to go back.";
            Vector2 pos = (new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) - font.MeasureString(str)) / 2;

            _spriteBatch.DrawString(font, str, pos, Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);

            DrawFader(gameTime);
        }
    }
}
