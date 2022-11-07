using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lega.Core.Monogame.Audio
{
    public class VirtualAudioDriver
    {
        public MixingSampleProvider Provider;
        private WaveOutEvent _outputMaster;
        private VirtualAudioChannel[] _channels;

        public VirtualAudioChannel this[int key]
        {
            get => _channels[key];
        }

        public VirtualAudioDriver(params VirtualAudioChannel[] providers)
        {
            _channels = providers;
            var array = new ISampleProvider[_channels.Length];
            int i = 0;
            while (i < array.Length)
            {
                array[i] = _channels[i].SignalGenerator;
                i++;
            }
            Provider = new MixingSampleProvider(array);
            _outputMaster = new WaveOutEvent();
            _outputMaster.Init(Provider);
        }

        public void Play()
        {
            _outputMaster.Play();
        }

        public void Stop()
        {
            _outputMaster.Stop();
        }

        public void Pause()
        {
            _outputMaster.Pause();
        }
    }
}
