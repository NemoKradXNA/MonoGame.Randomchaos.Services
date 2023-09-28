namespace Samples.MonoGame.Randomchaos.Windows.Audio.WaveForms
{
    internal class NoiseWaveProvider : WaveProviderBase
    {
        public NoiseWaveProvider(int sampleRate = 44100, int channels = 1) : base(sampleRate, channels) { }


        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int i = 0; i < sampleCount / WaveFormat.Channels; i++)
            {
                buffer[i + offset] = GetRandom() * Gain;

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
