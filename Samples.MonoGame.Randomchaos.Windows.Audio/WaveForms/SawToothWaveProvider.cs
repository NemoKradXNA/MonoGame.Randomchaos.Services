using NAudio.Wave;

namespace Samples.MonoGame.Randomchaos.Windows.Audio.WaveForms
{
    public class SawToothWaveProvider : WaveProviderBase
    {
        public SawToothWaveProvider(int sampleRate = 44100, int channels = 1) : base(sampleRate, channels) { }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int i = 0; i < sampleCount / WaveFormat.Channels; i++)
            {
                buffer[i + offset] = 2f * Frequency / WaveFormat.SampleRate;
                buffer[i + offset] = _sample * buffer[i + offset] % 2f - 1f;
                buffer[i + offset] = Gain * buffer[i + offset];

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
