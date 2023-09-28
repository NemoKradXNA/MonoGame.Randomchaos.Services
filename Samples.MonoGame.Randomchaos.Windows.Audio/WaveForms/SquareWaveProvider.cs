using Microsoft.Xna.Framework;
using NAudio.Wave;
using System;

namespace Samples.MonoGame.Randomchaos.Windows.Audio.WaveForms
{
    public class SquareWaveProvider :  WaveProviderBase
    {
        public SquareWaveProvider(int sampleRate = 44100, int channels = 1) : base(sampleRate, channels) { }


        public override int Read(float[] buffer, int offset, int sampleCount)
        {

            for (int i = 0; i < sampleCount / WaveFormat.Channels; i++)
            {
                buffer[i + offset] = 2.0f * Frequency / WaveFormat.SampleRate;
                buffer[i + offset] = _sample * buffer[i + offset] % 2.0f - 1.0f;
                buffer[i + offset] = (float)((buffer[i + offset] >= 0.0) ? Gain : (0.0 - Gain));

                float v = (float)(Gain * Math.Sin((2 * Math.PI * _sample * Frequency) / WaveFormat.SampleRate)) * 2;

                buffer[i + offset] = MathHelper.Lerp(buffer[i + offset], v, _sample / WaveFormat.SampleRate);

                _sample++;
                if (_sample >= WaveFormat.SampleRate)
                {
                    _sample = 0;
                }

                SetShape(buffer[i + offset]);
            }


            return sampleCount;
        }
    }
}
