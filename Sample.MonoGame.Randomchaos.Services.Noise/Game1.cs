
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Noise;
using System.Globalization;

namespace Sample.MonoGame.Randomchaos.Services.Noise
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

        /// <summary>   The noise texture. </summary>
        protected Texture2D noiseTexture;
        /// <summary>   The noise texture with ramp. </summary>
        protected Texture2D noiseTextureWithRamp;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the noise service. </summary>
        ///
        /// <value> The noise service. </value>
        ///-------------------------------------------------------------------------------------------------

        KeijiroPerlinService noiseService {get { return (KeijiroPerlinService)Services.GetService<KeijiroPerlinService>(); } }

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

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            new KeijiroPerlinService(this);

            base.Initialize();
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
            noiseTexture = new Texture2D(GraphicsDevice, 128, 128);
            noiseTextureWithRamp = new Texture2D(GraphicsDevice, 128, 128);

            Color[] color = new Color[noiseTexture.Width * noiseTexture.Height];
            Color[] colorRamp = new Color[noiseTexture.Width * noiseTexture.Height];
            Color[] ramp = new Color[] { Color.Blue, Color.Tan, Color.Green, Color.DarkGray, Color.White };


            // fill a texture with noise.
            for (int x = 0; x < noiseTexture.Width; x++)
            {
                for (int y = 0; y < noiseTexture.Height; y++)
                {
                    Vector2 coord = new Vector2((float)x / noiseTexture.Width, (float)y / noiseTexture.Height);
                    float n = GetNoise(coord * 2);

                    // Move  into the range of 0 to 1
                    n = (n + 1) * .5f;

                    color[x + y * noiseTexture.Width] = new Color(n, n, n, 1);

                    int r = (int)MathHelper.Lerp(0, ramp.Length-1, n);

                    colorRamp[x + y * noiseTexture.Width] = ramp[r];
                }
            }

            noiseTexture.SetData(color);
            noiseTextureWithRamp.SetData(colorRamp);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a noise. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="coord">    The coordinate. </param>
        ///
        /// <returns>   The noise. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected float GetNoise(Vector2 coord)
        {
           return noiseService.Noise(coord)
                            + (.5f * noiseService.Noise(coord * 2))
                            + (.25f * noiseService.Noise(coord * 4))
                            + (.125f * noiseService.Noise(coord * 8));
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

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp);

            _spriteBatch.Draw(noiseTexture, new Rectangle(8, 8, 256, 256), Color.White);

            _spriteBatch.Draw(noiseTextureWithRamp, new Rectangle(256 + 16, 8, 256, 256), Color.White);

            _spriteBatch.End();
        }
    }
}
