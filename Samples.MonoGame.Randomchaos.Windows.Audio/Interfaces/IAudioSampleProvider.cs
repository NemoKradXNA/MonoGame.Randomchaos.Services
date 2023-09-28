using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.MonoGame.Randomchaos.Windows.Audio.Interfaces
{
    public interface IAudioSampleProvider
    {
        int? Seed { get; set; }
        float Frequency { get; set; }

        float Gain { get; set; }
        float MinValue { get; set; }
        float MaxValue { get; set; }
        List<float> Shape { get; set; }
    }
}
