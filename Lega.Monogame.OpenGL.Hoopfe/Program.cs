using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System.Threading;
using System;
using System.Threading.Tasks;
using Lega.Core.Monogame.Audio;
using System.Collections.Generic;
using NAudio.Mixer;

VirtualAudioChannel sqr = new VirtualAudioChannel(SignalGeneratorType.Square);
VirtualAudioChannel sin = new VirtualAudioChannel(SignalGeneratorType.Sin);
VirtualAudioChannel tri = new VirtualAudioChannel(SignalGeneratorType.Triangle);

VirtualAudioDriver driver = new VirtualAudioDriver(sin);
driver.Play();

driver[0].Frequency = 200;

using var game = new Lega.Monogame.Shared.Hoopfe.Game1();
game.Run();
