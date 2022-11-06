using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Lega.LoopTime
{
    internal class Game
    {

        public Stopwatch Timer { get; set; }
        public Stopwatch Stopwatch { get; set; }
        public bool IsRunning { get; set; }

        public float x = 64;
        public float y = 64;

        public void Init()
        {
            Stopwatch = new Stopwatch();
            Timer = new Stopwatch();
        }

        public void Run()
        {
            Init();
            Stopwatch.Restart();

            long lastTime = Stopwatch.ElapsedMilliseconds;
            long lastTimer = Stopwatch.ElapsedMilliseconds; //to output the ticks and fps in the console
            double unprocessed = 0; //counts unprocessed ticks to compensate
            double msPerTick = 1000 / 30d; //updates per seconds
            int frames = 0;
            int ticks = 0;

            IsRunning = true;

            Timer.Start();

            while (IsRunning)
            {
                long now = Stopwatch.ElapsedMilliseconds;
                unprocessed += (now - lastTime) / msPerTick; //calculates unprocessed time
                lastTime = now;
                bool shouldRender = true;

                //if game skipped updates, compensate for that 
                while (unprocessed >= 1)
                {
                    ticks++;
                    Update();
                    unprocessed -= 1;
                    shouldRender = true;
                }

                if (shouldRender)
                {
                    frames++;
                    Draw();
                }

                if (Stopwatch.ElapsedMilliseconds - lastTimer > 1000)
                {
                    lastTimer += 1000;
                   // Console.WriteLine(ticks + " ticks, " + frames + " f/s");
                    frames = 0;
                    ticks = 0;
                }

            }

        }

        public void Update()
        {
            y = y + 1;
            if (y >= 128 && Timer.IsRunning)
            {
                Timer.Stop();
                Console.WriteLine(Timer.Elapsed.TotalSeconds);
                y = 64;
                Timer.Restart();
            }
        }

        public void Draw()
        {

        }

    }
}
