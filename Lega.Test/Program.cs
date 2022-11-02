/*var memory = new Lega.Core.Memory.VirtualMemory(32);
var display = new Lega.Core.Monogame.Graphics.VirtualDisplay(0, 0);

display.Map(memory, 0, 8);
memory.Poke(0, 0b11110000);

for (var i = 0; i < memory.Capacity; i++)
{
    Console.WriteLine($"{i:X2} : {memory.Peek(i):X2}");
}

for (var i = 0; i < display.MemoryLength; i++)
{
    Console.WriteLine($"{i:X2} : {display.MemoryRegion.Peek(i):X2}");
}*/
using System.Diagnostics;

long l = 0;
Stopwatch sw = new Stopwatch();

void Perf(long itt)
{
	l = 0;
	sw.Restart();
	while (l < itt)
	{
		l++;
	}
	sw.Stop();
	Console.WriteLine(l);
	Console.WriteLine(sw.Elapsed.TotalMilliseconds);
}

for (var i = 0; i < 10; i++)
{
	Perf(100);

	Perf(1_000);
	Perf(10_000);
	Perf(100_000);

	Perf(1_000_000);
	Perf(10_000_000);
	Perf(100_000_000);

	Perf(1_000_000_000);
	Perf(10_000_000_000);
}