using System;
using System.Collections.Generic;
using System.Text;

namespace DoDSamples.Samples
{
    public class BitOperations
    {
        public static int CountChar(string text, char needle)
        {
            int count = 0;
            byte charToFind = (byte)needle;
            byte before = (byte)(charToFind - 1);
            byte after = (byte)(charToFind + 1);

            unsafe
            {
                fixed (char* ch = text)
                {
                    int len = text.Length;

                    int i = 0;
                    int lastBlockIndex = len - (len % 4);

                    while (i < lastBlockIndex)
                    {
                        ulong* p = (ulong*)(ch + i);
                        var result = CountBetween(*p, before, after);
                        count += (int)result;

                        i += 4;
                    }

                    while (i < len)
                    {
                        char* c = (char*)(ch + i);
                        if (*c == charToFind)
                        {
                            count++;
                        }
                        i += 1;
                    }

                    return count;
                }
            }
        }

        //
        // Bit Twindling Hacks:
        // https://graphics.stanford.edu/~seander/bithacks.html
        //

        public static ulong HasZero(ulong v)
        {
            return (((v) - 0x0101010101010101UL) & ~(v) & 0x8080808080808080UL);
        }

        public static ulong HasValue(ulong x, byte n)
        {
            return (HasZero((x) ^ (~0UL / 255 * (n))));
        }

        public static ulong HasLess(ulong x, byte n)
        {
            return (((x) - ~0UL / 255 * (n)) & ~(x) & ~0UL / 255 * 128);
        }

        public static ulong CountLess(ulong x, byte n)
        {
            return (((~0UL / 255 * (127 + ((ulong)n)) - ((x) & ~0UL / 255 * 127)) & ~(x) & ~0UL / 255 * 128) / 128 % 255);
        }

        public static ulong HasBetween(ulong x, byte m, byte n)
        {
            return ((~0UL / 255 * (127 + ((ulong)n))
                - ((x) & ~0UL / 255 * 127) &
                ~(x) & ((x) & ~0UL / 255 * 127) +
                ~0UL / 255 * (127 - ((ulong)m))) & ~0UL / 255 * 128);
        }

        public static ulong CountBetween(ulong x, byte m, byte n)
        {
            return HasBetween(x, m, n) / 128 % 255;
        }



    }
}
