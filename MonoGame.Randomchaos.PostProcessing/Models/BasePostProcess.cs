
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.PostProcessing;

namespace MonoGame.Randomchaos.PostProcessing.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A base post process. </summary>
    ///
    /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class BasePostProcess : IBasePostProcess
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the new scene. </summary>
        ///
        /// <value> The new scene. </value>
        ///-------------------------------------------------------------------------------------------------

        public RenderTarget2D NewScene { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Current Post Processing BackBuffer. </summary>
        ///
        /// <value> A buffer for back data. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D BackBuffer { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the camera. </summary>
        ///
        /// <value> The camera. </value>
        ///-------------------------------------------------------------------------------------------------

        public ICameraService Camera { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is enabled. </summary>
        ///
        /// <value> True if enabled, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool Enabled { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sampler. </summary>
        ///
        /// <value> The sampler. </value>
        ///-------------------------------------------------------------------------------------------------

        public SamplerState Sampler { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the effect. </summary>
        ///
        /// <value> The effect. </value>
        ///-------------------------------------------------------------------------------------------------

        protected Effect effect { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the game. </summary>
        ///
        /// <value> The game. </value>
        ///-------------------------------------------------------------------------------------------------

        public Game Game { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the screen quad. </summary>
        ///
        /// <value> The screen quad. </value>
        ///-------------------------------------------------------------------------------------------------

        protected ScreenQuad _screenQuad { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the original scene. </summary>
        ///
        /// <value> The original scene. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D OriginalScene { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the buffer for depth data. </summary>
        ///
        /// <value> A buffer for depth data. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D DepthBuffer { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sprite batch. </summary>
        ///
        /// <value> The sprite batch. </value>
        ///-------------------------------------------------------------------------------------------------

        protected SpriteBatch _spriteBatch { get; set; }
        /// <summary>   The sort mode. </summary>
        public SpriteSortMode SortMode = SpriteSortMode.Immediate;
        /// <summary>   The blend. </summary>
        public BlendState Blend = BlendState.Opaque;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object use quad. </summary>
        ///
        /// <value> True if use quad, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool UseQuad { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public BasePostProcess(Game game)
        {
            Sampler = SamplerState.AnisotropicClamp;
            Enabled = true;
            Game = game;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Update(GameTime gameTime) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Draw(GameTime gameTime)
        {
            if (Enabled)
            {
                if (UseQuad)
                {
                    Game.GraphicsDevice.SamplerStates[0] = Sampler;

                    effect.CurrentTechnique.Passes[0].Apply();

                    if (_screenQuad == null)
                    {
                        _screenQuad = new ScreenQuad(Game);
                        _screenQuad.Initialize();
                    }

                    _screenQuad.Draw(-Vector2.One, Vector2.One);
                }
                else
                {
                    if (_spriteBatch == null)
                        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

                    _spriteBatch.Begin(SortMode, Blend, Sampler, DepthStencilState.DepthRead, RasterizerState.CullCounterClockwise);

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        _spriteBatch.Draw(BackBuffer, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), Color.White);
                    }
                    _spriteBatch.End();
                }

            }
        }
    }
}
