
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Samples.MonoGame.Randomchaos.EFCore.DbContext;
using Samples.MonoGame.Randomchaos.EFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.MonoGame.Randomchaos.EFCore
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A game 1. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Game1 : Game
    {
        /// <summary>   The graphics. </summary>
        private GraphicsDeviceManager _graphics;
        /// <summary>   The sprite batch. </summary>
        private SpriteBatch _spriteBatch;
        /// <summary>   The font. </summary>
        private SpriteFont font;

        /// <summary>   Context for the database. </summary>
        protected SampleDbContext dbContext;

        /// <summary>   The test data items. </summary>
        List<TestDataClass> testDataItems;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            dbContext = new SampleDbContext($"Data Source=DataBase/Test.db");

            dbContext.TestDataClass.Add(new TestDataClass() { Id = Guid.NewGuid(), Text = $"{DateTime.UtcNow.ToString("dd-MM-yyyy hh:mm:ss")} - Hello World!!" });
            dbContext.SaveChanges();


            testDataItems = dbContext.TestDataClass.ToList();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            font = Content.Load<SpriteFont>("Fonts/Font");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads the content. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(font, $"You have {testDataItems.Count} rows in the TestDataClass table", new Vector2(8, 8), Color.Black);
            _spriteBatch.DrawString(font, $"Row 1 = [{testDataItems[0].Text}]", new Vector2(8, 8 + font.LineSpacing * 1), Color.Black);
            _spriteBatch.DrawString(font, $"Row {testDataItems.Count} = [{testDataItems[testDataItems.Count-1].Text}]", new Vector2(8, 8 + font.LineSpacing * 2), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}