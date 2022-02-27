using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface IAudioService
    {
        Song CurrentSong { get; set; }
        bool loopCurrenSong { get; set; }
        Dictionary<string, SoundEffectInstance> CurrentSFXInstances { get; set; }
        bool IsMusicPlaying { get; }
        bool IsMusicStopped { get; }
        bool IsMusicPaused { get; }
        string CurrentSongAsset { get; }
        MediaState MediaState { get; }
        float MasterVolume { get; set; }
        float SFXVolume { get; set; }
        float MusicVolume { get; set; }

        void PlaySong(string songAsset, float volume = 1, bool loop = true);
        void PlaySong(Song song);
        void PlaySFX(string sfxAsset, float volume = 1, AudioListener listener = null, AudioEmitter emitter = null, float pitch = 0, float pan = 0);
        void PlaySound(string sfxAsset, float volume = 1, AudioListener listener = null, AudioEmitter emitter = null, bool loop = false, float pitch = 0, float pan = 0);
        void StopSound(string sfxAsset = null);
        void StopMusic();
        void PauseMusic(string audio);
        void PauseSound(string sfxAsset = null);
        void ResumeMusic();
        void ResumeSound(string sfxAsset = null);

    }
}
