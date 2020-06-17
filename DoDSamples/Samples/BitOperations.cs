using Microsoft.Diagnostics.Runtime.Utilities;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace DoDSamples.Samples
{
    public class BitOperations
    {

        //
        // Branch Elimination Example: lets count even numbers
        //



        public static int CountEven(int[] numbers)
        {
            int counter = 0;
            for(int i = 0; i < numbers.Length; i++)
            {
                if(numbers[i] % 2 == 0)
                {
                    counter++; 
                }
            }

            return counter;
        }







        public static int CountEvenBranchFree(int[] numbers)
        {
            int counter = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                var add = numbers[i] & 1;
                counter += (1 - add);
            }

            return counter;
        }




        public static int CountEvenBranchFreeBetter(int[] numbers)
        {
            int counter = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                var odd = numbers[i] & 1;
                counter += odd;
            }

            // Take all odd numbers and substract them from the set
            // leaving only even numbers
            return numbers.Length - counter;
        }





        public static unsafe int CountEvenSIMD(int[] numbers)
        {
            int counter = 0;
            int len = numbers.Length;

            fixed (int* num = numbers)
            {
                Vector128<int> vresult = Vector128<int>.Zero;
                Vector128<int> ones = Vector128.Create(1);

                int i = 0;
                int lastBlockIndex = len - (len % 4);

                while (i < lastBlockIndex)
                {
                    var vec = Sse2.LoadVector128(num + i);
                    var odds = Sse2.And(vec, ones);
                    vresult = Sse2.Add(vresult, odds);

                    i += 4;
                }

                vresult = Ssse3.HorizontalAdd(vresult, vresult);
                vresult = Ssse3.HorizontalAdd(vresult, vresult);

                counter = vresult.ToScalar();

                while (i < len)
                {
                    var odd = numbers[i] & 1;
                    counter += odd;

                    i += 1;
                }
            }

            return numbers.Length - counter;
        }

        public static int CountChar(string text, char needle)
        {
            int count = 0;

            foreach(var c in text)
            {
                if (c == needle)
                    count++;
            }

            return count;
        }






        private static int[] jump_table = new int[256];
        
        public static void InitJumpTable(char needle)
        {
            int cIdx = (byte)needle;

            for(int i = 0; i < jump_table.Length; i++)
            {
                jump_table[i] = 0;
                if (i == cIdx)
                {
                    jump_table[i] = 1;
                }
            }
        }

        public static int CountCharJumpTable(string text, char needle)
        {
            int count = 0;

            foreach (var c in text)
            {
                count += jump_table[(byte)c];
            }

            return count;
        }

        public static int CountCharWORD(string text, char needle)
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
                        var result = (int)CountBetween(*p, before, after);
                        count += result;

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

                    return count - len;
                }
            }
        }

        public static int CountWhitespaceWORDLess(string text)
        {
            int count = 0;
            byte charToFind = (byte)' ';
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
                        var result = (int)CountLess(*p, after);
                        count += result;

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

                    return count - len;
                }
            }
        }

        public static int CountCharWORDCompact(string text, char needle)
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

                        var has = HasValue(*p, charToFind);
                        if (has > 0)
                        {
                            var result = CountBetween(*p, before, after);
                            count += (int)result;
                        }

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

        public static int CountWhitespaceWORDCompactLess(string text)
        {
            int count = 0;
            byte charToFind = (byte)' ';
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

                        var has = HasValue(*p, charToFind);
                        if (has > 0)
                        {
                            var result = CountLess(*p, after);
                            count += (int)result;
                        }

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
