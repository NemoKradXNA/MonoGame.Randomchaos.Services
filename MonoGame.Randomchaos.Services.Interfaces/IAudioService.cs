
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for audio service. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IAudioService
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the current song. </summary>
        ///
        /// <value> The current song. </value>
        ///-------------------------------------------------------------------------------------------------

        Song CurrentSong { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the loop curren song. </summary>
        ///
        /// <value> True if loop curren song, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool loopCurrenSong { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the current sfx instances. </summary>
        ///
        /// <value> The current sfx instances. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<string, SoundEffectInstance> CurrentSFXInstances { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object is music playing. </summary>
        ///
        /// <value> True if this object is music playing, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool IsMusicPlaying { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object is music stopped. </summary>
        ///
        /// <value> True if this object is music stopped, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool IsMusicStopped { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object is music paused. </summary>
        ///
        /// <value> True if this object is music paused, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool IsMusicPaused { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current song asset. </summary>
        ///
        /// <value> The current song asset. </value>
        ///-------------------------------------------------------------------------------------------------

        string CurrentSongAsset { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the state of the media. </summary>
        ///
        /// <value> The media state. </value>
        ///-------------------------------------------------------------------------------------------------

        MediaState MediaState { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the master volume. </summary>
        ///
        /// <value> The master volume. </value>
        ///-------------------------------------------------------------------------------------------------

        float MasterVolume { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sfx volume. </summary>
        ///
        /// <value> The sfx volume. </value>
        ///-------------------------------------------------------------------------------------------------

        float SFXVolume { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the music volume. </summary>
        ///
        /// <value> The music volume. </value>
        ///-------------------------------------------------------------------------------------------------

        float MusicVolume { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Play song. </summary>
        ///
        /// <param name="songAsset">    The song asset. </param>
        /// <param name="volume">       (Optional) The volume. </param>
        /// <param name="loop">         (Optional) True to loop. </param>
        ///-------------------------------------------------------------------------------------------------

        void PlaySong(string songAsset, float volume = 1, bool loop = true);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Play song. </summary>
        ///
        /// <param name="song"> The song. </param>
        ///-------------------------------------------------------------------------------------------------

        void PlaySong(Song song);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Play sfx. </summary>
        ///
        /// <param name="sfxAsset"> The sfx asset. </param>
        /// <param name="volume">   (Optional) The volume. </param>
        /// <param name="listener"> (Optional) The listener. </param>
        /// <param name="emitter">  (Optional) The emitter. </param>
        /// <param name="pitch">    (Optional) The pitch. </param>
        /// <param name="pan">      (Optional) The pan. </param>
        ///-------------------------------------------------------------------------------------------------

        void PlaySFX(string sfxAsset, float volume = 1, AudioListener listener = null, AudioEmitter emitter = null, float pitch = 0, float pan = 0);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Play sound. </summary>
        ///
        /// <param name="sfxAsset"> The sfx asset. </param>
        /// <param name="volume">   (Optional) The volume. </param>
        /// <param name="listener"> (Optional) The listener. </param>
        /// <param name="emitter">  (Optional) The emitter. </param>
        /// <param name="loop">     (Optional) True to loop. </param>
        /// <param name="pitch">    (Optional) The pitch. </param>
        /// <param name="pan">      (Optional) The pan. </param>
        ///-------------------------------------------------------------------------------------------------

        void PlaySound(string sfxAsset, float volume = 1, AudioListener listener = null, AudioEmitter emitter = null, bool loop = false, float pitch = 0, float pan = 0);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Stops a sound. </summary>
        ///
        /// <param name="sfxAsset"> (Optional) The sfx asset. </param>
        ///-------------------------------------------------------------------------------------------------

        void StopSound(string sfxAsset = null);
        /// <summary>   Stops a music. </summary>
        void StopMusic();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pause music. </summary>
        ///
        /// <param name="audio">    The audio. </param>
        ///-------------------------------------------------------------------------------------------------

        void PauseMusic(string audio);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pause sound. </summary>
        ///
        /// <param name="sfxAsset"> (Optional) The sfx asset. </param>
        ///-------------------------------------------------------------------------------------------------

        void PauseSound(string sfxAsset = null);
        /// <summary>   Resume music. </summary>
        void ResumeMusic();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Resume sound. </summary>
        ///
        /// <param name="sfxAsset"> (Optional) The sfx asset. </param>
        ///-------------------------------------------------------------------------------------------------

        void ResumeSound(string sfxAsset = null);

    }
}
