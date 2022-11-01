using System.Collections;

var memory = new Lega.Core.Memory.VirtualMemory(1024);
var systemTimer = new System.Diagnostics.Stopwatch();
var systemHz = 10;

var random = new Random();

var deltaElapsed = 0f;
var totalElapsedBeforeUpdate = 1000d * systemHz;
var totalElapsed = 0f;
var lastElapsed = 0f;

const int bytesPerLine = 2;

var running = false;

memory.Poke(0, 0b01100110);
memory.Poke(1, 0b10011001);
memory.Poke(2, 0b10000001);
memory.Poke(3, 0b01000010);
memory.Poke(4, 0b01000010);
memory.Poke(5, 0b00100100);
memory.Poke(6, 0b00100100);
memory.Poke(7, 0b00011000);

memory.Poke(8, 0b11001100);

systemTimer.Start();
running = true;

int x = 0;
int y = 0;

while(running)
{

    totalElapsed = (float)systemTimer.Elapsed.TotalMilliseconds;
    deltaElapsed = totalElapsed - lastElapsed;
    lastElapsed = totalElapsed;

    totalElapsedBeforeUpdate = totalElapsedBeforeUpdate + deltaElapsed;

    if (totalElapsedBeforeUpdate >= (1000f / systemHz))
    {
        totalElapsedBeforeUpdate = 0;
        Tick();
    }
    Draw();
    Present();
}

void Tick()
{
    if (Console.KeyAvailable)
    {
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.D)
        {
            x += 1;
        }
        if (key.Key == ConsoleKey.A)
        {
            x -= 1;
        }
        if (key.Key == ConsoleKey.S)
        {
            y += 1;
        }
        if (key.Key == ConsoleKey.W)
        {
            y -= 1;
        }
    }
    //PresentSprite(0, 0, 0);
    //memory.Poke(random.Next(128, 8 * (bytesPerLine * bytesPerLine)), (byte)random.Next(0x00, 0xFF));
}

void Draw()
{
    ClearVRam();
    PresentSprite(x, y, 0);
    PresentSprite(0, 0, 1);
}

void ClearVRam()
{
    memory.Poke(128, 8 * (bytesPerLine * bytesPerLine), 0x00);
}

void Present()
{
    Console.SetCursorPosition(0, 0);
    var vram = memory.Peek(128, 8 * (bytesPerLine * bytesPerLine));
    var bits = new BitArray(vram.ToArray());

    for (int y = 0; y < bits.Count / (bytesPerLine * 8); y++)
    {
        for (int x = 0; x < (bytesPerLine * 8); x++)
        {
            var value = bits[y * (bytesPerLine * 8) + x];
            SetPixel(x, y, value ? ConsoleColor.White : ConsoleColor.Black);
        }
    }
}

void SetPixel(int x, int y, ConsoleColor color)
{
    const int charX = 2;
    const int charY = 1;
    Console.BackgroundColor = color;
    Console.ForegroundColor = color;
    for (int line = y; line < y + charY; line++) {
        Console.SetCursorPosition(x * charX, y);
        for (int col = x; col < x + charX; col++)
        {
            Console.Write(" ");
        }
    }
}

void PresentSprite(int x, int y, int id)
{
    var sprRam = memory.Peek(8 * id, 8);
    var vram = memory.Peek(128, 8 * (bytesPerLine * bytesPerLine));
    var byteOffset = x / 8 + y * bytesPerLine;
    var bitOffset = x % 8;


    memory.Poke(128 + byteOffset + (0 * bytesPerLine), (byte)((byte)(sprRam[0] << bitOffset) | memory.Peek(128 + byteOffset + (0 * bytesPerLine))));
    memory.Poke(128 + byteOffset + (1 * bytesPerLine), (byte)((byte)(sprRam[1] << bitOffset) | memory.Peek(128 + byteOffset + (1 * bytesPerLine))));
    memory.Poke(128 + byteOffset + (2 * bytesPerLine), (byte)((byte)(sprRam[2] << bitOffset) | memory.Peek(128 + byteOffset + (2 * bytesPerLine))));
    memory.Poke(128 + byteOffset + (3 * bytesPerLine), (byte)((byte)(sprRam[3] << bitOffset) | memory.Peek(128 + byteOffset + (3 * bytesPerLine))));
    memory.Poke(128 + byteOffset + (4 * bytesPerLine), (byte)((byte)(sprRam[4] << bitOffset) | memory.Peek(128 + byteOffset + (4 * bytesPerLine))));
    memory.Poke(128 + byteOffset + (5 * bytesPerLine), (byte)((byte)(sprRam[5] << bitOffset) | memory.Peek(128 + byteOffset + (5 * bytesPerLine))));
    memory.Poke(128 + byteOffset + (6 * bytesPerLine), (byte)((byte)(sprRam[6] << bitOffset) | memory.Peek(128 + byteOffset + (6 * bytesPerLine))));
    memory.Poke(128 + byteOffset + (7 * bytesPerLine), (byte)((byte)(sprRam[7] << bitOffset) | memory.Peek(128 + byteOffset + (7 * bytesPerLine))));
    
    if (bitOffset != 0)
    {
        memory.Poke(128 + byteOffset + 1, (byte)((byte)(sprRam[0] >> 8 - bitOffset) | memory.Peek(128 + byteOffset + 1)));
    }

    /*var bits = new BitArray(sprRam.ToArray());

    for (int sprY = 0; sprY < bits.Count / 8; sprY++)
    {
        for (int sprX = 0; sprX < 8; sprX++)
        {
            var value = bits[sprY * 8 + sprX];
            SetPixel(sprX + x, sprY + y, value ? ConsoleColor.White : ConsoleColor.Black);
        }
    }*/
}