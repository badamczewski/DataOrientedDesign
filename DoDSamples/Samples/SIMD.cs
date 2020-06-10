using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace DoDSamples
{
    public class SIMDSum
    {
        public int[] GetArray()
        {
            var array = new int[128 * 1024 * 1024];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i;
            }

            return array;
        }

        public int Sum(ReadOnlySpan<int> source)
        {
            int result = 0;
            for(int i = 0; i  < source.Length; i++)
            {
                result += source[i];
            }

            return result;
        }

        public unsafe int SumVectorizedSse2(ReadOnlySpan<int> source)
        {
            int result;

            fixed (int* pSource = source)
            {
                Vector128<int> vresult = Vector128<int>.Zero;

                int i = 0;
                int lastBlockIndex = source.Length - (source.Length % 4);

                while (i < lastBlockIndex)
                {
                    vresult = Sse2.Add(vresult, Sse2.LoadVector128(pSource + i));
                    i += 4;
                }

                if (Ssse3.IsSupported)
                {
                    vresult = Ssse3.HorizontalAdd(vresult, vresult);
                    vresult = Ssse3.HorizontalAdd(vresult, vresult);
                }
                else
                {
                    vresult = Sse2.Add(vresult, Sse2.Shuffle(vresult, 0x4E));
                    vresult = Sse2.Add(vresult, Sse2.Shuffle(vresult, 0xB1));
                }
                result = vresult.ToScalar();

                while (i < source.Length)
                {
                    result += pSource[i];
                    i += 1;
                }
            }

            return result;
        }
    }
}
