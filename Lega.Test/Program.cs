public class Program
{
    public static void Main()
    {
        // start the timer, callback in 10000 milliseconds, and don't fire more than once
        var timer = new Timer(TimerCallback, null, 10000, Timeout.Infinite);

        // to reset the timer when you receive data
        timer.Change(10000, Timeout.Infinite);

        // to stop the timer completely
        timer.Change(Timeout.Infinite, Timeout.Infinite);
    }

    public static void TimerCallback(object state)
    {
        Console.WriteLine("Test");
    }
}