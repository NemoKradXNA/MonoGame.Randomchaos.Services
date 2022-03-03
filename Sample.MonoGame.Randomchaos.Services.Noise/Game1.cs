using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Noise;

namespace Sample.MonoGame.Randomchaos.Services.Noise
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        protected Texture2D noiseTexture;
        protected Texture2D noiseTextureWithRamp;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            new KeijiroPerlinService(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            noiseTexture = new Texture2D(GraphicsDevice, 128, 128);
            noiseTextureWithRamp = new Texture2D(GraphicsDevice, 128, 128);

            Color[] color = new Color[noiseTexture.Width * noiseTexture.Height];
            Color[] colorRamp = new Color[noiseTexture.Width * noiseTexture.Height];
            Color[] ramp = new Color[] { Color.Blue, Color.Tan, Color.Green, Color.DarkGray, Color.White };


            KeijiroPerlinService noiseService = Services.GetService<KeijiroPerlinService>();

            // fill a texture with noise.
            for (int x = 0; x < noiseTexture.Width; x++)
            {
                for (int y = 0; y < noiseTexture.Height; y++)
                {
                    Vector2 coord = new Vector2((float)x / noiseTexture.Width, (float)y / noiseTexture.Height);
                    float n = noiseService.Noise(coord * 8);

                    // Move  into the range of 0 to 1
                    n = (n + 1) * .5f;

                    color[x + y * noiseTexture.Width] = new Color(n, n, n, 1);

                    int r = (int)MathHelper.Lerp(0, ramp.Length, n);

                    colorRamp[x + y * noiseTexture.Width] = ramp[r];
                }
            }

            noiseTexture.SetData(color);
            noiseTextureWithRamp.SetData(colorRamp);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            

            base.Update(gameTime);
        }

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
