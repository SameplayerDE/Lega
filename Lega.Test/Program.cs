var memory = new Lega.Core.Memory.VirtualMemory(512);

for (var i = 0; i < memory.Capacity; i++)
{
    Console.WriteLine($"{i:X2} : {memory.Peek(i):X2}");
}