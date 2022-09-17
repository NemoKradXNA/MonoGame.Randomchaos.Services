using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Coroutine.Models;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System;
using System.Collections;

namespace MonoGame.Randomchaos.Services.Scene.Models
{
    public abstract class SceneFadeBase : SceneBase
    {
        protected SpriteBatch _spriteBatch;
        protected Texture2D fader;
        protected Color fadeColor = Color.Black;

        protected byte fadeSpeed = 4;

        public bool FadeAudioIn { get; set; }
        public bool FadeAudioOut { get; set; }

        protected float _MusicMaxVolume;
        public float MusicMaxVolume
        {
            get { return _MusicMaxVolume; }
            set 
            {
                _MusicMaxVolume = Math.Min(1,Math.Max(0, value));
            }
        }

        public SceneFadeBase(Game game, string name) : base(game, name) 
        {
            FadeAudioIn = true;
            FadeAudioOut = true;
            MusicMaxVolume = 1;
        }

        public SceneFadeBase(Game game, string name, string audioAsset) : base(game, name, audioAsset)
        {
            FadeAudioIn = true;
            FadeAudioOut = true;
            MusicMaxVolume = 1;
        }

        public override void LoadScene()
        {
            base.LoadScene();
            coroutineService.StartCoroutine(FadeIn());
        }

        public override void UnloadScene()
        {
            base.UnloadScene();
            coroutineService.StartCoroutine(FadeOut());
        }

        public override void Initialize()
        {
            if (!string.IsNullOrEmpty(AudioMusicAsset)) 
            {
                if (!audioManager.IsMusicPlaying || audioManager.CurrentSongAsset != AudioMusicAsset)
                {
                    audioManager.PlaySong(AudioMusicAsset, .25f);
                }
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            fader = new Texture2D(Game.GraphicsDevice, 1, 1);
            fader.SetData(new Color[] { Color.White });

            base.LoadContent();
        }

        public virtual void DrawFader(GameTime gameTime) 
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (State != SceneStateEnum.Loaded)
                _spriteBatch.Draw(fader, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), fadeColor);

            _spriteBatch.End();
        }

        protected virtual IEnumerator FadeIn()
        {
            byte a = 255;
            
            fadeColor = new Color(fadeColor.R, fadeColor.G, fadeColor.B, a);

            while (a > 0)
            {
                yield return new WaitForEndOfFrame(Game);
                a = (byte)Math.Max(0, a - fadeSpeed);
                fadeColor = new Color(fadeColor.R, fadeColor.G, fadeColor.B, a);

                if (FadeAudioIn)
                {
                    audioManager.MusicVolume = Math.Min(MusicMaxVolume, 1f - (a / 255f));
                }
            }

            State = SceneStateEnum.Loaded;
        }

        protected virtual IEnumerator FadeOut()
        {
            byte a = 0;
            fadeColor = new Color(fadeColor.R, fadeColor.G, fadeColor.B, a);

            while (a < 255)
            {
                yield return new WaitForEndOfFrame(Game);
                a = (byte)Math.Min(255, a + fadeSpeed);
                fadeColor = new Color(fadeColor.R, fadeColor.G, fadeColor.B, a);

                if (FadeAudioOut)
                {
                    audioManager.MusicVolume = Math.Min(MusicMaxVolume, 1f - (a / 255f));
                }
            }

            State = SceneStateEnum.Unloaded;

        }
    }
}
