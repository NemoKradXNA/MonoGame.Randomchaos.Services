
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Audio
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A service for accessing audioes information. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class AudioService : ServiceBase<IAudioService>, IAudioService
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Current song loaded. </summary>
        ///
        /// <value> The current song. </value>
        ///-------------------------------------------------------------------------------------------------

        public Song CurrentSong { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Toggle looping of song. </summary>
        ///
        /// <value> True if loop curren song, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool loopCurrenSong { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Current sound effect loaded. </summary>
        ///
        /// <value> The current sfx instances. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<string, SoundEffectInstance> CurrentSFXInstances { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Property to flag if Music is currently playing. </summary>
        ///
        /// <value> True if this object is music playing, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsMusicPlaying { get { return MediaPlayer.State == MediaState.Playing; } }

        /// <summary>   The current song asset. </summary>
        string _CurrentSongAsset;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current song asset. </summary>
        ///
        /// <value> The current song asset. </value>
        ///-------------------------------------------------------------------------------------------------

        public string CurrentSongAsset { get { return _CurrentSongAsset; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Property to flag if Music is currently stopped. </summary>
        ///
        /// <value> True if this object is music stopped, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsMusicStopped { get { return MediaPlayer.State == MediaState.Stopped; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Property to flag if Music is currently paused. </summary>
        ///
        /// <value> True if this object is music paused, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsMusicPaused { get { return MediaPlayer.State == MediaState.Paused; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Current Media player state. </summary>
        ///
        /// <value> The media state. </value>
        ///-------------------------------------------------------------------------------------------------

        public MediaState MediaState { get { return MediaPlayer.State; } }

        /// <summary>   The master volume. </summary>
        protected float _MasterVolume = 1;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the master volume. </summary>
        ///
        /// <value> The master volume. </value>
        ///-------------------------------------------------------------------------------------------------

        public float MasterVolume
        {
            get { return _MasterVolume; }
            set
            {
                float v = MathHelper.Max(0, MathHelper.Min(1, value));
                if (_MasterVolume != v)
                {
                    _MasterVolume = v;

                    // Trigger the SFX and music volumes to change with the master volume..
                    SFXVolume = _SFXVolume;
                    MusicVolume = _MusicVolume;
                }
            }
        }

        /// <summary>   The sfx volume. </summary>
        protected float _SFXVolume = 1;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sfx volume. </summary>
        ///
        /// <value> The sfx volume. </value>
        ///-------------------------------------------------------------------------------------------------

        public float SFXVolume
        {
            get { return _SFXVolume; }
            set
            {
                _SFXVolume = MathHelper.Max(0, MathHelper.Min(1, value));
                if (CurrentSFXInstances != null && CurrentSFXInstances.Count > 0)
                {
                    foreach (string key in CurrentSFXInstances.Keys)
                        CurrentSFXInstances[key].Volume = SFXVolume * MasterVolume;
                }
            }
        }
        /// <summary>   The music volume. </summary>
        protected float _MusicVolume = 1;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the music volume. </summary>
        ///
        /// <value> The music volume. </value>
        ///-------------------------------------------------------------------------------------------------

        public float MusicVolume
        {
            get { return _MusicVolume; }
            set
            {
                _MusicVolume = MathHelper.Max(0, MathHelper.Min(1, value));
                MediaPlayer.Volume = MusicVolume * MasterVolume;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   ctor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">         . </param>
        /// <param name="masterVolume"> (Optional) The master volume. </param>
        /// <param name="musicVolume">  (Optional) The music volume. </param>
        /// <param name="sfxVolume">    (Optional) The sfx volume. </param>
        ///-------------------------------------------------------------------------------------------------

        public AudioService(Game game, float masterVolume = 1, float musicVolume = 1, float sfxVolume = 1) : base(game)
        {
            CurrentSFXInstances = new Dictionary<string, SoundEffectInstance>();

            SFXVolume = sfxVolume;
            MusicVolume = musicVolume;
            MasterVolume = masterVolume;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method to load and play a song based on it's asset name. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="songAsset">    Song asset to play. </param>
        /// <param name="volume">       (Optional) Volume 0-1 (default 1) </param>
        /// <param name="loop">         (Optional) Loop song? </param>
        ///-------------------------------------------------------------------------------------------------

        public void PlaySong(string songAsset, float volume = 1, bool loop = true)
        {
            _CurrentSongAsset = songAsset;
            CurrentSong = Game.Content.Load<Song>(songAsset);

            MediaPlayer.Volume = volume * MusicVolume * MasterVolume;
            loopCurrenSong = loop;

            PlaySong(CurrentSong);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method to play a Song. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="song"> Song to play. </param>
        ///-------------------------------------------------------------------------------------------------

        public void PlaySong(Song song)
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();

            MediaPlayer.Play(song);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Update call. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> . </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (CurrentSong != null && loopCurrenSong && IsMusicStopped)
                PlaySong(CurrentSong);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method to play a sound effect asset. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="sfxAsset"> SFX asset. </param>
        /// <param name="volume">   (Optional) Volume 0-1 (default 1) </param>
        /// <param name="listener"> (Optional) Listener of the sound, if 3D sound is required. </param>
        /// <param name="emitter">  (Optional) Source of the sound if 3D sound is required. </param>
        /// <param name="pitch">    (Optional) Pitch 0-1 (default .5) </param>
        /// <param name="pan">      (Optional) Pan 0-1 (default .5) </param>
        ///-------------------------------------------------------------------------------------------------

        public void PlaySFX(string sfxAsset, float volume = 1, AudioListener listener = null, AudioEmitter emitter = null, float pitch = 0, float pan = 0)
        {
            SoundEffectInstance sfx = Game.Content.Load<SoundEffect>(sfxAsset).CreateInstance();
            sfx.Volume = volume * SFXVolume * MasterVolume;
            sfx.Pitch = pitch;
            sfx.Pan = pan;

            if (listener != null && emitter != null)
                sfx.Apply3D(listener, emitter);

            sfx.Play();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method to play a sound effect. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="sfxAsset"> SFX asset. </param>
        /// <param name="volume">   (Optional) Volume 0-1 (default 1) </param>
        /// <param name="listener"> (Optional) Listener of the sound, if 3D sound is required. </param>
        /// <param name="emitter">  (Optional) Source of the sound if 3D sound is required. </param>
        /// <param name="loop">     (Optional) Loop SFX? </param>
        /// <param name="pitch">    (Optional) Pitch 0-1 (default .5) </param>
        /// <param name="pan">      (Optional) Pan 0-1 (default .5) </param>
        ///-------------------------------------------------------------------------------------------------

        public void PlaySound(string sfxAsset, float volume = 1, AudioListener listener = null, AudioEmitter emitter = null, bool loop = false, float pitch = 0, float pan = 0)
        {
            if (!CurrentSFXInstances.ContainsKey(sfxAsset))
                CurrentSFXInstances.Add(sfxAsset, Game.Content.Load<SoundEffect>(sfxAsset).CreateInstance());


            CurrentSFXInstances[sfxAsset].IsLooped = loop;
            CurrentSFXInstances[sfxAsset].Pan = pan;
            CurrentSFXInstances[sfxAsset].Pitch = pitch;
            CurrentSFXInstances[sfxAsset].Volume = volume * SFXVolume * MasterVolume;

            if (listener != null && emitter != null)
                CurrentSFXInstances[sfxAsset].Apply3D(listener, emitter);

            CurrentSFXInstances[sfxAsset].Play();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method to stop current SFX. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="sfxAsset"> (Optional) SFX asset. </param>
        ///-------------------------------------------------------------------------------------------------

        public void StopSound(string sfxAsset = null)
        {
            if (CurrentSFXInstances != null && CurrentSFXInstances.Count > 0)
            {
                if (sfxAsset == null) // Stop them all
                {
                    foreach (string key in CurrentSFXInstances.Keys)
                    {
                        if (CurrentSFXInstances[key].State != SoundState.Stopped)
                            CurrentSFXInstances[key].Stop(true);
                    }
                }
                else
                {
                    if (CurrentSFXInstances[sfxAsset].State != SoundState.Stopped)
                        CurrentSFXInstances[sfxAsset].Stop(true);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method to stop Media Player. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void StopMusic()
        {
            if (!IsMusicStopped)
            {
                loopCurrenSong = false;
                MediaPlayer.Stop();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method to pause Media Player. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="audio">    . </param>
        ///-------------------------------------------------------------------------------------------------

        public void PauseMusic(string audio)
        {
            if (IsMusicPlaying)
                MediaPlayer.Pause();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method to Pause SFX. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="sfxAsset"> (Optional) SFX asset. </param>
        ///-------------------------------------------------------------------------------------------------

        public void PauseSound(string sfxAsset = null)
        {
            if (CurrentSFXInstances != null && CurrentSFXInstances.Count > 0)
            {
                if (sfxAsset == null) // Stop them all
                {
                    foreach (string key in CurrentSFXInstances.Keys)
                    {
                        if (CurrentSFXInstances[key].State == SoundState.Playing)
                            CurrentSFXInstances[key].Pause();
                    }
                }
                else
                {
                    if (CurrentSFXInstances[sfxAsset].State == SoundState.Playing)
                        CurrentSFXInstances[sfxAsset].Pause();
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method to resume Media Player. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// ### <param name="audio">    . </param>
        ///-------------------------------------------------------------------------------------------------

        public void ResumeMusic()
        {
            if (IsMusicPaused)
                MediaPlayer.Resume();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method to resume current SFX. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="sfxAsset"> (Optional) SFX asset. </param>
        ///-------------------------------------------------------------------------------------------------

        public void ResumeSound(string sfxAsset = null)
        {
            if (CurrentSFXInstances != null && CurrentSFXInstances.Count > 0)
            {
                if (sfxAsset == null) // Stop them all
                {
                    foreach (string key in CurrentSFXInstances.Keys)
                    {
                        if (CurrentSFXInstances[key].State == SoundState.Paused)
                            CurrentSFXInstances[key].Resume();
                    }
                }
                else
                {
                    if (CurrentSFXInstances[sfxAsset].State == SoundState.Paused)
                        CurrentSFXInstances[sfxAsset].Resume();
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method to clean up object when disposed. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="disposing">    . </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopSound();

                CurrentSFXInstances.Clear();

                if (!IsMusicStopped)
                    MediaPlayer.Stop();
            }
            base.Dispose(disposing);
        }
    }
}
