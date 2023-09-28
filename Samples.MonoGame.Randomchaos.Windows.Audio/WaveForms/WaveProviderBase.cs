using Microsoft.Xna.Framework;
using NAudio.Wave;
using Samples.MonoGame.Randomchaos.Windows.Audio.Interfaces;
using System;
using System.Collections.Generic;

namespace Samples.MonoGame.Randomchaos.Windows.Audio.WaveForms
{
    public abstract class WaveProviderBase : WaveProvider32, IAudioSampleProvider
    {
        public int? Seed { get; set; } = null;
        public float Gain { get; set; } = .2f;
        public float Frequency { get; set; } = 500;

        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public List<float> Shape { get; set; } = new List<float>();

        protected int _renderWidth { get; set; } = 256;
        protected int _sample;

        protected Random _random{get; private set;}

        public WaveProviderBase(int sampleRate = 44100, int channels = 1) : base(sampleRate, channels) 
        {
        }

        protected void SetShape(float n)
        {
            MinValue = Math.Min(n, MinValue);
            MaxValue = Math.Max(n, MaxValue);

            Shape.Insert(0, n);
            if (Shape.Count > _renderWidth)
            {
                Shape = Shape.GetRange(0, _renderWidth);
            }
        }

        protected float GetRandom()
        {
            if (_random == null)
            {
                if (Seed == null)
                {
                    Seed = DateTime.UtcNow.Millisecond;
                }

                _random = new Random(Seed.Value);
            }

            return MathHelper.Lerp(-1, 1, (float)_random.NextDouble());
        }
    }
}
