using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lega.Monogame.Shared.Hoopfe.Core
{
    public static class Util
    {

        public static Color[] Colors { get; } =
        {
			Util.FromHex("211f1f"),
			//Color.Transparent,
            Util.FromHex("372c38"),
            Util.FromHex("7a7272"),
            Util.FromHex("ababab"),
        };

        public static Color[] FromBuffer(ReadOnlySpan<byte> data)
        {
            var result = new Color[data.Length * 4];
            for (var i = 0; i < result.Length; i += 4)
            {
                var @byte = data[i / 4];
                int upper = @byte >> 4;
                int lower = @byte & 0x0F;

                int a = upper >> 2;
                int b = upper & 0x03;
                int c = lower >> 2;
                int d = lower & 0x03;

                result[i + 0] = GetColor(a);
                result[i + 1] = GetColor(b);
                result[i + 2] = GetColor(c);
                result[i + 3] = GetColor(d);
            }
            return result;
        }

        public static Color GetColor(int a)
        {
            return Colors[a % 4];
        }

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
}
