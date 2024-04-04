
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.PostProcessing;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Randomchaos.PostProcessing.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A post processing component. </summary>
    ///
    /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class PostProcessingComponent: GameComponent, IPostProcessingComponent
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the camera. </summary>
        ///
        /// <value> The camera. </value>
        ///-------------------------------------------------------------------------------------------------

        public ICameraService Camera { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the post processing effects. </summary>
        ///
        /// <value> The post processing effects. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<IBasePostProcessingEffect> PostProcessingEffects { get; set; }

        /// <summary>   The original scene. </summary>
        private RenderTarget2D _originalScene;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the final render texture. </summary>
        ///
        /// <value> The final render texture. </value>
        ///-------------------------------------------------------------------------------------------------

        public RenderTarget2D FinalRenderTexture { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the render target screen scale. The screen render target is divided by this
        /// value.
        /// </summary>
        ///
        /// <value> The render target screen scale. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point RenderTargetScreenScale { get; set; } = new Point(1, 1);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the render target depth format. </summary>
        ///
        /// <value> The render target depth format. </value>
        ///-------------------------------------------------------------------------------------------------

        public DepthFormat RenderTargetDepthFormat { get; set; } = DepthFormat.Depth24;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the number of render target multi samples. </summary>
        ///
        /// <value> The number of render target multi samples. </value>
        ///-------------------------------------------------------------------------------------------------

        public int RenderTargetMultiSampleCount { get; set; } = 2;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the render target clear. </summary>
        ///
        /// <value> The color of the render target clear. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color RenderTargetClearColor { get; set; } = Color.Magenta;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sprite batch. </summary>
        ///
        /// <value> The sprite batch. </value>
        ///-------------------------------------------------------------------------------------------------

        protected SpriteBatch _spriteBatch { get; set; }

        /// <summary>   The current scene. </summary>
        protected RenderTarget2D currentScene = null;
        /// <summary>   The depth. </summary>
        protected RenderTarget2D depth = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
        ///
        /// <param name="game">         The game. </param>
        /// <param name="thisCamera">   (Optional) This camera. </param>
        ///-------------------------------------------------------------------------------------------------

        public PostProcessingComponent(Game game, ICameraService thisCamera = null) : base(game)
        {
            PostProcessingEffects = new List<IBasePostProcessingEffect>();
            Enabled = false;
            Camera = thisCamera;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an effect. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
        ///
        /// <param name="ppEfect">  The efect. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddEffect(IBasePostProcessingEffect ppEfect)
        {
            PostProcessingEffects.Add(ppEfect);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Update(GameTime gameTime)
        {
            int maxEffect = 0;
            if (PostProcessingEffects != null)
            {
                maxEffect = PostProcessingEffects.Count;

                if (maxEffect != 0)
                {
                    for (int e = 0; e < maxEffect; e++)
                    {
                        if (PostProcessingEffects[e].Enabled)
                        {
                            // May have lost ref to Game after serialization
                            if (PostProcessingEffects[e].Game == null)
                                PostProcessingEffects[e].Game = Game;

                            PostProcessingEffects[e].Update(gameTime);
                        }
                    }
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
        ///
        /// <param name="gameTime">     The game time. </param>
        /// <param name="currentScene"> The current scene. </param>
        /// <param name="depthBuffer">  Buffer for depth data. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Draw(GameTime gameTime, RenderTarget2D currentScene, RenderTarget2D depthBuffer)
        {
            _originalScene = currentScene;
            int maxEffect = PostProcessingEffects.Count;

            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            for (int e = 0; e < maxEffect; e++)
            {
                if (PostProcessingEffects[e].Enabled)
                {

                    // May have lost ref to Game after serialization
                    if (PostProcessingEffects[e].Game == null)
                        PostProcessingEffects[e].Game = Game;

                    if (PostProcessingEffects[e].Camera == null)
                        PostProcessingEffects[e].Camera = Camera;

                    PostProcessingEffects[e].OriginalScene = _originalScene;
                    PostProcessingEffects[e].Draw(gameTime, currentScene, depthBuffer);

                    currentScene = PostProcessingEffects[e].LastScene;
                }
            }

            FinalRenderTexture = currentScene;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts post process. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public void StartPostProcess(GameTime gameTime)
        {
            if (Enabled)
            {
                if (currentScene == null || depth == null)
                {
                    Point screenSize = new Point(Game.GraphicsDevice.Viewport.Width / RenderTargetScreenScale.X, Game.GraphicsDevice.Viewport.Height / RenderTargetScreenScale.Y);

                    currentScene = new RenderTarget2D(Game.GraphicsDevice,
                                                    screenSize.X,
                                                    screenSize.Y,
                                                    false,
                                                    SurfaceFormat.Color,
                                                    RenderTargetDepthFormat,// DepthFormat.Depth24, 
                                                    RenderTargetMultiSampleCount,// 2, 
                                                    RenderTargetUsage.DiscardContents,
                                                    true);

                    depth = new RenderTarget2D(Game.GraphicsDevice,
                                                   screenSize.X,
                                                    screenSize.Y,
                                                    false,
                                                    SurfaceFormat.Single,
                                                    RenderTargetDepthFormat,
                                                    RenderTargetMultiSampleCount,
                                                    RenderTargetUsage.DiscardContents,
                                                    true);
                }

                Game.GraphicsDevice.SetRenderTargets(currentScene, depth);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ends post process. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public void EndPostProcess(GameTime gameTime, bool drawFinal = false)
        {
            if (Enabled)
            {
                Game.GraphicsDevice.SetRenderTarget(null);

                Game.GraphicsDevice.Clear(RenderTargetClearColor);

                Draw(gameTime, currentScene, depth);

                if (drawFinal)
                {
                    DrawFinalRenderTexture();
                }
            }
        }

        public void DrawFinalRenderTexture(params IPostProcessingComponent[] postProcessors)
        {
            if (_spriteBatch == null)
            {
                _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            }

            if (postProcessors != null && postProcessors.Length > 0)
            {
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

                foreach (IPostProcessingComponent postProcessor in postProcessors)
                {
                    if (postProcessor.Enabled)
                    {
                        _spriteBatch.Draw(postProcessor.FinalRenderTexture, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), Color.White);
                    }
                }
            }
            else
            {
                _spriteBatch.Begin(SpriteSortMode.Immediate);
            }

            _spriteBatch.Draw(FinalRenderTexture, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), Color.White);
            _spriteBatch.End();
        }
    }
}
