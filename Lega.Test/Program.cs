using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

public class Program
{

    private static bool running = true;
    static Timer timer = new Timer(1000) { AutoReset = false };

    public static void Main()
    {
        // to set up the timer
       
        timer.Elapsed += TimerOnElapsed;

        // to start the timer running
        timer.Start();

        // to reset the timer
        timer.Stop();
        timer.Start();

        while (running)
        {
            
        }
    }


    // and the callback
    private static void TimerOnElapsed(object sender, ElapsedEventArgs args)
    {
        var time = (long)(args.SignalTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
        Console.WriteLine("Hallo" + time);
        timer.Start();
    }
}