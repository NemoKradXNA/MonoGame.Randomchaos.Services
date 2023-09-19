
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Coroutine.Models;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System;
using System.Collections;

namespace MonoGame.Randomchaos.Services.Scene.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A scene fade base. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class SceneFadeBase : SceneBase
    {
        /// <summary>   The sprite batch. </summary>
        protected SpriteBatch _spriteBatch;
        /// <summary>   The fader. </summary>
        protected Texture2D fader;
        /// <summary>   The fade color. </summary>
        protected Color fadeColor = Color.Black;

        /// <summary>   The fade speed. </summary>
        protected byte fadeSpeed = 4;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the fade audio in. </summary>
        ///
        /// <value> True if fade audio in, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool FadeAudioIn { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the fade audio out. </summary>
        ///
        /// <value> True if fade audio out, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool FadeAudioOut { get; set; }

        /// <summary>   The music maximum volume. </summary>
        protected float _MusicMaxVolume;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the music maximum volume. </summary>
        ///
        /// <value> The music maximum volume. </value>
        ///-------------------------------------------------------------------------------------------------

        public float MusicMaxVolume
        {
            get { return _MusicMaxVolume; }
            set 
            {
                _MusicMaxVolume = Math.Min(1,Math.Max(0, value));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public SceneFadeBase(Game game, string name) : base(game, name) 
        {
            FadeAudioIn = true;
            FadeAudioOut = true;
            MusicMaxVolume = 1;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">         The game. </param>
        /// <param name="name">         The name. </param>
        /// <param name="audioAsset">   The audio asset. </param>
        ///-------------------------------------------------------------------------------------------------

        public SceneFadeBase(Game game, string name, string audioAsset) : base(game, name, audioAsset)
        {
            FadeAudioIn = true;
            FadeAudioOut = true;
            MusicMaxVolume = 1;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads a scene. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void LoadScene()
        {
            base.LoadScene();
            coroutineService.StartCoroutine(FadeIn());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Unload scene. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void UnloadScene()
        {
            base.UnloadScene();
            coroutineService.StartCoroutine(FadeOut());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            if (!string.IsNullOrEmpty(AudioMusicAsset)) 
            {
                if (!audioManager.IsMusicPlaying || audioManager.CurrentSongAsset != AudioMusicAsset)
                {
                    audioManager.PlaySong(AudioMusicAsset, MusicMaxVolume);
                }
            }

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
            
            fader = new Texture2D(Game.GraphicsDevice, 1, 1);
            fader.SetData(new Color[] { Color.White });

            base.LoadContent();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draw fader. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void DrawFader(GameTime gameTime) 
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (State != SceneStateEnum.Loaded)
                _spriteBatch.Draw(fader, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), fadeColor);

            _spriteBatch.End();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fade in. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <returns>   An IEnumerator. </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fade out. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <returns>   An IEnumerator. </returns>
        ///-------------------------------------------------------------------------------------------------

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
