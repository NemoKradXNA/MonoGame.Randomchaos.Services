using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using NAudio.Wave;
using Samples.MonoGame.Randomchaos.Windows.Audio.WaveForms;

namespace Samples.MonoGame.Randomchaos.Windows.Audio
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        InputHandlerService inputService;

        AudioTest audio;

        float masterVolume = 1;
        float gain = 1;
        float frequency = 500;

        string selectedAudio = null;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            inputService = new InputHandlerService(this, new KeyboardStateManager(this), new MouseStateManager(this));

            audio = new AudioTest(this);

            audio.AddOutput("sign", AudioTest.SignalTypes["sign"]);
            audio.AddOutput("triangle", AudioTest.SignalTypes["triangle"]);
            audio.AddOutput("square", AudioTest.SignalTypes["square"]);
            audio.AddOutput("saw tooth", AudioTest.SignalTypes["saw tooth"]);
            audio.AddOutput("pink noise", AudioTest.SignalTypes["pink noise"]);
            audio.AddOutput("white noise", AudioTest.SignalTypes["white noise"]);
            audio.AddOutput("chirp", AudioTest.SignalTypes["chirp"]);
            audio.AddOutput("sign2", new SinWaveProvider());
            audio.AddOutput("square2", new SquareWaveProvider());
            audio.AddOutput("triangle2", new TriangleWaveProvider());
            audio.AddOutput("saw tooth2", new SawToothWaveProvider());
            audio.AddOutput("noise", new NoiseWaveProvider());
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            inputService.PreUpdate(gameTime);
            base.Update(gameTime);


            if (inputService.KeyboardManager.KeyPress(Keys.F1))
            {
                ToggleSound("sign");
            }
            if (inputService.KeyboardManager.KeyPress(Keys.F2))
            {
                ToggleSound("square");
            }
            if (inputService.KeyboardManager.KeyPress(Keys.F3))
            {
                ToggleSound("triangle");
            }            
            if (inputService.KeyboardManager.KeyPress(Keys.F4))
            {
                ToggleSound("saw tooth");
            }
            if (inputService.KeyboardManager.KeyPress(Keys.F5))
            {
                ToggleSound("pink noise");
            }
            if (inputService.KeyboardManager.KeyPress(Keys.F6))
            {
                ToggleSound("white noise");
            }
            if (inputService.KeyboardManager.KeyPress(Keys.F7))
            {
                ToggleSound("chirp");
            }

            

            if (inputService.KeyboardManager.KeyPress(Keys.F8))
            {
                ToggleSound("sign2");
            }
            if (inputService.KeyboardManager.KeyPress(Keys.F9))
            {
                ToggleSound("square2");
            }
            if (inputService.KeyboardManager.KeyPress(Keys.F10))
            {
                ToggleSound("triangle2");
            }
            if (inputService.KeyboardManager.KeyPress(Keys.F11))
            {
                ToggleSound("saw tooth2");
            }
            if (inputService.KeyboardManager.KeyPress(Keys.F12))
            {
                ToggleSound("noise");
            }

            if (inputService.KeyboardManager.KeyDown(Keys.OemPlus))
            {
                gain = MathHelper.Min(1, gain + .01f);
            }

            if (inputService.KeyboardManager.KeyDown(Keys.OemMinus))
            {
                gain = MathHelper.Max(0, gain - .01f);
            }

            if (inputService.KeyboardManager.KeyDown(Keys.OemPeriod))
            {
                frequency += 1f;
            }

            if (inputService.KeyboardManager.KeyDown(Keys.OemComma))
            {
                frequency = MathHelper.Max(0, frequency - 1f);
            }

            audio.SetMasterVolume(masterVolume);

            if (selectedAudio != null)
            {
                audio.SetGain(selectedAudio, gain);
                audio.SetFrequency(selectedAudio, frequency);

                texture = audio.GetRender(selectedAudio);
            }

            Window.Title = $"Frequency: {frequency}, Gain: {gain* 100}%, Volume: {masterVolume*100}%";
        }

        public void ToggleSound(string name)
        {
            if (audio.GetOutputState(name) != PlaybackState.Playing)
            {
                selectedAudio = name;
                audio.Play(name);
            }
            else
            {
                audio.Stop(name);
            }
        }
        Texture2D texture;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (texture != null)
            {
                _spriteBatch.Draw(texture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            }

            _spriteBatch.End();
        }
    }
}