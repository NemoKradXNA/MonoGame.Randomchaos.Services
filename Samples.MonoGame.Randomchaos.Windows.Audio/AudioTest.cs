using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Samples.MonoGame.Randomchaos.Windows.Audio.Interfaces;
using System.Collections.Generic;

namespace Samples.MonoGame.Randomchaos.Windows.Audio
{
    // Only ever will work on Windows :(
    public class AudioTest : GameComponent
    {

        public static Dictionary<string, ISampleProvider> SignalTypes = new Dictionary<string, ISampleProvider>()
        {
            { "sign", new SignalGenerator()
                {
                    Gain = .2,
                    Frequency = 500,
                    Type = SignalGeneratorType.Sin
                }
            },
            { "triangle", new SignalGenerator()
                {
                    Gain = 0.2,
                    Frequency = 500,
                    Type = SignalGeneratorType.Triangle
                }
            },
            { "square", new SignalGenerator()
                {
                    Gain = 0.2,
                    Frequency = 500,
                    Type = SignalGeneratorType.Square
                }
            },
            { "saw tooth", new SignalGenerator()
                {
                    Gain = 0.2,
                    Frequency = 500,
                    Type = SignalGeneratorType.SawTooth
                }
            },
            { "pink noise", new SignalGenerator()
                {
                    Gain = 0.2,
                    Type = SignalGeneratorType.Pink
                }
            },
            { "white noise", new SignalGenerator()
                {
                    Gain = 0.2,
                    Type = SignalGeneratorType.White
                }
            },
            { "chirp", new SignalGenerator()
                {
                    Gain = 1,
                    Frequency = 500, // start frequency of the sweep
                    FrequencyEnd = 2000,
                    Type = SignalGeneratorType.Sweep,
                    SweepLengthSecs = .125f
                }
            }
        };

        protected Dictionary<string, WaveOutEvent> _outputs = new Dictionary<string, WaveOutEvent>();
        protected Dictionary<string, ISampleProvider> _bank = new Dictionary<string, ISampleProvider>();

        public AudioTest(Game game) : base(game) 
        {

        }

        public void AddOutput(string name, ISampleProvider sound)
        {
            if (!_outputs.ContainsKey(name))
            {
                _outputs.Add(name, new WaveOutEvent());
                _bank.Add(name, sound);
            }
            
            _bank[name] = sound;
            _outputs[name].Init(_bank[name]);

            _outputs[name].PlaybackStopped += AudioTest_PlaybackStopped;
        }

        private void AudioTest_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            //string name = _outputs.Where(w => w.Value == (WaveOutEvent)sender).Select(s => s.Key).FirstOrDefault();

            //if (name != null)
            //{
            //    _outputs[name] = new WaveOutEvent();
            //    _outputs[name].Init(_bank[name]);
            //}
        }

        public Texture2D GetRender(string name)
        {
            Texture2D t;

            if (_bank[name] is IAudioSampleProvider)
            {
                List<float> data = ((IAudioSampleProvider)_bank[name]).Shape;

                if (data.Count > 0)
                {
                    Game.Window.Title = $"Min: {((IAudioSampleProvider)_bank[name]).MinValue * .5f}, Max: {((IAudioSampleProvider)_bank[name]).MaxValue * .5f} ";
                    t = new Texture2D(Game.GraphicsDevice, data.Count, data.Count/2);

                    Color[] c = new Color[t.Width * t.Height];

                    int m = t.Height / 2;

                    for (int x = 0; x < t.Width; x++)
                    {
                        for (int y = 0; y < t.Height; y++)
                        {
                            c[x + (y * t.Width)] = Color.Black;

                            float s = data[x] * .5f;
                            int v = (int)(m + (m * s) * .5f);

                            if (y == v)
                            {
                                c[x + (y * t.Width)] = Color.LimeGreen;
                            }
                        }
                    }
                    t.SetData(c);
                }
                else
                {
                    t = new Texture2D(Game.GraphicsDevice, 1, 1);
                    t.SetData(new Color[] { Color.Black });
                }
            }
            else
            {
                t = new Texture2D(Game.GraphicsDevice, 1, 1);
                t.SetData(new Color[] { Color.Black });
            }

            return t;
        }


        public void SetMasterVolume(float volume)
        {
            foreach (WaveOutEvent waveOut in _outputs.Values)
            {
                waveOut.Volume = volume;
            }
        }

        public void SetFrequency(string name, float frequency)
        {
            if (_bank[name] is SignalGenerator)
            {
                ((SignalGenerator)_bank[name]).Frequency = frequency;
            }
            if (_bank[name] is IAudioSampleProvider)
            {
                ((IAudioSampleProvider)_bank[name]).Frequency = frequency;
            }
        }

        public void SetGain(string name, float gain)
        {
            if (_bank[name] is SignalGenerator)
            {
                ((SignalGenerator)_bank[name]).Gain = gain;
            }
            if (_bank[name] is IAudioSampleProvider)
            {
                ((IAudioSampleProvider)_bank[name]).Gain = gain;
            }
        }
        public void Play(string name)
        {
            _outputs[name].Play();
        }

        public void Stop(string name)
        {
            _outputs[name].Stop();
        }

        public PlaybackState GetOutputState(string name)
        {
            return _outputs[name].PlaybackState;
        }
    }

    
}
