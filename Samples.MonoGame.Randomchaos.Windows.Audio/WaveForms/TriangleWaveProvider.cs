using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.MonoGame.Randomchaos.Windows.Audio.WaveForms
{
    public class TriangleWaveProvider : WaveProviderBase
    {
        public TriangleWaveProvider(int sampleRate = 44100, int channels = 1) : base(sampleRate, channels) { }


        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int i = 0; i < sampleCount / WaveFormat.Channels; i++)
            {
                buffer[i + offset] = 2f * Frequency / WaveFormat.SampleRate;
                buffer[i + offset] = _sample * buffer[i + offset] % 2f;
                buffer[i + offset] = 2f * buffer[i + offset];

                if (buffer[i + offset] > 1)
                {
                    buffer[i + offset] = 2f - buffer[i + offset];
                }

                if (buffer[i + offset] < -1)
                {
                    buffer[i + offset] = -2 - buffer[i + offset];
                }

                buffer[i + offset] *= Gain;

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
