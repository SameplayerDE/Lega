using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System.Threading;
using System;
using System.Threading.Tasks;
using Lega.Core.Monogame.Audio;

VirtualAudioChannel sqr = new VirtualAudioChannel(SignalGeneratorType.Square);
VirtualAudioChannel saw = new VirtualAudioChannel(SignalGeneratorType.SawTooth);
VirtualAudioChannel tri = new VirtualAudioChannel(SignalGeneratorType.Triangle);

sqr.Play(0.1f, 100, 100);
tri.Play(0.1f, 300, 100);
saw.Play(0.1f, 100, 100);
sqr.Play(0.1f, 100, 100);

using var game = new Lega.Monogame.Shared.Hoopfe.Game1();
game.Run();
