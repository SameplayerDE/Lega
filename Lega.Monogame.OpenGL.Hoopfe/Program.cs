using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System.Threading;
using System;
using System.Threading.Tasks;

Task.Run(() =>
{
    var beep = new SignalGenerator()
    {
        Gain = 0.1,
        Frequency = 100,
        Type = SignalGeneratorType.SawTooth
    }.Take(TimeSpan.FromSeconds(5));

    var boop = new SignalGenerator()
    {
        Gain = 0.1,
        Frequency = 100,
        Type = SignalGeneratorType.SawTooth
    }.Take(TimeSpan.FromSeconds(5));

    using (var wo = new WaveOutEvent())
    {
        wo.Init(sine20Seconds);
        wo.Play();
        while (wo.PlaybackState == PlaybackState.Playing)
        {
            Thread.Sleep(500);
        }
    }
});


using var game = new Lega.Monogame.Shared.Hoopfe.Game1();
game.Run();
