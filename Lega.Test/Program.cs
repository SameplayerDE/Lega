var memory = new Lega.Core.Memory.VirtualMemory(512);
var display = new Lega.Core.Monogame.Graphics.VirtualDisplay(0, 0);

display.Map(memory, 0, 8);
memory.Poke(0, 0b11110000);

for (var i = 0; i < memory.Capacity; i++)
{
    Console.WriteLine($"{i:X2} : {memory.Peek(i):X2}");
}

for (var i = 0; i < display.MappedMemory.Length; i++)
{
    Console.WriteLine($"{i:X2} : {display.MappedMemory.Span[i]:X2}");
}