using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lega.Core.Monogame.Audio
{
    public class VirtualAudioChannel
    {
        public SignalGeneratorType SignalType;
        public SignalGenerator SignalGenerator;
       
        public double Gain
        {
            get => SignalGenerator.Gain;
            set => SignalGenerator.Gain = value;
        }

        public double Frequency
        {
            get => SignalGenerator.Frequency;
            set => SignalGenerator.Frequency = value;
        }

        public VirtualAudioChannel(SignalGeneratorType type)
        {
            SignalType = type;
            SignalGenerator = new SignalGenerator();
            SignalGenerator.Type = type;
        }

        public void Play(float gain, float frqz, double duration = 10)
        {
            SignalGenerator.Gain = gain;
            SignalGenerator.Frequency = frqz;
            var i = SignalGenerator.Take(TimeSpan.FromMilliseconds(duration));
            using (var wo = new WaveOutEvent())
            {
                wo.Init(i);
                wo.Play();
                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(500);
                }
            }
        }
    }
}
