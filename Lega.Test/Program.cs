using NAudio.Wave;
using NAudio.Wave.SampleProviders;

SignalGenerator SineGenerator;
SignalGenerator SquareGenerator;
SignalGenerator NoiseGenerator;

MixingSampleProvider Mixer;

WaveOutEvent Master;


SineGenerator = new SignalGenerator()
{
    Frequency = 100,
    Gain = 0.25,
    Type = SignalGeneratorType.Sin
};
SquareGenerator = new SignalGenerator()
{
    Frequency = 100,
    Gain = 0.25,
    Type = SignalGeneratorType.Square
};
NoiseGenerator = new SignalGenerator()
{
    Frequency = 100,
    Gain = 0.25,
    Type = SignalGeneratorType.White
};

Mixer = new MixingSampleProvider(new ISampleProvider[] {
    SineGenerator, SquareGenerator, NoiseGenerator
});

Master = new WaveOutEvent();
Master.Init(Mixer);
Master.Play();

while(Master.PlaybackState == PlaybackState.Playing)
{
    var key = Console.ReadKey(true).KeyChar;
    if (key == 'x')
    {
        Master.Stop();
    }
    //ADD
    if (key == 'q')
    {
        SineGenerator.Frequency += 100;
    }
    if (key == 'w')
    {
        SquareGenerator.Frequency += 100;
    }
    if (key == 'e')
    {
        NoiseGenerator.Frequency += 100;
    }
    //SUB
    if (key == 'a')
    {
        SineGenerator.Frequency -= 100;
    }
    if (key == 's')
    {
        SquareGenerator.Frequency -= 100;
    }
    if (key == 'd')
    {
        NoiseGenerator.Frequency -= 100;
    }
    // GAIN
    if (key == 't')
    {
        SineGenerator.Gain += 0.1;
    }
    if (key == 'z')
    {
        SquareGenerator.Gain += 0.1;
    }
    if (key == 'u')
    {
        NoiseGenerator.Gain += 0.1;
    }
    // GAIN
    if (key == 'g')
    {
        SineGenerator.Gain -= 0.1;
    }
    if (key == 'h')
    {
        SquareGenerator.Gain -= 0.1;
    }
    if (key == 'j')
    {
        NoiseGenerator.Gain -= 0.1;
    }
    SineGenerator.Gain = Math.Clamp(SineGenerator.Gain, 0, 1);
    SquareGenerator.Gain = Math.Clamp(SquareGenerator.Gain, 0, 1);
    NoiseGenerator.Gain = Math.Clamp(NoiseGenerator.Gain, 0, 1);
}
