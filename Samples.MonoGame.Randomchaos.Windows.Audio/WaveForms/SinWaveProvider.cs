using NAudio.Wave;
using System;

namespace Samples.MonoGame.Randomchaos.Windows.Audio.WaveForms
{
    public class SinWaveProvider : WaveProviderBase
    {
        public SinWaveProvider(int sampleRate = 44100, int channels = 1) : base(sampleRate, channels) { }
        

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int i = 0; i < sampleCount / WaveFormat.Channels; i++)
            {
                buffer[i + offset] = (float)(Gain * Math.Sin((2 * Math.PI * _sample * Frequency) / WaveFormat.SampleRate)) * 2;
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
