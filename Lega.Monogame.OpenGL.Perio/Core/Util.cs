using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Lega.Monogame.OpenGL.Perio.Core;

public static class Util
{
    public static Color FromHex(string hex)
    {
        var enumerable = SplitInParts(hex, 2);
        var rgb = enumerable.ToArray();
        var r = Convert.ToInt32(rgb[0], 16);
        var g = Convert.ToInt32(rgb[1], 16);
        var b = Convert.ToInt32(rgb[2], 16);
        return new Color(r, g, b);
    }

    public static (byte r, byte g, byte b) FromColor(Color color)
    {
        return (r: color.R, g: color.G, b: color.B);
    }

    public static Color FromBytes(byte[] rgb)
    {
        return new Color(rgb[0], rgb[1], rgb[2]);
    }

    public static Color FromBytes(byte r, byte g, byte b)
    {
        return new Color(r, g, b);
    }

    public static Color[] FromBuffer(byte[] data)
    {
        var result = new Color[data.Length];
        for (var i = 0; i < data.Length; i++)
        {
            result[i] = VirtualSystem.Colors[data[i]];
        }
        return result;
    }

    public static Color[] FromBufferPerio(byte[] data)
    {
        var result = new Color[data.Length * 2];
        for (var i = 0; i < result.Length; i += 2)
        {
            //Console.WriteLine($"{data[i]:X}");
            //Console.WriteLine($"{(data[i] >> 4) & 0x0F:X}");
            //Console.WriteLine($"{(data[i]) & 0x0F:X}");
            result[i + 0] = VirtualSystem.Colors[(data[i / 2] >> 4) & 0x0F];
            result[i + 1] = VirtualSystem.Colors[data[i / 2] & 0x0F];
        }
        return result;
    }

    private static IEnumerable<string> SplitInParts(this string s, int partLength)
    {
        if (s == null)
            throw new ArgumentNullException(nameof(s));
        if (partLength <= 0)
            throw new ArgumentException("Part length has to be positive.", nameof(partLength));

        for (var i = 0; i < s.Length; i += partLength)
            yield return s.Substring(i, Math.Min(partLength, s.Length - i));
    }
}