
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.PostProcessing;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.PostProcessing.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A base post processing effect. </summary>
    ///
    /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class BasePostProcessingEffect : IBasePostProcessingEffect
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the post processes. </summary>
        ///
        /// <value> The post processes. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<IBasePostProcess> PostProcesses { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the game. </summary>
        ///
        /// <value> The game. </value>
        ///-------------------------------------------------------------------------------------------------

        public Game Game { get; set; }

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
        /// <summary>   Gets or sets the last scene. </summary>
        ///
        /// <value> The last scene. </value>
        ///-------------------------------------------------------------------------------------------------

        public RenderTarget2D LastScene { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the original scene. </summary>
        ///
        /// <value> The original scene. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D OriginalScene { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public BasePostProcessingEffect(Game game)
        {
            PostProcesses = new List<IBasePostProcess>();
            Enabled = false;
            Game = game;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a post process. </summary>
        ///
        /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
        ///
        /// <param name="postProcess">  The post process. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddPostProcess(IBasePostProcess postProcess)
        {
            PostProcesses.Add(postProcess);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Update(GameTime gameTime)
        {
            int maxProcess = PostProcesses.Count;
            for (int p = 0; p < maxProcess; p++)
            {
                if (PostProcesses[p].Enabled)
                    PostProcesses[p].Update(gameTime);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws. </summary>
        ///
        /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
        ///
        /// <param name="gameTime">     The game time. </param>
        /// <param name="currentScene"> The current scene. </param>
        /// <param name="depthBuffer">  Buffer for depth data. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Draw(GameTime gameTime, RenderTarget2D currentScene, RenderTarget2D depthBuffer)
        {
            if (!Enabled)
                return;

            int maxProcess = PostProcesses.Count;
            LastScene = null;

            for (int p = 0; p < maxProcess; p++)
            {
                if (PostProcesses[p].Enabled)
                {
                    if (PostProcesses[p].Game == null)
                        PostProcesses[p].Game = Game;

                    if (PostProcesses[p].Camera == null)
                        PostProcesses[p].Camera = Camera;

                    if (PostProcesses[p].NewScene == null)
                        PostProcesses[p].NewScene = new RenderTarget2D(Game.GraphicsDevice, Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                            Game.GraphicsDevice.PresentationParameters.BackBufferHeight, false, Game.GraphicsDevice.PresentationParameters.BackBufferFormat,
                            DepthFormat.Depth24Stencil8);

                    PostProcesses[p].OriginalScene = OriginalScene;
                    PostProcesses[p].DepthBuffer = depthBuffer;

                    Game.GraphicsDevice.SetRenderTarget(PostProcesses[p].NewScene);

                    // Has the scene been rendered yet (first effect may be disabled)
                    if (LastScene != null)
                        PostProcesses[p].BackBuffer = LastScene;
                    else // No, so use the scene texture passed in.
                        PostProcesses[p].BackBuffer = currentScene;

                    PostProcesses[p].Draw(gameTime);

                    Game.GraphicsDevice.SetRenderTarget(null);

                    LastScene = PostProcesses[p].NewScene;
                }
            }
        }
    }
}
