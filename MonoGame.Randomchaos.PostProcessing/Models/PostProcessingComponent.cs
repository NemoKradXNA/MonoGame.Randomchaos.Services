using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.PostProcessing;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Randomchaos.PostProcessing.Models
{
    public class PostProcessingComponent: GameComponent, IPostProcessingComponent
    {
        public ICameraService Camera { get; set; }
        public List<IBasePostProcessingEffect> PostProcessingEffects { get; set; }

        private RenderTarget2D _originalScene;
        public RenderTarget2D FinalRenderTexture { get; set; }

        public PostProcessingComponent(Game game, ICameraService thisCamera = null) : base(game)
        {
            PostProcessingEffects = new List<IBasePostProcessingEffect>();
            Enabled = false;
            Camera = thisCamera;
        }

        public void AddEffect(IBasePostProcessingEffect ppEfect)
        {
            PostProcessingEffects.Add(ppEfect);
        }

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

            Enabled = maxEffect > 0;

            if (Enabled)
                Enabled = PostProcessingEffects.Any(e => e.Enabled);
        }

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
    }
}
